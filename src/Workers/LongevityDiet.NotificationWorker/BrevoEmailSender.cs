using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace LongevityDiet.NotificationWorker;

public sealed class BrevoEmailSender(
    HttpClient httpClient,
    IOptions<BrevoOptions> options,
    ILogger<BrevoEmailSender> logger) : IEmailSender
{
    private readonly BrevoOptions brevoOptions = options.Value;

    public async Task SendAsync(
        EmailMessage message,
        CancellationToken cancellationToken = default)
    {
        if (!IsConfigured(brevoOptions.ApiKey, brevoOptions.FromEmail))
        {
            logger.LogInformation(
                "Brevo is not configured. Email payload logged for {Category} {MessageId}: To={RecipientEmail}; Subject={Subject}; Body={Body}",
                message.Category,
                message.MessageId,
                message.RecipientEmail,
                message.Subject,
                message.Body);
            return;
        }

        ConfigureHttpClient();

        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            brevoOptions.EmailsEndpoint);

        request.Headers.TryAddWithoutValidation("api-key", brevoOptions.ApiKey);
        request.Headers.TryAddWithoutValidation("accept", "application/json");
        request.Content = JsonContent.Create(new BrevoEmailRequest(
            new BrevoContact(brevoOptions.FromEmail.Trim(), NullIfBlank(brevoOptions.FromName)),
            [new BrevoContact(message.RecipientEmail.Trim(), null)],
            message.Subject,
            message.Body));

        using var response = await httpClient.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new InvalidOperationException(
                $"Brevo returned HTTP {(int)response.StatusCode}: {Truncate(responseBody)}");
        }

        logger.LogInformation(
            "Email {MessageId} for {Category} sent to {RecipientEmail} through Brevo.",
            message.MessageId,
            message.Category,
            message.RecipientEmail);
    }

    private void ConfigureHttpClient()
    {
        if (httpClient.BaseAddress is null)
        {
            httpClient.BaseAddress = new Uri(EnsureTrailingSlash(brevoOptions.BaseUrl));
        }
    }

    private static bool IsConfigured(string apiKey, string fromEmail)
    {
        return !string.IsNullOrWhiteSpace(apiKey) &&
            !apiKey.Equals("development-only", StringComparison.OrdinalIgnoreCase) &&
            !string.IsNullOrWhiteSpace(fromEmail);
    }

    private static string? NullIfBlank(string value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private static string EnsureTrailingSlash(string value)
    {
        return value.EndsWith("/", StringComparison.Ordinal)
            ? value
            : $"{value}/";
    }

    private static string Truncate(string value)
    {
        return value.Length <= 500 ? value : value[..500];
    }

    private sealed record BrevoContact(
        string Email,
        [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? Name);

    private sealed record BrevoEmailRequest(
        BrevoContact Sender,
        IReadOnlyCollection<BrevoContact> To,
        string Subject,
        string TextContent);
}
