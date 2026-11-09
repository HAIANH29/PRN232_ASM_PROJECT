using System.Net.Http.Json;
using System.Text.Json;

namespace LongevityDiet.Recommendation.Service;

// Asks Diet Knowledge Service (which owns the VectorDb) which approved items are closest to the
// user's preferences. Recommendation never reads the VectorDb directly.
public interface ISemanticKnowledgeSearch
{
    // Ids of approved guidelines, foods and recipes, most relevant first. Empty when unavailable.
    Task<IReadOnlyList<Guid>> FindRelevantIdsAsync(
        IReadOnlyCollection<string> preferences,
        CancellationToken cancellationToken = default);
}

public sealed class HttpSemanticKnowledgeSearch(
    HttpClient httpClient,
    ILogger<HttpSemanticKnowledgeSearch> logger) : ISemanticKnowledgeSearch
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<IReadOnlyList<Guid>> FindRelevantIdsAsync(
        IReadOnlyCollection<string> preferences,
        CancellationToken cancellationToken = default)
    {
        if (preferences.Count == 0)
        {
            return Array.Empty<Guid>();
        }

        var query = Uri.EscapeDataString(string.Join(", ", preferences));

        try
        {
            using var response = await httpClient.GetAsync(
                $"/api/knowledge/search?q={query}&limit=30",
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogInformation(
                    "Semantic knowledge search returned HTTP {StatusCode}; using plain preference matching.",
                    (int)response.StatusCode);
                return Array.Empty<Guid>();
            }

            var envelope = await response.Content.ReadFromJsonAsync<Envelope>(JsonOptions, cancellationToken);
            return envelope?.Data?.Select(hit => hit.SourceId).ToArray() ?? Array.Empty<Guid>();
        }
        catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException or JsonException)
        {
            logger.LogInformation(exception, "Semantic knowledge search is unavailable; using plain preference matching.");
            return Array.Empty<Guid>();
        }
    }

    private sealed record Envelope(bool Succeeded, IReadOnlyCollection<Hit>? Data);

    private sealed record Hit(string SourceType, Guid SourceId);
}
