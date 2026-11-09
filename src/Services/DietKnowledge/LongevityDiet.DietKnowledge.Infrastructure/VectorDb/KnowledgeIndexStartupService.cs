using LongevityDiet.DietKnowledge.Application.Abstractions;
using LongevityDiet.DietKnowledge.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LongevityDiet.DietKnowledge.Infrastructure.VectorDb;

// After startup, creates the VectorDb schema and runs an incremental index. Unchanged items are
// skipped, so restarts are cheap. Failures are logged and never stop the service.
public sealed class KnowledgeIndexStartupService(
    IServiceScopeFactory scopeFactory,
    IOptions<KnowledgeIndexOptions> options,
    ILogger<KnowledgeIndexStartupService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        const int maxAttempts = 6;

        for (var attempt = 1; attempt <= maxAttempts && !stoppingToken.IsCancellationRequested; attempt++)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var store = scope.ServiceProvider.GetRequiredService<IKnowledgeVectorStore>();
                if (!store.IsConfigured)
                {
                    logger.LogInformation("VectorDb is not configured; knowledge indexing is skipped.");
                    return;
                }

                await store.EnsureCreatedAsync(stoppingToken);

                if (options.Value.IndexOnStartup)
                {
                    var service = scope.ServiceProvider.GetRequiredService<IKnowledgeIndexService>();
                    var result = await service.ReindexAsync(stoppingToken);
                    logger.LogInformation(
                        "Knowledge index {Status}: {Indexed} indexed, {Unchanged} unchanged, {Removed} removed, {Failed} failed.",
                        result.Status,
                        result.Indexed,
                        result.Unchanged,
                        result.Removed,
                        result.Failed);
                }

                return;
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception exception)
            {
                logger.LogWarning(
                    exception,
                    "Knowledge index startup attempt {Attempt} of {MaxAttempts} failed.",
                    attempt,
                    maxAttempts);

                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
            }
        }
    }
}
