using LongevityDiet.DietKnowledge.Application.Abstractions;
using LongevityDiet.DietKnowledge.Application.Models;
using Microsoft.EntityFrameworkCore;
using Pgvector.EntityFrameworkCore;

namespace LongevityDiet.DietKnowledge.Infrastructure.VectorDb;

public sealed class PgVectorKnowledgeStore(VectorDbContext dbContext) : IKnowledgeVectorStore
{
    public bool IsConfigured => true;

    public async Task EnsureCreatedAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.Database.MigrateAsync(cancellationToken);
    }

    public async Task<IReadOnlyDictionary<string, string>> GetIndexedHashesAsync(
        string embeddingModel,
        CancellationToken cancellationToken = default)
    {
        var rows = await dbContext.KnowledgeEmbeddings
            .AsNoTracking()
            .Where(embedding => embedding.EmbeddingModel == embeddingModel)
            .Select(embedding => new { embedding.SourceType, embedding.SourceId, embedding.ContentHash })
            .ToArrayAsync(cancellationToken);

        return rows.ToDictionary(row => $"{row.SourceType}:{row.SourceId}", row => row.ContentHash);
    }

    public async Task<Guid> StartRunAsync(CancellationToken cancellationToken = default)
    {
        var run = new EmbeddingIndexRun { Id = Guid.NewGuid(), StartedAtUtc = DateTimeOffset.UtcNow };
        dbContext.EmbeddingIndexRuns.Add(run);
        await dbContext.SaveChangesAsync(cancellationToken);
        return run.Id;
    }

    public async Task UpsertAsync(
        Guid runId,
        KnowledgeVectorItem item,
        string embeddingModel,
        CancellationToken cancellationToken = default)
    {
        var row = await dbContext.KnowledgeEmbeddings.FirstOrDefaultAsync(
            embedding => embedding.SourceType == item.SourceType && embedding.SourceId == item.SourceId,
            cancellationToken);

        if (row is null)
        {
            row = new KnowledgeEmbedding
            {
                Id = Guid.NewGuid(),
                SourceType = item.SourceType,
                SourceId = item.SourceId,
                CreatedAtUtc = DateTimeOffset.UtcNow
            };
            dbContext.KnowledgeEmbeddings.Add(row);
        }
        else
        {
            row.UpdatedAtUtc = DateTimeOffset.UtcNow;
        }

        row.IndexRunId = runId;
        row.ContentHash = item.ContentHash;
        row.Content = item.Content;
        row.Embedding = new Pgvector.Vector(item.Embedding);
        row.EmbeddingModel = embeddingModel;

        await dbContext.SaveChangesAsync(cancellationToken);
        dbContext.ChangeTracker.Clear();
    }

    public async Task<int> RemoveNotInAsync(
        IReadOnlySet<string> keepKeys,
        CancellationToken cancellationToken = default)
    {
        var rows = await dbContext.KnowledgeEmbeddings
            .Select(embedding => new { embedding.Id, embedding.SourceType, embedding.SourceId })
            .ToArrayAsync(cancellationToken);

        var staleIds = rows
            .Where(row => !keepKeys.Contains($"{row.SourceType}:{row.SourceId}"))
            .Select(row => row.Id)
            .ToArray();

        if (staleIds.Length == 0)
        {
            return 0;
        }

        return await dbContext.KnowledgeEmbeddings
            .Where(embedding => staleIds.Contains(embedding.Id))
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task CompleteRunAsync(
        Guid runId,
        int itemsIndexed,
        int itemsFailed,
        string status,
        CancellationToken cancellationToken = default)
    {
        await dbContext.EmbeddingIndexRuns
            .Where(run => run.Id == runId)
            .ExecuteUpdateAsync(
                setters => setters
                    .SetProperty(run => run.ItemsIndexed, itemsIndexed)
                    .SetProperty(run => run.ItemsFailed, itemsFailed)
                    .SetProperty(run => run.Status, status)
                    .SetProperty(run => run.CompletedAtUtc, DateTimeOffset.UtcNow),
                cancellationToken);
    }

    public async Task<KnowledgeIndexRunModel?> GetLatestRunAsync(CancellationToken cancellationToken = default)
    {
        var run = await dbContext.EmbeddingIndexRuns
            .AsNoTracking()
            .OrderByDescending(item => item.StartedAtUtc)
            .FirstOrDefaultAsync(cancellationToken);

        return run is null
            ? null
            : new KnowledgeIndexRunModel(
                run.Id,
                run.StartedAtUtc,
                run.CompletedAtUtc,
                run.ItemsIndexed,
                run.ItemsFailed,
                run.Status);
    }

    public Task<int> CountAsync(string embeddingModel, CancellationToken cancellationToken = default)
    {
        return dbContext.KnowledgeEmbeddings.CountAsync(
            embedding => embedding.EmbeddingModel == embeddingModel,
            cancellationToken);
    }

    public async Task<IReadOnlyCollection<KnowledgeSearchHit>> SearchAsync(
        float[] queryEmbedding,
        string embeddingModel,
        IReadOnlyCollection<string>? sourceTypes,
        int limit,
        CancellationToken cancellationToken = default)
    {
        var query = new Pgvector.Vector(queryEmbedding);
        var source = dbContext.KnowledgeEmbeddings
            .AsNoTracking()
            .Where(embedding => embedding.EmbeddingModel == embeddingModel);

        if (sourceTypes is { Count: > 0 })
        {
            source = source.Where(embedding => sourceTypes.Contains(embedding.SourceType));
        }

        var rows = await source
            .Select(embedding => new
            {
                embedding.SourceType,
                embedding.SourceId,
                embedding.Content,
                Distance = embedding.Embedding.CosineDistance(query)
            })
            .OrderBy(row => row.Distance)
            .Take(limit)
            .ToArrayAsync(cancellationToken);

        return rows
            .Select(row =>
            {
                var newline = row.Content.IndexOf('\n');
                var title = newline < 0 ? row.Content : row.Content[..newline];
                var body = newline < 0 ? string.Empty : row.Content[(newline + 1)..].Trim();
                var snippet = body.Length > 240 ? body[..240] + "…" : body;
                return new KnowledgeSearchHit(row.SourceType, row.SourceId, title, snippet, 1 - row.Distance);
            })
            .ToArray();
    }
}

// Used when no VectorDb connection string is configured (for example in unit tests).
public sealed class DisabledKnowledgeVectorStore : IKnowledgeVectorStore
{
    public bool IsConfigured => false;

    public Task EnsureCreatedAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

    public Task<IReadOnlyDictionary<string, string>> GetIndexedHashesAsync(
        string embeddingModel,
        CancellationToken cancellationToken = default) => throw NotConfigured();

    public Task<Guid> StartRunAsync(CancellationToken cancellationToken = default) => throw NotConfigured();

    public Task UpsertAsync(
        Guid runId,
        KnowledgeVectorItem item,
        string embeddingModel,
        CancellationToken cancellationToken = default) => throw NotConfigured();

    public Task<int> RemoveNotInAsync(
        IReadOnlySet<string> keepKeys,
        CancellationToken cancellationToken = default) => throw NotConfigured();

    public Task CompleteRunAsync(
        Guid runId,
        int itemsIndexed,
        int itemsFailed,
        string status,
        CancellationToken cancellationToken = default) => throw NotConfigured();

    public Task<KnowledgeIndexRunModel?> GetLatestRunAsync(CancellationToken cancellationToken = default) =>
        throw NotConfigured();

    public Task<int> CountAsync(string embeddingModel, CancellationToken cancellationToken = default) =>
        throw NotConfigured();

    public Task<IReadOnlyCollection<KnowledgeSearchHit>> SearchAsync(
        float[] queryEmbedding,
        string embeddingModel,
        IReadOnlyCollection<string>? sourceTypes,
        int limit,
        CancellationToken cancellationToken = default) => throw NotConfigured();

    private static InvalidOperationException NotConfigured()
    {
        return new InvalidOperationException("The VectorDb is not configured for this service.");
    }
}
