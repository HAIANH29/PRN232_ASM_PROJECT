namespace LongevityDiet.DietKnowledge.Application.Models;

public sealed record CategoryModel(
    Guid Id,
    string Name,
    string CategoryType,
    string Description);
