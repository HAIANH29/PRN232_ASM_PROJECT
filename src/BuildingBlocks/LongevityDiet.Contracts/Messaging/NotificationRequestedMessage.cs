namespace LongevityDiet.Contracts.Messaging;

public sealed record NotificationRequestedMessage(
    Guid NotificationId,
    Guid UserId,
    string RecipientEmail,
    string Subject,
    string Body,
    DateTimeOffset RequestedAtUtc);
