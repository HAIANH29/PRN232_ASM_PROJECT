using LongevityDiet.MealPlanning.Domain.Entities;

namespace LongevityDiet.MealPlanning.Application.Abstractions;

public interface IMealPlanRepository
{
    IQueryable<MealPlan> Query();
}
