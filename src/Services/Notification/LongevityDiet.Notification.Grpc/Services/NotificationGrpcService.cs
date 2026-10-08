using Grpc.Core;
using LongevityDiet.Contracts.Messaging;

namespace LongevityDiet.Notification.Grpc.Services;

public sealed class NotificationGrpcService(INotificationPublisher notificationPublisher)
    : Notification.NotificationBase
{
    public override async Task<NotificationReply> RequestProgressNotification(
        ProgressNotificationRequest request,
        ServerCallContext context)
    {
        var notificationId = Guid.NewGuid();
        var userId = Guid.TryParse(request.UserId, out var parsedUserId)
            ? parsedUserId
            : Guid.Empty;

        await notificationPublisher.PublishAsync(
            new NotificationRequestedMessage(
                notificationId,
                userId,
                request.RecipientEmail,
                request.Subject,
                request.Body,
                DateTimeOffset.UtcNow),
            context.CancellationToken);

        return new NotificationReply
        {
            NotificationId = notificationId.ToString(),
            Accepted = true,
            Message = "Notification message accepted for asynchronous delivery."
        };
    }
}
