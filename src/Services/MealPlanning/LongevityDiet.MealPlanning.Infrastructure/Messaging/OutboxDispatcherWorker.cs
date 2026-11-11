using LongevityDiet.MealPlanning.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LongevityDiet.MealPlanning.Infrastructure.Messaging;

public sealed class OutboxDispatcherWorker(
    IServiceScopeFactory scopeFactory,
    IConfiguration configuration,
    ILogger<OutboxDispatcherWorker> logger) : BackgroundService
{
    private const int BatchSize = 50;
    private static readonly TimeSpan Retention = TimeSpan.FromDays(1);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var pollSeconds = int.TryParse(configuration["Outbox:PollSeconds"], out var parsed) && parsed > 0
            ? parsed
            : 2;
        var lastCleanup = DateTimeOffset.MinValue;

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var dispatcher = scope.ServiceProvider.GetRequiredService<OutboxDispatcher>();
                while (await dispatcher.DispatchPendingAsync(BatchSize, stoppingToken) == BatchSize)
                {
                }

                if (DateTimeOffset.UtcNow - lastCleanup > TimeSpan.FromHours(1))
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<MealPlanningDbContext>();
                    var cutoff = DateTimeOffset.UtcNow - Retention;
                    await dbContext.OutboxMessages
                        .Where(message => message.ProcessedAtUtc != null && message.ProcessedAtUtc < cutoff)
                        .ExecuteDeleteAsync(stoppingToken);
                    lastCleanup = DateTimeOffset.UtcNow;
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception exception)
            {
                logger.LogWarning(exception, "Outbox dispatch loop failed; retrying.");
            }

            await Task.Delay(TimeSpan.FromSeconds(pollSeconds), stoppingToken);
        }
    }
}
