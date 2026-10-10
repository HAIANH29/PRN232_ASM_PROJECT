using System.ComponentModel.DataAnnotations;

namespace LongevityDiet.Web.Models;

public sealed record MealPlanResponse(
    Guid Id,
    Guid UserId,
    string Name,
    DateOnly StartDate,
    DateOnly EndDate,
    IReadOnlyCollection<MealPlanItemResponse> Items,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc);

public sealed record MealPlanItemResponse(
    Guid Id,
    DateOnly PlannedDate,
    string MealSlot,
    Guid? RecipeId,
    Guid? FoodId,
    string Notes,
    DateTimeOffset ReminderAtUtc,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc);

public sealed class MealPlansIndexViewModel
{
    public PagedResponse<MealPlanResponse> MealPlans { get; set; } = KnowledgeIndexViewModel.Empty<MealPlanResponse>();

    public MealPlanForm Form { get; set; } = new()
    {
        StartDate = DateOnly.FromDateTime(DateTime.Today),
        EndDate = DateOnly.FromDateTime(DateTime.Today.AddDays(6))
    };
}

public sealed class MealPlanDetailsViewModel
{
    public MealPlanResponse MealPlan { get; set; } = default!;

    public MealPlanForm EditForm { get; set; } = new();

    public MealPlanItemForm ItemForm { get; set; } = new()
    {
        PlannedDate = DateOnly.FromDateTime(DateTime.Today),
        MealSlot = "Breakfast"
    };

    public IReadOnlyCollection<FoodResponse> Foods { get; set; } = Array.Empty<FoodResponse>();

    public IReadOnlyCollection<RecipeResponse> Recipes { get; set; } = Array.Empty<RecipeResponse>();
}

public sealed class MealPlanForm
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public DateOnly StartDate { get; set; }

    [Required]
    public DateOnly EndDate { get; set; }
}

public sealed class MealPlanItemForm
{
    [Required]
    public DateOnly PlannedDate { get; set; }

    [Required]
    [MaxLength(40)]
    public string MealSlot { get; set; } = "Breakfast";

    public Guid? RecipeId { get; set; }

    public Guid? FoodId { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }

    public DateTimeOffset? ReminderAtUtc { get; set; }
}

public sealed record RecommendationRequestResponse(
    Guid Id,
    Guid UserId,
    IReadOnlyCollection<string> PreferenceTags,
    int Days,
    string Status,
    IReadOnlyCollection<string> SuggestedMealTitles,
    string Disclaimer,
    Guid? AcceptedMealPlanId,
    DateTimeOffset RequestedAtUtc,
    DateTimeOffset? CompletedAtUtc,
    DateTimeOffset? UpdatedAtUtc);

public sealed class RecommendationsIndexViewModel
{
    public PagedResponse<RecommendationRequestResponse> Requests { get; set; } =
        KnowledgeIndexViewModel.Empty<RecommendationRequestResponse>();

    public RecommendationForm Form { get; set; } = new();
}

public sealed class RecommendationDetailsViewModel
{
    public RecommendationRequestResponse Request { get; set; } = default!;

    public AcceptRecommendationForm AcceptForm { get; set; } = new()
    {
        StartDate = DateOnly.FromDateTime(DateTime.Today),
        EndDate = DateOnly.FromDateTime(DateTime.Today.AddDays(6)),
        PlannedDate = DateOnly.FromDateTime(DateTime.Today),
        MealSlot = "Breakfast"
    };

    public IReadOnlyCollection<FoodResponse> Foods { get; set; } = Array.Empty<FoodResponse>();

    public IReadOnlyCollection<RecipeResponse> Recipes { get; set; } = Array.Empty<RecipeResponse>();
}

public sealed class RecommendationForm
{
    [Range(1, 14)]
    public int Days { get; set; } = 7;

    [Display(Name = "Preference tags")]
    public string PreferenceTagsText { get; set; } = "plant focused, simple";
}

public sealed class AcceptRecommendationForm
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public DateOnly StartDate { get; set; }

    [Required]
    public DateOnly EndDate { get; set; }

    [Required]
    public DateOnly PlannedDate { get; set; }

    [Required]
    [MaxLength(40)]
    public string MealSlot { get; set; } = "Breakfast";

    public Guid? RecipeId { get; set; }

    public Guid? FoodId { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }
}
