namespace LongevityDiet.MealPlanning.Application.Abstractions;

public interface IRecommendationClient
{
    Task<IReadOnlyCollection<string>> GetMealPlanSuggestionsAsync(
        Guid userId,
        IReadOnlyCollection<string> preferenceTags,
        int days,
        CancellationToken cancellationToken = default);
}
