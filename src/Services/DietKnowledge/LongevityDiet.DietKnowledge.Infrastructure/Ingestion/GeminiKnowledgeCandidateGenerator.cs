using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using LongevityDiet.DietKnowledge.Application.Abstractions;
using LongevityDiet.DietKnowledge.Application.Common;
using LongevityDiet.DietKnowledge.Application.Models;
using LongevityDiet.DietKnowledge.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LongevityDiet.DietKnowledge.Infrastructure.Ingestion;

public sealed class GeminiKnowledgeCandidateGenerator(
    HttpClient httpClient,
    IOptions<GeminiOptions> geminiOptionsAccessor,
    IOptions<BookKnowledgeIngestionOptions> ingestionOptionsAccessor,
    ILogger<GeminiKnowledgeCandidateGenerator> logger) : IKnowledgeCandidateGenerator
{
    // Several small chunks share one Gemini request, up to this many characters of PDF text.
    private const int MaxCharactersPerRequest = 6000;
    private const int MaxCandidatesPerChunk = 2;
    private const int MaxCandidatesPerRequest = 12;
    private const int FallbackDraftLimit = 5;

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        NumberHandling = JsonNumberHandling.AllowReadingFromString
    };

    private readonly GeminiOptions geminiOptions = geminiOptionsAccessor.Value;
    private readonly BookKnowledgeIngestionOptions ingestionOptions = ingestionOptionsAccessor.Value;

    private bool IsEnabled =>
        geminiOptions.Enabled &&
        !string.IsNullOrWhiteSpace(geminiOptions.ApiKey) &&
        !geminiOptions.ApiKey.Equals("development-only", StringComparison.OrdinalIgnoreCase);

    public async Task<KnowledgeCandidateGenerationResult> GenerateAsync(
        BookSourceDocument document,
        IReadOnlyCollection<BookSourceChunk> chunks,
        CancellationToken cancellationToken = default)
    {
        var selectedChunks = chunks
            .Where(chunk => !string.IsNullOrWhiteSpace(BookChunkRelevance.CleanText(chunk.Text)))
            .OrderBy(chunk => chunk.ChunkIndex)
            .Take(Math.Clamp(ingestionOptions.MaxChunksPerGeneration, 1, 60))
            .ToArray();

        if (selectedChunks.Length == 0)
        {
            return new KnowledgeCandidateGenerationResult([], []);
        }

        if (!IsEnabled)
        {
            var fallbackChunks = selectedChunks.Take(FallbackDraftLimit).ToArray();
            return new KnowledgeCandidateGenerationResult(
                CreateFallbackDrafts(document, fallbackChunks),
                fallbackChunks.Select(chunk => chunk.Id).ToArray());
        }

        var drafts = new List<KnowledgeCandidateDraft>();
        var processedChunkIds = new List<Guid>();
        var deadline = DateTime.UtcNow.AddSeconds(Math.Max(1, geminiOptions.TotalBudgetSeconds));
        var consecutiveFailures = 0;

        foreach (var group in GroupChunks(selectedChunks))
        {
            // Gemini looks unavailable or the time budget is spent: leave the rest pending so the
            // admin can continue later instead of filling the review queue with placeholders.
            if (consecutiveFailures >= Math.Max(1, geminiOptions.MaxConsecutiveFailures) ||
                DateTime.UtcNow >= deadline)
            {
                break;
            }

            try
            {
                var generated = await GenerateWithRetryAsync(document, group, deadline, cancellationToken);
                drafts.AddRange(generated);
                processedChunkIds.AddRange(group.Select(chunk => chunk.Id));
                consecutiveFailures = 0;
            }
            catch (Exception exception) when (!cancellationToken.IsCancellationRequested)
            {
                consecutiveFailures++;
                logger.LogWarning(
                    exception,
                    "Gemini failed to generate knowledge candidates for document {DocumentId} chunks {FirstChunk}-{LastChunk}; they stay pending.",
                    document.Id,
                    group[0].ChunkIndex,
                    group[^1].ChunkIndex);
            }
        }

        return new KnowledgeCandidateGenerationResult(drafts, processedChunkIds);
    }

    private static IEnumerable<IReadOnlyList<BookSourceChunk>> GroupChunks(
        IReadOnlyCollection<BookSourceChunk> chunks)
    {
        var current = new List<BookSourceChunk>();
        var currentLength = 0;
        foreach (var chunk in chunks)
        {
            var length = Math.Min(BookChunkRelevance.CleanText(chunk.Text).Length, MaxCharactersPerRequest);
            if (current.Count > 0 && currentLength + length > MaxCharactersPerRequest)
            {
                yield return current;
                current = [];
                currentLength = 0;
            }

            current.Add(chunk);
            currentLength += length;
        }

        if (current.Count > 0)
        {
            yield return current;
        }
    }

    private async Task<IReadOnlyCollection<KnowledgeCandidateDraft>> GenerateWithRetryAsync(
        BookSourceDocument document,
        IReadOnlyList<BookSourceChunk> group,
        DateTime deadline,
        CancellationToken cancellationToken)
    {
        var maxAttempts = Math.Max(0, geminiOptions.MaxRetries) + 1;
        for (var attempt = 1; ; attempt++)
        {
            try
            {
                // Bound each call by the remaining budget so one slow request cannot overrun it.
                using var budgetCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                budgetCts.CancelAfter(deadline - DateTime.UtcNow > TimeSpan.Zero
                    ? deadline - DateTime.UtcNow
                    : TimeSpan.FromMilliseconds(1));
                return await GenerateFromChunksAsync(document, group, budgetCts.Token);
            }
            catch (Exception exception) when (
                attempt < maxAttempts &&
                !cancellationToken.IsCancellationRequested &&
                IsTransient(exception))
            {
                var delay = TimeSpan.FromSeconds(Math.Max(0, geminiOptions.RetryDelaySeconds) * attempt);
                if (DateTime.UtcNow + delay >= deadline)
                {
                    throw;
                }

                logger.LogInformation(
                    "Gemini transient failure on chunks {FirstChunk}-{LastChunk} (attempt {Attempt}/{MaxAttempts}); retrying in {Delay}s.",
                    group[0].ChunkIndex,
                    group[^1].ChunkIndex,
                    attempt,
                    maxAttempts,
                    delay.TotalSeconds);
                await Task.Delay(delay, cancellationToken);
            }
        }
    }

    private static bool IsTransient(Exception exception)
    {
        return exception switch
        {
            GeminiHttpException http => http.StatusCode is 408 or 429 or >= 500,
            HttpRequestException => true,
            TaskCanceledException => true, // HttpClient timeout (caller cancellation is filtered by the caller).
            _ => false
        };
    }

    private sealed class GeminiHttpException(int statusCode, string message) : Exception(message)
    {
        public int StatusCode { get; } = statusCode;
    }

    private async Task<IReadOnlyCollection<KnowledgeCandidateDraft>> GenerateFromChunksAsync(
        BookSourceDocument document,
        IReadOnlyList<BookSourceChunk> group,
        CancellationToken cancellationToken)
    {
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
                        [new GeminiPart(BuildPrompt(group))])
                ],
                new GeminiGenerationConfig(
                    Math.Clamp(geminiOptions.Temperature, 0, 1),
                    Math.Clamp(geminiOptions.MaxOutputTokens, 128, 4096))),
            options: JsonOptions);

        using var response = await httpClient.SendAsync(request, cancellationToken);
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            throw new GeminiHttpException(
                (int)response.StatusCode,
                $"Gemini returned HTTP {(int)response.StatusCode}: {Truncate(responseBody, 500)}");
        }

        var geminiResponse = JsonSerializer.Deserialize<GeminiGenerateContentResponse>(
            responseBody,
            JsonOptions);
        var text = geminiResponse?.Candidates?
            .SelectMany(candidate => candidate.Content?.Parts ?? Array.Empty<GeminiPart>())
            .Select(part => part.Text)
            .FirstOrDefault(partText => !string.IsNullOrWhiteSpace(partText));

        if (string.IsNullOrWhiteSpace(text))
        {
            throw new InvalidOperationException("Gemini returned no text for these chunks.");
        }

        // Unparseable output (for example truncated JSON) is a failure so the chunks are retried later.
        var parsed = ParseCandidates(text)
            ?? throw new InvalidOperationException("Gemini returned output that is not a JSON array.");

        var chunksByIndex = group.ToDictionary(chunk => chunk.ChunkIndex);
        var maxCandidates = Math.Min(MaxCandidatesPerRequest, MaxCandidatesPerChunk * group.Count);
        return parsed
            .Where(candidate => !string.IsNullOrWhiteSpace(candidate.Title) &&
                !string.IsNullOrWhiteSpace(candidate.Summary))
            .Take(maxCandidates)
            .Select(candidate =>
            {
                var chunk = candidate.ChunkIndex is { } index && chunksByIndex.TryGetValue(index, out var match)
                    ? match
                    : group[0];
                var isRecipe = string.Equals(
                    candidate.CandidateType,
                    KnowledgeCandidateTypes.Recipe,
                    StringComparison.OrdinalIgnoreCase);
                var ingredients = isRecipe ? GroundedIngredients(candidate.Ingredients, chunk) : [];
                var summary = candidate.Summary!.Trim();
                if (isRecipe && ingredients.Count > 0)
                {
                    summary += " Ingredients: " + string.Join(
                        "; ",
                        ingredients.Select(ingredient => string.IsNullOrWhiteSpace(ingredient.Quantity)
                            ? ingredient.Name
                            : $"{ingredient.Name} ({ingredient.Quantity})")) + ".";
                }

                return new KnowledgeCandidateDraft(
                    chunk.Id,
                    KnowledgeCandidateTypes.IsValid(candidate.CandidateType)
                        ? KnowledgeCandidateTypes.Normalize(candidate.CandidateType!)
                        : KnowledgeCandidateTypes.DietGuideline,
                    candidate.Title!.Trim(),
                    summary,
                    "The Longevity Diet",
                    string.IsNullOrWhiteSpace(candidate.SourceChapter)
                        ? "Uploaded PDF review"
                        : candidate.SourceChapter.Trim(),
                    chunk.PageNumber.ToString(),
                    SourceReference(document, chunk),
                    "Gemini",
                    ingredients);
            })
            // A recipe with no ingredient found in the excerpt is not grounded in the book; drop it.
            .Where(draft => !string.Equals(draft.CandidateType, KnowledgeCandidateTypes.Recipe, StringComparison.OrdinalIgnoreCase) ||
                draft.Ingredients is { Count: > 0 })
            .ToArray();
    }

    // Keeps only ingredients whose name appears in the excerpt, so nothing outside the book is added.
    private static IReadOnlyList<CandidateIngredient> GroundedIngredients(
        GeminiIngredientDraft[]? ingredients,
        BookSourceChunk chunk)
    {
        var text = BookChunkRelevance.CleanText(chunk.Text);
        return (ingredients ?? [])
            .Where(ingredient => !string.IsNullOrWhiteSpace(ingredient.Name) &&
                text.Contains(ingredient.Name.Trim(), StringComparison.OrdinalIgnoreCase))
            .GroupBy(ingredient => ingredient.Name!.Trim(), StringComparer.OrdinalIgnoreCase)
            .Select(group => new CandidateIngredient(
                group.Key,
                (group.First().Quantity ?? string.Empty).Trim()))
            .ToArray();
    }

    private static string BuildPrompt(IReadOnlyList<BookSourceChunk> group)
    {
        var maxCandidates = Math.Min(MaxCandidatesPerRequest, MaxCandidatesPerChunk * group.Count);
        var builder = new StringBuilder();
        builder.AppendLine("You support an educational class project called Longevity Diet Platform.");
        builder.AppendLine("Convert the PDF excerpts below into concise knowledge candidates for admin review.");
        builder.AppendLine();
        builder.AppendLine("Rules:");
        builder.AppendLine("- Use ONLY information stated in the excerpts. Do not add outside knowledge, general nutrition facts,");
        builder.AppendLine("  examples or assumptions. If the excerpt does not state it, leave it out.");
        builder.AppendLine("- Return only a JSON array. Return [] when nothing is suitable.");
        builder.AppendLine("- Each item shape: candidateType, title, summary, sourceChapter, chunkIndex (Recipe items also: ingredients).");
        builder.AppendLine("- candidateType is DietGuideline for an eating principle, habit or pattern.");
        builder.AppendLine("- Use candidateType Food only when the text names ONE specific food with a concrete nutrition note,");
        builder.AppendLine("  for example a food listed as a source of a vitamin or mineral. For Food, title is only the food name.");
        builder.AppendLine("- Use candidateType Recipe only for a dish the excerpt itself gives with its ingredients. Add an");
        builder.AppendLine("  ingredients array of {name, quantity}. Copy each ingredient name exactly as written in the excerpt, in the");
        builder.AppendLine("  same language, and copy its quantity as written (or empty). Never add an ingredient the excerpt does not list.");
        builder.AppendLine($"- Return at most {maxCandidates} items and at most {MaxCandidatesPerChunk} per excerpt.");
        builder.AppendLine("- Skip author biography, history, anecdotes, acknowledgments, quotes and anything promotional.");
        builder.AppendLine("- Write each summary in original words; do not copy long passages.");
        builder.AppendLine("- No diagnosis, treatment advice, disease prediction, lifespan prediction, or disease-specific medical advice.");
        builder.AppendLine("  Skip excerpts about treating or preventing a specific disease.");
        builder.AppendLine("- Keep summaries educational and project-scoped.");
        builder.AppendLine("- chunkIndex is the chunkIndex number of the excerpt the item comes from.");
        builder.AppendLine();
        builder.AppendLine("Excerpts:");
        foreach (var chunk in group)
        {
            builder.AppendLine($"[chunkIndex={chunk.ChunkIndex} page={chunk.PageNumber}]");
            builder.AppendLine(Truncate(BookChunkRelevance.CleanText(chunk.Text), MaxCharactersPerRequest));
            builder.AppendLine();
        }

        return builder.ToString();
    }

    private static IReadOnlyCollection<KnowledgeCandidateDraft> CreateFallbackDrafts(
        BookSourceDocument document,
        IReadOnlyCollection<BookSourceChunk> chunks)
    {
        return chunks
            .Select(chunk => new KnowledgeCandidateDraft(
                chunk.Id,
                KnowledgeCandidateTypes.DietGuideline,
                $"Review {Path.GetFileNameWithoutExtension(document.OriginalFileName)} page {chunk.PageNumber} chunk {chunk.ChunkIndex}",
                "Admin review required. Use this chunk to write a concise project-ready summary in the team's own words before approving it.",
                "The Longevity Diet",
                "Uploaded PDF review",
                chunk.PageNumber.ToString(),
                SourceReference(document, chunk),
                "LocalChunkFallback"))
            .ToArray();
    }

    // Returns null when the text holds no parseable JSON array; an empty array is a valid answer.
    private static IReadOnlyCollection<GeminiCandidateDraft>? ParseCandidates(string text)
    {
        var json = ExtractJsonArray(text);
        if (string.IsNullOrWhiteSpace(json))
        {
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize<GeminiCandidateDraft[]>(json, JsonOptions);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private static string ExtractJsonArray(string value)
    {
        var start = value.IndexOf('[', StringComparison.Ordinal);
        var end = value.LastIndexOf(']');
        if (start < 0 || end <= start)
        {
            return string.Empty;
        }

        return value[start..(end + 1)];
    }

    private static string SourceReference(BookSourceDocument document, BookSourceChunk chunk)
    {
        return $"Uploaded PDF {document.OriginalFileName}; page {chunk.PageNumber}; chunk {chunk.ChunkIndex}";
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

    private static string Truncate(string value, int maxLength)
    {
        return value.Length <= maxLength ? value : value[..maxLength];
    }

    private sealed record GeminiCandidateDraft(
        string? CandidateType,
        string? Title,
        string? Summary,
        string? SourceChapter,
        int? ChunkIndex,
        GeminiIngredientDraft[]? Ingredients = null);

    private sealed record GeminiIngredientDraft(string? Name, string? Quantity);

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
