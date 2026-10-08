using LongevityDiet.DietKnowledge.Application.Abstractions;
using LongevityDiet.DietKnowledge.Infrastructure.Persistence;
using LongevityDiet.DietKnowledge.Infrastructure.Repositories;
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
            options.UseNpgsql(configuration.GetConnectionString("DietKnowledgeDb")));

        services.AddScoped<IDietKnowledgeRepository, DietKnowledgeRepository>();

        return services;
    }
}
