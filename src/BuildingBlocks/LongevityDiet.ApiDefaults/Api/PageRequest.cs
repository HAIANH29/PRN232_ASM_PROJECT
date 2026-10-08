using System.ComponentModel.DataAnnotations;

namespace LongevityDiet.ApiDefaults.Api;

public sealed record PageRequest
{
    [Range(1, int.MaxValue)]
    public int PageNumber { get; init; } = 1;

    [Range(1, 100)]
    public int PageSize { get; init; } = 20;
}
