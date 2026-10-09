using System.Text;
using System.Text.Json;
using LongevityDiet.Contracts.Messaging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LongevityDiet.Recommendation.Service;

public sealed class RecommendationMessageWorker(
    IConfiguration configuration,
    IRecommendationGenerator recommendationGenerator,
    ILogger<RecommendationMessageWorker> logger) : BackgroundService
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
                    "Recommendation Service RabbitMQ loop failed. Retrying in 5 seconds.");

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }

    private async Task ConsumeAsync(CancellationToken stoppingToken)
    {
        await using var connection = await CreateConnectionFactory()
            .CreateConnectionAsync(stoppingToken);
        await using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        var requestTopology = BuildRequestTopology();
        var resultTopology = BuildResultTopology();
        var prefetchCount = TryGetUShort(
            configuration[$"{RabbitMqOptions.SectionName}:ConsumerPrefetchCount"],
            4);

        await DeclareRequestTopologyAsync(channel, requestTopology, stoppingToken);
        await DeclareResultTopologyAsync(channel, resultTopology, stoppingToken);
        await channel.BasicQosAsync(0, prefetchCount, false, stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (_, args) =>
            await HandleDeliveryAsync(channel, requestTopology, resultTopology, args, stoppingToken);

        await channel.BasicConsumeAsync(
            requestTopology.Queue,
            autoAck: false,
            consumer,
            stoppingToken);

        logger.LogInformation(
            "Recommendation Service is consuming {RequestQueue} and publishing {ResultQueue}. Gemini enabled: {GeminiEnabled}.",
            requestTopology.Queue,
            resultTopology.Queue,
            configuration.GetValue<bool>($"{GeminiOptions.SectionName}:Enabled"));

        await Task.Delay(Timeout.InfiniteTimeSpan, stoppingToken);
    }

    private static async Task DeclareRequestTopologyAsync(
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

    private static async Task DeclareResultTopologyAsync(
        IChannel channel,
        ResultTopology topology,
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
    }

    private async Task HandleDeliveryAsync(
        IChannel channel,
        QueueTopology requestTopology,
        ResultTopology resultTopology,
        BasicDeliverEventArgs args,
        CancellationToken stoppingToken)
    {
        try
        {
            var request = JsonSerializer.Deserialize<RecommendationRequestMessage>(
                    args.Body.Span,
                    JsonOptions)
                ?? throw new InvalidDataException("Recommendation request payload is empty or invalid.");

            var generated = await recommendationGenerator.GenerateAsync(request, stoppingToken);
            var result = new RecommendationResultMessage(
                request.RequestId,
                request.UserId,
                generated.SuggestedMealTitles,
                generated.Disclaimer,
                DateTimeOffset.UtcNow);

            await PublishResultAsync(channel, resultTopology, result, stoppingToken);
            await channel.BasicAckAsync(args.DeliveryTag, false, stoppingToken);

            logger.LogInformation(
                "Recommendation request {RequestId} completed with {SuggestionCount} suggestions.",
                request.RequestId,
                result.SuggestedMealTitles.Count);
        }
        catch (Exception exception)
        {
            await HandleFailureAsync(channel, requestTopology, args, exception, stoppingToken);
        }
    }

    private async Task HandleFailureAsync(
        IChannel channel,
        QueueTopology topology,
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
                    "Failed to process recommendation request from {Queue}. Retrying attempt {Attempt}/{MaxAttempts} after {RetryDelay}.",
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
                "Failed to process recommendation request from {Queue} after {MaxAttempts} attempts. Moving to {DeadLetterQueue}.",
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
                "Could not publish retry/dead-letter copy for recommendation request from {Queue}. Requeueing original message.",
                topology.Queue);

            await channel.BasicNackAsync(args.DeliveryTag, false, true, stoppingToken);
        }
    }

    private async Task PublishResultAsync(
        IChannel channel,
        ResultTopology topology,
        RecommendationResultMessage message,
        CancellationToken cancellationToken)
    {
        var properties = new BasicProperties
        {
            ContentType = "application/json",
            DeliveryMode = DeliveryModes.Persistent
        };
        var body = JsonSerializer.SerializeToUtf8Bytes(message, JsonOptions);

        await channel.BasicPublishAsync(
            topology.Exchange,
            topology.RoutingKey,
            false,
            properties,
            body,
            cancellationToken);
    }

    private QueueTopology BuildRequestTopology()
    {
        var exchange = configuration[$"{RabbitMqOptions.SectionName}:RecommendationExchange"]
            ?? "longevity.recommendations";
        var queue = configuration[$"{RabbitMqOptions.SectionName}:RecommendationRequestQueue"]
            ?? "recommendation.requests";
        var routingKey = configuration[$"{RabbitMqOptions.SectionName}:RecommendationRequestRoutingKey"]
            ?? "recommendation.requested";

        return QueueTopology.Create(exchange, queue, routingKey);
    }

    private ResultTopology BuildResultTopology()
    {
        var exchange = configuration[$"{RabbitMqOptions.SectionName}:RecommendationExchange"]
            ?? "longevity.recommendations";
        var queue = configuration[$"{RabbitMqOptions.SectionName}:RecommendationResultQueue"]
            ?? "recommendation.results";
        var routingKey = configuration[$"{RabbitMqOptions.SectionName}:RecommendationResultRoutingKey"]
            ?? "recommendation.completed";

        return new ResultTopology(exchange, queue, routingKey);
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

    private static int TryGetInt(string? value, int fallback)
    {
        return int.TryParse(value, out var parsed) ? parsed : fallback;
    }

    private static ushort TryGetUShort(string? value, ushort fallback)
    {
        return ushort.TryParse(value, out var parsed) ? parsed : fallback;
    }

    private sealed record ResultTopology(
        string Exchange,
        string Queue,
        string RoutingKey);

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
}
