namespace LongevityDiet.Recommendation.Service;

public static class RecommendationSafetyPolicy
{
    public const string Disclaimer =
        "Educational meal planning suggestion based on approved project knowledge and user preferences. It is not medical advice, diagnosis, treatment, disease prediction, or lifespan prediction.";

    private static readonly string[] ForbiddenTerms =
    [
        "diagnosis",
        "diagnose",
        "treatment",
        "treat ",
        "cure",
        "disease",
        "diabetes",
        "cancer",
        "lifespan",
        "life span",
        "predict",
        "prediction"
    ];

    public static IReadOnlyCollection<string> CleanPreferenceTags(
        IReadOnlyCollection<string> preferenceTags)
    {
        return preferenceTags
            .Select(tag => tag.Trim())
            .Where(tag => !string.IsNullOrWhiteSpace(tag))
            .Where(tag => !ContainsForbiddenScope(tag))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Take(12)
            .ToArray();
    }

    public static IReadOnlyCollection<string> CleanSuggestedTitles(
        IEnumerable<string> titles,
        int count)
    {
        return titles
            .Select(title => title.Trim())
            .Where(title => !string.IsNullOrWhiteSpace(title))
            .Where(title => !ContainsForbiddenScope(title))
            .Select(title => title.Length <= 120 ? title : title[..120])
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Take(count)
            .ToArray();
    }

    public static bool ContainsForbiddenScope(string value)
    {
        return ForbiddenTerms.Any(term =>
            value.Contains(term, StringComparison.OrdinalIgnoreCase));
    }
}
