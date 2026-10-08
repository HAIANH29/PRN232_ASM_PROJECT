using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LongevityDiet.DietKnowledge.Infrastructure.Seed;

public static class DietKnowledgeSeedExtensions
{
    public static async Task SeedDietKnowledgeAsync(this IServiceProvider serviceProvider)
    {
        const int maxAttempts = 10;

        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var seeder = scope.ServiceProvider.GetRequiredService<IDietKnowledgeSeeder>();
                await seeder.SeedAsync();
                return;
            }
            catch (Exception exception) when (attempt < maxAttempts)
            {
                var logger = serviceProvider
                    .GetRequiredService<ILoggerFactory>()
                    .CreateLogger("DietKnowledgeSeed");

                logger.LogWarning(
                    exception,
                    "Diet Knowledge database seed attempt {Attempt} of {MaxAttempts} failed. Retrying in 2 seconds.",
                    attempt,
                    maxAttempts);

                await Task.Delay(TimeSpan.FromSeconds(2));
            }
        }
    }
}
