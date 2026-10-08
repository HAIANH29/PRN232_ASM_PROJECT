using LongevityDiet.MealPlanning.Application.Abstractions;
using LongevityDiet.MealPlanning.Domain.Entities;
using LongevityDiet.MealPlanning.Infrastructure.Persistence;

namespace LongevityDiet.MealPlanning.Infrastructure.Repositories;

public sealed class MealPlanRepository(MealPlanningDbContext dbContext) : IMealPlanRepository
{
    public IQueryable<MealPlan> Query()
    {
        return dbContext.MealPlans;
    }
}
