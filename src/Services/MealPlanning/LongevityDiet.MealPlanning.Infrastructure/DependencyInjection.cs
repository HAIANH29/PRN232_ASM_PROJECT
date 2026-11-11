using LongevityDiet.MealPlanning.Application.Abstractions;
using LongevityDiet.MealPlanning.Application.Common;
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

        // "Today" and "meal time has passed" are judged in the users' local time (minutes east of UTC).
        var offsetMinutes = int.TryParse(configuration["MealPlanning:UtcOffsetMinutes"], out var configuredOffset)
            ? configuredOffset
            : (int)MealSchedulePolicy.DefaultUtcOffset.TotalMinutes;
        services.AddSingleton(TimeProvider.System);
        services.AddSingleton(new MealSchedulePolicy(TimeSpan.FromMinutes(offsetMinutes)));

        services.AddScoped<IMealPlanRepository, MealPlanRepository>();
        services.AddScoped<IReminderPublisher, RabbitMqReminderPublisher>();
        services.AddScoped<IRecommendationRequestPublisher, RabbitMqRecommendationRequestPublisher>();
        services.AddScoped<IRecommendationResultConsumer, RabbitMqRecommendationResultConsumer>();
        services.AddScoped<IMealPlanOutbox, EfMealPlanOutbox>();
        services.AddScoped<OutboxDispatcher>();
        services.AddHostedService<OutboxDispatcherWorker>();
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
