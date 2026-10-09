using LongevityDiet.Tracking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LongevityDiet.Tracking.Infrastructure.Seed;

public sealed class TrackingDatabaseInitializer(
    TrackingDbContext dbContext) : ITrackingDatabaseInitializer
{
    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.Database.MigrateAsync(cancellationToken);
    }
}
