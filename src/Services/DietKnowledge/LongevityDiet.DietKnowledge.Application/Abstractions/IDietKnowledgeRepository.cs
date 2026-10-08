using LongevityDiet.DietKnowledge.Domain.Entities;

namespace LongevityDiet.DietKnowledge.Application.Abstractions;

public interface IDietKnowledgeRepository
{
    IQueryable<DietGuideline> Guidelines();

    IQueryable<Food> Foods();

    IQueryable<Recipe> Recipes();
}
