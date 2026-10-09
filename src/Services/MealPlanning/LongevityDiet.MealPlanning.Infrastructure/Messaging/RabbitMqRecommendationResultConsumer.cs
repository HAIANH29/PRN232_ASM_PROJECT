using System.Text.Json;
using LongevityDiet.Contracts.Messaging;
using LongevityDiet.MealPlanning.Application.Abstractions;
using LongevityDiet.MealPlanning.Application.Common;

namespace LongevityDiet.MealPlanning.Infrastructure.Messaging;

public sealed class RabbitMqRecommendationResultConsumer(
    IMealPlanRepository repository) : IRecommendationResultConsumer
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task HandleAsync(
        RecommendationResultMessage message,
        CancellationToken cancellationToken = default)
    {
        var request = await repository.GetRecommendationRequestForUpdateAsync(
            message.RequestId,
            cancellationToken);

        if (request is null || request.UserId != message.UserId)
        {
            return;
        }

        if (request.Status == RecommendationRequestStatuses.Accepted)
        {
            return;
        }

        request.Status = RecommendationRequestStatuses.Completed;
        request.SuggestedMealTitlesJson = JsonSerializer.Serialize(
            message.SuggestedMealTitles,
            JsonOptions);
        request.Disclaimer = message.Disclaimer;
        request.CompletedAtUtc = message.CompletedAtUtc;
        request.UpdatedAtUtc = DateTimeOffset.UtcNow;

        await repository.SaveChangesAsync(cancellationToken);
    }
}
