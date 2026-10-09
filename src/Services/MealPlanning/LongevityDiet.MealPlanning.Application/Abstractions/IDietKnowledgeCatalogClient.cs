namespace LongevityDiet.MealPlanning.Application.Abstractions;

public interface IDietKnowledgeCatalogClient
{
    Task<bool> FoodExistsAsync(Guid foodId, CancellationToken cancellationToken = default);

    Task<bool> RecipeExistsAsync(Guid recipeId, CancellationToken cancellationToken = default);
}
