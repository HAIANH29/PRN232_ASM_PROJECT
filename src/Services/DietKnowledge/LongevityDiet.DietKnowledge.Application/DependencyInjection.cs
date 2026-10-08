using LongevityDiet.DietKnowledge.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LongevityDiet.DietKnowledge.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddDietKnowledgeApplication(this IServiceCollection services)
    {
        services.AddScoped<IDietKnowledgeService, DietKnowledgeService>();

        return services;
    }
}
