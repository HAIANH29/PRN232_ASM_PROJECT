namespace LongevityDiet.MealPlanning.Application.Models;

public sealed record MealPlanQuery(
    int PageNumber,
    int PageSize,
    string? Search,
    DateOnly? FromDate,
    DateOnly? ToDate,
    string? SortBy,
    string? SortDirection);

public sealed record MealPlanModel(
    Guid Id,
    Guid UserId,
    string Name,
    DateOnly StartDate,
    DateOnly EndDate,
    IReadOnlyCollection<MealPlanItemModel> Items,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc);

public sealed record MealPlanItemModel(
    Guid Id,
    DateOnly PlannedDate,
    string MealSlot,
    Guid? RecipeId,
    Guid? FoodId,
    string Notes,
    DateTimeOffset ReminderAtUtc,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc);

public sealed record CreateMealPlanCommand(
    Guid UserId,
    string UserEmail,
    string Name,
    DateOnly StartDate,
    DateOnly EndDate,
    IReadOnlyCollection<CreateMealPlanItemCommand> Items);

public sealed record UpdateMealPlanCommand(
    Guid UserId,
    string UserEmail,
    string Name,
    DateOnly StartDate,
    DateOnly EndDate);

public sealed record CreateMealPlanItemCommand(
    DateOnly PlannedDate,
    string MealSlot,
    Guid? RecipeId,
    Guid? FoodId,
    string? Notes,
    DateTimeOffset? ReminderAtUtc);

public sealed record AddMealPlanItemCommand(
    Guid UserId,
    string UserEmail,
    DateOnly PlannedDate,
    string MealSlot,
    Guid? RecipeId,
    Guid? FoodId,
    string? Notes,
    DateTimeOffset? ReminderAtUtc);

public sealed record UpdateMealPlanItemCommand(
    Guid UserId,
    string UserEmail,
    DateOnly PlannedDate,
    string MealSlot,
    Guid? RecipeId,
    Guid? FoodId,
    string? Notes,
    DateTimeOffset? ReminderAtUtc);
