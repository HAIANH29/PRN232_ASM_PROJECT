namespace LongevityDiet.DietKnowledge.Domain.Entities;

public sealed class RecipeStep
{
    public Guid Id { get; set; }

    public Guid RecipeId { get; set; }

    public Recipe? Recipe { get; set; }

    public int StepNumber { get; set; }

    public string Instruction { get; set; } = string.Empty;
}
