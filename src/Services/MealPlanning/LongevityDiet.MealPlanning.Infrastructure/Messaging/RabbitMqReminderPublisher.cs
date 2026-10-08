using LongevityDiet.Contracts.Messaging;
using LongevityDiet.MealPlanning.Application.Abstractions;
using Microsoft.Extensions.Configuration;

namespace LongevityDiet.MealPlanning.Infrastructure.Messaging;

public sealed class RabbitMqReminderPublisher(IConfiguration configuration) : IReminderPublisher
{
    public Task PublishAsync(ReminderRequestedMessage message, CancellationToken cancellationToken = default)
    {
        var exchange = configuration[$"{RabbitMqOptions.SectionName}:ReminderExchange"] ?? "longevity.reminders";
        var routingKey = configuration[$"{RabbitMqOptions.SectionName}:ReminderRoutingKey"] ?? "reminder.requested";

        _ = exchange;
        _ = routingKey;
        _ = message;

        return Task.CompletedTask;
    }
}
