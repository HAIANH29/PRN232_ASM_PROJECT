using System.Text;
using System.Text.Json;
using LongevityDiet.Contracts.Messaging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LongevityDiet.NotificationWorker;

public sealed class NotificationMessageWorker(
    IConfiguration configuration,
    IEmailSender emailSender,
    ILogger<NotificationMessageWorker> logger) : BackgroundService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ConsumeAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception exception)
            {
                logger.LogWarning(
                    exception,
                    "Notification Worker RabbitMQ loop failed. Retrying in 5 seconds.");

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }

    private async Task ConsumeAsync(CancellationToken stoppingToken)
    {
        await using var connection = await CreateConnectionFactory()
            .CreateConnectionAsync(stoppingToken);
        await using var reminderChannel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);
        await using var notificationChannel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        var reminderTopology = BuildReminderTopology();
        var notificationTopology = BuildNotificationTopology();
        var prefetchCount = TryGetUShort(
            configuration[$"{RabbitMqOptions.SectionName}:ConsumerPrefetchCount"],
            4);

        await ConfigureConsumerAsync(
            reminderChannel,
            reminderTopology,
            prefetchCount,
            MessageKind.Reminder,
            stoppingToken);
        await ConfigureConsumerAsync(
            notificationChannel,
            notificationTopology,
            prefetchCount,
            MessageKind.Notification,
            stoppingToken);

        logger.LogInformation(
            "Notification Worker is consuming {ReminderQueue} and {NotificationQueue}.",
            reminderTopology.Queue,
            notificationTopology.Queue);

        await Task.Delay(Timeout.InfiniteTimeSpan, stoppingToken);
    }

    private async Task ConfigureConsumerAsync(
        IChannel channel,
        QueueTopology topology,
        ushort prefetchCount,
        MessageKind kind,
        CancellationToken stoppingToken)
    {
        await DeclareTopologyAsync(channel, topology, stoppingToken);
        await channel.BasicQosAsync(0, prefetchCount, false, stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (_, args) =>
            await HandleDeliveryAsync(channel, topology, kind, args, stoppingToken);

        await channel.BasicConsumeAsync(
            topology.Queue,
            autoAck: false,
            consumer,
            stoppingToken);
    }

    private async Task DeclareTopologyAsync(
        IChannel channel,
        QueueTopology topology,
        CancellationToken cancellationToken)
    {
        await channel.ExchangeDeclareAsync(
            topology.Exchange,
            ExchangeType.Direct,
            durable: true,
            autoDelete: false,
            cancellationToken: cancellationToken);

        await channel.QueueDeclareAsync(
            topology.Queue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: cancellationToken);

        await channel.QueueBindAsync(
            topology.Queue,
            topology.Exchange,
            topology.RoutingKey,
            cancellationToken: cancellationToken);

        await channel.ExchangeDeclareAsync(
            topology.DeadLetterExchange,
            ExchangeType.Direct,
            durable: true,
            autoDelete: false,
            cancellationToken: cancellationToken);

        await channel.QueueDeclareAsync(
            topology.DeadLetterQueue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: cancellationToken);

        await channel.QueueBindAsync(
            topology.DeadLetterQueue,
            topology.DeadLetterExchange,
            topology.DeadLetterRoutingKey,
            cancellationToken: cancellationToken);
    }

    private async Task HandleDeliveryAsync(
        IChannel channel,
        QueueTopology topology,
        MessageKind kind,
        BasicDeliverEventArgs args,
        CancellationToken stoppingToken)
    {
        try
        {
            var email = BuildEmail(kind, args.Body.Span);
            await emailSender.SendAsync(email, stoppingToken);
            await channel.BasicAckAsync(args.DeliveryTag, false, stoppingToken);

            logger.LogInformation(
                "Delivered {Kind} message {MessageId} to {RecipientEmail}.",
                kind,
                email.MessageId,
                email.RecipientEmail);
        }
        catch (Exception exception)
        {
            await HandleFailureAsync(channel, topology, kind, args, exception, stoppingToken);
        }
    }

    private async Task HandleFailureAsync(
        IChannel channel,
        QueueTopology topology,
        MessageKind kind,
        BasicDeliverEventArgs args,
        Exception exception,
        CancellationToken stoppingToken)
    {
        var retryCount = GetRetryCount(args);
        var maxDeliveryAttempts = TryGetInt(
            configuration[$"{RabbitMqOptions.SectionName}:MaxDeliveryAttempts"],
            3);

        try
        {
            if (retryCount + 1 < maxDeliveryAttempts)
            {
                var nextRetryCount = retryCount + 1;
                var retryDelay = TimeSpan.FromSeconds(TryGetInt(
                    configuration[$"{RabbitMqOptions.SectionName}:RetryDelaySeconds"],
                    5));

                logger.LogWarning(
                    exception,
                    "Failed to process {Kind} message from {Queue}. Retrying attempt {Attempt}/{MaxAttempts} after {RetryDelay}.",
                    kind,
                    topology.Queue,
                    nextRetryCount + 1,
                    maxDeliveryAttempts,
                    retryDelay);

                await Task.Delay(retryDelay, stoppingToken);
                await PublishCopyAsync(
                    channel,
                    topology.Exchange,
                    topology.RoutingKey,
                    args.Body.ToArray(),
                    nextRetryCount,
                    exception.Message,
                    stoppingToken);
                await channel.BasicAckAsync(args.DeliveryTag, false, stoppingToken);
                return;
            }

            logger.LogError(
                exception,
                "Failed to process {Kind} message from {Queue} after {MaxAttempts} attempts. Moving to {DeadLetterQueue}.",
                kind,
                topology.Queue,
                maxDeliveryAttempts,
                topology.DeadLetterQueue);

            await PublishCopyAsync(
                channel,
                topology.DeadLetterExchange,
                topology.DeadLetterRoutingKey,
                args.Body.ToArray(),
                retryCount,
                exception.Message,
                stoppingToken);
            await channel.BasicAckAsync(args.DeliveryTag, false, stoppingToken);
        }
        catch (Exception publishException)
        {
            logger.LogError(
                publishException,
                "Could not publish retry/dead-letter copy for {Kind} message from {Queue}. Requeueing original message.",
                kind,
                topology.Queue);

            await channel.BasicNackAsync(args.DeliveryTag, false, true, stoppingToken);
        }
    }

    private EmailMessage BuildEmail(MessageKind kind, ReadOnlySpan<byte> body)
    {
        return kind switch
        {
            MessageKind.Reminder => BuildReminderEmail(body),
            MessageKind.Notification => BuildNotificationEmail(body),
            _ => throw new InvalidOperationException($"Unsupported message kind {kind}.")
        };
    }

    private static EmailMessage BuildReminderEmail(ReadOnlySpan<byte> body)
    {
        var message = JsonSerializer.Deserialize<ReminderRequestedMessage>(body, JsonOptions)
            ?? throw new InvalidDataException("Reminder message payload is empty or invalid.");

        ValidateRecipient(message.RecipientEmail);

        return new EmailMessage(
            message.ReminderId,
            message.RecipientEmail,
            message.Subject,
            message.Body,
            "reminder");
    }

    private static EmailMessage BuildNotificationEmail(ReadOnlySpan<byte> body)
    {
        var message = JsonSerializer.Deserialize<NotificationRequestedMessage>(body, JsonOptions)
            ?? throw new InvalidDataException("Notification message payload is empty or invalid.");

        ValidateRecipient(message.RecipientEmail);

        return new EmailMessage(
            message.NotificationId,
            message.RecipientEmail,
            message.Subject,
            message.Body,
            "notification");
    }

    private QueueTopology BuildReminderTopology()
    {
        var exchange = configuration[$"{RabbitMqOptions.SectionName}:ReminderExchange"]
            ?? "longevity.reminders";
        var queue = configuration[$"{RabbitMqOptions.SectionName}:ReminderQueue"]
            ?? "reminder.requests";
        var routingKey = configuration[$"{RabbitMqOptions.SectionName}:ReminderRoutingKey"]
            ?? "reminder.requested";

        return QueueTopology.Create(exchange, queue, routingKey);
    }

    private QueueTopology BuildNotificationTopology()
    {
        var exchange = configuration[$"{RabbitMqOptions.SectionName}:NotificationExchange"]
            ?? "longevity.notifications";
        var queue = configuration[$"{RabbitMqOptions.SectionName}:NotificationQueue"]
            ?? "notification.messages";
        var routingKey = configuration[$"{RabbitMqOptions.SectionName}:NotificationRoutingKey"]
            ?? "notification.requested";

        return QueueTopology.Create(exchange, queue, routingKey);
    }

    private ConnectionFactory CreateConnectionFactory()
    {
        return new ConnectionFactory
        {
            HostName = configuration[$"{RabbitMqOptions.SectionName}:HostName"] ?? "localhost",
            Port = TryGetInt(configuration[$"{RabbitMqOptions.SectionName}:Port"], 5672),
            UserName = configuration[$"{RabbitMqOptions.SectionName}:UserName"] ?? "guest",
            Password = configuration[$"{RabbitMqOptions.SectionName}:Password"] ?? "guest",
            AutomaticRecoveryEnabled = true
        };
    }

    private static async Task PublishCopyAsync(
        IChannel channel,
        string exchange,
        string routingKey,
        byte[] body,
        int retryCount,
        string error,
        CancellationToken cancellationToken)
    {
        var properties = new BasicProperties
        {
            ContentType = "application/json",
            DeliveryMode = DeliveryModes.Persistent,
            Headers = new Dictionary<string, object?>
            {
                ["x-retry-count"] = retryCount,
                ["x-last-error"] = error
            }
        };

        await channel.BasicPublishAsync(
            exchange,
            routingKey,
            false,
            properties,
            body,
            cancellationToken);
    }

    private static int GetRetryCount(BasicDeliverEventArgs args)
    {
        if (args.BasicProperties.Headers is null ||
            !args.BasicProperties.Headers.TryGetValue("x-retry-count", out var rawValue))
        {
            return 0;
        }

        return rawValue switch
        {
            byte[] bytes when int.TryParse(Encoding.UTF8.GetString(bytes), out var parsed) => parsed,
            int value => value,
            long value => (int)value,
            _ => 0
        };
    }

    private static void ValidateRecipient(string recipientEmail)
    {
        if (string.IsNullOrWhiteSpace(recipientEmail))
        {
            throw new InvalidDataException("Recipient email is required.");
        }
    }

    private static int TryGetInt(string? value, int fallback)
    {
        return int.TryParse(value, out var parsed) ? parsed : fallback;
    }

    private static ushort TryGetUShort(string? value, ushort fallback)
    {
        return ushort.TryParse(value, out var parsed) ? parsed : fallback;
    }

    private sealed record QueueTopology(
        string Exchange,
        string Queue,
        string RoutingKey,
        string DeadLetterExchange,
        string DeadLetterQueue,
        string DeadLetterRoutingKey)
    {
        public static QueueTopology Create(
            string exchange,
            string queue,
            string routingKey)
        {
            return new QueueTopology(
                exchange,
                queue,
                routingKey,
                $"{exchange}.dead-letter",
                $"{queue}.dead-letter",
                $"{routingKey}.dead-letter");
        }
    }

    private enum MessageKind
    {
        Reminder,
        Notification
    }
}
