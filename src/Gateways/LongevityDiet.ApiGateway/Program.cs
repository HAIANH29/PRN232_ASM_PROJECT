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
app.MapReverseProxy();

app.Run();
