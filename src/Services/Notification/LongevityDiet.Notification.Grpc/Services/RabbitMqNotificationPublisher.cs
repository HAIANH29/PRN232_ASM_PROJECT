using System.Text.Json;
using LongevityDiet.Contracts.Messaging;
using RabbitMQ.Client;

namespace LongevityDiet.Notification.Grpc.Services;

public sealed class RabbitMqNotificationPublisher(IConfiguration configuration) : INotificationPublisher
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task PublishAsync(
        NotificationRequestedMessage message,
        CancellationToken cancellationToken = default)
    {
        var exchange = configuration[$"{RabbitMqOptions.SectionName}:NotificationExchange"]
            ?? "longevity.notifications";
        var queue = configuration[$"{RabbitMqOptions.SectionName}:NotificationQueue"]
            ?? "notification.messages";
        var routingKey = configuration[$"{RabbitMqOptions.SectionName}:NotificationRoutingKey"]
            ?? "notification.requested";

        var factory = new ConnectionFactory
        {
            HostName = configuration[$"{RabbitMqOptions.SectionName}:HostName"] ?? "localhost",
            Port = int.TryParse(configuration[$"{RabbitMqOptions.SectionName}:Port"], out var port)
                ? port
                : 5672,
            UserName = configuration[$"{RabbitMqOptions.SectionName}:UserName"] ?? "guest",
            Password = configuration[$"{RabbitMqOptions.SectionName}:Password"] ?? "guest",
            AutomaticRecoveryEnabled = true
        };

        await using var connection = await factory.CreateConnectionAsync(cancellationToken);
        await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await channel.ExchangeDeclareAsync(
            exchange,
            ExchangeType.Direct,
            durable: true,
            autoDelete: false,
            cancellationToken: cancellationToken);

        await channel.QueueDeclareAsync(
            queue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: cancellationToken);

        await channel.QueueBindAsync(
            queue,
            exchange,
            routingKey,
            cancellationToken: cancellationToken);

        var properties = new BasicProperties
        {
            ContentType = "application/json",
            DeliveryMode = DeliveryModes.Persistent
        };

        var body = JsonSerializer.SerializeToUtf8Bytes(message, JsonOptions);

        await channel.BasicPublishAsync(
            exchange,
            routingKey,
            false,
            properties,
            body,
            cancellationToken);
    }
}
