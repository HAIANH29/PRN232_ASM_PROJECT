using LongevityDiet.DietKnowledge.Application.Abstractions;
using LongevityDiet.DietKnowledge.Domain.Entities;
using LongevityDiet.DietKnowledge.Infrastructure.Persistence;

namespace LongevityDiet.DietKnowledge.Infrastructure.Repositories;

public sealed class DietKnowledgeRepository(DietKnowledgeDbContext dbContext) : IDietKnowledgeRepository
{
    public IQueryable<DietGuideline> Guidelines()
    {
        return dbContext.DietGuidelines;
    }

    public IQueryable<Food> Foods()
    {
        return dbContext.Foods;
    }

    public IQueryable<Recipe> Recipes()
    {
        return dbContext.Recipes;
    }
}
