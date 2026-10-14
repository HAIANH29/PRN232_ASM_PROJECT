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
    [Display(Name = "Plan name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Start date")]
    public DateOnly StartDate { get; set; }

    [Required]
    [Display(Name = "End date")]
    public DateOnly EndDate { get; set; }
}

public sealed class MealPlanItemForm
{
    [Required]
    [Display(Name = "Date")]
    public DateOnly PlannedDate { get; set; }

    [Required]
    [MaxLength(40)]
    [Display(Name = "Meal")]
    public string MealSlot { get; set; } = "Breakfast";

    public Guid? RecipeId { get; set; }

    public Guid? FoodId { get; set; }

    [MaxLength(1000)]
    [Display(Name = "Notes (optional)")]
    public string? Notes { get; set; }

    [Display(Name = "Reminder time (optional)")]
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

    public AcceptRecommendationForm AcceptForm { get; set; } = new();

    public IReadOnlyCollection<FoodResponse> Foods { get; set; } = Array.Empty<FoodResponse>();

    public IReadOnlyCollection<RecipeResponse> Recipes { get; set; } = Array.Empty<RecipeResponse>();
}

public sealed class RecommendationForm
{
    [Range(1, 14, ErrorMessage = "Choose between 1 and 14 meals.")]
    [Display(Name = "How many meal ideas?")]
    public int Days { get; set; } = 3;

    [Display(Name = "Your preferences")]
    public string PreferenceTagsText { get; set; } = string.Empty;
}

public sealed class AcceptRecommendationForm
{
    [Required(ErrorMessage = "Give your plan a name.")]
    [MaxLength(200)]
    [Display(Name = "Plan name")]
    public string Name { get; set; } = string.Empty;

    public List<AcceptMealRow> Meals { get; set; } = [];
}

public sealed class AcceptMealRow
{
    public bool Include { get; set; } = true;

    // The idea shown to the user; informational only.
    public string Title { get; set; } = string.Empty;

    public DateOnly PlannedDate { get; set; }

    [MaxLength(40)]
    public string MealSlot { get; set; } = "Lunch";

    // "recipe:{guid}" or "food:{guid}" so one dropdown covers both catalog types.
    public string? Choice { get; set; }
}
