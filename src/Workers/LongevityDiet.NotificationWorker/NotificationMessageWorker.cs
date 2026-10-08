using LongevityDiet.Contracts.Messaging;

namespace LongevityDiet.NotificationWorker;

public sealed class NotificationMessageWorker(
    IConfiguration configuration,
    ILogger<NotificationMessageWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var notificationQueue = configuration[$"{RabbitMqOptions.SectionName}:NotificationQueue"]
            ?? "notification.messages";
        var reminderQueue = configuration[$"{RabbitMqOptions.SectionName}:ReminderQueue"]
            ?? "reminder.requests";
        var resendBaseUrl = configuration[$"{ResendOptions.SectionName}:BaseUrl"]
            ?? "https://api.resend.com";

        logger.LogInformation(
            "Notification Worker placeholder is ready. Consume {NotificationQueue} and {ReminderQueue}; send through Resend {ResendBaseUrl}.",
            notificationQueue,
            reminderQueue,
            resendBaseUrl);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}
