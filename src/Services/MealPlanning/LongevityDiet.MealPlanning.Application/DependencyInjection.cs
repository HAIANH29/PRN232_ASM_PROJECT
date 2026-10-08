using LongevityDiet.MealPlanning.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LongevityDiet.MealPlanning.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddMealPlanningApplication(this IServiceCollection services)
    {
        services.AddScoped<IMealPlanningService, MealPlanningService>();

        return services;
    }
}
