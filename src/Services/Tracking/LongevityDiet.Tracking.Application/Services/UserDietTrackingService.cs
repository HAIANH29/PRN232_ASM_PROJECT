using System.ComponentModel.DataAnnotations;
using LongevityDiet.Tracking.Application.Models;
using LongevityDiet.Tracking.Domain.Entities;

namespace LongevityDiet.Tracking.Application.Services;

public sealed class UserDietTrackingService(IUserDietTrackingRepository repository) : IUserDietTrackingService
{
    public async Task<PagedResult<UserDietTrackingModel>> ListAsync(
        Guid userId,
        UserDietTrackingQuery query,
        CancellationToken cancellationToken = default)
    {
        EnsureUser(userId);

        var status = string.IsNullOrWhiteSpace(query.Status) ? null : query.Status.Trim();
        if (status is not null && !IsKnownStatus(status))
        {
            throw new ValidationException("Status must be Active, Completed, or Cancelled.");
        }

        var normalized = query with
        {
            PageNumber = Math.Max(1, query.PageNumber),
            PageSize = Math.Clamp(query.PageSize, 1, 100),
            Status = status
        };

        var result = await repository.ListAsync(userId, normalized, cancellationToken);
        return new PagedResult<UserDietTrackingModel>(
            result.Items.Select(Map).ToArray(),
            result.PageNumber,
            result.PageSize,
            result.TotalCount);
    }

    public async Task<UserDietTrackingModel> GetAsync(
        Guid userId,
        Guid id,
        CancellationToken cancellationToken = default)
    {
        EnsureUser(userId);

        // Another user's tracking looks the same as a missing one.
        var tracking = await repository.GetAsync(userId, id, cancellationToken)
            ?? throw new KeyNotFoundException("Diet tracking was not found.");

        return Map(tracking);
    }

    public async Task<UserDietTrackingModel> StartAsync(
        StartUserDietTrackingCommand command,
        CancellationToken cancellationToken = default)
    {
        EnsureUser(command.UserId);

        if (command.DietRegimenId is null && command.MealPlanId is null)
        {
            throw new ValidationException("Choose a diet regimen, a meal plan, or both to track.");
        }

        if (command.DietRegimenId == Guid.Empty || command.MealPlanId == Guid.Empty)
        {
            throw new ValidationException("Diet regimen and meal plan ids must be valid.");
        }

        if (command.EndDate is not null && command.EndDate < command.StartDate)
        {
            throw new ValidationException("EndDate cannot be before StartDate.");
        }

        if (await repository.HasActiveAsync(command.UserId, cancellationToken))
        {
            throw new InvalidOperationException(
                "You already have an active diet tracking. Finish or cancel it first.");
        }

        var tracking = new UserDietTracking
        {
            Id = Guid.NewGuid(),
            UserId = command.UserId,
            DietRegimenId = command.DietRegimenId,
            MealPlanId = command.MealPlanId,
            StartDate = command.StartDate,
            EndDate = command.EndDate,
            Status = UserDietTrackingStatuses.Active,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };

        await repository.AddAsync(tracking, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        return Map(tracking);
    }

    public async Task<UserDietTrackingModel> FinishAsync(
        Guid userId,
        Guid id,
        string status,
        CancellationToken cancellationToken = default)
    {
        EnsureUser(userId);

        if (!UserDietTrackingStatuses.IsFinal(status))
        {
            throw new ValidationException("Status must be Completed or Cancelled.");
        }

        var tracking = await repository.GetAsync(userId, id, cancellationToken)
            ?? throw new KeyNotFoundException("Diet tracking was not found.");

        if (tracking.Status != UserDietTrackingStatuses.Active)
        {
            throw new InvalidOperationException("Only an active diet tracking can be finished.");
        }

        tracking.Status = UserDietTrackingStatuses.Normalize(status);
        tracking.UpdatedAtUtc = DateTimeOffset.UtcNow;
        await repository.SaveChangesAsync(cancellationToken);
        return Map(tracking);
    }

    private static bool IsKnownStatus(string status)
    {
        return string.Equals(status, UserDietTrackingStatuses.Active, StringComparison.OrdinalIgnoreCase) ||
            UserDietTrackingStatuses.IsFinal(status);
    }

    private static void EnsureUser(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ValidationException("A valid user id is required.");
        }
    }

    private static UserDietTrackingModel Map(UserDietTracking tracking)
    {
        return new UserDietTrackingModel(
            tracking.Id,
            tracking.UserId,
            tracking.DietRegimenId,
            tracking.MealPlanId,
            tracking.StartDate,
            tracking.EndDate,
            tracking.Status,
            tracking.CreatedAtUtc,
            tracking.UpdatedAtUtc);
    }
}
