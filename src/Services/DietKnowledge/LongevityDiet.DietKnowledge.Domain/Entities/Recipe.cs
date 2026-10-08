namespace LongevityDiet.DietKnowledge.Domain.Entities;

public sealed class Recipe
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? UpdatedAtUtc { get; set; }

    public ICollection<RecipeIngredient> Ingredients { get; } = new List<RecipeIngredient>();
}
