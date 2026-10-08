using LongevityDiet.Identity.Application.Abstractions;
using LongevityDiet.Identity.Infrastructure.Persistence;
using LongevityDiet.Identity.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LongevityDiet.Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<IdentityDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("IdentityDb")));

        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
