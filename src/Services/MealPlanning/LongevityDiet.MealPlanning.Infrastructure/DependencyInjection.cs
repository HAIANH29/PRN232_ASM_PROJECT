using LongevityDiet.MealPlanning.Application.Abstractions;
using LongevityDiet.MealPlanning.Infrastructure.DietKnowledge;
using LongevityDiet.MealPlanning.Infrastructure.Messaging;
using LongevityDiet.MealPlanning.Infrastructure.Persistence;
using LongevityDiet.MealPlanning.Infrastructure.Repositories;
using LongevityDiet.MealPlanning.Infrastructure.Seed;
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
        services.AddScoped<IRecommendationRequestPublisher, RabbitMqRecommendationRequestPublisher>();
        services.AddScoped<IRecommendationResultConsumer, RabbitMqRecommendationResultConsumer>();
        services.AddScoped<IMealPlanningDatabaseInitializer, MealPlanningDatabaseInitializer>();

        services.AddHttpClient<IDietKnowledgeCatalogClient, HttpDietKnowledgeCatalogClient>(client =>
        {
            var baseUrl = configuration[$"{DietKnowledgeServiceOptions.SectionName}:BaseUrl"]
                ?? "http://diet-knowledge-service:8080";
            client.BaseAddress = new Uri(baseUrl);
        });

        services.AddHostedService<RabbitMqRecommendationResultWorker>();

        return services;
    }
}
