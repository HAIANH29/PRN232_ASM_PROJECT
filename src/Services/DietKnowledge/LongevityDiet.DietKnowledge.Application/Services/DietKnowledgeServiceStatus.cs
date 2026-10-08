namespace LongevityDiet.DietKnowledge.Application.Services;

public sealed record DietKnowledgeServiceStatus(
    string Service,
    string Database,
    IReadOnlyCollection<string> Owns);
