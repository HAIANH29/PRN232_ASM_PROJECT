using LongevityDiet.Tracking.Application.Abstractions;
using LongevityDiet.Tracking.Infrastructure.Persistence;
using LongevityDiet.Tracking.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LongevityDiet.Tracking.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddTrackingInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<TrackingDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("TrackingDb")));

        services.AddScoped<ITrackingRepository, TrackingRepository>();

        return services;
    }
}
