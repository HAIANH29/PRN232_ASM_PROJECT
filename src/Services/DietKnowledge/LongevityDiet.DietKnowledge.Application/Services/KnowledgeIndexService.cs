using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using LongevityDiet.DietKnowledge.Application.Abstractions;
using LongevityDiet.DietKnowledge.Application.Models;
using Microsoft.Extensions.Logging;

namespace LongevityDiet.DietKnowledge.Application.Services;

public interface IKnowledgeIndexService
{
    // Embeds new or changed active Approved knowledge and removes rows that are no longer eligible.
    Task<KnowledgeIndexResultModel> ReindexAsync(CancellationToken cancellationToken = default);

    Task<KnowledgeIndexStatusModel> GetStatusAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<KnowledgeSearchHit>> SearchAsync(
        string query,
        string? sourceType,
        int limit,
        CancellationToken cancellationToken = default);
}

// Only active Approved guidelines, foods and recipes are ever embedded. Raw book chunks and
// unreviewed candidates never reach the VectorDb.
public sealed class KnowledgeIndexService(
    IDietKnowledgeRepository repository,
    IKnowledgeVectorStore vectorStore,
    IEmbeddingGenerator embeddingGenerator,
    ILogger<KnowledgeIndexService> logger) : IKnowledgeIndexService
{
    private const int PageSize = 100;
    private const int MaxContentLength = 4000;
    private const int MaxQueryLength = 300;

    public async Task<KnowledgeIndexResultModel> ReindexAsync(CancellationToken cancellationToken = default)
    {
        EnsureConfigured();
        await vectorStore.EnsureCreatedAsync(cancellationToken);

        var model = embeddingGenerator.ModelName;
        var existing = await vectorStore.GetIndexedHashesAsync(model, cancellationToken);
        var eligible = await LoadEligibleAsync(cancellationToken);
        var runId = await vectorStore.StartRunAsync(cancellationToken);

        var indexed = 0;
        var unchanged = 0;
        var failed = 0;

        foreach (var source in eligible)
        {
            var key = Key(source.SourceType, source.SourceId);
            if (existing.TryGetValue(key, out var currentHash) && currentHash == source.ContentHash)
            {
                unchanged++;
                continue;
            }

            try
            {
                var embedding = await embeddingGenerator.EmbedAsync(
                    source.Content,
                    EmbeddingPurpose.Document,
                    cancellationToken);

                await vectorStore.UpsertAsync(
                    runId,
                    new KnowledgeVectorItem(
                        source.SourceType,
                        source.SourceId,
                        source.ContentHash,
                        source.Content,
                        embedding),
                    model,
                    cancellationToken);
                indexed++;
            }
            catch (Exception exception) when (exception is not OperationCanceledException)
            {
                failed++;
                logger.LogWarning(
                    exception,
                    "Could not embed {SourceType} {SourceId}; it stays out of the index until the next run.",
                    source.SourceType,
                    source.SourceId);
            }
        }

        // Only prune when every eligible item was processed, so a failed embedding never deletes a good row.
        var removed = 0;
        if (failed == 0)
        {
            removed = await vectorStore.RemoveNotInAsync(
                eligible.Select(source => Key(source.SourceType, source.SourceId)).ToHashSet(),
                cancellationToken);
        }

        var status = failed == 0 ? "Completed" : indexed > 0 ? "CompletedWithErrors" : "Failed";
        await vectorStore.CompleteRunAsync(runId, indexed, failed, status, cancellationToken);

        return new KnowledgeIndexResultModel(runId, indexed, unchanged, removed, failed, status);
    }

    public async Task<KnowledgeIndexStatusModel> GetStatusAsync(CancellationToken cancellationToken = default)
    {
        if (!vectorStore.IsConfigured)
        {
            return new KnowledgeIndexStatusModel(
                false,
                embeddingGenerator.ModelName,
                embeddingGenerator.Dimensions,
                0,
                null);
        }

        await vectorStore.EnsureCreatedAsync(cancellationToken);

        return new KnowledgeIndexStatusModel(
            true,
            embeddingGenerator.ModelName,
            embeddingGenerator.Dimensions,
            await vectorStore.CountAsync(embeddingGenerator.ModelName, cancellationToken),
            await vectorStore.GetLatestRunAsync(cancellationToken));
    }

    public async Task<IReadOnlyCollection<KnowledgeSearchHit>> SearchAsync(
        string query,
        string? sourceType,
        int limit,
        CancellationToken cancellationToken = default)
    {
        EnsureConfigured();

        var text = query?.Trim() ?? string.Empty;
        if (text.Length == 0)
        {
            throw new ValidationException("A search query is required.");
        }

        if (text.Length > MaxQueryLength)
        {
            throw new ValidationException($"The search query can be at most {MaxQueryLength} characters.");
        }

        string[]? types = null;
        if (!string.IsNullOrWhiteSpace(sourceType))
        {
            var normalized = KnowledgeSourceTypes.Normalize(sourceType)
                ?? throw new ValidationException("Type must be Guideline, Food, or Recipe.");
            types = [normalized];
        }

        var embedding = await embeddingGenerator.EmbedAsync(text, EmbeddingPurpose.Query, cancellationToken);

        return await vectorStore.SearchAsync(
            embedding,
            embeddingGenerator.ModelName,
            types,
            Math.Clamp(limit, 1, 50),
            cancellationToken);
    }

    private async Task<IReadOnlyList<EligibleItem>> LoadEligibleAsync(CancellationToken cancellationToken)
    {
        var items = new List<EligibleItem>();

        for (var page = 1; ; page++)
        {
            var result = await repository.ListGuidelinesAsync(
                new DietGuidelineQuery(page, PageSize, null, false, true, null, "title", "asc"),
                cancellationToken);

            items.AddRange(result.Items.Select(guideline => Build(
                KnowledgeSourceTypes.Guideline,
                guideline.Id,
                guideline.Title,
                guideline.CategoryRef?.Name,
                guideline.Summary)));

            if (page * PageSize >= result.TotalCount)
            {
                break;
            }
        }

        for (var page = 1; ; page++)
        {
            var result = await repository.ListFoodsAsync(
                new FoodQuery(page, PageSize, null, null, false, true, null, "name", "asc"),
                cancellationToken);

            items.AddRange(result.Items.Select(food => Build(
                KnowledgeSourceTypes.Food,
                food.Id,
                food.Name,
                food.CategoryRef?.Name,
                food.CompatibilityNotes)));

            if (page * PageSize >= result.TotalCount)
            {
                break;
            }
        }

        for (var page = 1; ; page++)
        {
            var result = await repository.ListRecipesAsync(
                new RecipeQuery(page, PageSize, null, null, false, true, null, "name", "asc"),
                cancellationToken);

            items.AddRange(result.Items.Select(recipe =>
            {
                var ingredients = string.Join(
                    ", ",
                    recipe.Ingredients
                        .Select(ingredient => ingredient.Food?.Name)
                        .Where(name => !string.IsNullOrWhiteSpace(name)));
                var steps = string.Join(
                    " ",
                    recipe.Steps.OrderBy(step => step.StepNumber).Select(step => step.Instruction));
                var body = string.Join(
                    "\n",
                    new[]
                    {
                        recipe.Description,
                        ingredients.Length > 0 ? $"Ingredients: {ingredients}" : string.Empty,
                        steps
                    }.Where(part => !string.IsNullOrWhiteSpace(part)));

                return Build(KnowledgeSourceTypes.Recipe, recipe.Id, recipe.Name, recipe.CategoryRef?.Name, body);
            }));

            if (page * PageSize >= result.TotalCount)
            {
                break;
            }
        }

        return items;
    }

    private static EligibleItem Build(
        string sourceType,
        Guid id,
        string title,
        string? category,
        string body)
    {
        var header = string.IsNullOrWhiteSpace(category) ? title : $"{title} ({category})";
        var content = $"{header}\n{body}".Trim();
        if (content.Length > MaxContentLength)
        {
            content = content[..MaxContentLength];
        }

        var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(content)));
        return new EligibleItem(sourceType, id, content, hash);
    }

    private void EnsureConfigured()
    {
        if (!vectorStore.IsConfigured)
        {
            throw new InvalidOperationException("The VectorDb is not configured for this service.");
        }
    }

    private static string Key(string sourceType, Guid id) => $"{sourceType}:{id}";

    private sealed record EligibleItem(string SourceType, Guid SourceId, string Content, string ContentHash);
}
