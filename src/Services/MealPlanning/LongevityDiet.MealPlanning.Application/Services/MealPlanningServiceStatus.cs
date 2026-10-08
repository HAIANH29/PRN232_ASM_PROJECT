namespace LongevityDiet.MealPlanning.Application.Services;

public sealed record MealPlanningServiceStatus(
    string Service,
    string Database,
    IReadOnlyCollection<string> Owns,
    IReadOnlyCollection<string> Integrations);
