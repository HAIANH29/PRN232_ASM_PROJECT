namespace LongevityDiet.Tracking.Domain.Entities;

// One user following a diet regimen and/or a meal plan over a period. Daily tracking and
// progress summaries that fall in the period are linked to it.
public sealed class UserDietTracking
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    // Cross-service reference (Diet Knowledge). No foreign key.
    public Guid? DietRegimenId { get; set; }

    // Cross-service reference (Meal Planning). No foreign key.
    public Guid? MealPlanId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string Status { get; set; } = "Active";

    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? UpdatedAtUtc { get; set; }

    public ICollection<DailyTracking> DailyTrackings { get; } = new List<DailyTracking>();

    public ICollection<ProgressSummary> ProgressSummaries { get; } = new List<ProgressSummary>();
}
