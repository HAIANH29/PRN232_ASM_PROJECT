namespace LongevityDiet.NotificationWorker;

public sealed record EmailMessage(
    Guid MessageId,
    string RecipientEmail,
    string Subject,
    string Body,
    string Category);
