namespace LongevityDiet.DietKnowledge.Application.Models;

public sealed record DietGuidelineQuery(
    int PageNumber,
    int PageSize,
    string? Search,
    bool IncludeInactive,
    string? SortBy,
    string? SortDirection);

public sealed record FoodQuery(
    int PageNumber,
    int PageSize,
    string? Search,
    string? Category,
    bool IncludeInactive,
    string? SortBy,
    string? SortDirection);

public sealed record RecipeQuery(
    int PageNumber,
    int PageSize,
    string? Search,
    Guid? FoodId,
    bool IncludeInactive,
    string? SortBy,
    string? SortDirection);
