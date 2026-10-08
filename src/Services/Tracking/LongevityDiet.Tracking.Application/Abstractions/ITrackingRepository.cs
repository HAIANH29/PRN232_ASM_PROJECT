using LongevityDiet.Tracking.Domain.Entities;

namespace LongevityDiet.Tracking.Application.Abstractions;

public interface ITrackingRepository
{
    IQueryable<DailyTracking> QueryDailyTracking();
}
