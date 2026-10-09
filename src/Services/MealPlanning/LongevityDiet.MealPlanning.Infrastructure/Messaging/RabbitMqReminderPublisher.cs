using LongevityDiet.Contracts.Messaging;
using LongevityDiet.MealPlanning.Application.Abstractions;
using Microsoft.Extensions.Configuration;

namespace LongevityDiet.MealPlanning.Infrastructure.Messaging;

public sealed class RabbitMqReminderPublisher(IConfiguration configuration) : IReminderPublisher
{
    public async Task PublishAsync(
        ReminderRequestedMessage message,
        CancellationToken cancellationToken = default)
    {
        var exchange = configuration[$"{RabbitMqOptions.SectionName}:ReminderExchange"] ?? "longevity.reminders";
        var queue = configuration[$"{RabbitMqOptions.SectionName}:ReminderQueue"] ?? "reminder.requests";
        var routingKey = configuration[$"{RabbitMqOptions.SectionName}:ReminderRoutingKey"] ?? "reminder.requested";

        await RabbitMqJsonPublisher.PublishAsync(
            RabbitMqConnectionFactory.Create(configuration),
            exchange,
            queue,
            routingKey,
            message,
            cancellationToken);
    }
}
