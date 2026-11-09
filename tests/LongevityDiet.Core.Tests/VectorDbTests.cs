using System.ComponentModel.DataAnnotations;
using LongevityDiet.Contracts.Messaging;
using LongevityDiet.DietKnowledge.Application.Abstractions;
using LongevityDiet.DietKnowledge.Application.Common;
using LongevityDiet.DietKnowledge.Application.Models;
using LongevityDiet.DietKnowledge.Application.Services;
using LongevityDiet.DietKnowledge.Domain.Entities;
using LongevityDiet.DietKnowledge.Infrastructure.Persistence;
using LongevityDiet.DietKnowledge.Infrastructure.Repositories;
using LongevityDiet.DietKnowledge.Infrastructure.VectorDb;
using LongevityDiet.Recommendation.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace LongevityDiet.Core.Tests;

public sealed class VectorDbTests
{
    [Fact]
    public async Task LocalHashEmbedding_is_stable_unit_length_and_prefers_shared_vocabulary()
    {
        var generator = new LocalHashEmbeddingGenerator();

        var a = await generator.EmbedAsync("Lentil soup with carrots", EmbeddingPurpose.Document);
        var again = await generator.EmbedAsync("Lentil soup with carrots", EmbeddingPurpose.Query);
        var near = await generator.EmbedAsync("carrot and lentil soup", EmbeddingPurpose.Query);
        var far = await generator.EmbedAsync("Grilled salmon fillet", EmbeddingPurpose.Query);

        Assert.Equal(VectorDimensions.Embedding, a.Length);
        Assert.Equal(a, again);
        Assert.InRange(a.Sum(value => value * value), 0.999f, 1.001f);
        Assert.True(Cosine(a, near) > Cosine(a, far));
    }

    [Fact]
    public async Task LocalHashEmbedding_ignores_accents_so_unaccented_typing_matches()
    {
        var generator = new LocalHashEmbeddingGenerator();

        var accented = await generator.EmbedAsync("đậu lăng hầm", EmbeddingPurpose.Document);
        var plain = await generator.EmbedAsync("dau lang ham", EmbeddingPurpose.Query);

        Assert.True(Cosine(accented, plain) > 0.99f);
    }

    [Fact]
    public async Task ReindexAsync_embeds_only_active_approved_items_then_skips_unchanged_and_prunes()
    {
        await using var dbContext = CreateDietDbContext();
        var approved = AddGuideline(dbContext, "Plant based plate", "Fill the plate with legumes.", approved: true);
        AddGuideline(dbContext, "Draft idea", "Not reviewed yet.", approved: false);
        var inactive = AddGuideline(dbContext, "Retired rule", "Old text.", approved: true);
        inactive.IsActive = false;
        var recipe = AddRecipe(dbContext, "Lentil stew", "Slow cooked lentils.", approved: true);
        await dbContext.SaveChangesAsync();

        var store = new FakeVectorStore();
        var service = CreateIndexService(dbContext, store);

        var first = await service.ReindexAsync();

        Assert.Equal(2, first.Indexed);
        Assert.Equal(0, first.Unchanged);
        Assert.Equal("Completed", first.Status);
        Assert.Equal(
            new[] { $"Guideline:{approved.Id}", $"Recipe:{recipe.Id}" }.OrderBy(key => key),
            store.Rows.Keys.OrderBy(key => key));
        Assert.DoesNotContain(store.Rows.Values, row => row.Content.Contains("Draft idea"));

        var second = await service.ReindexAsync();
        Assert.Equal(0, second.Indexed);
        Assert.Equal(2, second.Unchanged);

        approved.Summary = "Fill the plate with beans and greens.";
        recipe.IsActive = false;
        await dbContext.SaveChangesAsync();

        var third = await service.ReindexAsync();
        Assert.Equal(1, third.Indexed);
        Assert.Equal(1, third.Removed);
        Assert.Single(store.Rows);
        Assert.Contains("beans and greens", store.Rows.Values.Single().Content);
    }

    [Fact]
    public async Task ReindexAsync_keeps_existing_rows_when_an_embedding_fails()
    {
        await using var dbContext = CreateDietDbContext();
        var guideline = AddGuideline(dbContext, "Plant based plate", "Legumes.", approved: true);
        AddGuideline(dbContext, "Whole grains", "Swap refined grains.", approved: true);
        await dbContext.SaveChangesAsync();

        var store = new FakeVectorStore();
        var embedder = new ToggleEmbeddingGenerator();
        var service = CreateIndexService(dbContext, store, embedder);
        await service.ReindexAsync();
        Assert.Equal(2, store.Rows.Count);

        // One guideline changes and its embedding fails; the other row must survive pruning.
        guideline.Summary = "Legumes and nuts.";
        await dbContext.SaveChangesAsync();
        embedder.FailOn = "nuts";

        var result = await service.ReindexAsync();

        Assert.Equal(1, result.Failed);
        Assert.Equal(0, result.Removed);
        Assert.Equal(2, store.Rows.Count);
        Assert.Equal("Failed", result.Status);
    }

    [Fact]
    public async Task SearchAsync_validates_input_and_ranks_the_closest_item_first()
    {
        await using var dbContext = CreateDietDbContext();
        AddGuideline(dbContext, "Plant based plate", "Fill the plate with legumes and greens.", approved: true);
        AddRecipe(dbContext, "Lentil stew", "Slow cooked lentils with carrots.", approved: true);
        AddRecipe(dbContext, "Salmon bake", "Oven baked salmon fillet.", approved: true);
        await dbContext.SaveChangesAsync();

        var store = new FakeVectorStore();
        var service = CreateIndexService(dbContext, store);
        await service.ReindexAsync();

        var hits = await service.SearchAsync("lentils carrots", null, 5);
        Assert.Equal("Lentil stew", hits.First().Title);
        Assert.Equal("Recipe", hits.First().SourceType);

        var onlyGuidelines = await service.SearchAsync("lentils carrots", "guideline", 5);
        Assert.All(onlyGuidelines, hit => Assert.Equal("Guideline", hit.SourceType));

        await Assert.ThrowsAsync<ValidationException>(() => service.SearchAsync("  ", null, 5));
        await Assert.ThrowsAsync<ValidationException>(() => service.SearchAsync("soup", "Chunk", 5));
        await Assert.ThrowsAsync<ValidationException>(() => service.SearchAsync(new string('x', 301), null, 5));
    }

    [Fact]
    public async Task GetStatusAsync_reports_an_unconfigured_vector_db_without_failing()
    {
        await using var dbContext = CreateDietDbContext();
        var service = CreateIndexService(dbContext, new DisabledKnowledgeVectorStore());

        var status = await service.GetStatusAsync();

        Assert.False(status.VectorDbConfigured);
        Assert.Equal(0, status.IndexedItems);
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.ReindexAsync());
    }

    [Fact]
    public async Task Recommendation_prompt_puts_semantically_relevant_items_first_and_survives_search_failure()
    {
        var guidelines = Enumerable.Range(1, 60)
            .Select(index => new ApprovedDietGuideline(
                Guid.NewGuid(), $"Guideline {index:00}", "A general eating principle.", "Ch 1", index.ToString(), "src"))
            .ToArray();
        var target = guidelines[47];
        var context = new ApprovedKnowledgeContext(guidelines, [], []);
        var requestId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        async Task<string> PromptWith(ISemanticKnowledgeSearch? search)
        {
            var gemini = new FakeGeminiClient
            {
                IsEnabled = true,
                Response = """{"suggestedMealTitles":["Meal"],"disclaimer":"ignored"}"""
            };
            var generator = new RecommendationGenerator(
                new FakeApprovedKnowledgeClient(context),
                gemini,
                NullLogger<RecommendationGenerator>.Instance,
                search);
            await generator.GenerateAsync(new RecommendationRequestMessage(
                requestId, Guid.NewGuid(), ["light dinners"], Days: 1, DateTimeOffset.UtcNow));
            return gemini.CapturedPrompt!;
        }

        var plain = await PromptWith(null);
        Assert.DoesNotContain(target.Title, plain);

        var semantic = await PromptWith(new FakeSemanticSearch([target.Id]));
        Assert.Contains(target.Title, semantic);

        var unavailable = await PromptWith(new FakeSemanticSearch([]));
        Assert.Equal(plain, unavailable);
    }

    private static KnowledgeIndexService CreateIndexService(
        DietKnowledgeDbContext dbContext,
        IKnowledgeVectorStore store,
        IEmbeddingGenerator? embedder = null)
    {
        return new KnowledgeIndexService(
            new DietKnowledgeRepository(dbContext),
            store,
            embedder ?? new LocalHashEmbeddingGenerator(),
            NullLogger<KnowledgeIndexService>.Instance);
    }

    private static DietGuideline AddGuideline(
        DietKnowledgeDbContext dbContext,
        string title,
        string summary,
        bool approved)
    {
        var guideline = new DietGuideline
        {
            Id = Guid.NewGuid(),
            Title = title,
            Summary = summary,
            ReviewStatus = approved ? KnowledgeReviewStatuses.Approved : KnowledgeReviewStatuses.NeedsReview,
            IsActive = true
        };
        dbContext.DietGuidelines.Add(guideline);
        return guideline;
    }

    private static Recipe AddRecipe(
        DietKnowledgeDbContext dbContext,
        string name,
        string description,
        bool approved)
    {
        var recipe = new Recipe
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            ReviewStatus = approved ? KnowledgeReviewStatuses.Approved : KnowledgeReviewStatuses.NeedsReview,
            IsActive = true
        };
        dbContext.Recipes.Add(recipe);
        return recipe;
    }

    private static DietKnowledgeDbContext CreateDietDbContext()
    {
        var options = new DbContextOptionsBuilder<DietKnowledgeDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new DietKnowledgeDbContext(options);
    }

    private static float Cosine(float[] a, float[] b)
    {
        float dot = 0;
        for (var i = 0; i < a.Length; i++)
        {
            dot += a[i] * b[i];
        }

        return dot;
    }

    private sealed class ToggleEmbeddingGenerator : IEmbeddingGenerator
    {
        private readonly LocalHashEmbeddingGenerator inner = new();

        public string? FailOn { get; set; }

        public string ModelName => inner.ModelName;

        public int Dimensions => inner.Dimensions;

        public Task<float[]> EmbedAsync(string text, EmbeddingPurpose purpose, CancellationToken cancellationToken = default)
        {
            if (FailOn is not null && text.Contains(FailOn, StringComparison.OrdinalIgnoreCase))
            {
                throw new HttpRequestException("embedding service unavailable");
            }

            return inner.EmbedAsync(text, purpose, cancellationToken);
        }
    }

    private sealed class FakeSemanticSearch(IReadOnlyList<Guid> ids) : ISemanticKnowledgeSearch
    {
        public Task<IReadOnlyList<Guid>> FindRelevantIdsAsync(
            IReadOnlyCollection<string> preferences,
            CancellationToken cancellationToken = default) => Task.FromResult(ids);
    }

    private sealed class FakeVectorStore : IKnowledgeVectorStore
    {
        public Dictionary<string, (string SourceType, Guid SourceId, string Hash, string Content, float[] Embedding, string Model)> Rows { get; } = [];

        public bool IsConfigured => true;

        public Task EnsureCreatedAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task<IReadOnlyDictionary<string, string>> GetIndexedHashesAsync(
            string embeddingModel,
            CancellationToken cancellationToken = default)
        {
            IReadOnlyDictionary<string, string> hashes = Rows
                .Where(row => row.Value.Model == embeddingModel)
                .ToDictionary(row => row.Key, row => row.Value.Hash);
            return Task.FromResult(hashes);
        }

        public Task<Guid> StartRunAsync(CancellationToken cancellationToken = default) => Task.FromResult(Guid.NewGuid());

        public Task UpsertAsync(Guid runId, KnowledgeVectorItem item, string embeddingModel, CancellationToken cancellationToken = default)
        {
            Rows[$"{item.SourceType}:{item.SourceId}"] =
                (item.SourceType, item.SourceId, item.ContentHash, item.Content, item.Embedding, embeddingModel);
            return Task.CompletedTask;
        }

        public Task<int> RemoveNotInAsync(IReadOnlySet<string> keepKeys, CancellationToken cancellationToken = default)
        {
            var stale = Rows.Keys.Where(key => !keepKeys.Contains(key)).ToArray();
            foreach (var key in stale)
            {
                Rows.Remove(key);
            }

            return Task.FromResult(stale.Length);
        }

        public Task CompleteRunAsync(Guid runId, int itemsIndexed, int itemsFailed, string status, CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        public Task<KnowledgeIndexRunModel?> GetLatestRunAsync(CancellationToken cancellationToken = default)
            => Task.FromResult<KnowledgeIndexRunModel?>(null);

        public Task<int> CountAsync(string embeddingModel, CancellationToken cancellationToken = default)
            => Task.FromResult(Rows.Count(row => row.Value.Model == embeddingModel));

        public Task<IReadOnlyCollection<KnowledgeSearchHit>> SearchAsync(
            float[] queryEmbedding,
            string embeddingModel,
            IReadOnlyCollection<string>? sourceTypes,
            int limit,
            CancellationToken cancellationToken = default)
        {
            IReadOnlyCollection<KnowledgeSearchHit> hits = Rows.Values
                .Where(row => row.Model == embeddingModel)
                .Where(row => sourceTypes is null || sourceTypes.Contains(row.SourceType))
                .Select(row => new KnowledgeSearchHit(
                    row.SourceType,
                    row.SourceId,
                    row.Content.Split('\n')[0],
                    string.Empty,
                    Cosine(row.Embedding, queryEmbedding)))
                .OrderByDescending(hit => hit.Score)
                .Take(limit)
                .ToArray();
            return Task.FromResult(hits);
        }
    }
}
