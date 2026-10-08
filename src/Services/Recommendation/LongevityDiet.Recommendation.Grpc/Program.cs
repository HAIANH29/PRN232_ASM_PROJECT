using LongevityDiet.Recommendation.Grpc.Options;
using LongevityDiet.Recommendation.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AiProviderOptions>(
    builder.Configuration.GetSection(AiProviderOptions.SectionName));
builder.Services.AddSingleton<IRecommendationProvider, PlaceholderRecommendationProvider>();
builder.Services.AddGrpc();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapGrpcService<RecommendationGrpcService>();
app.MapHealthChecks("/health");
app.MapGet("/", () => "Recommendation Service uses gRPC. Call RecommendMealPlan from Meal Planning Service.");

app.Run();
