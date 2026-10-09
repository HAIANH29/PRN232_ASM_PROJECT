using LongevityDiet.Tracking.Domain.Entities;
using LongevityDiet.Tracking.Application.Models;

namespace LongevityDiet.Tracking.Application.Abstractions;

public interface ITrackingRepository
{
    Task<PagedResult<DailyTracking>> ListDailyTrackingAsync(
        Guid userId,
        DailyTrackingQuery query,
        CancellationToken cancellationToken = default);

    Task<DailyTracking?> GetDailyTrackingAsync(
        Guid userId,
        DateOnly trackingDate,
        CancellationToken cancellationToken = default);

    Task<DailyTracking> GetOrCreateDailyTrackingAsync(
        Guid userId,
        DateOnly trackingDate,
        CancellationToken cancellationToken = default);

    void RemoveDailyTracking(DailyTracking dailyTracking);

    MealTracking UpsertMealTracking(
        DailyTracking dailyTracking,
        Guid mealPlanItemId,
        bool isCompleted);

    Task<ProgressSummary> UpsertProgressSummaryAsync(
        Guid userId,
        DateOnly periodStartDate,
        DateOnly periodEndDate,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
