using LongevityDiet.Tracking.Application.Models;
using LongevityDiet.Tracking.Application.Services;
using LongevityDiet.Tracking.Domain.Entities;
using LongevityDiet.Tracking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LongevityDiet.Tracking.Infrastructure.Repositories;

public sealed class UserDietTrackingRepository(TrackingDbContext dbContext) : IUserDietTrackingRepository
{
    public async Task<PagedResult<UserDietTracking>> ListAsync(
        Guid userId,
        UserDietTrackingQuery query,
        CancellationToken cancellationToken = default)
    {
        var source = dbContext.UserDietTrackings
            .AsNoTracking()
            .Where(tracking => tracking.UserId == userId);

        if (!string.IsNullOrWhiteSpace(query.Status))
        {
            var status = query.Status.ToLower();
            source = source.Where(tracking => tracking.Status.ToLower() == status);
        }

        source = source.OrderByDescending(tracking => tracking.StartDate);

        var total = await source.CountAsync(cancellationToken);
        var items = await source
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToArrayAsync(cancellationToken);

        return new PagedResult<UserDietTracking>(items, query.PageNumber, query.PageSize, total);
    }

    public Task<UserDietTracking?> GetAsync(
        Guid userId,
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return dbContext.UserDietTrackings.FirstOrDefaultAsync(
            tracking => tracking.Id == id && tracking.UserId == userId,
            cancellationToken);
    }

    public Task<bool> HasActiveAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return dbContext.UserDietTrackings.AnyAsync(
            tracking => tracking.UserId == userId &&
                tracking.Status == UserDietTrackingStatuses.Active,
            cancellationToken);
    }

    public async Task AddAsync(UserDietTracking tracking, CancellationToken cancellationToken = default)
    {
        await dbContext.UserDietTrackings.AddAsync(tracking, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
