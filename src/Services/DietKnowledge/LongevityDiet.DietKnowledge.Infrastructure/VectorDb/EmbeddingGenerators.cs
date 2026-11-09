using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using LongevityDiet.DietKnowledge.Application.Abstractions;
using LongevityDiet.DietKnowledge.Infrastructure.Ingestion;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LongevityDiet.DietKnowledge.Infrastructure.VectorDb;

public sealed class KnowledgeIndexOptions
{
    public string EmbeddingModel { get; set; } = "gemini-embedding-001";

    public bool IndexOnStartup { get; set; } = true;

    public int MaxRetries { get; set; } = 2;

    public int RetryDelaySeconds { get; set; } = 2;
}

// Offline fallback used when Gemini is off: hashed bag-of-words (words and word pairs) mapped to
// a fixed-size vector and normalized. It captures shared vocabulary, not meaning, but it keeps
// the VectorDb working in development and tests without any external call.
public sealed class LocalHashEmbeddingGenerator : IEmbeddingGenerator
{
    public string ModelName => $"local-hash-{Dimensions}";

    public int Dimensions => VectorDimensions.Embedding;

    public Task<float[]> EmbedAsync(
        string text,
        EmbeddingPurpose purpose,
        CancellationToken cancellationToken = default)
    {
        var vector = new float[Dimensions];
        var tokens = Tokenize(text);

        for (var i = 0; i < tokens.Count; i++)
        {
            Add(vector, tokens[i], 1f);
            if (i + 1 < tokens.Count)
            {
                Add(vector, tokens[i] + " " + tokens[i + 1], 0.5f);
            }
        }

        Normalize(vector);
        return Task.FromResult(vector);
    }

    private static void Add(float[] vector, string feature, float weight)
    {
        var hash = Fnv1a(feature);
        var index = (int)(hash % (uint)vector.Length);
        var sign = ((hash >> 31) & 1) == 0 ? 1f : -1f;
        vector[index] += sign * weight;
    }

    // FNV-1a is stable across processes (string.GetHashCode is not), so stored vectors stay comparable.
    private static uint Fnv1a(string value)
    {
        var hash = 2166136261u;
        foreach (var b in Encoding.UTF8.GetBytes(value))
        {
            hash ^= b;
            hash *= 16777619u;
        }

        return hash;
    }

    private static List<string> Tokenize(string text)
    {
        var decomposed = text.ToLowerInvariant().Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder(decomposed.Length);
        foreach (var character in decomposed)
        {
            var category = CharUnicodeInfo.GetUnicodeCategory(character);
            if (category == UnicodeCategory.NonSpacingMark)
            {
                continue;
            }

            // "đ" does not decompose, so map it to "d" to match unaccented typing.
            builder.Append(char.IsLetterOrDigit(character) ? (character == 'đ' ? 'd' : character) : ' ');
        }

        return builder.ToString()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Where(token => token.Length > 1)
            .ToList();
    }

    internal static void Normalize(float[] vector)
    {
        double sum = 0;
        foreach (var value in vector)
        {
            sum += value * value;
        }

        if (sum <= 0)
        {
            vector[0] = 1f;
            return;
        }

        var length = (float)Math.Sqrt(sum);
        for (var i = 0; i < vector.Length; i++)
        {
            vector[i] /= length;
        }
    }
}

public sealed class GeminiEmbeddingGenerator(
    HttpClient httpClient,
    IOptions<GeminiOptions> geminiOptions,
    IOptions<KnowledgeIndexOptions> indexOptions,
    ILogger<GeminiEmbeddingGenerator> logger) : IEmbeddingGenerator
{
    public string ModelName => $"gemini:{indexOptions.Value.EmbeddingModel}:{Dimensions}";

    public int Dimensions => VectorDimensions.Embedding;

    public async Task<float[]> EmbedAsync(
        string text,
        EmbeddingPurpose purpose,
        CancellationToken cancellationToken = default)
    {
        var gemini = geminiOptions.Value;
        var model = indexOptions.Value.EmbeddingModel;
        var maxAttempts = Math.Max(1, indexOptions.Value.MaxRetries + 1);

        for (var attempt = 1; ; attempt++)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, $"v1beta/models/{model}:embedContent");
            request.Headers.Add("x-goog-api-key", gemini.ApiKey);
            request.Content = JsonContent.Create(new
            {
                model = $"models/{model}",
                content = new { parts = new[] { new { text } } },
                taskType = purpose == EmbeddingPurpose.Query ? "RETRIEVAL_QUERY" : "RETRIEVAL_DOCUMENT",
                outputDimensionality = Dimensions
            });

            try
            {
                using var response = await httpClient.SendAsync(request, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    using var document = await JsonDocument.ParseAsync(
                        await response.Content.ReadAsStreamAsync(cancellationToken),
                        cancellationToken: cancellationToken);

                    var values = document.RootElement.GetProperty("embedding").GetProperty("values");
                    var vector = new float[values.GetArrayLength()];
                    var i = 0;
                    foreach (var value in values.EnumerateArray())
                    {
                        vector[i++] = value.GetSingle();
                    }

                    if (vector.Length != Dimensions)
                    {
                        throw new InvalidOperationException(
                            $"Gemini returned {vector.Length} dimensions, expected {Dimensions}.");
                    }

                    // Truncated embeddings are not unit length, so normalize before storing/comparing.
                    LocalHashEmbeddingGenerator.Normalize(vector);
                    return vector;
                }

                var transient = response.StatusCode is HttpStatusCode.TooManyRequests ||
                    (int)response.StatusCode >= 500;
                if (!transient || attempt >= maxAttempts)
                {
                    throw new HttpRequestException(
                        $"Gemini embedding request failed with HTTP {(int)response.StatusCode}.");
                }
            }
            catch (Exception exception) when (
                exception is HttpRequestException or TaskCanceledException &&
                attempt < maxAttempts &&
                !cancellationToken.IsCancellationRequested)
            {
                logger.LogWarning(exception, "Gemini embedding attempt {Attempt} failed; retrying.", attempt);
            }

            await Task.Delay(
                TimeSpan.FromSeconds(Math.Max(1, indexOptions.Value.RetryDelaySeconds) * attempt),
                cancellationToken);
        }
    }
}
