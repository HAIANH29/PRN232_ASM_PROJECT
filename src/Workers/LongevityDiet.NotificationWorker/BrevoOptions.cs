namespace LongevityDiet.NotificationWorker;

public sealed class BrevoOptions
{
    public const string SectionName = "Brevo";

    public string BaseUrl { get; set; } = "https://api.brevo.com/v3";

    public string EmailsEndpoint { get; set; } = "smtp/email";

    public string ApiKey { get; set; } = string.Empty;

    public string FromEmail { get; set; } = string.Empty;

    public string FromName { get; set; } = "Longevity Diet Platform";
}
