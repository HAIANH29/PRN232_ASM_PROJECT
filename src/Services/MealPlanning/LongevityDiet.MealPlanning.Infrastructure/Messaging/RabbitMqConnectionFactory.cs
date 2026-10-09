using LongevityDiet.Contracts.Messaging;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace LongevityDiet.MealPlanning.Infrastructure.Messaging;

internal static class RabbitMqConnectionFactory
{
    public static ConnectionFactory Create(IConfiguration configuration)
    {
        return new ConnectionFactory
        {
            HostName = configuration[$"{RabbitMqOptions.SectionName}:HostName"] ?? "localhost",
            Port = TryGetInt(configuration[$"{RabbitMqOptions.SectionName}:Port"], 5672),
            UserName = configuration[$"{RabbitMqOptions.SectionName}:UserName"] ?? "guest",
            Password = configuration[$"{RabbitMqOptions.SectionName}:Password"] ?? "guest"
        };
    }

    private static int TryGetInt(string? value, int fallback)
    {
        return int.TryParse(value, out var parsed) ? parsed : fallback;
    }
}
