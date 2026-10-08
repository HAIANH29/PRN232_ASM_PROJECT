using LongevityDiet.Contracts.Messaging;

namespace LongevityDiet.ReminderWorker;

public sealed class ReminderMessageWorker(
    IConfiguration configuration,
    ILogger<ReminderMessageWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var queueName = configuration[$"{RabbitMqOptions.SectionName}:ReminderQueue"]
            ?? "reminder.requests";

        logger.LogInformation(
            "Reminder worker placeholder is ready to consume RabbitMQ queue {QueueName}.",
            queueName);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}
