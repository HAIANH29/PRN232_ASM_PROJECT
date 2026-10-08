using LongevityDiet.DietKnowledge.Domain.Entities;
using LongevityDiet.DietKnowledge.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LongevityDiet.DietKnowledge.Infrastructure.Seed;

public sealed class DietKnowledgeSeeder(DietKnowledgeDbContext dbContext) : IDietKnowledgeSeeder
{
    private const string SourceNote = "Approved demo content for the Longevity Diet Platform educational scope.";

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.Database.MigrateAsync(cancellationToken);

        await EnsureGuidelineAsync(
            "Plant-forward daily meals",
            "Build most meals around vegetables, legumes, whole grains, nuts, and olive-oil-based fats. This keeps the platform focused on educational diet planning rather than medical advice.",
            cancellationToken);

        await EnsureGuidelineAsync(
            "Prefer legumes and whole grains",
            "Use beans, lentils, chickpeas, and minimally refined grains as regular staples for meal plans and recipe ideas.",
            cancellationToken);

        await EnsureGuidelineAsync(
            "Keep added sugar and refined grains limited",
            "Treat highly refined grains, sweetened drinks, and dessert-style foods as occasional choices rather than everyday planned meals.",
            cancellationToken);

        await EnsureGuidelineAsync(
            "Use simple meal timing for planning",
            "Keep meal schedules consistent and practical for the user. Do not present timing guidance as treatment for any disease.",
            cancellationToken);

        var foods = new[]
        {
            new FoodSeed("Lentils", "Legume", "Useful base for soups, salads, and bowls."),
            new FoodSeed("Chickpeas", "Legume", "Plant-protein staple for salads, stews, and bowls."),
            new FoodSeed("Cannellini beans", "Legume", "Mild bean option for soups and vegetable dishes."),
            new FoodSeed("Brown rice", "Whole grain", "Minimally refined grain for balanced meal bowls."),
            new FoodSeed("Whole-grain pasta", "Whole grain", "Use moderate portions with vegetables and legumes."),
            new FoodSeed("Leafy greens", "Vegetable", "Flexible vegetable base for salads, soups, and warm bowls."),
            new FoodSeed("Broccoli", "Vegetable", "Cruciferous vegetable suitable for simple meals."),
            new FoodSeed("Tomatoes", "Vegetable", "Works well in soups, sauces, salads, and grain bowls."),
            new FoodSeed("Walnuts", "Nut", "Use small portions for texture and plant-based fats."),
            new FoodSeed("Almonds", "Nut", "Use as a small topping or snack option."),
            new FoodSeed("Extra virgin olive oil", "Healthy fat", "Primary cooking and dressing fat for demo recipes."),
            new FoodSeed("Herbs and lemon", "Seasoning", "Flavor meals without turning the content into medical advice.")
        };

        foreach (var food in foods)
        {
            await EnsureFoodAsync(food, cancellationToken);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        var foodMap = await dbContext.Foods
            .Where(food => food.IsActive)
            .ToDictionaryAsync(food => food.Name, cancellationToken);

        await EnsureRecipeAsync(
            "Chickpea vegetable bowl",
            "A plant-forward bowl with chickpeas, brown rice, greens, tomatoes, herbs, lemon, and olive oil.",
            foodMap,
            [
                new IngredientSeed("Chickpeas", "1 cup cooked"),
                new IngredientSeed("Brown rice", "3/4 cup cooked"),
                new IngredientSeed("Leafy greens", "2 cups"),
                new IngredientSeed("Tomatoes", "1/2 cup"),
                new IngredientSeed("Extra virgin olive oil", "1 tbsp"),
                new IngredientSeed("Herbs and lemon", "to taste")
            ],
            cancellationToken);

        await EnsureRecipeAsync(
            "Lentil walnut salad",
            "A simple salad using lentils, leafy greens, walnuts, tomatoes, herbs, lemon, and olive oil.",
            foodMap,
            [
                new IngredientSeed("Lentils", "1 cup cooked"),
                new IngredientSeed("Leafy greens", "2 cups"),
                new IngredientSeed("Walnuts", "2 tbsp"),
                new IngredientSeed("Tomatoes", "1/2 cup"),
                new IngredientSeed("Extra virgin olive oil", "1 tbsp"),
                new IngredientSeed("Herbs and lemon", "to taste")
            ],
            cancellationToken);

        await EnsureRecipeAsync(
            "Bean and greens soup",
            "A warm soup with cannellini beans, greens, tomatoes, broccoli, herbs, and olive oil.",
            foodMap,
            [
                new IngredientSeed("Cannellini beans", "1 cup cooked"),
                new IngredientSeed("Leafy greens", "1 cup"),
                new IngredientSeed("Tomatoes", "1 cup"),
                new IngredientSeed("Broccoli", "1 cup"),
                new IngredientSeed("Extra virgin olive oil", "1 tbsp"),
                new IngredientSeed("Herbs and lemon", "to taste")
            ],
            cancellationToken);

        await EnsureRecipeAsync(
            "Whole-grain pasta with legumes",
            "A modest whole-grain pasta dish with chickpeas, tomatoes, greens, herbs, lemon, and olive oil.",
            foodMap,
            [
                new IngredientSeed("Whole-grain pasta", "1 cup cooked"),
                new IngredientSeed("Chickpeas", "3/4 cup cooked"),
                new IngredientSeed("Tomatoes", "1 cup"),
                new IngredientSeed("Leafy greens", "1 cup"),
                new IngredientSeed("Extra virgin olive oil", "1 tbsp"),
                new IngredientSeed("Herbs and lemon", "to taste")
            ],
            cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task EnsureGuidelineAsync(
        string title,
        string summary,
        CancellationToken cancellationToken)
    {
        var exists = await dbContext.DietGuidelines
            .AnyAsync(guideline => guideline.Title == title, cancellationToken);

        if (exists)
        {
            return;
        }

        await dbContext.DietGuidelines.AddAsync(new DietGuideline
        {
            Id = Guid.NewGuid(),
            Title = title,
            Summary = summary,
            SourceNote = SourceNote,
            IsActive = true,
            CreatedAtUtc = DateTimeOffset.UtcNow
        }, cancellationToken);
    }

    private async Task EnsureFoodAsync(
        FoodSeed seed,
        CancellationToken cancellationToken)
    {
        var exists = await dbContext.Foods
            .AnyAsync(food => food.Name == seed.Name, cancellationToken);

        if (exists)
        {
            return;
        }

        await dbContext.Foods.AddAsync(new Food
        {
            Id = Guid.NewGuid(),
            Name = seed.Name,
            Category = seed.Category,
            CompatibilityNotes = seed.CompatibilityNotes,
            IsActive = true,
            CreatedAtUtc = DateTimeOffset.UtcNow
        }, cancellationToken);
    }

    private async Task EnsureRecipeAsync(
        string name,
        string description,
        IReadOnlyDictionary<string, Food> foodMap,
        IReadOnlyCollection<IngredientSeed> ingredients,
        CancellationToken cancellationToken)
    {
        var exists = await dbContext.Recipes
            .AnyAsync(recipe => recipe.Name == name, cancellationToken);

        if (exists)
        {
            return;
        }

        var recipe = new Recipe
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            IsActive = true,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };

        foreach (var ingredient in ingredients)
        {
            if (!foodMap.TryGetValue(ingredient.FoodName, out var food))
            {
                continue;
            }

            recipe.Ingredients.Add(new RecipeIngredient
            {
                Id = Guid.NewGuid(),
                FoodId = food.Id,
                QuantityText = ingredient.QuantityText
            });
        }

        await dbContext.Recipes.AddAsync(recipe, cancellationToken);
    }

    private sealed record FoodSeed(
        string Name,
        string Category,
        string CompatibilityNotes);

    private sealed record IngredientSeed(
        string FoodName,
        string QuantityText);
}
