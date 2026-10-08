using LongevityDiet.Contracts.Messaging;

namespace LongevityDiet.MealPlanning.Application.Abstractions;

public interface IReminderPublisher
{
    Task PublishAsync(ReminderRequestedMessage message, CancellationToken cancellationToken = default);
}
