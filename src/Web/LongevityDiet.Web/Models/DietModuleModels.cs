using System.ComponentModel.DataAnnotations;

namespace LongevityDiet.Web.Models;

// ---- Responses from the Diet Knowledge, Tracking and Identity APIs for the newer modules.

public sealed record NutritionFactResponse(
    Guid Id,
    string ServingSize,
    decimal EnergyKcal,
    decimal ProteinG,
    decimal CarbohydrateG,
    decimal FatG,
    decimal FiberG,
    string SourceReference);

public sealed record RecipeStepResponse(
    Guid Id,
    int StepNumber,
    string Instruction);

public sealed record CategoryResponse(
    Guid Id,
    string Name,
    string CategoryType,
    string Description);

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

public sealed record KnowledgeSearchHitResponse(
    string SourceType,
    Guid SourceId,
    string Title,
    string Snippet,
    double Score);

public sealed record KnowledgeIndexRunResponse(
    Guid Id,
    DateTimeOffset StartedAtUtc,
    DateTimeOffset? CompletedAtUtc,
    int ItemsIndexed,
    int ItemsFailed,
    string Status);

public sealed record KnowledgeIndexStatusResponse(
    bool VectorDbConfigured,
    string EmbeddingModel,
    int Dimensions,
    int IndexedItems,
    KnowledgeIndexRunResponse? LatestRun);

public sealed record KnowledgeIndexResultResponse(
    Guid RunId,
    int Indexed,
    int Unchanged,
    int Removed,
    int Failed,
    string Status);

public sealed record UserDietTrackingResponse(
    Guid Id,
    Guid? DietRegimenId,
    Guid? MealPlanId,
    DateOnly StartDate,
    DateOnly? EndDate,
    string Status,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc);

// ---- View models and forms.

public sealed class PreferencesForm
{
    // Comma separated in the form; split before it is sent.
    [MaxLength(800)]
    [Display(Name = "Preference tags")]
    public string? Tags { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }
}

public sealed class ProfileViewModel
{
    public UserProfileResponse Profile { get; set; } = null!;

    public PreferencesForm Form { get; set; } = new();
}

public sealed class StartDietTrackingForm
{
    public Guid? DietRegimenId { get; set; }

    public Guid? MealPlanId { get; set; }

    [Required]
    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    public DateOnly? EndDate { get; set; }
}

public sealed class RegimensIndexViewModel
{
    public string? Search { get; set; }

    public PagedResponse<DietRegimenResponse> Regimens { get; set; } = KnowledgeIndexViewModel.Empty<DietRegimenResponse>();
}

public sealed class RegimenDetailsViewModel
{
    public DietRegimenResponse Regimen { get; set; } = null!;

    public IReadOnlyCollection<RecipeResponse> Recipes { get; set; } = Array.Empty<RecipeResponse>();

    public UserDietTrackingResponse? ActiveTracking { get; set; }

    public bool CanStartTracking { get; set; }

    public StartDietTrackingForm StartForm { get; set; } = new();

    public IReadOnlyCollection<MealPlanResponse> MealPlans { get; set; } = Array.Empty<MealPlanResponse>();
}

public sealed class KnowledgeSearchViewModel
{
    public string? Query { get; set; }

    public string? Type { get; set; }

    public IReadOnlyCollection<KnowledgeSearchHitResponse> Hits { get; set; } = Array.Empty<KnowledgeSearchHitResponse>();

    public string? Problem { get; set; }
}
