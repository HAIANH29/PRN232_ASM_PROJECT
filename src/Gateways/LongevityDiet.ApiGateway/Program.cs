var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

var publicRoutes = new[]
{
    new { Prefix = "/identity", Downstream = "Identity Service", Examples = new[] { "/identity/api/auth/login", "/identity/api/auth/profile" } },
    new { Prefix = "/diet-knowledge", Downstream = "Diet Knowledge Service", Examples = new[] { "/diet-knowledge/api/foods", "/diet-knowledge/api/recipes" } },
    new { Prefix = "/meal-planning", Downstream = "Meal Planning Service", Examples = new[] { "/meal-planning/api/meal-plans", "/meal-planning/api/meal-recommendations" } },
    new { Prefix = "/tracking", Downstream = "Tracking Service", Examples = new[] { "/tracking/api/daily-trackings", "/tracking/api/progress-summaries" } }
};

app.MapHealthChecks("/health");
app.MapGet("/", () => Results.Ok(new
{
    service = "Longevity Diet API Gateway",
    routing = "YARP reverse proxy",
    publicRoutes,
    internalServicesNotRouted = new[] { "recommendation-service", "notification-service", "notification-worker", "rabbitmq", "postgres" }
}));
app.MapGet("/routes", () => Results.Ok(publicRoutes));

// Service-to-service endpoints (for example the reminder check used by the Notification Worker)
// live under /internal on a service and must never be reachable through the public gateway.
app.Use(async (context, next) =>
{
    if (context.Request.Path.Value?.Contains("/internal/", StringComparison.OrdinalIgnoreCase) == true ||
        context.Request.Path.Value?.EndsWith("/internal", StringComparison.OrdinalIgnoreCase) == true)
    {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        return;
    }

    await next();
});

app.MapReverseProxy();

app.Run();
