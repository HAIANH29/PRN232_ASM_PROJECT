using LongevityDiet.MealPlanning.Application.Abstractions;
using LongevityDiet.MealPlanning.Infrastructure.Messaging;
using LongevityDiet.MealPlanning.Infrastructure.Persistence;
using LongevityDiet.MealPlanning.Infrastructure.Recommendation;
using LongevityDiet.MealPlanning.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LongevityDiet.MealPlanning.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddMealPlanningInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<MealPlanningDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("MealPlanningDb")));

        services.AddScoped<IMealPlanRepository, MealPlanRepository>();
        services.AddScoped<IReminderPublisher, RabbitMqReminderPublisher>();
        services.AddScoped<IRecommendationClient, GrpcRecommendationClient>();

        return services;
    }
}
