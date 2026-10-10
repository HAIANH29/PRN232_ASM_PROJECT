using System.Text.Json;
using LongevityDiet.DietKnowledge.Application.Common;
using LongevityDiet.DietKnowledge.Domain.Entities;
using LongevityDiet.DietKnowledge.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace LongevityDiet.DietKnowledge.Infrastructure.Seed;

public sealed class DietKnowledgeSeeder(
    DietKnowledgeDbContext dbContext,
    IOptions<DietKnowledgeSeedOptions> options) : IDietKnowledgeSeeder
{
    private const string BookSeedFileName = "book-knowledge-seed.json";
    private const string DemoSeedFileName = "demo-approved-knowledge-seed.json";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly DietKnowledgeSeedOptions seedOptions = options.Value;

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.Database.MigrateAsync(cancellationToken);

        await SeedFromFileAsync(BookSeedFileName, cancellationToken);

        if (seedOptions.IncludeDemoApprovedContent)
        {
            await SeedFromFileAsync(DemoSeedFileName, cancellationToken);
        }
    }

    private async Task SeedFromFileAsync(string seedFileName, CancellationToken cancellationToken)
    {
        var seed = await LoadSeedAsync(seedFileName, cancellationToken);

        foreach (var guideline in seed.Guidelines)
        {
            await EnsureGuidelineAsync(guideline, cancellationToken);
        }

        foreach (var food in seed.Foods)
        {
            await EnsureFoodAsync(food, cancellationToken);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        if (seed.Recipes.Count > 0)
        {
            var recipeFoodNames = seed.Recipes
                .SelectMany(recipe => recipe.Ingredients)
                .Select(ingredient => ingredient.FoodName)
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Distinct()
                .ToArray();

            var foods = await dbContext.Foods
                .Where(food => recipeFoodNames.Contains(food.Name))
                .ToListAsync(cancellationToken);
            var foodMap = foods
                .GroupBy(food => food.Name)
                .ToDictionary(group => group.Key, group => group.First());

            foreach (var recipe in seed.Recipes)
            {
                await EnsureRecipeAsync(recipe, foodMap, cancellationToken);
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    private static async Task<BookKnowledgeSeed> LoadSeedAsync(
        string seedFileName,
        CancellationToken cancellationToken)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Seed", "Data", seedFileName);

        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Diet knowledge seed file was not found.", path);
        }

        await using var stream = File.OpenRead(path);
        var seed = await JsonSerializer.DeserializeAsync<BookKnowledgeSeed>(
            stream,
            JsonOptions,
            cancellationToken);

        return seed ?? new BookKnowledgeSeed();
    }

    private async Task EnsureGuidelineAsync(
        GuidelineSeed seed,
        CancellationToken cancellationToken)
    {
        var guideline = await dbContext.DietGuidelines
            .FirstOrDefaultAsync(existing => existing.Title == seed.Title, cancellationToken);

        if (guideline is null)
        {
            guideline = new DietGuideline
            {
                Id = Guid.NewGuid(),
                Title = seed.Title,
                CreatedAtUtc = DateTimeOffset.UtcNow
            };

            await dbContext.DietGuidelines.AddAsync(guideline, cancellationToken);
        }

        guideline.Summary = seed.Summary;
        guideline.SourceNote = seed.SourceNote;
        ApplyReviewMetadata(guideline, seed);
        guideline.IsActive = seed.IsActive;
        guideline.UpdatedAtUtc = DateTimeOffset.UtcNow;
    }

    private async Task EnsureFoodAsync(
        FoodSeed seed,
        CancellationToken cancellationToken)
    {
        var food = await dbContext.Foods
            .FirstOrDefaultAsync(existing => existing.Name == seed.Name, cancellationToken);

        if (food is null)
        {
            food = new Food
            {
                Id = Guid.NewGuid(),
                Name = seed.Name,
                CreatedAtUtc = DateTimeOffset.UtcNow
            };

            await dbContext.Foods.AddAsync(food, cancellationToken);
        }

        food.Category = seed.Category;
        food.CompatibilityNotes = seed.CompatibilityNotes;
        ApplyReviewMetadata(food, seed);
        food.IsActive = seed.IsActive;
        food.UpdatedAtUtc = DateTimeOffset.UtcNow;
    }

    private async Task EnsureRecipeAsync(
        RecipeSeed seed,
        IReadOnlyDictionary<string, Food> foodMap,
        CancellationToken cancellationToken)
    {
        var recipe = await dbContext.Recipes
            .Include(existing => existing.Ingredients)
            .FirstOrDefaultAsync(existing => existing.Name == seed.Name, cancellationToken);

        if (recipe is null)
        {
            recipe = new Recipe
            {
                Id = Guid.NewGuid(),
                Name = seed.Name,
                CreatedAtUtc = DateTimeOffset.UtcNow
            };

            await dbContext.Recipes.AddAsync(recipe, cancellationToken);
        }

        recipe.Description = seed.Description;
        ApplyReviewMetadata(recipe, seed);
        recipe.IsActive = seed.IsActive;
        recipe.UpdatedAtUtc = DateTimeOffset.UtcNow;

        var requestedIngredients = seed.Ingredients
            .Where(ingredient => foodMap.ContainsKey(ingredient.FoodName))
            .Select(ingredient => new
            {
                Food = foodMap[ingredient.FoodName],
                ingredient.QuantityText
            })
            .ToArray();

        var requestedFoodIds = requestedIngredients
            .Select(ingredient => ingredient.Food.Id)
            .ToHashSet();
        var ingredientsToRemove = recipe.Ingredients
            .Where(existing => !requestedFoodIds.Contains(existing.FoodId))
            .ToArray();

        dbContext.RecipeIngredients.RemoveRange(ingredientsToRemove);

        var existingByFoodId = recipe.Ingredients
            .Where(existing => requestedFoodIds.Contains(existing.FoodId))
            .ToDictionary(existing => existing.FoodId);

        foreach (var ingredient in requestedIngredients)
        {
            if (existingByFoodId.TryGetValue(ingredient.Food.Id, out var existing))
            {
                existing.QuantityText = ingredient.QuantityText;
                continue;
            }

            recipe.Ingredients.Add(new RecipeIngredient
            {
                Id = Guid.NewGuid(),
                FoodId = ingredient.Food.Id,
                QuantityText = ingredient.QuantityText
            });
        }
    }

    private static void ApplyReviewMetadata(DietGuideline guideline, ReviewMetadataSeed seed)
    {
        guideline.SourceTitle = CleanOrDefault(seed.SourceTitle, "The Longevity Diet");
        guideline.SourceChapter = Clean(seed.SourceChapter);
        guideline.SourcePage = Clean(seed.SourcePage);
        guideline.SourceReference = Clean(seed.SourceReference);
        guideline.ReviewStatus = NormalizeReviewStatus(seed.ReviewStatus);
        guideline.ReviewedBy = Clean(seed.ReviewedBy);
        guideline.ReviewedAtUtc = seed.ReviewedAtUtc;
    }

    private static void ApplyReviewMetadata(Food food, ReviewMetadataSeed seed)
    {
        food.SourceTitle = CleanOrDefault(seed.SourceTitle, "The Longevity Diet");
        food.SourceChapter = Clean(seed.SourceChapter);
        food.SourcePage = Clean(seed.SourcePage);
        food.SourceReference = Clean(seed.SourceReference);
        food.ReviewStatus = NormalizeReviewStatus(seed.ReviewStatus);
        food.ReviewedBy = Clean(seed.ReviewedBy);
        food.ReviewedAtUtc = seed.ReviewedAtUtc;
    }

    private static void ApplyReviewMetadata(Recipe recipe, ReviewMetadataSeed seed)
    {
        recipe.SourceTitle = CleanOrDefault(seed.SourceTitle, "The Longevity Diet");
        recipe.SourceChapter = Clean(seed.SourceChapter);
        recipe.SourcePage = Clean(seed.SourcePage);
        recipe.SourceReference = Clean(seed.SourceReference);
        recipe.ReviewStatus = NormalizeReviewStatus(seed.ReviewStatus);
        recipe.ReviewedBy = Clean(seed.ReviewedBy);
        recipe.ReviewedAtUtc = seed.ReviewedAtUtc;
    }

    private static string NormalizeReviewStatus(string? status)
    {
        return KnowledgeReviewStatuses.IsValid(status)
            ? KnowledgeReviewStatuses.Normalize(status)
            : KnowledgeReviewStatuses.NeedsReview;
    }

    private static string Clean(string? value)
    {
        return value?.Trim() ?? string.Empty;
    }

    private static string CleanOrDefault(string? value, string defaultValue)
    {
        return string.IsNullOrWhiteSpace(value) ? defaultValue : value.Trim();
    }

    private sealed class BookKnowledgeSeed
    {
        public IReadOnlyCollection<GuidelineSeed> Guidelines { get; init; } = Array.Empty<GuidelineSeed>();

        public IReadOnlyCollection<FoodSeed> Foods { get; init; } = Array.Empty<FoodSeed>();

        public IReadOnlyCollection<RecipeSeed> Recipes { get; init; } = Array.Empty<RecipeSeed>();
    }

    private abstract class ReviewMetadataSeed
    {
        public string SourceTitle { get; init; } = "The Longevity Diet";

        public string SourceChapter { get; init; } = string.Empty;

        public string SourcePage { get; init; } = string.Empty;

        public string SourceReference { get; init; } = string.Empty;

        public string ReviewStatus { get; init; } = KnowledgeReviewStatuses.NeedsReview;

        public string ReviewedBy { get; init; } = string.Empty;

        public DateTimeOffset? ReviewedAtUtc { get; init; }

        public bool IsActive { get; init; } = true;
    }

    private sealed class GuidelineSeed : ReviewMetadataSeed
    {
        public string Title { get; init; } = string.Empty;

        public string Summary { get; init; } = string.Empty;

        public string SourceNote { get; init; } = string.Empty;
    }

    private sealed class FoodSeed : ReviewMetadataSeed
    {
        public string Name { get; init; } = string.Empty;

        public string Category { get; init; } = string.Empty;

        public string CompatibilityNotes { get; init; } = string.Empty;
    }

    private sealed class RecipeSeed : ReviewMetadataSeed
    {
        public string Name { get; init; } = string.Empty;

        public string Description { get; init; } = string.Empty;

        public IReadOnlyCollection<IngredientSeed> Ingredients { get; init; } =
            Array.Empty<IngredientSeed>();
    }

    private sealed class IngredientSeed
    {
        public string FoodName { get; init; } = string.Empty;

        public string QuantityText { get; init; } = string.Empty;
    }
}
