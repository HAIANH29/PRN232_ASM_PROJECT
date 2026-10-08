namespace LongevityDiet.DietKnowledge.Application.Models;

public sealed record DietGuidelineModel(
    Guid Id,
    string Title,
    string Summary,
    string SourceNote,
    bool IsActive,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc);

public sealed record CreateDietGuidelineCommand(
    string Title,
    string Summary,
    string SourceNote);

public sealed record UpdateDietGuidelineCommand(
    string Title,
    string Summary,
    string SourceNote);
