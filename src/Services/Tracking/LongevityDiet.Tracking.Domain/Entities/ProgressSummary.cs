namespace LongevityDiet.Tracking.Domain.Entities;

public sealed class ProgressSummary
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public DateOnly PeriodStartDate { get; set; }

    public DateOnly PeriodEndDate { get; set; }

    public int PlannedMeals { get; set; }

    public int CompletedMeals { get; set; }

    public DateTimeOffset CalculatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}
