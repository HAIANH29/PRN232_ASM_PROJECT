using Grpc.Net.Client;
using LongevityDiet.MealPlanning.Application.Abstractions;
using LongevityDiet.Recommendation.Grpc;
using Microsoft.Extensions.Configuration;
using RecommendationGrpcClient = LongevityDiet.Recommendation.Grpc.Recommendation.RecommendationClient;

namespace LongevityDiet.MealPlanning.Infrastructure.Recommendation;

public sealed class GrpcRecommendationClient(IConfiguration configuration) : IRecommendationClient
{
    public async Task<IReadOnlyCollection<string>> GetMealPlanSuggestionsAsync(
        Guid userId,
        IReadOnlyCollection<string> preferenceTags,
        int days,
        CancellationToken cancellationToken = default)
    {
        var address = configuration["RecommendationService:GrpcAddress"];
        if (string.IsNullOrWhiteSpace(address))
        {
            return [];
        }

        using var channel = GrpcChannel.ForAddress(address);
        var client = new RecommendationGrpcClient(channel);
        var request = new RecommendationRequest
        {
            UserId = userId.ToString(),
            Days = days
        };

        request.PreferenceTags.AddRange(preferenceTags);

        var reply = await client.RecommendMealPlanAsync(request, cancellationToken: cancellationToken);

        return reply.Suggestions.Select(suggestion => suggestion.Title).ToArray();
    }
}
