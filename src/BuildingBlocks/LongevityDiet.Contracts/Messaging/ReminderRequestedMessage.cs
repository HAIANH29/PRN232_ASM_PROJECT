namespace LongevityDiet.Contracts.Messaging;

public sealed record ReminderRequestedMessage(
    Guid ReminderId,
    Guid UserId,
    Guid MealPlanId,
    string RecipientEmail,
    DateTimeOffset ScheduledForUtc,
    string Subject,
    string Body);
