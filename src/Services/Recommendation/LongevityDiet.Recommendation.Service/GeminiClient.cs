using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace LongevityDiet.Recommendation.Service;

public sealed class GeminiClient(
    HttpClient httpClient,
    IOptions<GeminiOptions> options,
    ILogger<GeminiClient> logger) : IGeminiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly GeminiOptions geminiOptions = options.Value;

    public bool IsEnabled =>
        geminiOptions.Enabled && IsConfigured(geminiOptions.ApiKey);

    public async Task<string?> GenerateTextAsync(
        string prompt,
        CancellationToken cancellationToken = default)
    {
        if (!IsEnabled)
        {
            return null;
        }

        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"v1beta/models/{NormalizeModel(geminiOptions.Model)}:generateContent");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.TryAddWithoutValidation("x-goog-api-key", geminiOptions.ApiKey);
        request.Content = JsonContent.Create(
            new GeminiGenerateContentRequest(
                [
                    new GeminiContent(
                        "user",
                        [new GeminiPart(prompt)])
                ],
                new GeminiGenerationConfig(
                    Math.Clamp(geminiOptions.Temperature, 0, 1),
                    Math.Clamp(geminiOptions.MaxOutputTokens, 128, 2048))),
            options: JsonOptions);

        using var response = await httpClient.SendAsync(request, cancellationToken);
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                $"Gemini returned HTTP {(int)response.StatusCode}: {Truncate(responseBody)}");
        }

        var geminiResponse = JsonSerializer.Deserialize<GeminiGenerateContentResponse>(
            responseBody,
            JsonOptions);

        var text = geminiResponse?.Candidates?
            .SelectMany(candidate => candidate.Content?.Parts ?? Array.Empty<GeminiPart>())
            .Select(part => part.Text)
            .Where(partText => !string.IsNullOrWhiteSpace(partText))
            .ToArray();

        if (text is null || text.Length == 0)
        {
            logger.LogWarning("Gemini returned a successful response without text content.");
            return null;
        }

        return string.Join(Environment.NewLine, text);
    }

    private static bool IsConfigured(string apiKey)
    {
        return !string.IsNullOrWhiteSpace(apiKey) &&
            !apiKey.Equals("development-only", StringComparison.OrdinalIgnoreCase);
    }

    private static string NormalizeModel(string model)
    {
        var normalized = string.IsNullOrWhiteSpace(model)
            ? "gemini-2.0-flash"
            : model.Trim();

        return normalized.StartsWith("models/", StringComparison.OrdinalIgnoreCase)
            ? normalized["models/".Length..]
            : normalized;
    }

    private static string Truncate(string value)
    {
        return value.Length <= 500 ? value : value[..500];
    }

    private sealed record GeminiGenerateContentRequest(
        IReadOnlyCollection<GeminiContent> Contents,
        GeminiGenerationConfig GenerationConfig);

    private sealed record GeminiGenerationConfig(
        double Temperature,
        int MaxOutputTokens);

    private sealed record GeminiGenerateContentResponse(
        IReadOnlyCollection<GeminiCandidate>? Candidates);

    private sealed record GeminiCandidate(
        GeminiContent? Content);

    private sealed record GeminiContent(
        string Role,
        IReadOnlyCollection<GeminiPart> Parts);

    private sealed record GeminiPart(
        string Text);
}
