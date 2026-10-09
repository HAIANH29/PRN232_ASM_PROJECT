using LongevityDiet.DietKnowledge.Application.Abstractions;
using LongevityDiet.DietKnowledge.Application.Common;
using LongevityDiet.DietKnowledge.Application.Models;
using LongevityDiet.DietKnowledge.Domain.Entities;
using LongevityDiet.DietKnowledge.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LongevityDiet.DietKnowledge.Infrastructure.Repositories;

public sealed class DietKnowledgeRepository(DietKnowledgeDbContext dbContext) : IDietKnowledgeRepository
{
    public async Task<PagedResult<DietGuideline>> ListGuidelinesAsync(
        DietGuidelineQuery query,
        CancellationToken cancellationToken = default)
    {
        var source = dbContext.DietGuidelines.AsNoTracking();

        if (!query.IncludeInactive)
        {
            source = source.Where(guideline => guideline.IsActive);
        }

        source = ApplyReviewFilter(source, query.ApprovedOnly, query.ReviewStatus);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.ToLower();
            source = source.Where(guideline =>
                guideline.Title.ToLower().Contains(search) ||
                guideline.Summary.ToLower().Contains(search));
        }

        source = ApplyGuidelineSort(source, query.SortBy, query.SortDirection);

        return await ToPagedResultAsync(source, query.PageNumber, query.PageSize, cancellationToken);
    }

    public Task<DietGuideline?> GetGuidelineByIdAsync(
        Guid id,
        bool includeInactive,
        bool approvedOnly,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.DietGuidelines.AsQueryable();

        if (!includeInactive)
        {
            query = query.Where(guideline => guideline.IsActive);
        }

        if (approvedOnly)
        {
            query = query.Where(guideline =>
                guideline.ReviewStatus == KnowledgeReviewStatuses.Approved);
        }

        return query.FirstOrDefaultAsync(guideline => guideline.Id == id, cancellationToken);
    }

    public Task<bool> GuidelineTitleExistsAsync(
        string title,
        Guid? excludedId = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedTitle = title.Trim().ToUpper();
        return dbContext.DietGuidelines.AnyAsync(
            guideline => guideline.Title.ToUpper() == normalizedTitle &&
                (!excludedId.HasValue || guideline.Id != excludedId.Value),
            cancellationToken);
    }

    public async Task AddGuidelineAsync(
        DietGuideline guideline,
        CancellationToken cancellationToken = default)
    {
        await dbContext.DietGuidelines.AddAsync(guideline, cancellationToken);
    }

    public async Task<PagedResult<Food>> ListFoodsAsync(
        FoodQuery query,
        CancellationToken cancellationToken = default)
    {
        var source = dbContext.Foods.AsNoTracking();

        if (!query.IncludeInactive)
        {
            source = source.Where(food => food.IsActive);
        }

        source = ApplyReviewFilter(source, query.ApprovedOnly, query.ReviewStatus);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.ToLower();
            source = source.Where(food =>
                food.Name.ToLower().Contains(search) ||
                food.CompatibilityNotes.ToLower().Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(query.Category))
        {
            var category = query.Category.ToLower();
            source = source.Where(food => food.Category.ToLower() == category);
        }

        source = ApplyFoodSort(source, query.SortBy, query.SortDirection);

        return await ToPagedResultAsync(source, query.PageNumber, query.PageSize, cancellationToken);
    }

    public Task<Food?> GetFoodByIdAsync(
        Guid id,
        bool includeInactive,
        bool approvedOnly,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.Foods.AsQueryable();

        if (!includeInactive)
        {
            query = query.Where(food => food.IsActive);
        }

        if (approvedOnly)
        {
            query = query.Where(food => food.ReviewStatus == KnowledgeReviewStatuses.Approved);
        }

        return query.FirstOrDefaultAsync(food => food.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Guid>> GetActiveFoodIdsAsync(
        IReadOnlyCollection<Guid> foodIds,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Foods
            .AsNoTracking()
            .Where(food =>
                food.IsActive &&
                food.ReviewStatus == KnowledgeReviewStatuses.Approved &&
                foodIds.Contains(food.Id))
            .Select(food => food.Id)
            .ToArrayAsync(cancellationToken);
    }

    public Task<bool> FoodNameExistsAsync(
        string name,
        Guid? excludedId = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedName = name.Trim().ToUpper();
        return dbContext.Foods.AnyAsync(
            food => food.Name.ToUpper() == normalizedName &&
                (!excludedId.HasValue || food.Id != excludedId.Value),
            cancellationToken);
    }

    public async Task AddFoodAsync(
        Food food,
        CancellationToken cancellationToken = default)
    {
        await dbContext.Foods.AddAsync(food, cancellationToken);
    }

    public async Task<PagedResult<Recipe>> ListRecipesAsync(
        RecipeQuery query,
        CancellationToken cancellationToken = default)
    {
        var source = RecipesWithDetails().AsNoTracking();

        if (!query.IncludeInactive)
        {
            source = source.Where(recipe => recipe.IsActive);
        }

        source = ApplyReviewFilter(source, query.ApprovedOnly, query.ReviewStatus);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.ToLower();
            source = source.Where(recipe =>
                recipe.Name.ToLower().Contains(search) ||
                recipe.Description.ToLower().Contains(search));
        }

        if (query.FoodId.HasValue)
        {
            source = source.Where(recipe =>
                recipe.Ingredients.Any(ingredient => ingredient.FoodId == query.FoodId.Value));
        }

        source = ApplyRecipeSort(source, query.SortBy, query.SortDirection);

        return await ToPagedResultAsync(source, query.PageNumber, query.PageSize, cancellationToken);
    }

    public Task<Recipe?> GetRecipeByIdAsync(
        Guid id,
        bool includeInactive,
        bool approvedOnly,
        CancellationToken cancellationToken = default)
    {
        var query = RecipesWithDetails();

        if (!includeInactive)
        {
            query = query.Where(recipe => recipe.IsActive);
        }

        if (approvedOnly)
        {
            query = query.Where(recipe => recipe.ReviewStatus == KnowledgeReviewStatuses.Approved);
        }

        return query.FirstOrDefaultAsync(recipe => recipe.Id == id, cancellationToken);
    }

    public Task<bool> RecipeNameExistsAsync(
        string name,
        Guid? excludedId = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedName = name.Trim().ToUpper();
        return dbContext.Recipes.AnyAsync(
            recipe => recipe.Name.ToUpper() == normalizedName &&
                (!excludedId.HasValue || recipe.Id != excludedId.Value),
            cancellationToken);
    }

    public async Task AddRecipeAsync(
        Recipe recipe,
        CancellationToken cancellationToken = default)
    {
        await dbContext.Recipes.AddAsync(recipe, cancellationToken);
    }

    public void ReplaceRecipeIngredients(
        Recipe recipe,
        IReadOnlyCollection<RecipeIngredient> ingredients)
    {
        var requestedFoodIds = ingredients.Select(ingredient => ingredient.FoodId).ToHashSet();
        var ingredientsToRemove = recipe.Ingredients
            .Where(existing => !requestedFoodIds.Contains(existing.FoodId))
            .ToArray();

        foreach (var ingredient in ingredientsToRemove)
        {
            dbContext.RecipeIngredients.Remove(ingredient);
        }

        var existingByFoodId = recipe.Ingredients
            .Where(existing => requestedFoodIds.Contains(existing.FoodId))
            .ToDictionary(existing => existing.FoodId);

        foreach (var requested in ingredients)
        {
            if (existingByFoodId.TryGetValue(requested.FoodId, out var existing))
            {
                existing.QuantityText = requested.QuantityText;
                continue;
            }

            recipe.Ingredients.Add(requested);
        }
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private IQueryable<Recipe> RecipesWithDetails()
    {
        return dbContext.Recipes
            .Include(recipe => recipe.Ingredients)
            .ThenInclude(ingredient => ingredient.Food);
    }

    private static IQueryable<DietGuideline> ApplyGuidelineSort(
        IQueryable<DietGuideline> source,
        string? sortBy,
        string? sortDirection)
    {
        var descending = IsDescending(sortDirection);

        return sortBy?.ToLowerInvariant() switch
        {
            "createdat" => descending
                ? source.OrderByDescending(guideline => guideline.CreatedAtUtc)
                : source.OrderBy(guideline => guideline.CreatedAtUtc),
            "updatedat" => descending
                ? source.OrderByDescending(guideline => guideline.UpdatedAtUtc)
                : source.OrderBy(guideline => guideline.UpdatedAtUtc),
            _ => descending
                ? source.OrderByDescending(guideline => guideline.Title)
                : source.OrderBy(guideline => guideline.Title)
        };
    }

    private static IQueryable<DietGuideline> ApplyReviewFilter(
        IQueryable<DietGuideline> source,
        bool approvedOnly,
        string? reviewStatus)
    {
        if (approvedOnly)
        {
            return source.Where(guideline =>
                guideline.ReviewStatus == KnowledgeReviewStatuses.Approved);
        }

        if (string.IsNullOrWhiteSpace(reviewStatus))
        {
            return source;
        }

        var normalizedStatus = KnowledgeReviewStatuses.Normalize(reviewStatus);
        return source.Where(guideline => guideline.ReviewStatus == normalizedStatus);
    }

    private static IQueryable<Food> ApplyReviewFilter(
        IQueryable<Food> source,
        bool approvedOnly,
        string? reviewStatus)
    {
        if (approvedOnly)
        {
            return source.Where(food => food.ReviewStatus == KnowledgeReviewStatuses.Approved);
        }

        if (string.IsNullOrWhiteSpace(reviewStatus))
        {
            return source;
        }

        var normalizedStatus = KnowledgeReviewStatuses.Normalize(reviewStatus);
        return source.Where(food => food.ReviewStatus == normalizedStatus);
    }

    private static IQueryable<Recipe> ApplyReviewFilter(
        IQueryable<Recipe> source,
        bool approvedOnly,
        string? reviewStatus)
    {
        if (approvedOnly)
        {
            return source.Where(recipe => recipe.ReviewStatus == KnowledgeReviewStatuses.Approved);
        }

        if (string.IsNullOrWhiteSpace(reviewStatus))
        {
            return source;
        }

        var normalizedStatus = KnowledgeReviewStatuses.Normalize(reviewStatus);
        return source.Where(recipe => recipe.ReviewStatus == normalizedStatus);
    }

    private static IQueryable<Food> ApplyFoodSort(
        IQueryable<Food> source,
        string? sortBy,
        string? sortDirection)
    {
        var descending = IsDescending(sortDirection);

        return sortBy?.ToLowerInvariant() switch
        {
            "category" => descending
                ? source.OrderByDescending(food => food.Category).ThenBy(food => food.Name)
                : source.OrderBy(food => food.Category).ThenBy(food => food.Name),
            "createdat" => descending
                ? source.OrderByDescending(food => food.CreatedAtUtc)
                : source.OrderBy(food => food.CreatedAtUtc),
            "updatedat" => descending
                ? source.OrderByDescending(food => food.UpdatedAtUtc)
                : source.OrderBy(food => food.UpdatedAtUtc),
            _ => descending
                ? source.OrderByDescending(food => food.Name)
                : source.OrderBy(food => food.Name)
        };
    }

    private static IQueryable<Recipe> ApplyRecipeSort(
        IQueryable<Recipe> source,
        string? sortBy,
        string? sortDirection)
    {
        var descending = IsDescending(sortDirection);

        return sortBy?.ToLowerInvariant() switch
        {
            "createdat" => descending
                ? source.OrderByDescending(recipe => recipe.CreatedAtUtc)
                : source.OrderBy(recipe => recipe.CreatedAtUtc),
            "updatedat" => descending
                ? source.OrderByDescending(recipe => recipe.UpdatedAtUtc)
                : source.OrderBy(recipe => recipe.UpdatedAtUtc),
            _ => descending
                ? source.OrderByDescending(recipe => recipe.Name)
                : source.OrderBy(recipe => recipe.Name)
        };
    }

    private static bool IsDescending(string? sortDirection)
    {
        return string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase);
    }

    private static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        IQueryable<T> source,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var totalCount = await source.CountAsync(cancellationToken);
        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync(cancellationToken);

        return new PagedResult<T>(items, pageNumber, pageSize, totalCount);
    }
}
