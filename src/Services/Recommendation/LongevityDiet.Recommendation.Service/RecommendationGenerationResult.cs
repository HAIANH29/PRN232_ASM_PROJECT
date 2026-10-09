namespace LongevityDiet.Recommendation.Service;

public sealed record RecommendationGenerationResult(
    IReadOnlyCollection<string> SuggestedMealTitles,
    string Disclaimer);
