using LongevityDiet.Tracking.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LongevityDiet.Tracking.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddTrackingApplication(this IServiceCollection services)
    {
        services.AddScoped<ITrackingService, TrackingService>();

        return services;
    }
}
