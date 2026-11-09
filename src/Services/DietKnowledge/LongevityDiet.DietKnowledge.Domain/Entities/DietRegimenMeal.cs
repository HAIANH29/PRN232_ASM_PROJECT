namespace LongevityDiet.DietKnowledge.Domain.Entities;

public sealed class DietRegimenMeal
{
    public Guid Id { get; set; }

    public Guid DietRegimenId { get; set; }

    public DietRegimen? DietRegimen { get; set; }

    public int DayNumber { get; set; }

    public string MealSlot { get; set; } = string.Empty;

    public Guid RecipeId { get; set; }

    public Recipe? Recipe { get; set; }

    public string Notes { get; set; } = string.Empty;
}
