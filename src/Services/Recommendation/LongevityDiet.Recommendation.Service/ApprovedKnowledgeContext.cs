namespace LongevityDiet.Recommendation.Service;

public sealed record ApprovedKnowledgeContext(
    IReadOnlyCollection<ApprovedDietGuideline> Guidelines,
    IReadOnlyCollection<ApprovedFood> Foods,
    IReadOnlyCollection<ApprovedRecipe> Recipes)
{
    public bool HasKnowledge => Guidelines.Count > 0 || Foods.Count > 0 || Recipes.Count > 0;
}

public sealed record ApprovedDietGuideline(
    Guid Id,
    string Title,
    string Summary,
    string SourceChapter,
    string SourcePage,
    string SourceReference);

public sealed record ApprovedFood(
    Guid Id,
    string Name,
    string Category,
    string CompatibilityNotes,
    string SourceChapter,
    string SourcePage,
    string SourceReference);

public sealed record ApprovedRecipe(
    Guid Id,
    string Name,
    string Description,
    IReadOnlyCollection<ApprovedRecipeIngredient> Ingredients,
    string SourceChapter,
    string SourcePage,
    string SourceReference);

public sealed record ApprovedRecipeIngredient(
    Guid FoodId,
    string FoodName,
    string FoodCategory,
    string QuantityText);
