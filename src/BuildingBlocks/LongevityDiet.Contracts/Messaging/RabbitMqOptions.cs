namespace LongevityDiet.Contracts.Messaging;

public sealed class RabbitMqOptions
{
    public const string SectionName = "RabbitMq";

    public string HostName { get; set; } = "localhost";

    public int Port { get; set; } = 5672;

    public string UserName { get; set; } = "guest";

    public string Password { get; set; } = "guest";

    public string ReminderExchange { get; set; } = "longevity.reminders";

    public string ReminderQueue { get; set; } = "reminder.requests";

    public string ReminderRoutingKey { get; set; } = "reminder.requested";

    public string RecommendationExchange { get; set; } = "longevity.recommendations";

    public string RecommendationRequestQueue { get; set; } = "recommendation.requests";

    public string RecommendationRequestRoutingKey { get; set; } = "recommendation.requested";

    public string RecommendationResultQueue { get; set; } = "recommendation.results";

    public string RecommendationResultRoutingKey { get; set; } = "recommendation.completed";

    public string NotificationExchange { get; set; } = "longevity.notifications";

    public string NotificationQueue { get; set; } = "notification.messages";

    public string NotificationRoutingKey { get; set; } = "notification.requested";
}
