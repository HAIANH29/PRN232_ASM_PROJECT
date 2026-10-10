using LongevityDiet.DietKnowledge.Application.Abstractions;
using LongevityDiet.DietKnowledge.Infrastructure.Persistence;
using LongevityDiet.DietKnowledge.Infrastructure.Repositories;
using LongevityDiet.DietKnowledge.Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LongevityDiet.DietKnowledge.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddDietKnowledgeInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<DietKnowledgeDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DietKnowledgeDb"),
                npgsql => npgsql.EnableRetryOnFailure()));

        services.Configure<DietKnowledgeSeedOptions>(options =>
        {
            if (bool.TryParse(
                configuration["DietKnowledgeSeed:IncludeDemoApprovedContent"],
                out var includeDemoApprovedContent))
            {
                options.IncludeDemoApprovedContent = includeDemoApprovedContent;
            }
        });

        services.AddScoped<IDietKnowledgeRepository, DietKnowledgeRepository>();
        services.AddScoped<IDietKnowledgeSeeder, DietKnowledgeSeeder>();

        return services;
    }
}
