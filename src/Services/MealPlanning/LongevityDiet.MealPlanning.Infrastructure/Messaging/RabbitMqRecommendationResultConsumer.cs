using LongevityDiet.Contracts.Messaging;
using LongevityDiet.MealPlanning.Application.Abstractions;

namespace LongevityDiet.MealPlanning.Infrastructure.Messaging;

public sealed class RabbitMqRecommendationResultConsumer : IRecommendationResultConsumer
{
    public Task HandleAsync(RecommendationResultMessage message, CancellationToken cancellationToken = default)
    {
        _ = message;

        return Task.CompletedTask;
    }
}
