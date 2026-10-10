using LongevityDiet.Contracts.Messaging;
using LongevityDiet.Recommendation.Service;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace LongevityDiet.Core.Tests;

public sealed class RecommendationServiceTests
{
    [Fact]
    public void SafetyPolicy_removes_forbidden_and_duplicate_user_preferences()
    {
        var result = RecommendationSafetyPolicy.CleanPreferenceTags(
            [" legumes ", "Legumes", "diabetes plan", "", "vegetables"]);

        Assert.Equal(["legumes", "vegetables"], result);
    }

    [Fact]
    public async Task GenerateAsync_uses_approved_context_and_filters_unsafe_gemini_titles()
    {
        var context = new ApprovedKnowledgeContext(
            Guidelines:
            [
                new ApprovedDietGuideline(
                    Guid.NewGuid(),
                    "Plant-rich meals",
                    "Favor plant-forward meals in the project scope.",
                    "Chapter 1",
                    "10",
                    "team source")
            ],
            Foods:
            [
                new ApprovedFood(
                    Guid.NewGuid(),
                    "Lentils",
                    "Legume",
                    "Compatible project food.",
                    "Chapter 2",
                    "22",
                    "team source")
            ],
            Recipes: []);
        var gemini = new FakeGeminiClient
        {
            IsEnabled = true,
            Response = """
            {"suggestedMealTitles":["Lentil lunch bowl","Cure diabetes dinner"],"disclaimer":"ignored"}
            """
        };
        var generator = new RecommendationGenerator(
            new FakeApprovedKnowledgeClient(context),
            gemini,
            NullLogger<RecommendationGenerator>.Instance);

        var result = await generator.GenerateAsync(new RecommendationRequestMessage(
            Guid.NewGuid(),
            Guid.NewGuid(),
            [" legumes ", "diabetes cure"],
            Days: 2,
            DateTimeOffset.UtcNow));

        Assert.Equal(["Lentil lunch bowl"], result.SuggestedMealTitles);
        Assert.Equal(RecommendationSafetyPolicy.Disclaimer, result.Disclaimer);
        Assert.NotNull(gemini.CapturedPrompt);
        Assert.Contains("Plant-rich meals", gemini.CapturedPrompt, StringComparison.Ordinal);
        Assert.Contains("Lentils", gemini.CapturedPrompt, StringComparison.Ordinal);
        Assert.Contains("legumes", gemini.CapturedPrompt, StringComparison.Ordinal);
        Assert.DoesNotContain("diabetes cure", gemini.CapturedPrompt, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task GenerateAsync_returns_safe_fallback_when_no_approved_knowledge_exists()
    {
        var generator = new RecommendationGenerator(
            new FakeApprovedKnowledgeClient(new ApprovedKnowledgeContext([], [], [])),
            new FakeGeminiClient { IsEnabled = false },
            NullLogger<RecommendationGenerator>.Instance);

        var result = await generator.GenerateAsync(new RecommendationRequestMessage(
            Guid.NewGuid(),
            Guid.NewGuid(),
            ["cancer prevention"],
            Days: 2,
            DateTimeOffset.UtcNow));

        Assert.Equal(2, result.SuggestedMealTitles.Count);
        Assert.All(result.SuggestedMealTitles, title =>
            Assert.False(RecommendationSafetyPolicy.ContainsForbiddenScope(title)));
        Assert.Contains("not medical advice", result.Disclaimer, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("No approved knowledge is available yet.", result.Disclaimer, StringComparison.Ordinal);
    }
}
