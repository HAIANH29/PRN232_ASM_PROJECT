namespace LongevityDiet.DietKnowledge.Application.Common;

public static class CategoryTypes
{
    public const string Food = "Food";
    public const string Recipe = "Recipe";
    public const string Guideline = "Guideline";

    public static bool IsValid(string? categoryType)
    {
        return string.Equals(categoryType, Food, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(categoryType, Recipe, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(categoryType, Guideline, StringComparison.OrdinalIgnoreCase);
    }
}
