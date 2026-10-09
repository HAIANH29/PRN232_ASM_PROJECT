using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;

namespace LongevityDiet.NotificationWorker;

public sealed class ResendEmailSender(
    HttpClient httpClient,
    IOptions<ResendOptions> options,
    ILogger<ResendEmailSender> logger) : IEmailSender
{
    private readonly ResendOptions resendOptions = options.Value;

    public async Task SendAsync(
        EmailMessage message,
        CancellationToken cancellationToken = default)
    {
        if (!IsConfigured(resendOptions.ApiKey))
        {
            logger.LogInformation(
                "Resend is not configured. Email payload logged for {Category} {MessageId}: To={RecipientEmail}; Subject={Subject}; Body={Body}",
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
            resendOptions.EmailsEndpoint);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", resendOptions.ApiKey);
        request.Headers.TryAddWithoutValidation("Idempotency-Key", $"{message.Category}/{message.MessageId}");
        request.Content = JsonContent.Create(new ResendEmailRequest(
            FormatSender(resendOptions.FromName, resendOptions.FromEmail),
            [message.RecipientEmail],
            message.Subject,
            message.Body));

        using var response = await httpClient.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new InvalidOperationException(
                $"Resend returned HTTP {(int)response.StatusCode}: {Truncate(responseBody)}");
        }

        logger.LogInformation(
            "Email {MessageId} for {Category} sent to {RecipientEmail} through Resend.",
            message.MessageId,
            message.Category,
            message.RecipientEmail);
    }

    private void ConfigureHttpClient()
    {
        if (httpClient.BaseAddress is null)
        {
            httpClient.BaseAddress = new Uri(EnsureTrailingSlash(resendOptions.BaseUrl));
        }
    }

    private static bool IsConfigured(string apiKey)
    {
        return !string.IsNullOrWhiteSpace(apiKey) &&
            !apiKey.Equals("development-only", StringComparison.OrdinalIgnoreCase);
    }

    private static string FormatSender(string fromName, string fromEmail)
    {
        return string.IsNullOrWhiteSpace(fromName)
            ? fromEmail.Trim()
            : $"{fromName.Trim()} <{fromEmail.Trim()}>";
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

    private sealed record ResendEmailRequest(
        string From,
        IReadOnlyCollection<string> To,
        string Subject,
        string Text);
}
