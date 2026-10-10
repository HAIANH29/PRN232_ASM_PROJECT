namespace LongevityDiet.DietKnowledge.Domain.Entities;

public sealed class BookSourceChunk
{
    public Guid Id { get; set; }

    public Guid DocumentId { get; set; }

    public BookSourceDocument? Document { get; set; }

    public int ChunkIndex { get; set; }

    public int PageNumber { get; set; }

    public string Text { get; set; } = string.Empty;

    public int CharacterCount { get; set; }

    public int TokenEstimate { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<KnowledgeCandidate> Candidates { get; } = new List<KnowledgeCandidate>();
}
