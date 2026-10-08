using System.ComponentModel.DataAnnotations;

namespace LongevityDiet.DietKnowledge.Api.Contracts;

public sealed record FoodListRequest : ListRequest
{
    [MaxLength(120)]
    public string? Category { get; init; }

    public bool IncludeInactive { get; init; }
}

public sealed record FoodResponse(
    Guid Id,
    string Name,
    string Category,
    string CompatibilityNotes,
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
}
