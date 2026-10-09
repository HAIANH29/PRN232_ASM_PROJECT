namespace LongevityDiet.Tracking.Infrastructure.Seed;

public interface ITrackingDatabaseInitializer
{
    Task InitializeAsync(CancellationToken cancellationToken = default);
}
