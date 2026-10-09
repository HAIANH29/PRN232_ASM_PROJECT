namespace LongevityDiet.MealPlanning.Infrastructure.DietKnowledge;

public sealed class DietKnowledgeServiceOptions
{
    public const string SectionName = "DietKnowledgeService";

    public string BaseUrl { get; set; } = "http://diet-knowledge-service:8080";
}
