using System.ComponentModel.DataAnnotations;

namespace LongevityDiet.DietKnowledge.Api.Contracts;

public sealed record FoodListRequest : ListRequest
{
    [MaxLength(120)]
    public string? Category { get; init; }

    public bool IncludeInactive { get; init; }

    [MaxLength(40)]
    public string? ReviewStatus { get; init; }
}

public sealed record FoodResponse(
    Guid Id,
    string Name,
    string Category,
    string CompatibilityNotes,
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

public sealed record CreateFoodRequest
{
    [Required]
    [MaxLength(160)]
    public string Name { get; init; } = string.Empty;

    [Required]
    [MaxLength(120)]
    public string Category { get; init; } = string.Empty;

    [MaxLength(1000)]
    public string CompatibilityNotes { get; init; } = string.Empty;

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

public sealed record UpdateFoodRequest
{
    [Required]
    [MaxLength(160)]
    public string Name { get; init; } = string.Empty;

    [Required]
    [MaxLength(120)]
    public string Category { get; init; } = string.Empty;

    [MaxLength(1000)]
    public string CompatibilityNotes { get; init; } = string.Empty;

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
