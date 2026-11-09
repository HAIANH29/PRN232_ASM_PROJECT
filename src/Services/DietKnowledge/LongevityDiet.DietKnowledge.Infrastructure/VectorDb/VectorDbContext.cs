using Microsoft.EntityFrameworkCore;
using Pgvector;

namespace LongevityDiet.DietKnowledge.Infrastructure.VectorDb;

public static class VectorDimensions
{
    // Gemini embeddings are requested at this size; the local fallback uses it too.
    public const int Embedding = 768;
}

// Row of the VectorDb "KnowledgeEmbeddings" table. SourceType + SourceId point at an approved
// guideline, food or recipe in DietKnowledgeDb by id only (no foreign key across databases).
public sealed class KnowledgeEmbedding
{
    public Guid Id { get; set; }

    public Guid IndexRunId { get; set; }

    public EmbeddingIndexRun? IndexRun { get; set; }

    public string SourceType { get; set; } = string.Empty;

    public Guid SourceId { get; set; }

    public string ContentHash { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public Pgvector.Vector Embedding { get; set; } = null!;

    public string EmbeddingModel { get; set; } = string.Empty;

    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? UpdatedAtUtc { get; set; }
}

public sealed class EmbeddingIndexRun
{
    public Guid Id { get; set; }

    public DateTimeOffset StartedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? CompletedAtUtc { get; set; }

    public int ItemsIndexed { get; set; }

    public int ItemsFailed { get; set; }

    public string Status { get; set; } = "Running";

    public ICollection<KnowledgeEmbedding> Embeddings { get; } = new List<KnowledgeEmbedding>();
}

public sealed class VectorDbContext(DbContextOptions<VectorDbContext> options) : DbContext(options)
{
    public DbSet<KnowledgeEmbedding> KnowledgeEmbeddings => Set<KnowledgeEmbedding>();

    public DbSet<EmbeddingIndexRun> EmbeddingIndexRuns => Set<EmbeddingIndexRun>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("vector");

        modelBuilder.Entity<EmbeddingIndexRun>(entity =>
        {
            entity.HasKey(run => run.Id);
            entity.Property(run => run.Status).HasMaxLength(40).IsRequired();
            entity.Property(run => run.StartedAtUtc).IsRequired();
            entity.HasIndex(run => run.StartedAtUtc);
        });

        modelBuilder.Entity<KnowledgeEmbedding>(entity =>
        {
            entity.HasKey(embedding => embedding.Id);
            entity.Property(embedding => embedding.SourceType).HasMaxLength(40).IsRequired();
            entity.Property(embedding => embedding.ContentHash).HasMaxLength(64).IsRequired();
            entity.Property(embedding => embedding.Content).HasMaxLength(4000).IsRequired();
            entity.Property(embedding => embedding.EmbeddingModel).HasMaxLength(120).IsRequired();
            entity.Property(embedding => embedding.Embedding)
                .HasColumnType($"vector({VectorDimensions.Embedding})")
                .IsRequired();
            entity.Property(embedding => embedding.CreatedAtUtc).IsRequired();
            entity.HasOne(embedding => embedding.IndexRun)
                .WithMany(run => run.Embeddings)
                .HasForeignKey(embedding => embedding.IndexRunId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(embedding => new { embedding.SourceType, embedding.SourceId }).IsUnique();
            entity.HasIndex(embedding => embedding.Embedding)
                .HasMethod("hnsw")
                .HasOperators("vector_cosine_ops");
        });
    }
}
