namespace LongevityDiet.DietKnowledge.Application.Models;

public sealed record RecipeModel(
    Guid Id,
    string Name,
    string Description,
    bool IsActive,
    string SourceTitle,
    string SourceChapter,
    string SourcePage,
    string SourceReference,
    string ReviewStatus,
    string ReviewedBy,
    DateTimeOffset? ReviewedAtUtc,
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
    string SourceTitle,
    string SourceChapter,
    string SourcePage,
    string SourceReference,
    string ReviewStatus,
    string ReviewedBy,
    IReadOnlyCollection<RecipeIngredientCommand> Ingredients);

public sealed record UpdateRecipeCommand(
    string Name,
    string Description,
    string SourceTitle,
    string SourceChapter,
    string SourcePage,
    string SourceReference,
    string ReviewStatus,
    string ReviewedBy,
    IReadOnlyCollection<RecipeIngredientCommand> Ingredients);
