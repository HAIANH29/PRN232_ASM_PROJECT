namespace LongevityDiet.MealPlanning.Domain.Entities;

public sealed class MealPlan
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Name { get; set; } = string.Empty;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<MealPlanItem> Items { get; } = new List<MealPlanItem>();
}
