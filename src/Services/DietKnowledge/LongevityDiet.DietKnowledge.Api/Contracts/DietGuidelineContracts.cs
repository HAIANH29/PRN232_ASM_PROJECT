using System.ComponentModel.DataAnnotations;

namespace LongevityDiet.DietKnowledge.Api.Contracts;

public sealed record DietGuidelineListRequest : ListRequest
{
    public bool IncludeInactive { get; init; }

    [MaxLength(40)]
    public string? ReviewStatus { get; init; }
}

public sealed record DietGuidelineResponse(
    Guid Id,
    string Title,
    string Summary,
    string SourceNote,
    string SourceTitle,
    string SourceChapter,
    string SourcePage,
    string SourceReference,
    string ReviewStatus,
    string ReviewedBy,
    DateTimeOffset? ReviewedAtUtc,
    bool IsActive,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc);

public sealed record CreateDietGuidelineRequest
{
    [Required]
    [MaxLength(200)]
    public string Title { get; init; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Summary { get; init; } = string.Empty;

    [MaxLength(500)]
    public string SourceNote { get; init; } = string.Empty;

    [MaxLength(200)]
    public string SourceTitle { get; init; } = "The Longevity Diet";

    [MaxLength(200)]
    public string SourceChapter { get; init; } = string.Empty;

    [MaxLength(80)]
    public string SourcePage { get; init; } = string.Empty;

    [MaxLength(500)]
    public string SourceReference { get; init; } = string.Empty;

    [MaxLength(40)]
    public string ReviewStatus { get; init; } = "NeedsReview";

    [MaxLength(120)]
    public string ReviewedBy { get; init; } = string.Empty;
}

public sealed record UpdateDietGuidelineRequest
{
    [Required]
    [MaxLength(200)]
    public string Title { get; init; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Summary { get; init; } = string.Empty;

    [MaxLength(500)]
    public string SourceNote { get; init; } = string.Empty;

    [MaxLength(200)]
    public string SourceTitle { get; init; } = "The Longevity Diet";

    [MaxLength(200)]
    public string SourceChapter { get; init; } = string.Empty;

    [MaxLength(80)]
    public string SourcePage { get; init; } = string.Empty;

    [MaxLength(500)]
    public string SourceReference { get; init; } = string.Empty;

    [MaxLength(40)]
    public string ReviewStatus { get; init; } = "NeedsReview";

    [MaxLength(120)]
    public string ReviewedBy { get; init; } = string.Empty;
}
