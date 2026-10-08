using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LongevityDiet.Identity.Infrastructure.Seed;

public static class IdentitySeedExtensions
{
    public static async Task SeedIdentityAsync(this IServiceProvider serviceProvider)
    {
        const int maxAttempts = 10;

        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var seeder = scope.ServiceProvider.GetRequiredService<IIdentitySeeder>();
                await seeder.SeedAsync();
                return;
            }
            catch (Exception exception) when (attempt < maxAttempts)
            {
                var logger = serviceProvider
                    .GetRequiredService<ILoggerFactory>()
                    .CreateLogger("IdentitySeed");

                logger.LogWarning(
                    exception,
                    "Identity database seed attempt {Attempt} of {MaxAttempts} failed. Retrying in 2 seconds.",
                    attempt,
                    maxAttempts);

                await Task.Delay(TimeSpan.FromSeconds(2));
            }
        }
    }
}
