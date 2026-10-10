using LongevityDiet.NotificationWorker;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Xunit;

namespace LongevityDiet.Core.Tests;

public sealed class NotificationWorkerTests
{
    [Fact]
    public async Task ResendEmailSender_logs_locally_without_http_when_resend_is_not_configured()
    {
        var handler = new RecordingHttpMessageHandler();
        var sender = new ResendEmailSender(
            new HttpClient(handler),
            Options.Create(new ResendOptions { ApiKey = "development-only" }),
            NullLogger<ResendEmailSender>.Instance);

        await sender.SendAsync(new EmailMessage(
            Guid.NewGuid(),
            "user@example.com",
            "Subject",
            "Body",
            "reminder"));

        Assert.Empty(handler.Requests);
    }

    [Fact]
    public async Task ResendEmailSender_sends_configured_request_with_authorization_and_idempotency()
    {
        var handler = new RecordingHttpMessageHandler();
        var sender = new ResendEmailSender(
            new HttpClient(handler),
            Options.Create(new ResendOptions
            {
                BaseUrl = "https://api.resend.test",
                EmailsEndpoint = "emails",
                ApiKey = "real-key",
                FromEmail = "sender@example.com",
                FromName = "Longevity Diet Platform"
            }),
            NullLogger<ResendEmailSender>.Instance);
        var messageId = Guid.NewGuid();

        await sender.SendAsync(new EmailMessage(
            messageId,
            "user@example.com",
            "Meal reminder",
            "Body",
            "reminder"));

        var request = Assert.Single(handler.Requests);
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal("Bearer", request.Headers.Authorization?.Scheme);
        Assert.Equal("real-key", request.Headers.Authorization?.Parameter);
        Assert.True(request.Headers.TryGetValues("Idempotency-Key", out var values));
        Assert.Equal($"reminder/{messageId}", values.Single());
    }
}
