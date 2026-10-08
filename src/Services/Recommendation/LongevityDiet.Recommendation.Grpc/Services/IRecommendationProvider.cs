namespace LongevityDiet.Recommendation.Grpc.Services;

public interface IRecommendationProvider
{
    IReadOnlyCollection<string> SuggestTitles(IReadOnlyCollection<string> preferenceTags, int days);
}
