using LongevityDiet.DietKnowledge.Application.Models;

namespace LongevityDiet.DietKnowledge.Application.Services;

public interface IDietKnowledgeService
{
    DietKnowledgeServiceStatus GetStatus();

    Task<PagedResult<DietGuidelineModel>> ListGuidelinesAsync(
        DietGuidelineQuery query,
        CancellationToken cancellationToken = default);

    Task<DietGuidelineModel> GetGuidelineAsync(
        Guid id,
        bool includeInactive = false,
        CancellationToken cancellationToken = default);

    Task<DietGuidelineModel> CreateGuidelineAsync(
        CreateDietGuidelineCommand command,
        CancellationToken cancellationToken = default);

    Task<DietGuidelineModel> UpdateGuidelineAsync(
        Guid id,
        UpdateDietGuidelineCommand command,
        CancellationToken cancellationToken = default);

    Task<DietGuidelineModel> SetGuidelineActivationAsync(
        Guid id,
        bool isActive,
        CancellationToken cancellationToken = default);

    Task<PagedResult<FoodModel>> ListFoodsAsync(
        FoodQuery query,
        CancellationToken cancellationToken = default);

    Task<FoodModel> GetFoodAsync(
        Guid id,
        bool includeInactive = false,
        CancellationToken cancellationToken = default);

    Task<FoodModel> CreateFoodAsync(
        CreateFoodCommand command,
        CancellationToken cancellationToken = default);

    Task<FoodModel> UpdateFoodAsync(
        Guid id,
        UpdateFoodCommand command,
        CancellationToken cancellationToken = default);

    Task<FoodModel> SetFoodActivationAsync(
        Guid id,
        bool isActive,
        CancellationToken cancellationToken = default);

    Task<PagedResult<RecipeModel>> ListRecipesAsync(
        RecipeQuery query,
        CancellationToken cancellationToken = default);

    Task<RecipeModel> GetRecipeAsync(
        Guid id,
        bool includeInactive = false,
        CancellationToken cancellationToken = default);

    Task<RecipeModel> CreateRecipeAsync(
        CreateRecipeCommand command,
        CancellationToken cancellationToken = default);

    Task<RecipeModel> UpdateRecipeAsync(
        Guid id,
        UpdateRecipeCommand command,
        CancellationToken cancellationToken = default);

    Task<RecipeModel> SetRecipeActivationAsync(
        Guid id,
        bool isActive,
        CancellationToken cancellationToken = default);
}
