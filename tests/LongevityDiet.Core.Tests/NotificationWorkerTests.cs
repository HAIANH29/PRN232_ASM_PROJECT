using LongevityDiet.NotificationWorker;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Xunit;

namespace LongevityDiet.Core.Tests;

public sealed class NotificationWorkerTests
{
    [Fact]
    public async Task BrevoEmailSender_logs_locally_without_http_when_brevo_is_not_configured()
    {
        var handler = new RecordingHttpMessageHandler();
        var sender = new BrevoEmailSender(
            new HttpClient(handler),
            Options.Create(new BrevoOptions { ApiKey = "development-only", FromEmail = "sender@example.com" }),
            NullLogger<BrevoEmailSender>.Instance);

        await sender.SendAsync(new EmailMessage(
            Guid.NewGuid(),
            "user@example.com",
            "Subject",
            "Body",
            "reminder"));

        Assert.Empty(handler.Requests);
    }

    [Fact]
    public async Task BrevoEmailSender_sends_configured_request_with_api_key()
    {
        var handler = new RecordingHttpMessageHandler();
        var sender = new BrevoEmailSender(
            new HttpClient(handler),
            Options.Create(new BrevoOptions
            {
                BaseUrl = "https://api.brevo.test",
                EmailsEndpoint = "smtp/email",
                ApiKey = "real-key",
                FromEmail = "sender@example.com",
                FromName = "Longevity Diet Platform"
            }),
            NullLogger<BrevoEmailSender>.Instance);
        var messageId = Guid.NewGuid();

        await sender.SendAsync(new EmailMessage(
            messageId,
            "user@example.com",
            "Meal reminder",
            "Body",
            "reminder"));

        var request = Assert.Single(handler.Requests);
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal("https://api.brevo.test/smtp/email", request.RequestUri?.ToString());
        Assert.True(request.Headers.TryGetValues("api-key", out var values));
        Assert.Equal("real-key", values.Single());
    }
}
