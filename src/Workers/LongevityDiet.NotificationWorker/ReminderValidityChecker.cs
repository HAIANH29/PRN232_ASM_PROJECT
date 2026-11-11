using System.Globalization;
using System.Text.Json;

namespace LongevityDiet.NotificationWorker;

// Asks Meal Planning whether a reminder that waited in a delay queue still matches its meal plan
// item. Deleted or rescheduled items make the reminder stale, so it must not be sent.
public interface IReminderValidityChecker
{
    Task<bool> IsCurrentAsync(
        Guid mealPlanItemId,
        DateTimeOffset scheduledForUtc,
        CancellationToken cancellationToken);
}

public sealed class HttpReminderValidityChecker(
    HttpClient httpClient,
    IConfiguration configuration,
    ILogger<HttpReminderValidityChecker> logger) : IReminderValidityChecker
{
    public async Task<bool> IsCurrentAsync(
        Guid mealPlanItemId,
        DateTimeOffset scheduledForUtc,
        CancellationToken cancellationToken)
    {
        var baseUrl = configuration["MealPlanningService:BaseUrl"];
        var key = configuration["InternalApi:Key"];
        if (string.IsNullOrWhiteSpace(baseUrl) || string.IsNullOrWhiteSpace(key))
        {
            return true;
        }

        try
        {
            var url = $"{baseUrl.TrimEnd('/')}/internal/reminders/{mealPlanItemId}" +
                $"?scheduledForUtc={Uri.EscapeDataString(scheduledForUtc.ToUniversalTime().ToString("O", CultureInfo.InvariantCulture))}";
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("X-Internal-Key", key);

            using var response = await httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            await using var body = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var document = await JsonDocument.ParseAsync(body, cancellationToken: cancellationToken);
            return document.RootElement.TryGetProperty("current", out var current) && current.GetBoolean();
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            // Fail open: a short Meal Planning outage must not swallow a real reminder.
            logger.LogWarning(
                exception,
                "Could not verify reminder for meal plan item {MealPlanItemId}; sending it anyway.",
                mealPlanItemId);
            return true;
        }
    }
}
