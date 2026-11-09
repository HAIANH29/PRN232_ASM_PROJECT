using LongevityDiet.Tracking.Application.Models;
using LongevityDiet.Tracking.Domain.Entities;

namespace LongevityDiet.Tracking.Application.Services;

public static class UserDietTrackingStatuses
{
    public const string Active = "Active";
    public const string Completed = "Completed";
    public const string Cancelled = "Cancelled";

    public static bool IsFinal(string? status)
    {
        return string.Equals(status, Completed, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(status, Cancelled, StringComparison.OrdinalIgnoreCase);
    }

    public static string Normalize(string status)
    {
        return string.Equals(status, Completed, StringComparison.OrdinalIgnoreCase)
            ? Completed
            : Cancelled;
    }
}

public sealed record UserDietTrackingModel(
    Guid Id,
    Guid UserId,
    Guid? DietRegimenId,
    Guid? MealPlanId,
    DateOnly StartDate,
    DateOnly? EndDate,
    string Status,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc);

public sealed record StartUserDietTrackingCommand(
    Guid UserId,
    Guid? DietRegimenId,
    Guid? MealPlanId,
    DateOnly StartDate,
    DateOnly? EndDate);

public sealed record UserDietTrackingQuery(
    int PageNumber,
    int PageSize,
    string? Status);

public interface IUserDietTrackingRepository
{
    Task<PagedResult<UserDietTracking>> ListAsync(
        Guid userId,
        UserDietTrackingQuery query,
        CancellationToken cancellationToken = default);

    Task<UserDietTracking?> GetAsync(
        Guid userId,
        Guid id,
        CancellationToken cancellationToken = default);

    Task<bool> HasActiveAsync(Guid userId, CancellationToken cancellationToken = default);

    Task AddAsync(UserDietTracking tracking, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

public interface IUserDietTrackingService
{
    Task<PagedResult<UserDietTrackingModel>> ListAsync(
        Guid userId,
        UserDietTrackingQuery query,
        CancellationToken cancellationToken = default);

    Task<UserDietTrackingModel> GetAsync(
        Guid userId,
        Guid id,
        CancellationToken cancellationToken = default);

    Task<UserDietTrackingModel> StartAsync(
        StartUserDietTrackingCommand command,
        CancellationToken cancellationToken = default);

    Task<UserDietTrackingModel> FinishAsync(
        Guid userId,
        Guid id,
        string status,
        CancellationToken cancellationToken = default);
}
