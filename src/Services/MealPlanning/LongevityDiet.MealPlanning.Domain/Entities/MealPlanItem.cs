namespace LongevityDiet.MealPlanning.Domain.Entities;

public sealed class MealPlanItem
{
    public Guid Id { get; set; }

    public Guid MealPlanId { get; set; }

    public MealPlan? MealPlan { get; set; }

    public DateOnly PlannedDate { get; set; }

    public string MealSlot { get; set; } = string.Empty;

    public Guid? RecipeId { get; set; }

    public Guid? FoodId { get; set; }

    public string Notes { get; set; } = string.Empty;

    public DateTimeOffset ReminderAtUtc { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? UpdatedAtUtc { get; set; }
}
