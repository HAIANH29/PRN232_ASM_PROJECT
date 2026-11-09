using System.ComponentModel.DataAnnotations;
using LongevityDiet.DietKnowledge.Application.Models;

namespace LongevityDiet.DietKnowledge.Api.Contracts;

public sealed record DietRegimenListRequest : ListRequest
{
    public bool IncludeInactive { get; init; }

    [MaxLength(40)]
    public string? ReviewStatus { get; init; }
}

public sealed record DietRegimenResponse(
    Guid Id,
    string Name,
    string Description,
    int DurationDays,
    string SourceTitle,
    string SourceChapter,
    string SourcePage,
    string ReviewStatus,
    bool IsActive,
    IReadOnlyCollection<DietRegimenGuidelineResponse> Guidelines,
    IReadOnlyCollection<DietRegimenMealResponse> Meals,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc);

public sealed record DietRegimenGuidelineResponse(
    Guid Id,
    Guid GuidelineId,
    string? GuidelineTitle,
    int SortOrder,
    string Note);

public sealed record DietRegimenMealResponse(
    Guid Id,
    int DayNumber,
    string MealSlot,
    Guid RecipeId,
    string? RecipeName,
    string Notes);

public sealed record DietRegimenGuidelineRequest
{
    [Required]
    public Guid GuidelineId { get; init; }

    [Range(0, 1000)]
    public int SortOrder { get; init; }

    [MaxLength(500)]
    public string Note { get; init; } = string.Empty;
}

public sealed record DietRegimenMealRequest
{
    [Range(1, 365)]
    public int DayNumber { get; init; }

    [Required]
    [MaxLength(40)]
    public string MealSlot { get; init; } = string.Empty;

    [Required]
    public Guid RecipeId { get; init; }

    [MaxLength(500)]
    public string Notes { get; init; } = string.Empty;
}

public sealed record SaveDietRegimenRequest
{
    [Required]
    [MaxLength(200)]
    public string Name { get; init; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Description { get; init; } = string.Empty;

    [Range(1, 365)]
    public int DurationDays { get; init; } = 1;

    [MaxLength(200)]
    public string SourceTitle { get; init; } = "The Longevity Diet";

    [MaxLength(200)]
    public string SourceChapter { get; init; } = string.Empty;

    [MaxLength(80)]
    public string SourcePage { get; init; } = string.Empty;

    [MaxLength(40)]
    public string ReviewStatus { get; init; } = "NeedsReview";

    public IReadOnlyCollection<DietRegimenGuidelineRequest> Guidelines { get; init; } =
        Array.Empty<DietRegimenGuidelineRequest>();

    public IReadOnlyCollection<DietRegimenMealRequest> Meals { get; init; } =
        Array.Empty<DietRegimenMealRequest>();
}

internal static class DietRegimenContractMapper
{
    public static DietRegimenQuery ToQuery(this DietRegimenListRequest request, bool forceActiveOnly)
    {
        return new DietRegimenQuery(
            request.PageNumber,
            request.PageSize,
            request.Search,
            IncludeInactive: !forceActiveOnly && request.IncludeInactive,
            ApprovedOnly: forceActiveOnly,
            request.ReviewStatus);
    }

    public static SaveDietRegimenCommand ToCommand(this SaveDietRegimenRequest request)
    {
        return new SaveDietRegimenCommand(
            request.Name,
            request.Description,
            request.DurationDays,
            request.SourceTitle,
            request.SourceChapter,
            request.SourcePage,
            request.ReviewStatus,
            request.Guidelines
                .Select(link => new DietRegimenGuidelineCommand(link.GuidelineId, link.SortOrder, link.Note))
                .ToArray(),
            request.Meals
                .Select(meal => new DietRegimenMealCommand(
                    meal.DayNumber,
                    meal.MealSlot,
                    meal.RecipeId,
                    meal.Notes))
                .ToArray());
    }

    public static DietRegimenResponse ToResponse(this DietRegimenModel model)
    {
        return new DietRegimenResponse(
            model.Id,
            model.Name,
            model.Description,
            model.DurationDays,
            model.SourceTitle,
            model.SourceChapter,
            model.SourcePage,
            model.ReviewStatus,
            model.IsActive,
            model.Guidelines
                .Select(link => new DietRegimenGuidelineResponse(
                    link.Id,
                    link.GuidelineId,
                    link.GuidelineTitle,
                    link.SortOrder,
                    link.Note))
                .ToArray(),
            model.Meals
                .Select(meal => new DietRegimenMealResponse(
                    meal.Id,
                    meal.DayNumber,
                    meal.MealSlot,
                    meal.RecipeId,
                    meal.RecipeName,
                    meal.Notes))
                .ToArray(),
            model.CreatedAtUtc,
            model.UpdatedAtUtc);
    }
}
