using LongevityDiet.Contracts.Messaging;

namespace LongevityDiet.Recommendation.Service;

public sealed class RecommendationMessageWorker(
    IConfiguration configuration,
    ILogger<RecommendationMessageWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var requestQueue = configuration[$"{RabbitMqOptions.SectionName}:RecommendationRequestQueue"]
            ?? "recommendation.requests";
        var resultQueue = configuration[$"{RabbitMqOptions.SectionName}:RecommendationResultQueue"]
            ?? "recommendation.results";
        var geminiEnabled = configuration.GetValue<bool>($"{GeminiOptions.SectionName}:Enabled");

        logger.LogInformation(
            "Recommendation Service placeholder is ready. Consume {RequestQueue}, publish {ResultQueue}, Gemini enabled: {GeminiEnabled}.",
            requestQueue,
            resultQueue,
            geminiEnabled);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}
