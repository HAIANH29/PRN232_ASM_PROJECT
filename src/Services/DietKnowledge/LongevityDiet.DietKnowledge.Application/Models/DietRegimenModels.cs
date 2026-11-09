namespace LongevityDiet.DietKnowledge.Application.Models;

public sealed record DietRegimenModel(
    Guid Id,
    string Name,
    string Description,
    int DurationDays,
    string SourceTitle,
    string SourceChapter,
    string SourcePage,
    string ReviewStatus,
    bool IsActive,
    IReadOnlyCollection<DietRegimenGuidelineModel> Guidelines,
    IReadOnlyCollection<DietRegimenMealModel> Meals,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc);

public sealed record DietRegimenGuidelineModel(
    Guid Id,
    Guid GuidelineId,
    string? GuidelineTitle,
    int SortOrder,
    string Note);

public sealed record DietRegimenMealModel(
    Guid Id,
    int DayNumber,
    string MealSlot,
    Guid RecipeId,
    string? RecipeName,
    string Notes);

public sealed record DietRegimenGuidelineCommand(
    Guid GuidelineId,
    int SortOrder,
    string Note);

public sealed record DietRegimenMealCommand(
    int DayNumber,
    string MealSlot,
    Guid RecipeId,
    string Notes);

public sealed record SaveDietRegimenCommand(
    string Name,
    string Description,
    int DurationDays,
    string SourceTitle,
    string SourceChapter,
    string SourcePage,
    string ReviewStatus,
    IReadOnlyCollection<DietRegimenGuidelineCommand> Guidelines,
    IReadOnlyCollection<DietRegimenMealCommand> Meals);

public sealed record DietRegimenQuery(
    int PageNumber,
    int PageSize,
    string? Search,
    bool IncludeInactive,
    bool ApprovedOnly,
    string? ReviewStatus);
