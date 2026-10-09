namespace LongevityDiet.MealPlanning.Application.Models;

public sealed record PagedResult<T>(
    IReadOnlyCollection<T> Items,
    int PageNumber,
    int PageSize,
    int TotalCount);
