namespace LongevityDiet.NotificationWorker;

public sealed class ResendOptions
{
    public const string SectionName = "Resend";

    public string BaseUrl { get; set; } = "https://api.resend.com";

    public string EmailsEndpoint { get; set; } = "emails";

    public string ApiKey { get; set; } = string.Empty;

    public string FromEmail { get; set; } = "onboarding@resend.dev";

    public string FromName { get; set; } = "Longevity Diet Platform";
}
