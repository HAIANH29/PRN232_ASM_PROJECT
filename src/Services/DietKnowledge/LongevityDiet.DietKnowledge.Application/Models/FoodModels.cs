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
    DateTimeOffset? UpdatedAtUtc);

public sealed record CreateFoodCommand(
    string Name,
    string Category,
    string CompatibilityNotes,
    string SourceTitle,
    string SourceChapter,
    string SourcePage,
    string SourceReference,
    string ReviewStatus,
    string ReviewedBy);

public sealed record UpdateFoodCommand(
    string Name,
    string Category,
    string CompatibilityNotes,
    string SourceTitle,
    string SourceChapter,
    string SourcePage,
    string SourceReference,
    string ReviewStatus,
    string ReviewedBy);
