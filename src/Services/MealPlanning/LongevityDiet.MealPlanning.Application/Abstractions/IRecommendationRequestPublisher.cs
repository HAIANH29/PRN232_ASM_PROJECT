using LongevityDiet.Contracts.Messaging;

namespace LongevityDiet.MealPlanning.Application.Abstractions;

public interface IRecommendationRequestPublisher
{
    Task PublishAsync(RecommendationRequestMessage message, CancellationToken cancellationToken = default);
}
