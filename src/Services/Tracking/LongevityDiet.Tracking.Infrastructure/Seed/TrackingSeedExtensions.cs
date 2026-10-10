using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LongevityDiet.Tracking.Infrastructure.Seed;

public static class TrackingSeedExtensions
{
    public static async Task InitializeTrackingAsync(this IServiceProvider serviceProvider)
    {
        const int maxAttempts = 6;

        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var initializer = scope.ServiceProvider.GetRequiredService<ITrackingDatabaseInitializer>();
                await initializer.InitializeAsync();
                return;
            }
            catch (Exception exception) when (attempt < maxAttempts)
            {
                var logger = serviceProvider
                    .GetRequiredService<ILoggerFactory>()
                    .CreateLogger("TrackingSeed");

                logger.LogWarning(
                    exception,
                    "Tracking database initialization attempt {Attempt} of {MaxAttempts} failed. Retrying in 3 seconds.",
                    attempt,
                    maxAttempts);

                await Task.Delay(TimeSpan.FromSeconds(3));
            }
        }

        using var finalScope = serviceProvider.CreateScope();
        var finalInitializer = finalScope.ServiceProvider.GetRequiredService<ITrackingDatabaseInitializer>();
        await finalInitializer.InitializeAsync();
    }
}
