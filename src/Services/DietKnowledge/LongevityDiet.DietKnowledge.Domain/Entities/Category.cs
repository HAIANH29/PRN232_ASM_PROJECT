namespace LongevityDiet.DietKnowledge.Domain.Entities;

public sealed class Category
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    // Food, Recipe or Guideline: which kind of content the category groups.
    public string CategoryType { get; set; } = "Food";

    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}
