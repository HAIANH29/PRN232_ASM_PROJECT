namespace LongevityDiet.Tracking.Domain.Entities;

public sealed class DailyTracking
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid? UserDietTrackingId { get; set; }

    public UserDietTracking? UserDietTracking { get; set; }

    public DateOnly TrackingDate { get; set; }

    public string Notes { get; set; } = string.Empty;

    // Meals planned for this day as reported by the client (the Tracking service cannot read
    // MealPlanningDb). Progress uses the larger of this and the number of tracked meals.
    public int? PlannedMealCount { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? UpdatedAtUtc { get; set; }

    public ICollection<MealTracking> Meals { get; } = new List<MealTracking>();
}
