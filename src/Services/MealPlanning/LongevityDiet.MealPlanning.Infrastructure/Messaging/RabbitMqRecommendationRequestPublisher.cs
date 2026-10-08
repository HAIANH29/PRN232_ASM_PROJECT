using LongevityDiet.Contracts.Messaging;
using LongevityDiet.MealPlanning.Application.Abstractions;
using Microsoft.Extensions.Configuration;

namespace LongevityDiet.MealPlanning.Infrastructure.Messaging;

public sealed class RabbitMqRecommendationRequestPublisher(IConfiguration configuration) : IRecommendationRequestPublisher
{
    public Task PublishAsync(RecommendationRequestMessage message, CancellationToken cancellationToken = default)
    {
        var exchange = configuration[$"{RabbitMqOptions.SectionName}:RecommendationExchange"]
            ?? "longevity.recommendations";
        var routingKey = configuration[$"{RabbitMqOptions.SectionName}:RecommendationRequestRoutingKey"]
            ?? "recommendation.requested";

        _ = exchange;
        _ = routingKey;
        _ = message;

        return Task.CompletedTask;
    }
}
