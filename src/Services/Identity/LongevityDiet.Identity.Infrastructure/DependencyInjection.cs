using LongevityDiet.Identity.Application.Abstractions;
using LongevityDiet.Identity.Infrastructure.Authentication;
using LongevityDiet.Identity.Infrastructure.Persistence;
using LongevityDiet.Identity.Infrastructure.Repositories;
using LongevityDiet.Identity.Infrastructure.Seed;
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
            options.UseNpgsql(
                configuration.GetConnectionString("IdentityDb"),
                npgsql => npgsql.EnableRetryOnFailure()));

        services.Configure<JwtOptions>(options =>
        {
            options.Issuer = configuration["Jwt:Issuer"] ?? options.Issuer;
            options.Audience = configuration["Jwt:Audience"] ?? options.Audience;
            options.SigningKey = configuration["Jwt:SigningKey"] ?? options.SigningKey;

            if (int.TryParse(configuration["Jwt:AccessTokenMinutes"], out var accessTokenMinutes))
            {
                options.AccessTokenMinutes = accessTokenMinutes;
            }
        });

        services.Configure<IdentitySeedOptions>(options =>
        {
            options.AdminEmail = configuration["IdentitySeed:AdminEmail"] ?? options.AdminEmail;
            options.AdminPassword = configuration["IdentitySeed:AdminPassword"] ?? options.AdminPassword;
            options.AdminDisplayName = configuration["IdentitySeed:AdminDisplayName"] ?? options.AdminDisplayName;
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IIdentitySeeder, IdentitySeeder>();
        services.AddSingleton<IPasswordHasher, Pbkdf2PasswordHasher>();

        return services;
    }
}
