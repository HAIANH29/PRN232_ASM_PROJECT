using System.ComponentModel.DataAnnotations;
using LongevityDiet.DietKnowledge.Application.Common;
using LongevityDiet.DietKnowledge.Application.Models;
using LongevityDiet.DietKnowledge.Application.Services;
using LongevityDiet.DietKnowledge.Domain.Entities;
using LongevityDiet.DietKnowledge.Infrastructure.Persistence;
using LongevityDiet.DietKnowledge.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LongevityDiet.Core.Tests;

public sealed class DietModuleTests
{
    [Fact]
    public async Task CreateFoodAsync_reuses_one_category_row_and_returns_its_name()
    {
        var repository = new InMemoryDietKnowledgeRepository();
        var service = new DietKnowledgeService(repository);

        var first = await service.CreateFoodAsync(FoodCommand("Chickpeas", "Legume"));
        var second = await service.CreateFoodAsync(FoodCommand("Lentils", "  legume "));

        Assert.Equal("Legume", first.Category);
        Assert.Equal("Legume", second.Category);
        Assert.Single(repository.Categories);
        Assert.Equal("Food", repository.Categories.Single().CategoryType);
    }

    [Fact]
    public async Task CreateFoodAsync_stores_nutrition_facts_and_rejects_negative_values()
    {
        var repository = new InMemoryDietKnowledgeRepository();
        var service = new DietKnowledgeService(repository);

        var created = await service.CreateFoodAsync(FoodCommand("Almonds", "Nut") with
        {
            NutritionFacts = [new NutritionFactCommand("30 g", 170, 6, 6, 15, 3.5m, "Book p. 40")]
        });

        var fact = Assert.Single(created.NutritionFacts!);
        Assert.Equal("30 g", fact.ServingSize);
        Assert.Equal(170, fact.EnergyKcal);

        await Assert.ThrowsAsync<ValidationException>(() =>
            service.CreateFoodAsync(FoodCommand("Walnuts", "Nut") with
            {
                NutritionFacts = [new NutritionFactCommand("30 g", -1, 0, 0, 0, 0, "")]
            }));
    }

    [Fact]
    public async Task CreateRecipeAsync_stores_steps_and_category_and_rejects_duplicate_step_numbers()
    {
        var repository = new InMemoryDietKnowledgeRepository();
        var service = new DietKnowledgeService(repository);
        var food = await service.CreateFoodAsync(ApprovedFoodCommand("Tomato", "Vegetable"));

        var created = await service.CreateRecipeAsync(RecipeCommand("Tomato soup", food.Id) with
        {
            Category = "Soup",
            Steps = [new RecipeStepCommand(2, "Simmer."), new RecipeStepCommand(1, "Chop.")]
        });

        Assert.Equal("Soup", created.Category);
        Assert.Equal(["Chop.", "Simmer."], created.Steps!.Select(step => step.Instruction).ToArray());

        await Assert.ThrowsAsync<ValidationException>(() =>
            service.CreateRecipeAsync(RecipeCommand("Tomato salad", food.Id) with
            {
                Steps = [new RecipeStepCommand(1, "Chop."), new RecipeStepCommand(1, "Mix.")]
            }));
    }

    [Fact]
    public async Task DietRegimenService_saves_guidelines_and_meals_in_day_order()
    {
        await using var dbContext = CreateDbContext();
        var guideline = AddApprovedGuideline(dbContext, "Eat mostly plants");
        var recipe = AddApprovedRecipe(dbContext, "Lentil stew");
        await dbContext.SaveChangesAsync();
        var service = new DietRegimenService(new DietRegimenRepository(dbContext));

        var created = await service.CreateAsync(RegimenCommand(
            "Two day sample menu",
            guideline.Id,
            recipe.Id,
            meals:
            [
                new DietRegimenMealCommand(2, "dinner", recipe.Id, ""),
                new DietRegimenMealCommand(1, "Lunch", recipe.Id, "Start here")
            ]));

        Assert.Equal(2, created.DurationDays);
        Assert.Equal("Eat mostly plants", Assert.Single(created.Guidelines).GuidelineTitle);
        Assert.Equal([1, 2], created.Meals.Select(meal => meal.DayNumber).ToArray());
        Assert.Equal("Dinner", created.Meals.Last().MealSlot);
        Assert.Equal("Lentil stew", created.Meals.First().RecipeName);
    }

    [Fact]
    public async Task DietRegimenService_rejects_days_outside_duration_unapproved_recipes_and_duplicates()
    {
        await using var dbContext = CreateDbContext();
        var guideline = AddApprovedGuideline(dbContext, "Eat mostly plants");
        var recipe = AddApprovedRecipe(dbContext, "Lentil stew");
        var draft = AddApprovedRecipe(dbContext, "Draft stew");
        draft.ReviewStatus = KnowledgeReviewStatuses.NeedsReview;
        await dbContext.SaveChangesAsync();
        var service = new DietRegimenService(new DietRegimenRepository(dbContext));

        await Assert.ThrowsAsync<ValidationException>(() => service.CreateAsync(RegimenCommand(
            "Too long", guideline.Id, recipe.Id,
            meals: [new DietRegimenMealCommand(3, "Lunch", recipe.Id, "")])));

        await Assert.ThrowsAsync<ValidationException>(() => service.CreateAsync(RegimenCommand(
            "Draft recipe", guideline.Id, recipe.Id,
            meals: [new DietRegimenMealCommand(1, "Lunch", draft.Id, "")])));

        await service.CreateAsync(RegimenCommand("Unique name", guideline.Id, recipe.Id));
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.CreateAsync(RegimenCommand("unique NAME", guideline.Id, recipe.Id)));
    }

    [Fact]
    public async Task DietRegimenService_public_reads_hide_inactive_and_unapproved_regimens()
    {
        await using var dbContext = CreateDbContext();
        var guideline = AddApprovedGuideline(dbContext, "Eat mostly plants");
        var recipe = AddApprovedRecipe(dbContext, "Lentil stew");
        await dbContext.SaveChangesAsync();
        var service = new DietRegimenService(new DietRegimenRepository(dbContext));

        var approved = await service.CreateAsync(RegimenCommand("Approved menu", guideline.Id, recipe.Id));
        var draft = await service.CreateAsync(RegimenCommand("Draft menu", guideline.Id, recipe.Id) with
        {
            ReviewStatus = KnowledgeReviewStatuses.NeedsReview,
            SourceChapter = "",
            SourcePage = ""
        });

        var publicList = await service.ListAsync(new DietRegimenQuery(1, 20, null, false, true, null));

        Assert.Equal(approved.Id, Assert.Single(publicList.Items).Id);
        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.GetAsync(draft.Id));
        Assert.Equal(draft.Id, (await service.GetAsync(draft.Id, includeInactive: true)).Id);

        await service.SetActivationAsync(approved.Id, false);
        Assert.Empty((await service.ListAsync(new DietRegimenQuery(1, 20, null, false, true, null))).Items);
    }

    // Regression: new child rows (preset ids) added under a tracked parent must be inserted, not updated.
    [Fact]
    public async Task Updating_a_saved_regimen_can_add_guidelines_and_meals()
    {
        await using var dbContext = CreateDbContext();
        var guideline = AddApprovedGuideline(dbContext, "Eat mostly plants");
        var second = AddApprovedGuideline(dbContext, "Whole grains first");
        var recipe = AddApprovedRecipe(dbContext, "Lentil stew");
        await dbContext.SaveChangesAsync();
        var service = new DietRegimenService(new DietRegimenRepository(dbContext));
        var created = await service.CreateAsync(RegimenCommand("Editable menu", guideline.Id, recipe.Id));

        var updated = await service.UpdateAsync(created.Id, RegimenCommand("Editable menu", guideline.Id, recipe.Id) with
        {
            Guidelines =
            [
                new DietRegimenGuidelineCommand(guideline.Id, 1, "Core rule"),
                new DietRegimenGuidelineCommand(second.Id, 2, "Second rule")
            ],
            Meals =
            [
                new DietRegimenMealCommand(1, "Lunch", recipe.Id, ""),
                new DietRegimenMealCommand(2, "Dinner", recipe.Id, "Added later")
            ]
        });

        Assert.Equal(2, updated.Guidelines.Count);
        Assert.Equal(2, updated.Meals.Count);
        Assert.Equal(2, dbContext.DietRegimenMeals.Count());
    }

    [Fact]
    public async Task Updating_saved_foods_and_recipes_can_add_nutrition_facts_and_steps()
    {
        await using var dbContext = CreateDbContext();
        var service = new DietKnowledgeService(new DietKnowledgeRepository(dbContext));
        var food = await service.CreateFoodAsync(ApprovedFoodCommand("Tomato", "Vegetable"));
        var recipe = await service.CreateRecipeAsync(RecipeCommand("Tomato soup", food.Id) with
        {
            Steps = [new RecipeStepCommand(1, "Chop.")]
        });

        var updatedFood = await service.UpdateFoodAsync(food.Id, new UpdateFoodCommand(
            "Tomato", "Vegetable", "Notes", "The Longevity Diet", "Ch. 2", "10", "", "Approved", "Admin",
            [new NutritionFactCommand("100 g", 18, 0.9m, 3.9m, 0.2m, 1.2m, "Book p. 40")]));
        Assert.Single(updatedFood.NutritionFacts!);

        // Replace: one fact is kept as a new row with a different serving size.
        updatedFood = await service.UpdateFoodAsync(food.Id, new UpdateFoodCommand(
            "Tomato", "Vegetable", "Notes", "The Longevity Diet", "Ch. 2", "10", "", "Approved", "Admin",
            [new NutritionFactCommand("1 medium", 22, 1, 5, 0.2m, 1.5m, "Book p. 40")]));
        Assert.Equal("1 medium", Assert.Single(updatedFood.NutritionFacts!).ServingSize);

        var updatedRecipe = await service.UpdateRecipeAsync(recipe.Id, new UpdateRecipeCommand(
            "Tomato soup", "Description", "The Longevity Diet", "Ch. 3", "20", "", "NeedsReview", "",
            [new RecipeIngredientCommand(food.Id, "2 cups")],
            Category: "Soup",
            Steps: [new RecipeStepCommand(1, "Chop finely."), new RecipeStepCommand(2, "Simmer.")]));

        Assert.Equal(["Chop finely.", "Simmer."], updatedRecipe.Steps!.Select(step => step.Instruction).ToArray());
        Assert.Equal("Soup", updatedRecipe.Category);
    }

    private static CreateFoodCommand FoodCommand(string name, string category)
    {
        return new CreateFoodCommand(
            name, category, "Notes", "The Longevity Diet", "", "", "", "NeedsReview", "");
    }

    private static CreateFoodCommand ApprovedFoodCommand(string name, string category)
    {
        return new CreateFoodCommand(
            name, category, "Notes", "The Longevity Diet", "Ch. 2", "10", "", "Approved", "Admin");
    }

    private static CreateRecipeCommand RecipeCommand(string name, Guid foodId)
    {
        return new CreateRecipeCommand(
            name,
            "Description",
            "The Longevity Diet",
            "Ch. 3",
            "20",
            "",
            "NeedsReview",
            "",
            [new RecipeIngredientCommand(foodId, "1 cup")]);
    }

    private static SaveDietRegimenCommand RegimenCommand(
        string name,
        Guid guidelineId,
        Guid recipeId,
        IReadOnlyCollection<DietRegimenMealCommand>? meals = null)
    {
        return new SaveDietRegimenCommand(
            name,
            "Educational sample menu.",
            DurationDays: 2,
            "The Longevity Diet",
            "Ch. 9",
            "120",
            "Approved",
            [new DietRegimenGuidelineCommand(guidelineId, 1, "Core rule")],
            meals ?? [new DietRegimenMealCommand(1, "Lunch", recipeId, "")]);
    }

    private static DietGuideline AddApprovedGuideline(DietKnowledgeDbContext dbContext, string title)
    {
        var guideline = new DietGuideline
        {
            Id = Guid.NewGuid(),
            Title = title,
            Summary = "Summary",
            ReviewStatus = KnowledgeReviewStatuses.Approved,
            IsActive = true
        };
        dbContext.DietGuidelines.Add(guideline);
        return guideline;
    }

    private static Recipe AddApprovedRecipe(DietKnowledgeDbContext dbContext, string name)
    {
        var recipe = new Recipe
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = "Description",
            ReviewStatus = KnowledgeReviewStatuses.Approved,
            IsActive = true
        };
        dbContext.Recipes.Add(recipe);
        return recipe;
    }

    private static DietKnowledgeDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<DietKnowledgeDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new DietKnowledgeDbContext(options);
    }
}
