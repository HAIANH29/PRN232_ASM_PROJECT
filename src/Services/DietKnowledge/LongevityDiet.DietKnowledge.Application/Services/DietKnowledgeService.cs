namespace LongevityDiet.DietKnowledge.Application.Services;

public sealed class DietKnowledgeService : IDietKnowledgeService
{
    public DietKnowledgeServiceStatus GetStatus()
    {
        return new DietKnowledgeServiceStatus(
            "Diet Knowledge Service",
            "DietKnowledgeDb",
            ["DietGuideline", "Food", "Recipe", "RecipeIngredient"]);
    }
}
