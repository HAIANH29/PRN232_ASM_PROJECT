namespace LongevityDiet.DietKnowledge.Application.Models;

public static class KnowledgeSourceTypes
{
    public const string Guideline = "Guideline";
    public const string Food = "Food";
    public const string Recipe = "Recipe";

    public static readonly string[] All = [Guideline, Food, Recipe];

    public static string? Normalize(string? value)
    {
        return All.FirstOrDefault(type => string.Equals(type, value?.Trim(), StringComparison.OrdinalIgnoreCase));
    }
}

public sealed record KnowledgeVectorItem(
    string SourceType,
    Guid SourceId,
    string ContentHash,
    string Content,
    float[] Embedding);

public sealed record KnowledgeSearchHit(
    string SourceType,
    Guid SourceId,
    string Title,
    string Snippet,
    double Score);

public sealed record KnowledgeIndexRunModel(
    Guid Id,
    DateTimeOffset StartedAtUtc,
    DateTimeOffset? CompletedAtUtc,
    int ItemsIndexed,
    int ItemsFailed,
    string Status);

public sealed record KnowledgeIndexResultModel(
    Guid RunId,
    int Indexed,
    int Unchanged,
    int Removed,
    int Failed,
    string Status);

public sealed record KnowledgeIndexStatusModel(
    bool VectorDbConfigured,
    string EmbeddingModel,
    int Dimensions,
    int IndexedItems,
    KnowledgeIndexRunModel? LatestRun);
