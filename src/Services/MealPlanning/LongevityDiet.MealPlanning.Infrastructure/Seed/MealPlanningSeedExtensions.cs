using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LongevityDiet.MealPlanning.Infrastructure.Seed;

public static class MealPlanningSeedExtensions
{
    public static async Task InitializeMealPlanningAsync(this IServiceProvider serviceProvider)
    {
        const int maxAttempts = 10;

        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var initializer = scope.ServiceProvider
                    .GetRequiredService<IMealPlanningDatabaseInitializer>();
                await initializer.InitializeAsync();
                return;
            }
            catch (Exception exception) when (attempt < maxAttempts)
            {
                var logger = serviceProvider
                    .GetRequiredService<ILoggerFactory>()
                    .CreateLogger("MealPlanningSeed");

                logger.LogWarning(
                    exception,
                    "Meal Planning database initialization attempt {Attempt} of {MaxAttempts} failed. Retrying in 2 seconds.",
                    attempt,
                    maxAttempts);

                await Task.Delay(TimeSpan.FromSeconds(2));
            }
        }
    }
}
