using LongevityDiet.Contracts.Messaging;
using LongevityDiet.MealPlanning.Application.Abstractions;
using Microsoft.Extensions.Configuration;

namespace LongevityDiet.MealPlanning.Infrastructure.Messaging;

public sealed class RabbitMqRecommendationRequestPublisher(IConfiguration configuration) : IRecommendationRequestPublisher
{
    public async Task PublishAsync(
        RecommendationRequestMessage message,
        CancellationToken cancellationToken = default)
    {
        var exchange = configuration[$"{RabbitMqOptions.SectionName}:RecommendationExchange"]
            ?? "longevity.recommendations";
        var queue = configuration[$"{RabbitMqOptions.SectionName}:RecommendationRequestQueue"]
            ?? "recommendation.requests";
        var routingKey = configuration[$"{RabbitMqOptions.SectionName}:RecommendationRequestRoutingKey"]
            ?? "recommendation.requested";

        await RabbitMqJsonPublisher.PublishAsync(
            RabbitMqConnectionFactory.Create(configuration),
            exchange,
            queue,
            routingKey,
            message,
            cancellationToken);
    }
}
