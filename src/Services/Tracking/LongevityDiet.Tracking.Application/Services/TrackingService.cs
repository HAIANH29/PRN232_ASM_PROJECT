using System.ComponentModel.DataAnnotations;
using LongevityDiet.Tracking.Application.Abstractions;
using LongevityDiet.Tracking.Application.Models;
using LongevityDiet.Tracking.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace LongevityDiet.Tracking.Application.Services;

public sealed class TrackingService(
    ITrackingRepository repository,
    INotificationClient notificationClient,
    ILogger<TrackingService> logger) : ITrackingService
{
    public TrackingServiceStatus GetStatus()
    {
        return new TrackingServiceStatus(
            "Tracking Service",
            "TrackingDb",
            ["DailyTracking", "MealTracking", "ProgressSummary"]);
    }

    public async Task<PagedResult<DailyTrackingModel>> ListDailyTrackingAsync(
        Guid userId,
        DailyTrackingQuery query,
        CancellationToken cancellationToken = default)
    {
        EnsureUser(userId);
        var normalized = Normalize(query);
        var result = await repository.ListDailyTrackingAsync(
            userId,
            normalized,
            cancellationToken);

        return new PagedResult<DailyTrackingModel>(
            result.Items.Select(MapDailyTracking).ToArray(),
            result.PageNumber,
            result.PageSize,
            result.TotalCount);
    }

    public async Task<DailyTrackingModel> GetDailyTrackingAsync(
        Guid userId,
        DateOnly trackingDate,
        CancellationToken cancellationToken = default)
    {
        EnsureUser(userId);
        var dailyTracking = await repository.GetDailyTrackingAsync(
                userId,
                trackingDate,
                cancellationToken)
            ?? throw new KeyNotFoundException("Daily tracking was not found.");

        return MapDailyTracking(dailyTracking);
    }

    public async Task<DailyTrackingModel> UpsertDailyTrackingAsync(
        UpsertDailyTrackingCommand command,
        CancellationToken cancellationToken = default)
    {
        EnsureUser(command.UserId);
        var dailyTracking = await repository.GetOrCreateDailyTrackingAsync(
            command.UserId,
            command.TrackingDate,
            cancellationToken);

        dailyTracking.Notes = CleanOptional(command.Notes);
        dailyTracking.UpdatedAtUtc = DateTimeOffset.UtcNow;

        await repository.SaveChangesAsync(cancellationToken);
        return MapDailyTracking(dailyTracking);
    }

    public async Task DeleteDailyTrackingAsync(
        Guid userId,
        DateOnly trackingDate,
        CancellationToken cancellationToken = default)
    {
        EnsureUser(userId);
        var dailyTracking = await repository.GetDailyTrackingAsync(
                userId,
                trackingDate,
                cancellationToken)
            ?? throw new KeyNotFoundException("Daily tracking was not found.");

        repository.RemoveDailyTracking(dailyTracking);
        await repository.SaveChangesAsync(cancellationToken);
        await repository.UpsertProgressSummaryAsync(
            userId,
            trackingDate,
            trackingDate,
            cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }

    public async Task<DailyTrackingModel> SetMealCompletionAsync(
        SetMealCompletionCommand command,
        CancellationToken cancellationToken = default)
    {
        EnsureUser(command.UserId);
        var userEmail = CleanRequired(command.UserEmail, "Authenticated user email is required.");
        if (command.MealPlanItemId == Guid.Empty)
        {
            throw new ValidationException("Meal plan item id is required.");
        }

        var dailyTracking = await repository.GetOrCreateDailyTrackingAsync(
            command.UserId,
            command.TrackingDate,
            cancellationToken);

        repository.UpsertMealTracking(
            dailyTracking,
            command.MealPlanItemId,
            command.IsCompleted);

        dailyTracking.UpdatedAtUtc = DateTimeOffset.UtcNow;
        await repository.SaveChangesAsync(cancellationToken);

        var summary = await repository.UpsertProgressSummaryAsync(
            command.UserId,
            command.TrackingDate,
            command.TrackingDate,
            cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        await NotifyIfDailyProgressCompletedAsync(
            summary,
            userEmail,
            cancellationToken);

        return MapDailyTracking(dailyTracking);
    }

    public async Task<ProgressSummaryModel> GetProgressSummaryAsync(
        Guid userId,
        DateOnly periodStartDate,
        DateOnly periodEndDate,
        CancellationToken cancellationToken = default)
    {
        EnsureUser(userId);
        ValidatePeriod(periodStartDate, periodEndDate);

        var summary = await repository.UpsertProgressSummaryAsync(
            userId,
            periodStartDate,
            periodEndDate,
            cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return MapProgressSummary(summary);
    }

    private async Task NotifyIfDailyProgressCompletedAsync(
        ProgressSummary summary,
        string userEmail,
        CancellationToken cancellationToken)
    {
        if (summary.PeriodStartDate != summary.PeriodEndDate ||
            summary.PlannedMeals == 0 ||
            summary.CompletedMeals != summary.PlannedMeals ||
            summary.LastNotificationSentAtUtc.HasValue)
        {
            return;
        }

        try
        {
            var notificationId = await notificationClient.RequestProgressNotificationAsync(
                summary.UserId,
                userEmail,
                "Daily meal progress completed",
                $"You completed {summary.CompletedMeals} of {summary.PlannedMeals} tracked meals for {summary.PeriodStartDate:yyyy-MM-dd}.",
                summary.Id,
                cancellationToken);

            summary.LastNotificationId = notificationId;
            summary.LastNotificationSentAtUtc = DateTimeOffset.UtcNow;
            await repository.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            logger.LogWarning(
                exception,
                "Progress notification could not be sent for summary {ProgressSummaryId}.",
                summary.Id);
        }
    }

    private static DailyTrackingModel MapDailyTracking(DailyTracking dailyTracking)
    {
        return new DailyTrackingModel(
            dailyTracking.Id,
            dailyTracking.UserId,
            dailyTracking.TrackingDate,
            dailyTracking.Notes,
            dailyTracking.Meals
                .OrderBy(meal => meal.CreatedAtUtc)
                .Select(MapMealTracking)
                .ToArray(),
            dailyTracking.CreatedAtUtc,
            dailyTracking.UpdatedAtUtc);
    }

    private static MealTrackingModel MapMealTracking(MealTracking mealTracking)
    {
        return new MealTrackingModel(
            mealTracking.Id,
            mealTracking.DailyTrackingId,
            mealTracking.MealPlanItemId,
            mealTracking.IsCompleted,
            mealTracking.CompletedAtUtc,
            mealTracking.CreatedAtUtc,
            mealTracking.UpdatedAtUtc);
    }

    private static ProgressSummaryModel MapProgressSummary(ProgressSummary summary)
    {
        var completionRate = summary.PlannedMeals == 0
            ? 0
            : Math.Round((decimal)summary.CompletedMeals / summary.PlannedMeals, 4);

        return new ProgressSummaryModel(
            summary.Id,
            summary.UserId,
            summary.PeriodStartDate,
            summary.PeriodEndDate,
            summary.PlannedMeals,
            summary.CompletedMeals,
            completionRate,
            summary.CalculatedAtUtc,
            summary.LastNotificationId,
            summary.LastNotificationSentAtUtc);
    }

    private static DailyTrackingQuery Normalize(DailyTrackingQuery query)
    {
        if (query.FromDate.HasValue &&
            query.ToDate.HasValue &&
            query.FromDate.Value > query.ToDate.Value)
        {
            throw new ValidationException("FromDate cannot be after ToDate.");
        }

        return query with
        {
            PageNumber = Math.Max(1, query.PageNumber),
            PageSize = Math.Clamp(query.PageSize, 1, 100)
        };
    }

    private static void ValidatePeriod(DateOnly periodStartDate, DateOnly periodEndDate)
    {
        if (periodStartDate > periodEndDate)
        {
            throw new ValidationException("Period start date cannot be after period end date.");
        }
    }

    private static void EnsureUser(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new UnauthorizedAccessException();
        }
    }

    private static string CleanRequired(string value, string message)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ValidationException(message);
        }

        return value.Trim();
    }

    private static string CleanOptional(string? value)
    {
        return value?.Trim() ?? string.Empty;
    }
}
