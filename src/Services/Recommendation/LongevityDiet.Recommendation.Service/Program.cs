using LongevityDiet.Recommendation.Service;
using Microsoft.Extensions.Options;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<DietKnowledgeServiceOptions>(
    builder.Configuration.GetSection(DietKnowledgeServiceOptions.SectionName));
builder.Services.Configure<GeminiOptions>(
    builder.Configuration.GetSection(GeminiOptions.SectionName));

builder.Services.AddHttpClient<IApprovedKnowledgeClient, HttpApprovedKnowledgeClient>((serviceProvider, client) =>
{
    var options = serviceProvider.GetRequiredService<IOptions<DietKnowledgeServiceOptions>>().Value;
    client.BaseAddress = new Uri(EnsureTrailingSlash(options.BaseUrl));
});

builder.Services.AddHttpClient<IGeminiClient, GeminiClient>((serviceProvider, client) =>
{
    var options = serviceProvider.GetRequiredService<IOptions<GeminiOptions>>().Value;
    client.BaseAddress = new Uri(EnsureTrailingSlash(options.Endpoint));
    client.Timeout = TimeSpan.FromSeconds(Math.Clamp(options.TimeoutSeconds, 5, 120));
});

builder.Services.AddSingleton<IRecommendationGenerator, RecommendationGenerator>();
builder.Services.AddHostedService<RecommendationMessageWorker>();

var host = builder.Build();
host.Run();

static string EnsureTrailingSlash(string value)
{
    return value.EndsWith("/", StringComparison.Ordinal)
        ? value
        : $"{value}/";
}
