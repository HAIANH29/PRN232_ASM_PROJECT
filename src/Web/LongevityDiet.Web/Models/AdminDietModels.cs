using System.ComponentModel.DataAnnotations;

namespace LongevityDiet.Web.Models;

public sealed class NutritionFactForm
{
    [Required]
    [MaxLength(120)]
    [Display(Name = "Serving size")]
    public string ServingSize { get; set; } = "100 g";

    [Range(0, 100000)]
    [Display(Name = "Energy (kcal)")]
    public decimal EnergyKcal { get; set; }

    [Range(0, 100000)]
    [Display(Name = "Protein (g)")]
    public decimal ProteinG { get; set; }

    [Range(0, 100000)]
    [Display(Name = "Carbohydrate (g)")]
    public decimal CarbohydrateG { get; set; }

    [Range(0, 100000)]
    [Display(Name = "Fat (g)")]
    public decimal FatG { get; set; }

    [Range(0, 100000)]
    [Display(Name = "Fiber (g)")]
    public decimal FiberG { get; set; }

    [MaxLength(500)]
    [Display(Name = "Source reference")]
    public string? SourceReference { get; set; } = string.Empty;
}

public sealed class RecipeStepForm
{
    [Range(1, 1000)]
    [Display(Name = "Step number")]
    public int StepNumber { get; set; } = 1;

    [Required]
    [MaxLength(2000)]
    public string Instruction { get; set; } = string.Empty;
}

public sealed class RegimenForm
{
    public Guid? Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Range(1, 365)]
    [Display(Name = "Duration (days)")]
    public int DurationDays { get; set; } = 14;

    [MaxLength(200)]
    public string? SourceTitle { get; set; } = "The Longevity Diet";

    [MaxLength(200)]
    [Display(Name = "Source chapter")]
    public string? SourceChapter { get; set; } = string.Empty;

    [MaxLength(80)]
    [Display(Name = "Source page")]
    public string? SourcePage { get; set; } = string.Empty;

    [MaxLength(40)]
    [Display(Name = "Review status")]
    public string ReviewStatus { get; set; } = "NeedsReview";
}

public sealed class RegimenGuidelineForm
{
    [Required]
    [Display(Name = "Guideline")]
    public Guid GuidelineId { get; set; }

    [Range(0, 1000)]
    [Display(Name = "Order")]
    public int SortOrder { get; set; } = 1;

    [MaxLength(500)]
    public string? Note { get; set; } = string.Empty;
}

public sealed class RegimenMealForm
{
    [Range(1, 365)]
    [Display(Name = "Day")]
    public int DayNumber { get; set; } = 1;

    [Required]
    [MaxLength(40)]
    [Display(Name = "Meal")]
    public string MealSlot { get; set; } = "Lunch";

    [Required]
    [Display(Name = "Recipe")]
    public Guid RecipeId { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; } = string.Empty;
}

public sealed class AdminRegimensIndexViewModel
{
    public PagedResponse<DietRegimenResponse> Regimens { get; set; } = KnowledgeIndexViewModel.Empty<DietRegimenResponse>();

    public RegimenForm Form { get; set; } = new();
}

public sealed class AdminRegimenEditViewModel
{
    public DietRegimenResponse Regimen { get; set; } = null!;

    public RegimenForm Form { get; set; } = new();

    public IReadOnlyCollection<DietGuidelineResponse> Guidelines { get; set; } = Array.Empty<DietGuidelineResponse>();

    public IReadOnlyCollection<RecipeResponse> Recipes { get; set; } = Array.Empty<RecipeResponse>();

    public RegimenGuidelineForm GuidelineForm { get; set; } = new();

    public RegimenMealForm MealForm { get; set; } = new();
}
