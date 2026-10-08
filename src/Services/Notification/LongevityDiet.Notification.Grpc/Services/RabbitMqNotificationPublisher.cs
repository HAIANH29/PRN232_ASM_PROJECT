using LongevityDiet.Contracts.Messaging;

namespace LongevityDiet.Notification.Grpc.Services;

public sealed class RabbitMqNotificationPublisher(IConfiguration configuration) : INotificationPublisher
{
    public Task PublishAsync(NotificationRequestedMessage message, CancellationToken cancellationToken = default)
    {
        var exchange = configuration[$"{RabbitMqOptions.SectionName}:NotificationExchange"]
            ?? "longevity.notifications";
        var routingKey = configuration[$"{RabbitMqOptions.SectionName}:NotificationRoutingKey"]
            ?? "notification.requested";

        _ = exchange;
        _ = routingKey;
        _ = message;

        return Task.CompletedTask;
    }
}
