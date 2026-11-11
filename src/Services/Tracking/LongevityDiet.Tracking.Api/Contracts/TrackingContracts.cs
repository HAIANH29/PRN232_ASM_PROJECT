using System.ComponentModel.DataAnnotations;

namespace LongevityDiet.Tracking.Api.Contracts;

public sealed record DailyTrackingListRequest : ListRequest
{
    public DateOnly? FromDate { get; init; }

    public DateOnly? ToDate { get; init; }
}

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

public sealed record UpsertDailyTrackingRequest
{
    [MaxLength(1000)]
    public string? Notes { get; init; }
}

public sealed record SetMealCompletionRequest
{
    [Required]
    public bool IsCompleted { get; init; }

    // Optional: how many meals the user has planned for this date. Lets progress count planned
    // meals that were never ticked off.
    [Range(0, 50)]
    public int? PlannedMealCount { get; init; }
}

public sealed record ProgressSummaryRequest
{
    [Required]
    public DateOnly PeriodStartDate { get; init; }

    [Required]
    public DateOnly PeriodEndDate { get; init; }
}
