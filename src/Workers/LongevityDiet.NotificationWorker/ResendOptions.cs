namespace LongevityDiet.NotificationWorker;

public sealed class ResendOptions
{
    public const string SectionName = "Resend";

    public string BaseUrl { get; set; } = "https://api.resend.com";

    public string ApiKey { get; set; } = string.Empty;
}
