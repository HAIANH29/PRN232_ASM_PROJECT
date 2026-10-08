namespace LongevityDiet.Tracking.Domain.Entities;

public sealed class DailyTracking
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public DateOnly TrackingDate { get; set; }

    public string Notes { get; set; } = string.Empty;

    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<MealTracking> Meals { get; } = new List<MealTracking>();
}
