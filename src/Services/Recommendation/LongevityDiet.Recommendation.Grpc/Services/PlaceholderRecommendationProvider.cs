namespace LongevityDiet.Recommendation.Grpc.Services;

public sealed class PlaceholderRecommendationProvider : IRecommendationProvider
{
    public IReadOnlyCollection<string> SuggestTitles(IReadOnlyCollection<string> preferenceTags, int days)
    {
        var limitedDays = Math.Clamp(days, 1, 7);
        var preferenceSummary = preferenceTags.Count == 0
            ? "approved longevity diet knowledge"
            : string.Join(", ", preferenceTags);

        return Enumerable.Range(1, limitedDays)
            .Select(day => $"Day {day}: plant-forward meal idea based on {preferenceSummary}")
            .ToArray();
    }
}
