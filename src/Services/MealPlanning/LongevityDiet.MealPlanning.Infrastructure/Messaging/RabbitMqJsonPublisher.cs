using System.Text.Json;
using RabbitMQ.Client;

namespace LongevityDiet.MealPlanning.Infrastructure.Messaging;

internal static class RabbitMqJsonPublisher
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public static async Task PublishAsync<T>(
        ConnectionFactory connectionFactory,
        string exchange,
        string queue,
        string routingKey,
        T message,
        CancellationToken cancellationToken)
    {
        await using var connection = await connectionFactory.CreateConnectionAsync(cancellationToken);
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
