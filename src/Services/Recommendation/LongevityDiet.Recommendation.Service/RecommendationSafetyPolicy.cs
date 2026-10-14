namespace LongevityDiet.Recommendation.Service;

public static class RecommendationSafetyPolicy
{
    public const string Disclaimer =
        "Educational meal planning suggestion based on approved project knowledge and user preferences. It is not medical advice, diagnosis, treatment, disease prediction, or lifespan prediction.";

    public const string DisclaimerVi =
        "Gợi ý lập kế hoạch bữa ăn mang tính giáo dục, dựa trên kiến thức đã được duyệt của dự án và sở thích của người dùng. Đây không phải lời khuyên y tế, chẩn đoán, điều trị, dự đoán bệnh tật hay dự đoán tuổi thọ.";

    public static string DisclaimerFor(string? language) =>
        string.Equals(language, "vi", StringComparison.OrdinalIgnoreCase) ? DisclaimerVi : Disclaimer;

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
        "prediction",
        "chẩn đoán",
        "điều trị",
        "chữa",
        "bệnh",
        "tiểu đường",
        "ung thư",
        "tuổi thọ",
        "dự đoán"
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
