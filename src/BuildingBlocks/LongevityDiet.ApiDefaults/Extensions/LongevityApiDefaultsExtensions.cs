using LongevityDiet.ApiDefaults.Api;
using LongevityDiet.ApiDefaults.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace LongevityDiet.ApiDefaults.Extensions;

public static class LongevityApiDefaultsExtensions
{
    public static IServiceCollection AddLongevityPublicApiDefaults(
        this IServiceCollection services,
        string serviceName,
        IConfiguration configuration)
    {
        services.AddControllers();
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = CreateValidationErrorResponse;
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = serviceName,
                Version = "v1"
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter a JWT Bearer token. Example: Bearer {token}",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        services.AddHealthChecks();
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var issuer = configuration["Jwt:Issuer"] ?? "LongevityDiet.Identity";
                var audience = configuration["Jwt:Audience"] ?? "LongevityDiet.Platform";
                var signingKey = configuration["Jwt:SigningKey"] ?? "development-only-replace-with-secret";

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(2)
                };
            });
        services.AddAuthorization();

        return services;
    }

    public static WebApplication UseLongevityPublicApiDefaults(this WebApplication app)
    {
        app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapHealthChecks("/health");
        app.MapControllers();

        return app;
    }

    private static IActionResult CreateValidationErrorResponse(ActionContext context)
    {
        var errors = context.ModelState
            .Where(entry => entry.Value?.Errors.Count > 0)
            .SelectMany(entry => entry.Value!.Errors.Select(error => new ApiError(
                Code: "Validation.Invalid",
                Message: string.IsNullOrWhiteSpace(error.ErrorMessage)
                    ? "The input is invalid."
                    : error.ErrorMessage,
                Target: string.IsNullOrWhiteSpace(entry.Key) ? null : entry.Key)))
            .ToArray();

        var response = ApiResponse<object?>.Failure(
            errors,
            "Validation failed.",
            context.HttpContext.TraceIdentifier);

        return new BadRequestObjectResult(response)
        {
            ContentTypes = { "application/json" }
        };
    }
}
