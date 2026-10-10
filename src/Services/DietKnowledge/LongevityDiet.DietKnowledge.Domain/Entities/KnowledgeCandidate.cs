namespace LongevityDiet.DietKnowledge.Domain.Entities;

public sealed class KnowledgeCandidate
{
    public Guid Id { get; set; }

    public Guid DocumentId { get; set; }

    public BookSourceDocument? Document { get; set; }

    public Guid? ChunkId { get; set; }

    public BookSourceChunk? Chunk { get; set; }

    public string CandidateType { get; set; } = "DietGuideline";

    public string Title { get; set; } = string.Empty;

    public string Summary { get; set; } = string.Empty;

    public string SourceTitle { get; set; } = "The Longevity Diet";

    public string SourceChapter { get; set; } = string.Empty;

    public string SourcePage { get; set; } = string.Empty;

    public string SourceReference { get; set; } = string.Empty;

    public string Status { get; set; } = "NeedsReview";

    public string GeneratedBy { get; set; } = string.Empty;

    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public string ReviewedBy { get; set; } = string.Empty;

    public DateTimeOffset? ReviewedAtUtc { get; set; }

    public Guid? ApprovedKnowledgeId { get; set; }
}
