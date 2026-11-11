using LongevityDiet.Tracking.Application.Abstractions;
using LongevityDiet.Tracking.Application.Models;
using LongevityDiet.Tracking.Domain.Entities;
using LongevityDiet.Tracking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LongevityDiet.Tracking.Infrastructure.Repositories;

public sealed class TrackingRepository(TrackingDbContext dbContext) : ITrackingRepository
{
    public async Task<PagedResult<DailyTracking>> ListDailyTrackingAsync(
        Guid userId,
        DailyTrackingQuery query,
        CancellationToken cancellationToken = default)
    {
        var source = dbContext.DailyTrackings
            .AsNoTracking()
            .Include(tracking => tracking.Meals)
            .Where(tracking => tracking.UserId == userId);

        if (query.FromDate.HasValue)
        {
            source = source.Where(tracking => tracking.TrackingDate >= query.FromDate.Value);
        }

        if (query.ToDate.HasValue)
        {
            source = source.Where(tracking => tracking.TrackingDate <= query.ToDate.Value);
        }

        source = source.OrderByDescending(tracking => tracking.TrackingDate);

        return await ToPagedResultAsync(
            source,
            query.PageNumber,
            query.PageSize,
            cancellationToken);
    }

    public Task<DailyTracking?> GetDailyTrackingAsync(
        Guid userId,
        DateOnly trackingDate,
        CancellationToken cancellationToken = default)
    {
        return dbContext.DailyTrackings
            .Include(tracking => tracking.Meals)
            .FirstOrDefaultAsync(
                tracking => tracking.UserId == userId &&
                    tracking.TrackingDate == trackingDate,
                cancellationToken);
    }

    public async Task<DailyTracking> GetOrCreateDailyTrackingAsync(
        Guid userId,
        DateOnly trackingDate,
        CancellationToken cancellationToken = default)
    {
        var existing = await GetDailyTrackingAsync(userId, trackingDate, cancellationToken);
        if (existing is not null)
        {
            existing.UserDietTrackingId ??= await FindActiveUserDietTrackingIdAsync(
                userId,
                trackingDate,
                cancellationToken);
            return existing;
        }

        var dailyTracking = new DailyTracking
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            UserDietTrackingId = await FindActiveUserDietTrackingIdAsync(userId, trackingDate, cancellationToken),
            TrackingDate = trackingDate,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };

        await dbContext.DailyTrackings.AddAsync(dailyTracking, cancellationToken);
        return dailyTracking;
    }

    public void RemoveDailyTracking(DailyTracking dailyTracking)
    {
        dbContext.DailyTrackings.Remove(dailyTracking);
    }

    public MealTracking UpsertMealTracking(
        DailyTracking dailyTracking,
        Guid mealPlanItemId,
        bool isCompleted)
    {
        var now = DateTimeOffset.UtcNow;
        var mealTracking = dailyTracking.Meals.FirstOrDefault(
            meal => meal.MealPlanItemId == mealPlanItemId);

        if (mealTracking is null)
        {
            mealTracking = new MealTracking
            {
                Id = Guid.NewGuid(),
                DailyTrackingId = dailyTracking.Id,
                MealPlanItemId = mealPlanItemId,
                CreatedAtUtc = now
            };

            dailyTracking.Meals.Add(mealTracking);
            dbContext.MealTrackings.Add(mealTracking);
        }

        mealTracking.IsCompleted = isCompleted;
        mealTracking.CompletedAtUtc = isCompleted ? now : null;
        mealTracking.UpdatedAtUtc = now;

        return mealTracking;
    }

    public async Task<ProgressSummary> UpsertProgressSummaryAsync(
        Guid userId,
        DateOnly periodStartDate,
        DateOnly periodEndDate,
        CancellationToken cancellationToken = default)
    {
        var summary = await dbContext.ProgressSummaries.FirstOrDefaultAsync(
            existing => existing.UserId == userId &&
                existing.PeriodStartDate == periodStartDate &&
                existing.PeriodEndDate == periodEndDate,
            cancellationToken);

        if (summary is null)
        {
            summary = new ProgressSummary
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                PeriodStartDate = periodStartDate,
                PeriodEndDate = periodEndDate
            };

            await dbContext.ProgressSummaries.AddAsync(summary, cancellationToken);
        }

        summary.UserDietTrackingId ??= await FindActiveUserDietTrackingIdAsync(
            userId,
            periodEndDate,
            cancellationToken);

        // A day counts every meal the client said was planned, even if never ticked off; days
        // without that information fall back to the meals that were tracked.
        var days = await dbContext.DailyTrackings
            .Where(day => day.UserId == userId &&
                day.TrackingDate >= periodStartDate &&
                day.TrackingDate <= periodEndDate)
            .Select(day => new
            {
                day.PlannedMealCount,
                Tracked = day.Meals.Count(),
                Completed = day.Meals.Count(meal => meal.IsCompleted)
            })
            .ToListAsync(cancellationToken);

        summary.PlannedMeals = days.Sum(day => Math.Max(day.PlannedMealCount ?? 0, day.Tracked));
        summary.CompletedMeals = days.Sum(day => day.Completed);
        summary.CalculatedAtUtc = DateTimeOffset.UtcNow;

        return summary;
    }

    // The newest Active tracking that covers the date; null when the user has none.
    private Task<Guid?> FindActiveUserDietTrackingIdAsync(
        Guid userId,
        DateOnly date,
        CancellationToken cancellationToken)
    {
        return dbContext.UserDietTrackings
            .Where(tracking => tracking.UserId == userId &&
                tracking.Status == "Active" &&
                tracking.StartDate <= date &&
                (tracking.EndDate == null || tracking.EndDate >= date))
            .OrderByDescending(tracking => tracking.StartDate)
            .Select(tracking => (Guid?)tracking.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        IQueryable<T> source,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var totalCount = await source.CountAsync(cancellationToken);
        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync(cancellationToken);

        return new PagedResult<T>(items, pageNumber, pageSize, totalCount);
    }
}
