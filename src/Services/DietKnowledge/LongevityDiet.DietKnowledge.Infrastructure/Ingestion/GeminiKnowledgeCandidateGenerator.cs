using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
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
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly GeminiOptions geminiOptions = geminiOptionsAccessor.Value;
    private readonly BookKnowledgeIngestionOptions ingestionOptions = ingestionOptionsAccessor.Value;

    private bool IsEnabled =>
        geminiOptions.Enabled &&
        !string.IsNullOrWhiteSpace(geminiOptions.ApiKey) &&
        !geminiOptions.ApiKey.Equals("development-only", StringComparison.OrdinalIgnoreCase);

    public async Task<IReadOnlyCollection<KnowledgeCandidateDraft>> GenerateAsync(
        BookSourceDocument document,
        IReadOnlyCollection<BookSourceChunk> chunks,
        CancellationToken cancellationToken = default)
    {
        var selectedChunks = chunks
            .Where(chunk => !string.IsNullOrWhiteSpace(chunk.Text))
            .OrderBy(chunk => chunk.ChunkIndex)
            .Take(Math.Clamp(ingestionOptions.MaxChunksPerGeneration, 1, 20))
            .ToArray();

        if (selectedChunks.Length == 0)
        {
            return Array.Empty<KnowledgeCandidateDraft>();
        }

        if (!IsEnabled)
        {
            return CreateFallbackDrafts(document, selectedChunks);
        }

        var drafts = new List<KnowledgeCandidateDraft>();
        foreach (var chunk in selectedChunks)
        {
            try
            {
                var generated = await GenerateFromChunkAsync(document, chunk, cancellationToken);
                drafts.AddRange(generated);
            }
            catch (Exception exception)
            {
                logger.LogWarning(
                    exception,
                    "Gemini failed to generate knowledge candidates for document {DocumentId} chunk {ChunkIndex}.",
                    document.Id,
                    chunk.ChunkIndex);
            }
        }

        return drafts.Count > 0
            ? drafts
            : CreateFallbackDrafts(document, selectedChunks);
    }

    private async Task<IReadOnlyCollection<KnowledgeCandidateDraft>> GenerateFromChunkAsync(
        BookSourceDocument document,
        BookSourceChunk chunk,
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
                        [new GeminiPart(BuildPrompt(document, chunk))])
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
            return Array.Empty<KnowledgeCandidateDraft>();
        }

        var candidates = ParseCandidates(text)
            .Where(candidate => !string.IsNullOrWhiteSpace(candidate.Title) &&
                !string.IsNullOrWhiteSpace(candidate.Summary))
            .Take(3)
            .Select(candidate => new KnowledgeCandidateDraft(
                chunk.Id,
                KnowledgeCandidateTypes.IsValid(candidate.CandidateType)
                    ? KnowledgeCandidateTypes.Normalize(candidate.CandidateType)
                    : KnowledgeCandidateTypes.DietGuideline,
                candidate.Title.Trim(),
                candidate.Summary.Trim(),
                string.IsNullOrWhiteSpace(candidate.SourceTitle)
                    ? "The Longevity Diet"
                    : candidate.SourceTitle.Trim(),
                string.IsNullOrWhiteSpace(candidate.SourceChapter)
                    ? "Uploaded PDF review"
                    : candidate.SourceChapter.Trim(),
                string.IsNullOrWhiteSpace(candidate.SourcePage)
                    ? chunk.PageNumber.ToString()
                    : candidate.SourcePage.Trim(),
                string.IsNullOrWhiteSpace(candidate.SourceReference)
                    ? SourceReference(document, chunk)
                    : candidate.SourceReference.Trim(),
                "Gemini"))
            .ToArray();

        return candidates;
    }

    private static string BuildPrompt(BookSourceDocument document, BookSourceChunk chunk)
    {
        return $"""
            You support an educational class project called Longevity Diet Platform.
            Convert this uploaded PDF chunk into concise knowledge candidates for admin review.

            Rules:
            - Return only a JSON array.
            - Each item shape: candidateType, title, summary, sourceTitle, sourceChapter, sourcePage, sourceReference.
            - candidateType must be DietGuideline unless the text clearly names a single compatible food.
            - Write the summary in original words; do not copy long passages.
            - No diagnosis, treatment advice, disease prediction, lifespan prediction, or disease-specific medical advice.
            - Keep summaries educational and project-scoped.
            - Use sourceTitle "The Longevity Diet" unless the text clearly says otherwise.
            - Use sourcePage "{chunk.PageNumber}" and sourceReference "{SourceReference(document, chunk)}".

            PDF file: {document.OriginalFileName}
            Page: {chunk.PageNumber}
            Chunk: {chunk.ChunkIndex}

            Chunk text:
            {Truncate(chunk.Text, 5000)}
            """;
    }

    private static IReadOnlyCollection<KnowledgeCandidateDraft> CreateFallbackDrafts(
        BookSourceDocument document,
        IReadOnlyCollection<BookSourceChunk> chunks)
    {
        return chunks
            .Take(5)
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

    private static IReadOnlyCollection<GeminiCandidateDraft> ParseCandidates(string text)
    {
        var json = ExtractJsonArray(text);
        if (string.IsNullOrWhiteSpace(json))
        {
            return Array.Empty<GeminiCandidateDraft>();
        }

        try
        {
            return JsonSerializer.Deserialize<GeminiCandidateDraft[]>(json, JsonOptions) ??
                Array.Empty<GeminiCandidateDraft>();
        }
        catch (JsonException)
        {
            return Array.Empty<GeminiCandidateDraft>();
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
        string CandidateType,
        string Title,
        string Summary,
        string SourceTitle,
        string SourceChapter,
        string SourcePage,
        string SourceReference);

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
