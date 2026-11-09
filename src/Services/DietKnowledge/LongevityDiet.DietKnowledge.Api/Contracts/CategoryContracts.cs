namespace LongevityDiet.DietKnowledge.Api.Contracts;

public sealed record CategoryResponse(
    Guid Id,
    string Name,
    string CategoryType,
    string Description);
