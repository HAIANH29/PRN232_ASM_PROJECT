using System.ComponentModel.DataAnnotations;

namespace LongevityDiet.MealPlanning.Api.Contracts;

public sealed record MealPlanListRequest : ListRequest
{
    public DateOnly? FromDate { get; init; }

    public DateOnly? ToDate { get; init; }
}

public sealed record MealPlanResponse(
    Guid Id,
    Guid UserId,
    string Name,
    DateOnly StartDate,
    DateOnly EndDate,
    IReadOnlyCollection<MealPlanItemResponse> Items,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc);

public sealed record MealPlanItemResponse(
    Guid Id,
    DateOnly PlannedDate,
    string MealSlot,
    Guid? RecipeId,
    Guid? FoodId,
    string Notes,
    DateTimeOffset ReminderAtUtc,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc);

public sealed record CreateMealPlanRequest
{
    [Required]
    [MaxLength(200)]
    public string Name { get; init; } = string.Empty;

    [Required]
    public DateOnly StartDate { get; init; }

    [Required]
    public DateOnly EndDate { get; init; }

    public IReadOnlyCollection<CreateMealPlanItemRequest> Items { get; init; } =
        Array.Empty<CreateMealPlanItemRequest>();
}

public sealed record UpdateMealPlanRequest
{
    [Required]
    [MaxLength(200)]
    public string Name { get; init; } = string.Empty;

    [Required]
    public DateOnly StartDate { get; init; }

    [Required]
    public DateOnly EndDate { get; init; }
}

public sealed record CreateMealPlanItemRequest
{
    [Required]
    public DateOnly PlannedDate { get; init; }

    [Required]
    [MaxLength(40)]
    public string MealSlot { get; init; } = string.Empty;

    public Guid? RecipeId { get; init; }

    public Guid? FoodId { get; init; }

    [MaxLength(1000)]
    public string? Notes { get; init; }

    public DateTimeOffset? ReminderAtUtc { get; init; }
}

public sealed record UpdateMealPlanItemRequest
{
    [Required]
    public DateOnly PlannedDate { get; init; }

    [Required]
    [MaxLength(40)]
    public string MealSlot { get; init; } = string.Empty;

    public Guid? RecipeId { get; init; }

    public Guid? FoodId { get; init; }

    [MaxLength(1000)]
    public string? Notes { get; init; }

    public DateTimeOffset? ReminderAtUtc { get; init; }
}
