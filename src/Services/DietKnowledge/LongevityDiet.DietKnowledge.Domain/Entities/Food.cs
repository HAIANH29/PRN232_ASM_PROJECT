namespace LongevityDiet.DietKnowledge.Domain.Entities;

public sealed class Food
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string CompatibilityNotes { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
