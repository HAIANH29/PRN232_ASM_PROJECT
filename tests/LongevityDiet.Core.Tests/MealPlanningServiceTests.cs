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
        var outbox = new RecordingOutbox();
        var service = new MealPlanningService(
            repository,
            catalog,
            outbox);

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
        Assert.Single(outbox.Reminders);
        Assert.Equal("user@example.com", outbox.Reminders.Single().RecipientEmail);
        Assert.Contains("Breakfast", outbox.Reminders.Single().Subject, StringComparison.Ordinal);
        Assert.Empty(outbox.Recommendations);
    }

    [Fact]
    public async Task CreateMealPlanAsync_rejects_missing_food_reference()
    {
        var repository = new InMemoryMealPlanRepository();
        var service = new MealPlanningService(
            repository,
            new FakeDietKnowledgeCatalogClient(),
            new RecordingOutbox());

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
        var outbox = new RecordingOutbox();
        var service = new MealPlanningService(
            repository,
            new FakeDietKnowledgeCatalogClient(),
            outbox);
        var userId = Guid.NewGuid();

        var result = await service.RequestRecommendationAsync(new CreateRecommendationRequestCommand(
            userId,
            [" legumes ", "", "Legumes", "vegetable"],
            3));

        Assert.Single(repository.RecommendationRequests);
        Assert.Single(outbox.Recommendations);
        Assert.Equal(result.Id, outbox.Recommendations.Single().RequestId);
        Assert.Equal(userId, outbox.Recommendations.Single().UserId);
        Assert.Equal(["legumes", "vegetable"], outbox.Recommendations.Single().PreferenceTags);
        Assert.Equal(3, outbox.Recommendations.Single().Days);
    }
}
