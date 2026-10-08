namespace LongevityDiet.DietKnowledge.Domain.Entities;

public sealed class RecipeIngredient
{
    public Guid Id { get; set; }

    public Guid RecipeId { get; set; }

    public Recipe? Recipe { get; set; }

    public Guid FoodId { get; set; }

    public string QuantityText { get; set; } = string.Empty;
}
