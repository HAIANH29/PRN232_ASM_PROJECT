namespace LongevityDiet.DietKnowledge.Application.Common;

public static class KnowledgeCandidateTypes
{
    public const string DietGuideline = "DietGuideline";
    public const string Food = "Food";
    public const string Recipe = "Recipe";

    public static bool IsValid(string? candidateType)
    {
        return string.Equals(candidateType, DietGuideline, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(candidateType, Food, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(candidateType, Recipe, StringComparison.OrdinalIgnoreCase);
    }

    public static string Normalize(string candidateType)
    {
        if (string.Equals(candidateType, Food, StringComparison.OrdinalIgnoreCase))
        {
            return Food;
        }

        if (string.Equals(candidateType, Recipe, StringComparison.OrdinalIgnoreCase))
        {
            return Recipe;
        }

        return DietGuideline;
    }
}
