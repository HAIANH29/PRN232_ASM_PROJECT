using Grpc.Core;

namespace LongevityDiet.Recommendation.Grpc.Services;

public sealed class RecommendationGrpcService(IRecommendationProvider recommendationProvider)
    : Recommendation.RecommendationBase
{
    public override Task<RecommendationReply> RecommendMealPlan(
        RecommendationRequest request,
        ServerCallContext context)
    {
        var titles = recommendationProvider.SuggestTitles(request.PreferenceTags, request.Days);
        var reply = new RecommendationReply
        {
            Disclaimer = "Educational planning support only. No diagnosis, treatment, or medical advice is provided."
        };

        reply.Suggestions.AddRange(titles.Select(title => new SuggestedMeal
        {
            Title = title,
            Source = "Approved Longevity Diet knowledge placeholder"
        }));

        return Task.FromResult(reply);
    }
}
