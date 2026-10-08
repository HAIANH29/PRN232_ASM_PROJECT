using Grpc.Net.Client;
using LongevityDiet.Notification.Grpc;
using LongevityDiet.Tracking.Application.Abstractions;
using Microsoft.Extensions.Configuration;
using NotificationGrpcClient = LongevityDiet.Notification.Grpc.Notification.NotificationClient;

namespace LongevityDiet.Tracking.Infrastructure.Notification;

public sealed class GrpcNotificationClient(IConfiguration configuration) : INotificationClient
{
    public async Task<Guid?> RequestProgressNotificationAsync(
        Guid userId,
        string recipientEmail,
        string subject,
        string body,
        Guid progressSummaryId,
        CancellationToken cancellationToken = default)
    {
        var address = configuration["NotificationService:GrpcAddress"];
        if (string.IsNullOrWhiteSpace(address))
        {
            return null;
        }

        using var channel = GrpcChannel.ForAddress(address);
        var client = new NotificationGrpcClient(channel);
        var reply = await client.RequestProgressNotificationAsync(
            new ProgressNotificationRequest
            {
                UserId = userId.ToString(),
                RecipientEmail = recipientEmail,
                Subject = subject,
                Body = body,
                ProgressSummaryId = progressSummaryId.ToString()
            },
            cancellationToken: cancellationToken);

        return Guid.TryParse(reply.NotificationId, out var notificationId)
            ? notificationId
            : null;
    }
}
