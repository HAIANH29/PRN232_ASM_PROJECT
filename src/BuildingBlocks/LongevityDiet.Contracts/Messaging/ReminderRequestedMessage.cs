namespace LongevityDiet.Contracts.Messaging;

// MealPlanItemId lets the Notification Worker ask Meal Planning whether the reminder is still
// current (the item may have been deleted or rescheduled while the message waited in a delay queue).
public sealed record ReminderRequestedMessage(
    Guid ReminderId,
    Guid UserId,
    Guid MealPlanId,
    string RecipientEmail,
    DateTimeOffset ScheduledForUtc,
    string Subject,
    string Body,
    Guid? MealPlanItemId = null);
