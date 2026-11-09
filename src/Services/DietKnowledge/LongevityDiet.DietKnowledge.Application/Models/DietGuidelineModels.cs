namespace LongevityDiet.DietKnowledge.Application.Models;

public sealed record DietGuidelineModel(
    Guid Id,
    string Title,
    string Summary,
    string SourceNote,
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
    string? Category = null);

public sealed record CreateDietGuidelineCommand(
    string Title,
    string Summary,
    string SourceNote,
    string SourceTitle,
    string SourceChapter,
    string SourcePage,
    string SourceReference,
    string ReviewStatus,
    string ReviewedBy,
    string? Category = null);

public sealed record UpdateDietGuidelineCommand(
    string Title,
    string Summary,
    string SourceNote,
    string SourceTitle,
    string SourceChapter,
    string SourcePage,
    string SourceReference,
    string ReviewStatus,
    string ReviewedBy,
    string? Category = null);

public sealed record DeleteAllKnowledgeResultModel(
    int GuidelinesDeleted,
    int FoodsDeleted,
    int RecipesDeleted,
    int CandidatesReset);
