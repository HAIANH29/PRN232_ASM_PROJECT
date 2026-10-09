using System.Text.Json;
using LongevityDiet.Contracts.Messaging;
using LongevityDiet.MealPlanning.Application.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LongevityDiet.MealPlanning.Infrastructure.Messaging;

public sealed class RabbitMqRecommendationResultWorker(
    IConfiguration configuration,
    IServiceScopeFactory scopeFactory,
    ILogger<RabbitMqRecommendationResultWorker> logger) : BackgroundService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ConsumeAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception exception)
            {
                logger.LogWarning(
                    exception,
                    "Recommendation result consumer failed. Retrying in 5 seconds.");

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }

    private async Task ConsumeAsync(CancellationToken stoppingToken)
    {
        var exchange = configuration[$"{RabbitMqOptions.SectionName}:RecommendationExchange"]
            ?? "longevity.recommendations";
        var queue = configuration[$"{RabbitMqOptions.SectionName}:RecommendationResultQueue"]
            ?? "recommendation.results";
        var routingKey = configuration[$"{RabbitMqOptions.SectionName}:RecommendationResultRoutingKey"]
            ?? "recommendation.completed";

        await using var connection = await RabbitMqConnectionFactory
            .Create(configuration)
            .CreateConnectionAsync(stoppingToken);
        await using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await channel.ExchangeDeclareAsync(
            exchange,
            ExchangeType.Direct,
            durable: true,
            autoDelete: false,
            cancellationToken: stoppingToken);

        await channel.QueueDeclareAsync(
            queue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: stoppingToken);

        await channel.QueueBindAsync(
            queue,
            exchange,
            routingKey,
            cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (_, args) =>
        {
            try
            {
                var body = args.Body.ToArray();
                var message = JsonSerializer.Deserialize<RecommendationResultMessage>(body, JsonOptions);
                if (message is null)
                {
                    await channel.BasicAckAsync(args.DeliveryTag, false, stoppingToken);
                    return;
                }

                using var scope = scopeFactory.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<IRecommendationResultConsumer>();
                await handler.HandleAsync(message, stoppingToken);
                await channel.BasicAckAsync(args.DeliveryTag, false, stoppingToken);
            }
            catch (Exception exception)
            {
                logger.LogWarning(exception, "Failed to handle recommendation result message.");
                await channel.BasicNackAsync(args.DeliveryTag, false, true, stoppingToken);
            }
        };

        await channel.BasicConsumeAsync(
            queue,
            autoAck: false,
            consumer,
            stoppingToken);

        logger.LogInformation("Consuming recommendation results from queue {Queue}.", queue);

        await Task.Delay(Timeout.InfiniteTimeSpan, stoppingToken);
    }
}
