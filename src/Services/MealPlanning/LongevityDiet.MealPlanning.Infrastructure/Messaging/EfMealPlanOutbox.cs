using System.Text.Json;
using LongevityDiet.Contracts.Messaging;
using LongevityDiet.MealPlanning.Application.Abstractions;
using LongevityDiet.MealPlanning.Application.Common;
using LongevityDiet.MealPlanning.Domain.Entities;
using LongevityDiet.MealPlanning.Infrastructure.Persistence;

namespace LongevityDiet.MealPlanning.Infrastructure.Messaging;

// Shares the scoped DbContext with the repository, so the message is saved by the same
// SaveChanges call as the meal plan or recommendation request it belongs to.
public sealed class EfMealPlanOutbox(MealPlanningDbContext dbContext) : IMealPlanOutbox
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public void Enqueue(ReminderRequestedMessage message) =>
        Add(OutboxMessageTypes.Reminder, JsonSerializer.Serialize(message, JsonOptions));

    public void Enqueue(RecommendationRequestMessage message) =>
        Add(OutboxMessageTypes.RecommendationRequest, JsonSerializer.Serialize(message, JsonOptions));

    private void Add(string type, string payload)
    {
        var now = DateTimeOffset.UtcNow;
        dbContext.OutboxMessages.Add(new OutboxMessage
        {
            Id = Guid.NewGuid(),
            MessageType = type,
            PayloadJson = payload,
            CreatedAtUtc = now,
            NextAttemptAtUtc = now
        });
    }
}
