using System.ComponentModel.DataAnnotations;
using LongevityDiet.DietKnowledge.Application.Abstractions;
using LongevityDiet.DietKnowledge.Application.Models;
using LongevityDiet.DietKnowledge.Domain.Entities;

namespace LongevityDiet.DietKnowledge.Application.Services;

public sealed class DietKnowledgeService(IDietKnowledgeRepository repository) : IDietKnowledgeService
{
    public DietKnowledgeServiceStatus GetStatus()
    {
        return new DietKnowledgeServiceStatus(
            "Diet Knowledge Service",
            "DietKnowledgeDb",
            ["DietGuideline", "Food", "Recipe", "RecipeIngredient"]);
    }

    public async Task<PagedResult<DietGuidelineModel>> ListGuidelinesAsync(
        DietGuidelineQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await repository.ListGuidelinesAsync(Normalize(query), cancellationToken);
        return MapPaged(result, MapGuideline);
    }

    public async Task<DietGuidelineModel> GetGuidelineAsync(
        Guid id,
        bool includeInactive = false,
        CancellationToken cancellationToken = default)
    {
        var guideline = await repository.GetGuidelineByIdAsync(id, includeInactive, cancellationToken)
            ?? throw new KeyNotFoundException("Diet guideline was not found.");

        return MapGuideline(guideline);
    }

    public async Task<DietGuidelineModel> CreateGuidelineAsync(
        CreateDietGuidelineCommand command,
        CancellationToken cancellationToken = default)
    {
        var title = Clean(command.Title);
        if (await repository.GuidelineTitleExistsAsync(title, cancellationToken: cancellationToken))
        {
            throw new InvalidOperationException("A diet guideline with this title already exists.");
        }

        var guideline = new DietGuideline
        {
            Id = Guid.NewGuid(),
            Title = title,
            Summary = Clean(command.Summary),
            SourceNote = CleanOptional(command.SourceNote),
            IsActive = true,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };

        await repository.AddGuidelineAsync(guideline, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return MapGuideline(guideline);
    }

    public async Task<DietGuidelineModel> UpdateGuidelineAsync(
        Guid id,
        UpdateDietGuidelineCommand command,
        CancellationToken cancellationToken = default)
    {
        var guideline = await repository.GetGuidelineByIdAsync(id, includeInactive: true, cancellationToken)
            ?? throw new KeyNotFoundException("Diet guideline was not found.");

        var title = Clean(command.Title);
        if (await repository.GuidelineTitleExistsAsync(title, id, cancellationToken))
        {
            throw new InvalidOperationException("A diet guideline with this title already exists.");
        }

        guideline.Title = title;
        guideline.Summary = Clean(command.Summary);
        guideline.SourceNote = CleanOptional(command.SourceNote);
        guideline.UpdatedAtUtc = DateTimeOffset.UtcNow;

        await repository.SaveChangesAsync(cancellationToken);
        return MapGuideline(guideline);
    }

    public async Task<DietGuidelineModel> SetGuidelineActivationAsync(
        Guid id,
        bool isActive,
        CancellationToken cancellationToken = default)
    {
        var guideline = await repository.GetGuidelineByIdAsync(id, includeInactive: true, cancellationToken)
            ?? throw new KeyNotFoundException("Diet guideline was not found.");

        guideline.IsActive = isActive;
        guideline.UpdatedAtUtc = DateTimeOffset.UtcNow;

        await repository.SaveChangesAsync(cancellationToken);
        return MapGuideline(guideline);
    }

    public async Task<PagedResult<FoodModel>> ListFoodsAsync(
        FoodQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await repository.ListFoodsAsync(Normalize(query), cancellationToken);
        return MapPaged(result, MapFood);
    }

    public async Task<FoodModel> GetFoodAsync(
        Guid id,
        bool includeInactive = false,
        CancellationToken cancellationToken = default)
    {
        var food = await repository.GetFoodByIdAsync(id, includeInactive, cancellationToken)
            ?? throw new KeyNotFoundException("Food was not found.");

        return MapFood(food);
    }

    public async Task<FoodModel> CreateFoodAsync(
        CreateFoodCommand command,
        CancellationToken cancellationToken = default)
    {
        var name = Clean(command.Name);
        if (await repository.FoodNameExistsAsync(name, cancellationToken: cancellationToken))
        {
            throw new InvalidOperationException("A food with this name already exists.");
        }

        var food = new Food
        {
            Id = Guid.NewGuid(),
            Name = name,
            Category = Clean(command.Category),
            CompatibilityNotes = CleanOptional(command.CompatibilityNotes),
            IsActive = true,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };

        await repository.AddFoodAsync(food, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return MapFood(food);
    }

    public async Task<FoodModel> UpdateFoodAsync(
        Guid id,
        UpdateFoodCommand command,
        CancellationToken cancellationToken = default)
    {
        var food = await repository.GetFoodByIdAsync(id, includeInactive: true, cancellationToken)
            ?? throw new KeyNotFoundException("Food was not found.");

        var name = Clean(command.Name);
        if (await repository.FoodNameExistsAsync(name, id, cancellationToken))
        {
            throw new InvalidOperationException("A food with this name already exists.");
        }

        food.Name = name;
        food.Category = Clean(command.Category);
        food.CompatibilityNotes = CleanOptional(command.CompatibilityNotes);
        food.UpdatedAtUtc = DateTimeOffset.UtcNow;

        await repository.SaveChangesAsync(cancellationToken);
        return MapFood(food);
    }

    public async Task<FoodModel> SetFoodActivationAsync(
        Guid id,
        bool isActive,
        CancellationToken cancellationToken = default)
    {
        var food = await repository.GetFoodByIdAsync(id, includeInactive: true, cancellationToken)
            ?? throw new KeyNotFoundException("Food was not found.");

        food.IsActive = isActive;
        food.UpdatedAtUtc = DateTimeOffset.UtcNow;

        await repository.SaveChangesAsync(cancellationToken);
        return MapFood(food);
    }

    public async Task<PagedResult<RecipeModel>> ListRecipesAsync(
        RecipeQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await repository.ListRecipesAsync(Normalize(query), cancellationToken);
        return MapPaged(result, MapRecipe);
    }

    public async Task<RecipeModel> GetRecipeAsync(
        Guid id,
        bool includeInactive = false,
        CancellationToken cancellationToken = default)
    {
        var recipe = await repository.GetRecipeByIdAsync(id, includeInactive, cancellationToken)
            ?? throw new KeyNotFoundException("Recipe was not found.");

        return MapRecipe(recipe);
    }

    public async Task<RecipeModel> CreateRecipeAsync(
        CreateRecipeCommand command,
        CancellationToken cancellationToken = default)
    {
        var name = Clean(command.Name);
        if (await repository.RecipeNameExistsAsync(name, cancellationToken: cancellationToken))
        {
            throw new InvalidOperationException("A recipe with this name already exists.");
        }

        await EnsureRecipeFoodsAreActiveAsync(command.Ingredients, cancellationToken);

        var recipe = new Recipe
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = Clean(command.Description),
            IsActive = true,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };

        foreach (var ingredient in MapIngredients(command.Ingredients))
        {
            recipe.Ingredients.Add(ingredient);
        }

        await repository.AddRecipeAsync(recipe, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return await GetRecipeAsync(recipe.Id, includeInactive: true, cancellationToken);
    }

    public async Task<RecipeModel> UpdateRecipeAsync(
        Guid id,
        UpdateRecipeCommand command,
        CancellationToken cancellationToken = default)
    {
        var recipe = await repository.GetRecipeByIdAsync(id, includeInactive: true, cancellationToken)
            ?? throw new KeyNotFoundException("Recipe was not found.");

        var name = Clean(command.Name);
        if (await repository.RecipeNameExistsAsync(name, id, cancellationToken))
        {
            throw new InvalidOperationException("A recipe with this name already exists.");
        }

        await EnsureRecipeFoodsAreActiveAsync(command.Ingredients, cancellationToken);

        recipe.Name = name;
        recipe.Description = Clean(command.Description);
        recipe.UpdatedAtUtc = DateTimeOffset.UtcNow;
        repository.ReplaceRecipeIngredients(recipe, MapIngredients(command.Ingredients));

        await repository.SaveChangesAsync(cancellationToken);
        return await GetRecipeAsync(recipe.Id, includeInactive: true, cancellationToken);
    }

    public async Task<RecipeModel> SetRecipeActivationAsync(
        Guid id,
        bool isActive,
        CancellationToken cancellationToken = default)
    {
        var recipe = await repository.GetRecipeByIdAsync(id, includeInactive: true, cancellationToken)
            ?? throw new KeyNotFoundException("Recipe was not found.");

        recipe.IsActive = isActive;
        recipe.UpdatedAtUtc = DateTimeOffset.UtcNow;

        await repository.SaveChangesAsync(cancellationToken);
        return MapRecipe(recipe);
    }

    private async Task EnsureRecipeFoodsAreActiveAsync(
        IReadOnlyCollection<RecipeIngredientCommand> ingredients,
        CancellationToken cancellationToken)
    {
        if (ingredients.Count == 0)
        {
            throw new ValidationException("A recipe must include at least one ingredient.");
        }

        var foodIds = ingredients.Select(ingredient => ingredient.FoodId).ToArray();
        if (foodIds.Any(id => id == Guid.Empty))
        {
            throw new ValidationException("Recipe ingredients must reference valid foods.");
        }

        var duplicatedFoodId = foodIds
            .GroupBy(id => id)
            .FirstOrDefault(group => group.Count() > 1)
            ?.Key;

        if (duplicatedFoodId is not null)
        {
            throw new ValidationException("A recipe cannot include the same food more than once.");
        }

        var activeFoodIds = await repository.GetActiveFoodIdsAsync(foodIds, cancellationToken);
        if (activeFoodIds.Count != foodIds.Length)
        {
            throw new ValidationException("All recipe ingredients must reference active approved foods.");
        }
    }

    private static IReadOnlyCollection<RecipeIngredient> MapIngredients(
        IReadOnlyCollection<RecipeIngredientCommand> ingredients)
    {
        return ingredients.Select(ingredient => new RecipeIngredient
        {
            Id = Guid.NewGuid(),
            FoodId = ingredient.FoodId,
            QuantityText = CleanOptional(ingredient.QuantityText)
        }).ToArray();
    }

    private static PagedResult<TDestination> MapPaged<TSource, TDestination>(
        PagedResult<TSource> result,
        Func<TSource, TDestination> mapper)
    {
        return new PagedResult<TDestination>(
            result.Items.Select(mapper).ToArray(),
            result.PageNumber,
            result.PageSize,
            result.TotalCount);
    }

    private static DietGuidelineModel MapGuideline(DietGuideline guideline)
    {
        return new DietGuidelineModel(
            guideline.Id,
            guideline.Title,
            guideline.Summary,
            guideline.SourceNote,
            guideline.IsActive,
            guideline.CreatedAtUtc,
            guideline.UpdatedAtUtc);
    }

    private static FoodModel MapFood(Food food)
    {
        return new FoodModel(
            food.Id,
            food.Name,
            food.Category,
            food.CompatibilityNotes,
            food.IsActive,
            food.CreatedAtUtc,
            food.UpdatedAtUtc);
    }

    private static RecipeModel MapRecipe(Recipe recipe)
    {
        return new RecipeModel(
            recipe.Id,
            recipe.Name,
            recipe.Description,
            recipe.IsActive,
            recipe.Ingredients
                .OrderBy(ingredient => ingredient.Food?.Name ?? string.Empty)
                .Select(ingredient => new RecipeIngredientModel(
                    ingredient.Id,
                    ingredient.FoodId,
                    ingredient.Food?.Name,
                    ingredient.Food?.Category,
                    ingredient.QuantityText))
                .ToArray(),
            recipe.CreatedAtUtc,
            recipe.UpdatedAtUtc);
    }

    private static DietGuidelineQuery Normalize(DietGuidelineQuery query)
    {
        return query with
        {
            PageNumber = Math.Max(1, query.PageNumber),
            PageSize = Math.Clamp(query.PageSize, 1, 100),
            Search = CleanOptional(query.Search),
            SortBy = CleanOptional(query.SortBy),
            SortDirection = CleanOptional(query.SortDirection)
        };
    }

    private static FoodQuery Normalize(FoodQuery query)
    {
        return query with
        {
            PageNumber = Math.Max(1, query.PageNumber),
            PageSize = Math.Clamp(query.PageSize, 1, 100),
            Search = CleanOptional(query.Search),
            Category = CleanOptional(query.Category),
            SortBy = CleanOptional(query.SortBy),
            SortDirection = CleanOptional(query.SortDirection)
        };
    }

    private static RecipeQuery Normalize(RecipeQuery query)
    {
        return query with
        {
            PageNumber = Math.Max(1, query.PageNumber),
            PageSize = Math.Clamp(query.PageSize, 1, 100),
            Search = CleanOptional(query.Search),
            SortBy = CleanOptional(query.SortBy),
            SortDirection = CleanOptional(query.SortDirection)
        };
    }

    private static string Clean(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ValidationException("A required value is missing.");
        }

        return value.Trim();
    }

    private static string CleanOptional(string? value)
    {
        return value?.Trim() ?? string.Empty;
    }
}
