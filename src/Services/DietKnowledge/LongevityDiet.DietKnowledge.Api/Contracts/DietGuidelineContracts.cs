using System.ComponentModel.DataAnnotations;

namespace LongevityDiet.DietKnowledge.Api.Contracts;

public sealed record DietGuidelineListRequest : ListRequest
{
    public bool IncludeInactive { get; init; }
}

public sealed record DietGuidelineResponse(
    Guid Id,
    string Title,
    string Summary,
    string SourceNote,
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
}
