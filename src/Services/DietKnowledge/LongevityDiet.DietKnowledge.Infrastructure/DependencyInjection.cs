using LongevityDiet.DietKnowledge.Application.Abstractions;
using LongevityDiet.DietKnowledge.Infrastructure.Ingestion;
using LongevityDiet.DietKnowledge.Infrastructure.Persistence;
using LongevityDiet.DietKnowledge.Infrastructure.Repositories;
using LongevityDiet.DietKnowledge.Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LongevityDiet.DietKnowledge.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddDietKnowledgeInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<DietKnowledgeDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DietKnowledgeDb"),
                npgsql => npgsql.EnableRetryOnFailure()));

        services.Configure<DietKnowledgeSeedOptions>(options =>
        {
            if (bool.TryParse(
                configuration["DietKnowledgeSeed:IncludeDemoApprovedContent"],
                out var includeDemoApprovedContent))
            {
                options.IncludeDemoApprovedContent = includeDemoApprovedContent;
            }
        });
        services.Configure<BookKnowledgeIngestionOptions>(options =>
        {
            options.StoragePath = configuration["BookKnowledgeIngestion:StoragePath"] ??
                options.StoragePath;
            if (int.TryParse(configuration["BookKnowledgeIngestion:MaxChunkCharacters"], out var maxChunkCharacters))
            {
                options.MaxChunkCharacters = maxChunkCharacters;
            }

            if (int.TryParse(configuration["BookKnowledgeIngestion:ChunkOverlapCharacters"], out var chunkOverlapCharacters))
            {
                options.ChunkOverlapCharacters = chunkOverlapCharacters;
            }

            if (int.TryParse(configuration["BookKnowledgeIngestion:MaxChunksPerGeneration"], out var maxChunksPerGeneration))
            {
                options.MaxChunksPerGeneration = maxChunksPerGeneration;
            }
        });
        services.Configure<GeminiOptions>(options =>
        {
            if (bool.TryParse(configuration["Gemini:Enabled"], out var enabled))
            {
                options.Enabled = enabled;
            }

            options.Endpoint = configuration["Gemini:Endpoint"] ?? options.Endpoint;
            options.ApiKey = configuration["Gemini:ApiKey"] ?? options.ApiKey;
            options.Model = configuration["Gemini:Model"] ?? options.Model;
            if (int.TryParse(configuration["Gemini:TimeoutSeconds"], out var timeoutSeconds))
            {
                options.TimeoutSeconds = timeoutSeconds;
            }

            if (double.TryParse(configuration["Gemini:Temperature"], out var temperature))
            {
                options.Temperature = temperature;
            }

            if (int.TryParse(configuration["Gemini:MaxOutputTokens"], out var maxOutputTokens))
            {
                options.MaxOutputTokens = maxOutputTokens;
            }
        });

        services.AddScoped<IDietKnowledgeRepository, DietKnowledgeRepository>();
        services.AddScoped<IDietKnowledgeSeeder, DietKnowledgeSeeder>();
        services.AddScoped<IBookKnowledgeFileStore, BookKnowledgeFileStore>();
        services.AddScoped<IBookTextExtractor, PdfBookTextExtractor>();
        services.AddScoped<IBookTextChunker, BookTextChunker>();
        services.AddHttpClient<IKnowledgeCandidateGenerator, GeminiKnowledgeCandidateGenerator>(
            (serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<GeminiOptions>>().Value;
                client.BaseAddress = new Uri(EnsureTrailingSlash(options.Endpoint));
                client.Timeout = TimeSpan.FromSeconds(Math.Clamp(options.TimeoutSeconds, 5, 120));
            });

        return services;
    }

    private static string EnsureTrailingSlash(string value)
    {
        var endpoint = string.IsNullOrWhiteSpace(value)
            ? "https://generativelanguage.googleapis.com"
            : value.Trim();

        return endpoint.EndsWith("/", StringComparison.Ordinal)
            ? endpoint
            : $"{endpoint}/";
    }
}
