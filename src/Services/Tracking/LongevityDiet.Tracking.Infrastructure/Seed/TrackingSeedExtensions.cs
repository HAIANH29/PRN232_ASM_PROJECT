using Microsoft.Extensions.DependencyInjection;

namespace LongevityDiet.Tracking.Infrastructure.Seed;

public static class TrackingSeedExtensions
{
    public static async Task InitializeTrackingAsync(this IServiceProvider serviceProvider)
    {
        const int maxAttempts = 6;
        using var scope = serviceProvider.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<ITrackingDatabaseInitializer>();

        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                await initializer.InitializeAsync();
                return;
            }
            catch when (attempt < maxAttempts)
            {
                await Task.Delay(TimeSpan.FromSeconds(3));
            }
        }

        await initializer.InitializeAsync();
    }
}
