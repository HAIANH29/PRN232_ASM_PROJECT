using System.ComponentModel.DataAnnotations;

namespace LongevityDiet.Tracking.Api.Contracts;

public abstract record ListRequest
{
    [Range(1, int.MaxValue)]
    public int PageNumber { get; init; } = 1;

    [Range(1, 100)]
    public int PageSize { get; init; } = 20;
}
