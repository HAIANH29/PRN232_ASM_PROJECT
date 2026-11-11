using LongevityDiet.Contracts.Messaging;

namespace LongevityDiet.MealPlanning.Application.Abstractions;

// Queues a message in the current unit of work. Nothing is sent until the repository saves, and
// the message is then delivered to RabbitMQ asynchronously with retries.
public interface IMealPlanOutbox
{
    void Enqueue(ReminderRequestedMessage message);

    void Enqueue(RecommendationRequestMessage message);
}
