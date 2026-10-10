using System.ComponentModel.DataAnnotations;

namespace LongevityDiet.Web.Models;

public sealed record DietGuidelineResponse(
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
    DateTimeOffset? UpdatedAtUtc);

public sealed record FoodResponse(
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

public sealed record RecipeResponse(
    Guid Id,
    string Name,
    string Description,
    bool IsActive,
    string SourceTitle,
    string SourceChapter,
    string SourcePage,
    string SourceReference,
    string ReviewStatus,
    string ReviewedBy,
    DateTimeOffset? ReviewedAtUtc,
    IReadOnlyCollection<RecipeIngredientResponse> Ingredients,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc);

public sealed record RecipeIngredientResponse(
    Guid Id,
    Guid FoodId,
    string? FoodName,
    string? FoodCategory,
    string QuantityText);

public sealed class KnowledgeIndexViewModel
{
    public string? Search { get; set; }

    public string? Category { get; set; }

    public PagedResponse<DietGuidelineResponse> Guidelines { get; set; } = Empty<DietGuidelineResponse>();

    public PagedResponse<FoodResponse> Foods { get; set; } = Empty<FoodResponse>();

    public PagedResponse<RecipeResponse> Recipes { get; set; } = Empty<RecipeResponse>();

    public static PagedResponse<T> Empty<T>() => new(Array.Empty<T>(), 1, 20, 0);
}

public sealed class AdminKnowledgeViewModel
{
    public PagedResponse<DietGuidelineResponse> Guidelines { get; set; } = KnowledgeIndexViewModel.Empty<DietGuidelineResponse>();

    public PagedResponse<FoodResponse> Foods { get; set; } = KnowledgeIndexViewModel.Empty<FoodResponse>();

    public PagedResponse<RecipeResponse> Recipes { get; set; } = KnowledgeIndexViewModel.Empty<RecipeResponse>();

    public GuidelineForm Guideline { get; set; } = new();

    public FoodForm Food { get; set; } = new();

    public RecipeForm Recipe { get; set; } = new();
}

public sealed class GuidelineForm
{
    public Guid? Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Summary { get; set; } = string.Empty;

    [MaxLength(500)]
    public string SourceNote { get; set; } = string.Empty;

    [MaxLength(200)]
    public string SourceTitle { get; set; } = "The Longevity Diet";

    [MaxLength(200)]
    public string SourceChapter { get; set; } = string.Empty;

    [MaxLength(80)]
    public string SourcePage { get; set; } = string.Empty;

    [MaxLength(500)]
    public string SourceReference { get; set; } = string.Empty;

    [MaxLength(40)]
    public string ReviewStatus { get; set; } = "NeedsReview";

    [MaxLength(120)]
    public string ReviewedBy { get; set; } = string.Empty;
}

public sealed class FoodForm
{
    public Guid? Id { get; set; }

    [Required]
    [MaxLength(160)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(120)]
    public string Category { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string CompatibilityNotes { get; set; } = string.Empty;

    [MaxLength(200)]
    public string SourceTitle { get; set; } = "The Longevity Diet";

    [MaxLength(200)]
    public string SourceChapter { get; set; } = string.Empty;

    [MaxLength(80)]
    public string SourcePage { get; set; } = string.Empty;

    [MaxLength(500)]
    public string SourceReference { get; set; } = string.Empty;

    [MaxLength(40)]
    public string ReviewStatus { get; set; } = "NeedsReview";

    [MaxLength(120)]
    public string ReviewedBy { get; set; } = string.Empty;
}

public sealed class RecipeForm
{
    public Guid? Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(200)]
    public string SourceTitle { get; set; } = "The Longevity Diet";

    [MaxLength(200)]
    public string SourceChapter { get; set; } = string.Empty;

    [MaxLength(80)]
    public string SourcePage { get; set; } = string.Empty;

    [MaxLength(500)]
    public string SourceReference { get; set; } = string.Empty;

    [MaxLength(40)]
    public string ReviewStatus { get; set; } = "NeedsReview";

    [MaxLength(120)]
    public string ReviewedBy { get; set; } = string.Empty;

    [Required]
    public Guid IngredientFoodId { get; set; }

    [MaxLength(120)]
    public string IngredientQuantityText { get; set; } = "1 serving";
}

public sealed record RecipeIngredientRequest(
    Guid FoodId,
    string QuantityText);
