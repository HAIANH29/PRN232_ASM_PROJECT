using System.Text.Json;
using LongevityDiet.Contracts.Messaging;
using LongevityDiet.MealPlanning.Application.Abstractions;
using LongevityDiet.MealPlanning.Application.Common;
using LongevityDiet.MealPlanning.Domain.Entities;
using LongevityDiet.MealPlanning.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LongevityDiet.MealPlanning.Infrastructure.Messaging;

// Publishes pending outbox rows in creation order. A failed row is retried later with a growing
// delay; it is never dropped, because a lost recommendation request would stay Pending forever.
// Delivery is at least once: a crash between publish and save can send a message twice.
public sealed class OutboxDispatcher(
    MealPlanningDbContext dbContext,
    IReminderPublisher reminderPublisher,
    IRecommendationRequestPublisher recommendationRequestPublisher,
    ILogger<OutboxDispatcher> logger)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    // Returns the number of messages published in this call.
    public async Task<int> DispatchPendingAsync(int batchSize, CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;
        var pending = await dbContext.OutboxMessages
            .Where(message => message.ProcessedAtUtc == null && message.NextAttemptAtUtc <= now)
            .OrderBy(message => message.CreatedAtUtc)
            .Take(batchSize)
            .ToListAsync(cancellationToken);

        var published = 0;
        foreach (var message in pending)
        {
            try
            {
                await PublishAsync(message, cancellationToken);
                message.ProcessedAtUtc = DateTimeOffset.UtcNow;
                message.LastError = null;
                published++;
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception exception) when (exception is not OperationCanceledException)
            {
                message.Attempts++;
                message.LastError = Truncate(exception.Message, 500);
                message.NextAttemptAtUtc = DateTimeOffset.UtcNow.Add(BackoffFor(message.Attempts));
                logger.LogWarning(
                    exception,
                    "Could not publish outbox message {MessageId} ({MessageType}); attempt {Attempts}.",
                    message.Id,
                    message.MessageType,
                    message.Attempts);

                // The broker is most likely down, so stop this round instead of failing every row.
                await dbContext.SaveChangesAsync(cancellationToken);
                return published;
            }
        }

        return published;
    }

    private async Task PublishAsync(OutboxMessage message, CancellationToken cancellationToken)
    {
        switch (message.MessageType)
        {
            case OutboxMessageTypes.Reminder:
                await reminderPublisher.PublishAsync(
                    JsonSerializer.Deserialize<ReminderRequestedMessage>(message.PayloadJson, JsonOptions)
                        ?? throw new InvalidDataException("Outbox reminder payload is empty."),
                    cancellationToken);
                break;
            case OutboxMessageTypes.RecommendationRequest:
                await recommendationRequestPublisher.PublishAsync(
                    JsonSerializer.Deserialize<RecommendationRequestMessage>(message.PayloadJson, JsonOptions)
                        ?? throw new InvalidDataException("Outbox recommendation payload is empty."),
                    cancellationToken);
                break;
            default:
                throw new InvalidDataException($"Unknown outbox message type '{message.MessageType}'.");
        }
    }

    internal static TimeSpan BackoffFor(int attempts) =>
        TimeSpan.FromSeconds(Math.Min(300, 5 * Math.Pow(2, Math.Min(attempts, 6))));

    private static string Truncate(string value, int max) => value.Length <= max ? value : value[..max];
}
