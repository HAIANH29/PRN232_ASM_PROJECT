namespace LongevityDiet.DietKnowledge.Application.Models;

public sealed record RecipeModel(
    Guid Id,
    string Name,
    string Description,
    bool IsActive,
    IReadOnlyCollection<RecipeIngredientModel> Ingredients,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc);

public sealed record RecipeIngredientModel(
    Guid Id,
    Guid FoodId,
    string? FoodName,
    string? FoodCategory,
    string QuantityText);

public sealed record RecipeIngredientCommand(
    Guid FoodId,
    string QuantityText);

public sealed record CreateRecipeCommand(
    string Name,
    string Description,
    IReadOnlyCollection<RecipeIngredientCommand> Ingredients);

public sealed record UpdateRecipeCommand(
    string Name,
    string Description,
    IReadOnlyCollection<RecipeIngredientCommand> Ingredients);
