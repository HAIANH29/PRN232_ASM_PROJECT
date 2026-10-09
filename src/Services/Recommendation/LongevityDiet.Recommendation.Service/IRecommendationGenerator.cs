using LongevityDiet.Contracts.Messaging;

namespace LongevityDiet.Recommendation.Service;

public interface IRecommendationGenerator
{
    Task<RecommendationGenerationResult> GenerateAsync(
        RecommendationRequestMessage request,
        CancellationToken cancellationToken = default);
}
