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

public sealed class DietKnowledgeTests
{
    [Fact]
    public async Task ListFoodsAsync_filters_active_approved_search_category_and_paginates()
    {
        await using var dbContext = CreateDietKnowledgeDbContext();
        var approvedFirst = CreateFood("Black lentils", "Legume", KnowledgeReviewStatuses.Approved);
        var approvedSecond = CreateFood("Green lentils", "Legume", KnowledgeReviewStatuses.Approved);
        var inactive = CreateFood("Red lentils", "Legume", KnowledgeReviewStatuses.Approved, isActive: false);
        var needsReview = CreateFood("Yellow lentils", "Legume", KnowledgeReviewStatuses.NeedsReview);
        var wrongCategory = CreateFood("Walnuts", "Nut", KnowledgeReviewStatuses.Approved);

        dbContext.Foods.AddRange(approvedSecond, inactive, wrongCategory, needsReview, approvedFirst);
        await dbContext.SaveChangesAsync();

        var repository = new DietKnowledgeRepository(dbContext);
        var result = await repository.ListFoodsAsync(new FoodQuery(
            PageNumber: 1,
            PageSize: 1,
            Search: "lentil",
            Category: "Legume",
            IncludeInactive: false,
            ApprovedOnly: true,
            ReviewStatus: null,
            SortBy: "name",
            SortDirection: "asc"));

        Assert.Equal(2, result.TotalCount);
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(1, result.PageSize);
        Assert.Single(result.Items);
        Assert.Equal("Black lentils", result.Items.Single().Name);
    }

    [Fact]
    public async Task CreateGuidelineAsync_requires_source_metadata_for_approved_content()
    {
        var service = new DietKnowledgeService(new InMemoryDietKnowledgeRepository());

        await Assert.ThrowsAsync<ValidationException>(() =>
            service.CreateGuidelineAsync(new CreateDietGuidelineCommand(
                Title: "Plant focused pattern",
                Summary: "Educational project summary.",
                SourceNote: "",
                SourceTitle: "The Longevity Diet",
                SourceChapter: "",
                SourcePage: "",
                SourceReference: "",
                ReviewStatus: KnowledgeReviewStatuses.Approved,
                ReviewedBy: "Admin")));
    }

    [Fact]
    public async Task CreateRecipeAsync_rejects_duplicate_ingredient_food_ids()
    {
        var foodId = Guid.NewGuid();
        var repository = new InMemoryDietKnowledgeRepository();
        repository.Foods.Add(CreateFood("Beans", "Legume", KnowledgeReviewStatuses.Approved, id: foodId));
        var service = new DietKnowledgeService(repository);

        await Assert.ThrowsAsync<ValidationException>(() =>
            service.CreateRecipeAsync(new CreateRecipeCommand(
                Name: "Bean bowl",
                Description: "Project recipe.",
                Ingredients:
                [
                    new RecipeIngredientCommand(foodId, "1 cup"),
                    new RecipeIngredientCommand(foodId, "extra")
                ],
                SourceTitle: "The Longevity Diet",
                SourceChapter: "Food choices",
                SourcePage: "42",
                SourceReference: "",
                ReviewStatus: KnowledgeReviewStatuses.Approved,
                ReviewedBy: "Admin")));
    }

    private static DietKnowledgeDbContext CreateDietKnowledgeDbContext()
    {
        var options = new DbContextOptionsBuilder<DietKnowledgeDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new DietKnowledgeDbContext(options);
    }

    private static Food CreateFood(
        string name,
        string category,
        string reviewStatus,
        bool isActive = true,
        Guid? id = null)
    {
        return new Food
        {
            Id = id ?? Guid.NewGuid(),
            Name = name,
            Category = category,
            CompatibilityNotes = $"{name} notes",
            SourceTitle = "The Longevity Diet",
            SourceChapter = "Demo chapter",
            SourcePage = "1",
            SourceReference = "",
            ReviewStatus = reviewStatus,
            ReviewedBy = reviewStatus == KnowledgeReviewStatuses.Approved ? "Admin" : "",
            ReviewedAtUtc = reviewStatus == KnowledgeReviewStatuses.Approved ? DateTimeOffset.UtcNow : null,
            IsActive = isActive,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };
    }
}
