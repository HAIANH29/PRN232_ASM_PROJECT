namespace LongevityDiet.DietKnowledge.Application.Models;

public sealed record FoodModel(
    Guid Id,
    string Name,
    string Category,
    string CompatibilityNotes,
    bool IsActive,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc);

public sealed record CreateFoodCommand(
    string Name,
    string Category,
    string CompatibilityNotes);

public sealed record UpdateFoodCommand(
    string Name,
    string Category,
    string CompatibilityNotes);
