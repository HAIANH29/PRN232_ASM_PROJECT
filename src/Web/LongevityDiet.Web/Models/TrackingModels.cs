using System.ComponentModel.DataAnnotations;

namespace LongevityDiet.Web.Models;

public sealed record DailyTrackingResponse(
    Guid Id,
    Guid UserId,
    DateOnly TrackingDate,
    string Notes,
    IReadOnlyCollection<MealTrackingResponse> Meals,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc);

public sealed record MealTrackingResponse(
    Guid Id,
    Guid DailyTrackingId,
    Guid MealPlanItemId,
    bool IsCompleted,
    DateTimeOffset? CompletedAtUtc,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc);

public sealed record ProgressSummaryResponse(
    Guid Id,
    Guid UserId,
    DateOnly PeriodStartDate,
    DateOnly PeriodEndDate,
    int PlannedMeals,
    int CompletedMeals,
    decimal CompletionRate,
    DateTimeOffset CalculatedAtUtc,
    Guid? LastNotificationId,
    DateTimeOffset? LastNotificationSentAtUtc);

public sealed class TrackingIndexViewModel
{
    public TrackingForm Form { get; set; } = new()
    {
        TrackingDate = DateOnly.FromDateTime(DateTime.Today),
        PeriodStartDate = DateOnly.FromDateTime(DateTime.Today),
        PeriodEndDate = DateOnly.FromDateTime(DateTime.Today)
    };

    public DailyTrackingResponse? DailyTracking { get; set; }

    public ProgressSummaryResponse? ProgressSummary { get; set; }

    public IReadOnlyCollection<MealPlanResponse> MealPlans { get; set; } = Array.Empty<MealPlanResponse>();

    public IReadOnlyCollection<FoodResponse> Foods { get; set; } = Array.Empty<FoodResponse>();

    public IReadOnlyCollection<RecipeResponse> Recipes { get; set; } = Array.Empty<RecipeResponse>();
}

public sealed class TrackingForm
{
    [Required]
    public DateOnly TrackingDate { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }

    public Guid? MealPlanItemId { get; set; }

    public bool IsCompleted { get; set; } = true;

    [Required]
    public DateOnly PeriodStartDate { get; set; }

    [Required]
    public DateOnly PeriodEndDate { get; set; }
}
