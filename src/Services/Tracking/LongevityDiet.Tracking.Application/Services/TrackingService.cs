namespace LongevityDiet.Tracking.Application.Services;

public sealed class TrackingService : ITrackingService
{
    public TrackingServiceStatus GetStatus()
    {
        return new TrackingServiceStatus(
            "Tracking Service",
            "TrackingDb",
            ["DailyTracking", "MealTracking", "ProgressSummary"]);
    }
}
