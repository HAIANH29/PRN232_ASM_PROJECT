namespace LongevityDiet.Tracking.Application.Abstractions;

public interface INotificationClient
{
    Task<Guid?> RequestProgressNotificationAsync(
        Guid userId,
        string recipientEmail,
        string subject,
        string body,
        Guid progressSummaryId,
        CancellationToken cancellationToken = default);
}
