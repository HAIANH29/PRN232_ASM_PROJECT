using LongevityDiet.Tracking.Application.Abstractions;
using LongevityDiet.Tracking.Domain.Entities;
using LongevityDiet.Tracking.Infrastructure.Persistence;

namespace LongevityDiet.Tracking.Infrastructure.Repositories;

public sealed class TrackingRepository(TrackingDbContext dbContext) : ITrackingRepository
{
    public IQueryable<DailyTracking> QueryDailyTracking()
    {
        return dbContext.DailyTrackings;
    }
}
