using System.ComponentModel.DataAnnotations;
using LongevityDiet.MealPlanning.Application.Common;
using LongevityDiet.MealPlanning.Application.Models;
using LongevityDiet.MealPlanning.Application.Services;
using Xunit;

namespace LongevityDiet.Core.Tests;

public sealed class MealPlanningServiceTests
{
    [Fact]
    public async Task CreateMealPlanAsync_validates_catalog_reference_and_publishes_reminder()
    {
        var foodId = Guid.NewGuid();
        var repository = new InMemoryMealPlanRepository();
        var catalog = new FakeDietKnowledgeCatalogClient();
        catalog.ExistingFoodIds.Add(foodId);
        var reminderPublisher = new RecordingReminderPublisher();
        var recommendationPublisher = new RecordingRecommendationRequestPublisher();
        var service = new MealPlanningService(
            repository,
            catalog,
            reminderPublisher,
            recommendationPublisher);

        var plan = await service.CreateMealPlanAsync(new CreateMealPlanCommand(
            UserId: Guid.NewGuid(),
            UserEmail: "user@example.com",
            Name: "Week one",
            StartDate: new DateOnly(2026, 10, 10),
            EndDate: new DateOnly(2026, 10, 12),
            Items:
            [
                new CreateMealPlanItemCommand(
                    PlannedDate: new DateOnly(2026, 10, 10),
                    MealSlot: "breakfast",
                    RecipeId: null,
                    FoodId: foodId,
                    Notes: "simple meal",
                    ReminderAtUtc: null)
            ]));

        Assert.Single(repository.MealPlans);
        Assert.Single(plan.Items);
        Assert.Equal(MealSlots.Breakfast, plan.Items.Single().MealSlot);
        Assert.Single(reminderPublisher.Messages);
        Assert.Equal("user@example.com", reminderPublisher.Messages.Single().RecipientEmail);
        Assert.Contains("Breakfast", reminderPublisher.Messages.Single().Subject, StringComparison.Ordinal);
        Assert.Empty(recommendationPublisher.Messages);
    }

    [Fact]
    public async Task CreateMealPlanAsync_rejects_missing_food_reference()
    {
        var repository = new InMemoryMealPlanRepository();
        var service = new MealPlanningService(
            repository,
            new FakeDietKnowledgeCatalogClient(),
            new RecordingReminderPublisher(),
            new RecordingRecommendationRequestPublisher());

        await Assert.ThrowsAsync<ValidationException>(() =>
            service.CreateMealPlanAsync(new CreateMealPlanCommand(
                UserId: Guid.NewGuid(),
                UserEmail: "user@example.com",
                Name: "Week one",
                StartDate: new DateOnly(2026, 10, 10),
                EndDate: new DateOnly(2026, 10, 12),
                Items:
                [
                    new CreateMealPlanItemCommand(
                        PlannedDate: new DateOnly(2026, 10, 10),
                        MealSlot: MealSlots.Lunch,
                        RecipeId: null,
                        FoodId: Guid.NewGuid(),
                        Notes: null,
                        ReminderAtUtc: null)
                ])));

        Assert.Empty(repository.MealPlans);
    }

    [Fact]
    public async Task RequestRecommendationAsync_cleans_tags_and_publishes_message()
    {
        var repository = new InMemoryMealPlanRepository();
        var recommendationPublisher = new RecordingRecommendationRequestPublisher();
        var service = new MealPlanningService(
            repository,
            new FakeDietKnowledgeCatalogClient(),
            new RecordingReminderPublisher(),
            recommendationPublisher);
        var userId = Guid.NewGuid();

        var result = await service.RequestRecommendationAsync(new CreateRecommendationRequestCommand(
            userId,
            [" legumes ", "", "Legumes", "vegetable"],
            3));

        Assert.Single(repository.RecommendationRequests);
        Assert.Single(recommendationPublisher.Messages);
        Assert.Equal(result.Id, recommendationPublisher.Messages.Single().RequestId);
        Assert.Equal(userId, recommendationPublisher.Messages.Single().UserId);
        Assert.Equal(["legumes", "vegetable"], recommendationPublisher.Messages.Single().PreferenceTags);
        Assert.Equal(3, recommendationPublisher.Messages.Single().Days);
    }
}
