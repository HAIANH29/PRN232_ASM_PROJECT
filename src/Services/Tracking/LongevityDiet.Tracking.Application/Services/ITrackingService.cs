using LongevityDiet.Tracking.Application.Models;

namespace LongevityDiet.Tracking.Application.Services;

public interface ITrackingService
{
    TrackingServiceStatus GetStatus();

    Task<PagedResult<DailyTrackingModel>> ListDailyTrackingAsync(
        Guid userId,
        DailyTrackingQuery query,
        CancellationToken cancellationToken = default);

    Task<DailyTrackingModel> GetDailyTrackingAsync(
        Guid userId,
        DateOnly trackingDate,
        CancellationToken cancellationToken = default);

    Task<DailyTrackingModel> UpsertDailyTrackingAsync(
        UpsertDailyTrackingCommand command,
        CancellationToken cancellationToken = default);

    Task DeleteDailyTrackingAsync(
        Guid userId,
        DateOnly trackingDate,
        CancellationToken cancellationToken = default);

    Task<DailyTrackingModel> SetMealCompletionAsync(
        SetMealCompletionCommand command,
        CancellationToken cancellationToken = default);

    Task<ProgressSummaryModel> GetProgressSummaryAsync(
        Guid userId,
        DateOnly periodStartDate,
        DateOnly periodEndDate,
        CancellationToken cancellationToken = default);
}
