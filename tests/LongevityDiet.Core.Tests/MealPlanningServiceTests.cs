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
            outbox,
            new FixedTimeProvider(new DateTimeOffset(2026, 10, 1, 0, 0, 0, TimeSpan.Zero)));

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
            new RecordingOutbox(),
            new FixedTimeProvider(new DateTimeOffset(2026, 10, 1, 0, 0, 0, TimeSpan.Zero)));

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

    // 2026-10-10 12:00 in UTC+7 (the default offset) = 05:00 UTC: it is lunchtime for the user.
    private static readonly DateTimeOffset LunchtimeNow = new(2026, 10, 10, 5, 0, 0, TimeSpan.Zero);
    private static readonly DateOnly Today = new(2026, 10, 10);

    private static (MealPlanningService Service, RecordingOutbox Outbox, Guid FoodId) CreateServiceAt(DateTimeOffset now)
    {
        var foodId = Guid.NewGuid();
        var catalog = new FakeDietKnowledgeCatalogClient();
        catalog.ExistingFoodIds.Add(foodId);
        var outbox = new RecordingOutbox();
        var service = new MealPlanningService(
            new InMemoryMealPlanRepository(), catalog, outbox, new FixedTimeProvider(now));
        return (service, outbox, foodId);
    }

    private static CreateMealPlanCommand PlanFor(DateOnly start, DateOnly end, Guid foodId, DateOnly itemDate, string slot) => new(
        UserId: Guid.NewGuid(),
        UserEmail: "user@example.com",
        Name: "Plan",
        StartDate: start,
        EndDate: end,
        Items: [new CreateMealPlanItemCommand(itemDate, slot, null, foodId, "", null)]);

    [Fact]
    public async Task CreateMealPlanAsync_rejects_a_meal_slot_that_already_passed_today()
    {
        var (service, outbox, foodId) = CreateServiceAt(LunchtimeNow);

        await Assert.ThrowsAsync<ValidationException>(() =>
            service.CreateMealPlanAsync(PlanFor(Today, Today.AddDays(2), foodId, Today, "Breakfast")));

        Assert.Empty(outbox.Reminders);
    }

    [Fact]
    public async Task CreateMealPlanAsync_allows_the_current_and_later_slots_today_but_only_schedules_future_reminders()
    {
        var (service, outbox, foodId) = CreateServiceAt(LunchtimeNow);

        // Lunch is in progress (its 11:00 reminder is already due): saved, but no reminder mail is queued.
        await service.CreateMealPlanAsync(PlanFor(Today, Today, foodId, Today, "Lunch"));
        Assert.Empty(outbox.Reminders);

        // Dinner today (17:00 reminder) is still ahead: saved with a reminder.
        await service.CreateMealPlanAsync(PlanFor(Today, Today, foodId, Today, "Dinner"));
        Assert.Single(outbox.Reminders);
        Assert.True(outbox.Reminders.Single().ScheduledForUtc > LunchtimeNow);
    }

    [Fact]
    public async Task CreateMealPlanAsync_rejects_a_plan_that_starts_in_the_past_and_accepts_tomorrow()
    {
        var (service, _, foodId) = CreateServiceAt(LunchtimeNow);

        await Assert.ThrowsAsync<ValidationException>(() =>
            service.CreateMealPlanAsync(PlanFor(Today.AddDays(-1), Today.AddDays(2), foodId, Today, "Dinner")));

        var plan = await service.CreateMealPlanAsync(
            PlanFor(Today.AddDays(1), Today.AddDays(2), foodId, Today.AddDays(1), "Breakfast"));
        Assert.Single(plan.Items);
    }

    [Fact]
    public async Task AddMealPlanItemAsync_rejects_an_earlier_slot_of_today_and_default_reminders_use_local_time()
    {
        var (service, outbox, foodId) = CreateServiceAt(LunchtimeNow);
        var userId = Guid.NewGuid();
        var plan = await service.CreateMealPlanAsync(new CreateMealPlanCommand(
            userId, "user@example.com", "Plan", Today, Today.AddDays(2), []));

        await Assert.ThrowsAsync<ValidationException>(() =>
            service.AddMealPlanItemAsync(plan.Id, new AddMealPlanItemCommand(
                userId, "user@example.com", Today, "Breakfast", null, foodId, "", null)));

        // Tomorrow's breakfast reminder is 07:00 local (UTC+7) = 00:00 UTC.
        await service.AddMealPlanItemAsync(plan.Id, new AddMealPlanItemCommand(
            userId, "user@example.com", Today.AddDays(1), "Breakfast", null, foodId, "", null));
        Assert.Equal(
            new DateTimeOffset(2026, 10, 11, 0, 0, 0, TimeSpan.Zero),
            outbox.Reminders.Single().ScheduledForUtc);
    }
}
