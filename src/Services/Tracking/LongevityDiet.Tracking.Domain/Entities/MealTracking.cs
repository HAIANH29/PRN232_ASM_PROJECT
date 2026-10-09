namespace LongevityDiet.Tracking.Domain.Entities;

public sealed class MealTracking
{
    public Guid Id { get; set; }

    public Guid DailyTrackingId { get; set; }

    public DailyTracking? DailyTracking { get; set; }

    public Guid MealPlanItemId { get; set; }

    public bool IsCompleted { get; set; }

    public DateTimeOffset? CompletedAtUtc { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? UpdatedAtUtc { get; set; }
}
