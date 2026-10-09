using System.Text;
using System.Text.Json;
using LongevityDiet.Contracts.Messaging;

namespace LongevityDiet.Recommendation.Service;

public sealed class RecommendationGenerator(
    IApprovedKnowledgeClient approvedKnowledgeClient,
    IGeminiClient geminiClient,
    ILogger<RecommendationGenerator> logger) : IRecommendationGenerator
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<RecommendationGenerationResult> GenerateAsync(
        RecommendationRequestMessage request,
        CancellationToken cancellationToken = default)
    {
        var knowledge = await approvedKnowledgeClient.GetApprovedKnowledgeAsync(cancellationToken);
        var safePreferences = RecommendationSafetyPolicy.CleanPreferenceTags(request.PreferenceTags);
        var count = Math.Clamp(request.Days, 1, 14);

        if (!knowledge.HasKnowledge)
        {
            return BuildFallbackResult(knowledge, safePreferences, count, "No approved knowledge is available yet.");
        }

        if (geminiClient.IsEnabled)
        {
            try
            {
                var prompt = BuildPrompt(knowledge, safePreferences, count);
                var generatedText = await geminiClient.GenerateTextAsync(prompt, cancellationToken);
                var generatedResult = TryParseGeminiResult(generatedText, count);
                if (generatedResult is not null)
                {
                    return generatedResult;
                }

                logger.LogWarning("Gemini response could not be parsed into safe recommendation titles.");
            }
            catch (Exception exception)
            {
                logger.LogWarning(exception, "Gemini generation failed. Falling back to local safe recommendation.");
            }
        }

        return BuildFallbackResult(knowledge, safePreferences, count);
    }

    private static string BuildPrompt(
        ApprovedKnowledgeContext knowledge,
        IReadOnlyCollection<string> safePreferences,
        int count)
    {
        var builder = new StringBuilder();
        builder.AppendLine("Create concise meal title suggestions for the Longevity Diet Platform.");
        builder.AppendLine("Use only the approved project knowledge listed below and the user's preferences.");
        builder.AppendLine("Do not add outside nutrition facts or medical claims.");
        builder.AppendLine("Do not provide diagnosis, treatment advice, disease prediction, or lifespan prediction.");
        builder.AppendLine("Return only valid JSON with this shape:");
        builder.AppendLine("""{"suggestedMealTitles":["title 1"],"disclaimer":"educational non-medical disclaimer"}""");
        builder.AppendLine($"Return exactly {count} suggestedMealTitles.");
        builder.AppendLine();
        builder.AppendLine("User preferences:");
        builder.AppendLine(safePreferences.Count == 0 ? "- none supplied" : string.Join(Environment.NewLine, safePreferences.Select(tag => $"- {tag}")));
        builder.AppendLine();
        builder.AppendLine("Approved diet guidelines:");
        foreach (var guideline in knowledge.Guidelines.Take(12))
        {
            builder.AppendLine($"- {guideline.Title}: {guideline.Summary} ({FormatSource(guideline.SourceChapter, guideline.SourcePage, guideline.SourceReference)})");
        }

        builder.AppendLine();
        builder.AppendLine("Approved foods:");
        foreach (var food in knowledge.Foods.Take(30))
        {
            builder.AppendLine($"- {food.Name} [{food.Category}]: {food.CompatibilityNotes} ({FormatSource(food.SourceChapter, food.SourcePage, food.SourceReference)})");
        }

        builder.AppendLine();
        builder.AppendLine("Approved recipes:");
        foreach (var recipe in knowledge.Recipes.Take(30))
        {
            var ingredients = recipe.Ingredients.Count == 0
                ? "ingredients managed in project catalog"
                : string.Join(", ", recipe.Ingredients.Take(8).Select(ingredient =>
                    string.IsNullOrWhiteSpace(ingredient.QuantityText)
                        ? ingredient.FoodName
                        : $"{ingredient.QuantityText} {ingredient.FoodName}"));
            builder.AppendLine($"- {recipe.Name}: {recipe.Description}; ingredients: {ingredients} ({FormatSource(recipe.SourceChapter, recipe.SourcePage, recipe.SourceReference)})");
        }

        return builder.ToString();
    }

    private static RecommendationGenerationResult? TryParseGeminiResult(
        string? generatedText,
        int count)
    {
        if (string.IsNullOrWhiteSpace(generatedText))
        {
            return null;
        }

        var json = ExtractJsonObject(generatedText);
        if (string.IsNullOrWhiteSpace(json))
        {
            return null;
        }

        GeminiRecommendationResponse? parsed;
        try
        {
            parsed = JsonSerializer.Deserialize<GeminiRecommendationResponse>(json, JsonOptions);
        }
        catch (JsonException)
        {
            return null;
        }

        if (parsed?.SuggestedMealTitles is null)
        {
            return null;
        }

        var titles = RecommendationSafetyPolicy.CleanSuggestedTitles(
            parsed.SuggestedMealTitles,
            count);

        if (titles.Count == 0)
        {
            return null;
        }

        return new RecommendationGenerationResult(
            titles,
            RecommendationSafetyPolicy.Disclaimer);
    }

    private static RecommendationGenerationResult BuildFallbackResult(
        ApprovedKnowledgeContext knowledge,
        IReadOnlyCollection<string> safePreferences,
        int count,
        string? reason = null)
    {
        var titles = BuildFallbackTitles(knowledge, safePreferences, count);
        var disclaimer = string.IsNullOrWhiteSpace(reason)
            ? RecommendationSafetyPolicy.Disclaimer
            : $"{RecommendationSafetyPolicy.Disclaimer} {reason}";

        return new RecommendationGenerationResult(titles, disclaimer);
    }

    private static IReadOnlyCollection<string> BuildFallbackTitles(
        ApprovedKnowledgeContext knowledge,
        IReadOnlyCollection<string> safePreferences,
        int count)
    {
        var preferenceSuffix = safePreferences.Count == 0
            ? string.Empty
            : $" ({string.Join(", ", safePreferences.Take(3))})";

        var recipeTitles = knowledge.Recipes
            .Select(recipe => $"{recipe.Name}{preferenceSuffix}");
        var foodTitles = knowledge.Foods
            .Select(food => $"{food.Name} {food.Category} meal{preferenceSuffix}");

        var cleanTitles = RecommendationSafetyPolicy.CleanSuggestedTitles(
            recipeTitles.Concat(foodTitles),
            count);
        if (cleanTitles.Count >= count)
        {
            return cleanTitles;
        }

        var completed = cleanTitles.ToList();
        while (completed.Count < count)
        {
            completed.Add($"Approved knowledge placeholder meal {completed.Count + 1}{preferenceSuffix}");
        }

        return RecommendationSafetyPolicy.CleanSuggestedTitles(completed, count);
    }

    private static string? ExtractJsonObject(string value)
    {
        var start = value.IndexOf('{');
        var end = value.LastIndexOf('}');
        if (start < 0 || end <= start)
        {
            return null;
        }

        return value[start..(end + 1)];
    }

    private static string FormatSource(
        string sourceChapter,
        string sourcePage,
        string sourceReference)
    {
        var parts = new[] { sourceChapter, sourcePage, sourceReference }
            .Where(part => !string.IsNullOrWhiteSpace(part))
            .ToArray();

        return parts.Length == 0 ? "approved source metadata" : string.Join(", ", parts);
    }

    private sealed record GeminiRecommendationResponse(
        IReadOnlyCollection<string>? SuggestedMealTitles,
        string? Disclaimer);
}
