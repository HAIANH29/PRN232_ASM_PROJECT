var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.MapHealthChecks("/health");
app.MapGet("/", () => Results.Ok(new
{
    service = "Longevity Diet API Gateway",
    routing = "YARP reverse proxy",
    downstream = new[] { "identity", "diet-knowledge", "meal-planning", "tracking" }
}));
app.MapReverseProxy();

app.Run();
