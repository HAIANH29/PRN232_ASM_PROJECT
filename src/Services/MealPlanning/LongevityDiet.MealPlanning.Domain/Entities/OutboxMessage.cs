namespace LongevityDiet.MealPlanning.Domain.Entities;

// A message saved in the same transaction as the data change that caused it. A background
// dispatcher publishes it to RabbitMQ, so a broker outage never loses a reminder or request.
public sealed class OutboxMessage
{
    public Guid Id { get; set; }

    public string MessageType { get; set; } = string.Empty;

    public string PayloadJson { get; set; } = string.Empty;

    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset NextAttemptAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public int Attempts { get; set; }

    public string? LastError { get; set; }

    public DateTimeOffset? ProcessedAtUtc { get; set; }
}
