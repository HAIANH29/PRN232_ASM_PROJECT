namespace LongevityDiet.DietKnowledge.Domain.Entities;

public sealed class DietRegimenGuideline
{
    public Guid Id { get; set; }

    public Guid DietRegimenId { get; set; }

    public DietRegimen? DietRegimen { get; set; }

    public Guid GuidelineId { get; set; }

    public DietGuideline? Guideline { get; set; }

    public int SortOrder { get; set; }

    public string Note { get; set; } = string.Empty;
}
