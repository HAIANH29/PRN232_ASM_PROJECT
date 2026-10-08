using LongevityDiet.Identity.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LongevityDiet.Identity.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityApplication(this IServiceCollection services)
    {
        services.AddScoped<IIdentityService, IdentityService>();

        return services;
    }
}
