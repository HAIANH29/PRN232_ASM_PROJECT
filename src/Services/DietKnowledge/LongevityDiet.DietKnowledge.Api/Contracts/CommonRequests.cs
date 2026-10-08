using System.ComponentModel.DataAnnotations;

namespace LongevityDiet.DietKnowledge.Api.Contracts;

public abstract record ListRequest
{
    [Range(1, int.MaxValue)]
    public int PageNumber { get; init; } = 1;

    [Range(1, 100)]
    public int PageSize { get; init; } = 20;

    [MaxLength(200)]
    public string? Search { get; init; }

    [MaxLength(40)]
    public string? SortBy { get; init; }

    [RegularExpression("^(asc|desc)$", ErrorMessage = "SortDirection must be asc or desc.")]
    public string? SortDirection { get; init; } = "asc";
}

public sealed record SetContentActivationRequest
{
    [Required]
    public bool IsActive { get; init; }
}
