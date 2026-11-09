namespace LongevityDiet.DietKnowledge.Domain.Entities;

// A named eating pattern taken from the book (for example a multi-day menu), made of
// guideline rules and scheduled meals. Educational content only.
public sealed class DietRegimen
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int DurationDays { get; set; }

    public string SourceTitle { get; set; } = "The Longevity Diet";

    public string SourceChapter { get; set; } = string.Empty;

    public string SourcePage { get; set; } = string.Empty;

    public string ReviewStatus { get; set; } = "NeedsReview";

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? UpdatedAtUtc { get; set; }

    public ICollection<DietRegimenGuideline> Guidelines { get; } = new List<DietRegimenGuideline>();

    public ICollection<DietRegimenMeal> Meals { get; } = new List<DietRegimenMeal>();
}
