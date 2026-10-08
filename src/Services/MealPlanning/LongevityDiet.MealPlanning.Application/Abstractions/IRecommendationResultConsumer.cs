using LongevityDiet.Contracts.Messaging;

namespace LongevityDiet.MealPlanning.Application.Abstractions;

public interface IRecommendationResultConsumer
{
    Task HandleAsync(RecommendationResultMessage message, CancellationToken cancellationToken = default);
}
