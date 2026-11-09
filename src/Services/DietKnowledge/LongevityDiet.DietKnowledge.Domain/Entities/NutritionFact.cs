namespace LongevityDiet.DietKnowledge.Domain.Entities;

// Nutrition values for one serving of a food, entered by an Admin from the book or another
// cited source. Informational only; it is never turned into medical advice.
public sealed class NutritionFact
{
    public Guid Id { get; set; }

    public Guid FoodId { get; set; }

    public Food? Food { get; set; }

    public string ServingSize { get; set; } = string.Empty;

    public decimal EnergyKcal { get; set; }

    public decimal ProteinG { get; set; }

    public decimal CarbohydrateG { get; set; }

    public decimal FatG { get; set; }

    public decimal FiberG { get; set; }

    public string SourceReference { get; set; } = string.Empty;
}
