namespace LongevityDiet.DietKnowledge.Application.Models;

public sealed record FoodModel(
    Guid Id,
    string Name,
    string Category,
    string CompatibilityNotes,
    string SourceTitle,
    string SourceChapter,
    string SourcePage,
    string SourceReference,
    string ReviewStatus,
    string ReviewedBy,
    DateTimeOffset? ReviewedAtUtc,
    bool IsActive,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc,
    IReadOnlyCollection<NutritionFactModel>? NutritionFacts = null);

public sealed record NutritionFactModel(
    Guid Id,
    string ServingSize,
    decimal EnergyKcal,
    decimal ProteinG,
    decimal CarbohydrateG,
    decimal FatG,
    decimal FiberG,
    string SourceReference);

public sealed record NutritionFactCommand(
    string ServingSize,
    decimal EnergyKcal,
    decimal ProteinG,
    decimal CarbohydrateG,
    decimal FatG,
    decimal FiberG,
    string SourceReference);

public sealed record CreateFoodCommand(
    string Name,
    string Category,
    string CompatibilityNotes,
    string SourceTitle,
    string SourceChapter,
    string SourcePage,
    string SourceReference,
    string ReviewStatus,
    string ReviewedBy,
    IReadOnlyCollection<NutritionFactCommand>? NutritionFacts = null);

public sealed record UpdateFoodCommand(
    string Name,
    string Category,
    string CompatibilityNotes,
    string SourceTitle,
    string SourceChapter,
    string SourcePage,
    string SourceReference,
    string ReviewStatus,
    string ReviewedBy,
    IReadOnlyCollection<NutritionFactCommand>? NutritionFacts = null);
