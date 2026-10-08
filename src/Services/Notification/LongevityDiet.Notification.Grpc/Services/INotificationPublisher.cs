using LongevityDiet.Contracts.Messaging;

namespace LongevityDiet.Notification.Grpc.Services;

public interface INotificationPublisher
{
    Task PublishAsync(NotificationRequestedMessage message, CancellationToken cancellationToken = default);
}
