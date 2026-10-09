namespace LongevityDiet.DietKnowledge.Domain.Entities;

public sealed class Food
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string CompatibilityNotes { get; set; } = string.Empty;

    public string SourceTitle { get; set; } = "The Longevity Diet";

    public string SourceChapter { get; set; } = string.Empty;

    public string SourcePage { get; set; } = string.Empty;

    public string SourceReference { get; set; } = string.Empty;

    public string ReviewStatus { get; set; } = "NeedsReview";

    public string ReviewedBy { get; set; } = string.Empty;

    public DateTimeOffset? ReviewedAtUtc { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? UpdatedAtUtc { get; set; }
}
