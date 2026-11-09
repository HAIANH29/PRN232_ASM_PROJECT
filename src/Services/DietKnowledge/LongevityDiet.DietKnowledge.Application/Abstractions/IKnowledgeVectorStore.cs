using LongevityDiet.DietKnowledge.Application.Models;

namespace LongevityDiet.DietKnowledge.Application.Abstractions;

// Turns text into a fixed-size embedding. Rows store the model name so an index can be rebuilt
// when the model changes and searches only compare vectors made by the same model.
public interface IEmbeddingGenerator
{
    string ModelName { get; }

    int Dimensions { get; }

    Task<float[]> EmbedAsync(
        string text,
        EmbeddingPurpose purpose,
        CancellationToken cancellationToken = default);
}

public enum EmbeddingPurpose
{
    Document,
    Query
}

// The separate VectorDb (PostgreSQL + pgvector) that Diet Knowledge Service owns.
public interface IKnowledgeVectorStore
{
    bool IsConfigured { get; }

    // Applies the VectorDb migrations (creates the vector extension and tables).
    Task EnsureCreatedAsync(CancellationToken cancellationToken = default);

    // "SourceType:SourceId" -> content hash, for rows made with the given model.
    Task<IReadOnlyDictionary<string, string>> GetIndexedHashesAsync(
        string embeddingModel,
        CancellationToken cancellationToken = default);

    Task<Guid> StartRunAsync(CancellationToken cancellationToken = default);

    Task UpsertAsync(
        Guid runId,
        KnowledgeVectorItem item,
        string embeddingModel,
        CancellationToken cancellationToken = default);

    // Removes rows whose "SourceType:SourceId" is not in the set (content that is no longer approved/active).
    Task<int> RemoveNotInAsync(
        IReadOnlySet<string> keepKeys,
        CancellationToken cancellationToken = default);

    Task CompleteRunAsync(
        Guid runId,
        int itemsIndexed,
        int itemsFailed,
        string status,
        CancellationToken cancellationToken = default);

    Task<KnowledgeIndexRunModel?> GetLatestRunAsync(CancellationToken cancellationToken = default);

    Task<int> CountAsync(string embeddingModel, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<KnowledgeSearchHit>> SearchAsync(
        float[] queryEmbedding,
        string embeddingModel,
        IReadOnlyCollection<string>? sourceTypes,
        int limit,
        CancellationToken cancellationToken = default);
}
