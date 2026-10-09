namespace LongevityDiet.Tracking.Application.Models;

public sealed record DailyTrackingQuery(
    int PageNumber,
    int PageSize,
    DateOnly? FromDate,
    DateOnly? ToDate);

public sealed record DailyTrackingModel(
    Guid Id,
    Guid UserId,
    DateOnly TrackingDate,
    string Notes,
    IReadOnlyCollection<MealTrackingModel> Meals,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc);

public sealed record MealTrackingModel(
    Guid Id,
    Guid DailyTrackingId,
    Guid MealPlanItemId,
    bool IsCompleted,
    DateTimeOffset? CompletedAtUtc,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc);

public sealed record ProgressSummaryModel(
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

public sealed record UpsertDailyTrackingCommand(
    Guid UserId,
    DateOnly TrackingDate,
    string? Notes);

public sealed record SetMealCompletionCommand(
    Guid UserId,
    string UserEmail,
    DateOnly TrackingDate,
    Guid MealPlanItemId,
    bool IsCompleted);
