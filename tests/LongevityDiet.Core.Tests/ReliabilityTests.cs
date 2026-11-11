using LongevityDiet.ApiDefaults.Extensions;
using LongevityDiet.Contracts.Messaging;
using LongevityDiet.MealPlanning.Application.Abstractions;
using LongevityDiet.MealPlanning.Application.Common;
using LongevityDiet.MealPlanning.Application.Models;
using LongevityDiet.MealPlanning.Application.Services;
using LongevityDiet.MealPlanning.Infrastructure.Messaging;
using LongevityDiet.MealPlanning.Infrastructure.Persistence;
using LongevityDiet.Tracking.Application.Models;
using LongevityDiet.Tracking.Application.Services;
using LongevityDiet.Tracking.Infrastructure.Persistence;
using LongevityDiet.Tracking.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace LongevityDiet.Core.Tests;

public sealed class ReliabilityTests
{
    private static readonly DateOnly PlanDay = new(2026, 11, 20);

    // ---- Reminders follow the meal plan item (issue: stale reminders) ----

    [Fact]
    public async Task UpdateMealPlanItemAsync_keeps_the_waiting_reminder_when_the_time_is_unchanged()
    {
        var (service, outbox, userId, planId, itemId, foodId) = await CreatePlanWithOneItemAsync();
        outbox.Reminders.Clear();

        await service.UpdateMealPlanItemAsync(planId, itemId, UpdateItem(userId, foodId, PlanDay, notes: "new note"));

        Assert.Empty(outbox.Reminders);
    }

    [Fact]
    public async Task UpdateMealPlanItemAsync_queues_a_new_reminder_when_the_time_moves()
    {
        var (service, outbox, userId, planId, itemId, foodId) = await CreatePlanWithOneItemAsync();
        outbox.Reminders.Clear();

        await service.UpdateMealPlanItemAsync(planId, itemId, UpdateItem(userId, foodId, PlanDay.AddDays(1), notes: ""));

        var reminder = Assert.Single(outbox.Reminders);
        Assert.Equal(itemId, reminder.MealPlanItemId);
    }

    [Fact]
    public async Task IsReminderCurrentAsync_is_false_after_the_item_moves_or_is_deleted()
    {
        var (service, outbox, userId, planId, itemId, foodId) = await CreatePlanWithOneItemAsync();
        var original = outbox.Reminders.Single();
        Assert.True(await service.IsReminderCurrentAsync(itemId, original.ScheduledForUtc));

        await service.UpdateMealPlanItemAsync(planId, itemId, UpdateItem(userId, foodId, PlanDay.AddDays(1), notes: ""));
        Assert.False(await service.IsReminderCurrentAsync(itemId, original.ScheduledForUtc));
        Assert.True(await service.IsReminderCurrentAsync(itemId, outbox.Reminders.Last().ScheduledForUtc));

        await service.DeleteMealPlanItemAsync(planId, itemId, userId);
        Assert.False(await service.IsReminderCurrentAsync(itemId, outbox.Reminders.Last().ScheduledForUtc));
    }

    // ---- Outbox (issue: plan saved but message lost) ----

    [Fact]
    public async Task OutboxDispatcher_publishes_pending_messages_once()
    {
        await using var dbContext = CreateMealPlanningDbContext();
        var reminders = new CapturingReminderPublisher();
        var requests = new CapturingRecommendationPublisher();
        var outbox = new EfMealPlanOutbox(dbContext);
        outbox.Enqueue(SampleReminder());
        outbox.Enqueue(new RecommendationRequestMessage(Guid.NewGuid(), Guid.NewGuid(), ["legumes"], 2, DateTimeOffset.UtcNow));
        await dbContext.SaveChangesAsync();

        var dispatcher = new OutboxDispatcher(dbContext, reminders, requests, NullLogger<OutboxDispatcher>.Instance);
        Assert.Equal(2, await dispatcher.DispatchPendingAsync(10, CancellationToken.None));
        Assert.Equal(0, await dispatcher.DispatchPendingAsync(10, CancellationToken.None));

        Assert.Single(reminders.Messages);
        Assert.Single(requests.Messages);
        Assert.All(dbContext.OutboxMessages, message => Assert.NotNull(message.ProcessedAtUtc));
    }

    [Fact]
    public async Task OutboxDispatcher_keeps_the_message_and_backs_off_when_the_broker_is_down()
    {
        await using var dbContext = CreateMealPlanningDbContext();
        var reminders = new CapturingReminderPublisher { Fail = true };
        new EfMealPlanOutbox(dbContext).Enqueue(SampleReminder());
        await dbContext.SaveChangesAsync();

        var dispatcher = new OutboxDispatcher(
            dbContext,
            reminders,
            new CapturingRecommendationPublisher(),
            NullLogger<OutboxDispatcher>.Instance);

        Assert.Equal(0, await dispatcher.DispatchPendingAsync(10, CancellationToken.None));

        var stored = Assert.Single(dbContext.OutboxMessages);
        Assert.Null(stored.ProcessedAtUtc);
        Assert.Equal(1, stored.Attempts);
        Assert.True(stored.NextAttemptAtUtc > DateTimeOffset.UtcNow);
        Assert.Equal(OutboxMessageTypes.Reminder, stored.MessageType);

        // Once the broker is back the same row is delivered (after its back-off has passed).
        reminders.Fail = false;
        stored.NextAttemptAtUtc = DateTimeOffset.UtcNow.AddSeconds(-1);
        await dbContext.SaveChangesAsync();
        Assert.Equal(1, await dispatcher.DispatchPendingAsync(10, CancellationToken.None));
        Assert.Single(reminders.Messages);
    }

    // ---- Progress counts planned meals (issue: 1/1 treated as a finished day) ----

    [Fact]
    public async Task SetMealCompletionAsync_uses_planned_meal_count_for_progress_and_notification()
    {
        await using var dbContext = CreateTrackingDbContext();
        var notifications = new RecordingNotificationClient();
        var service = new TrackingService(
            new TrackingRepository(dbContext),
            notifications,
            NullLogger<TrackingService>.Instance);
        var userId = Guid.NewGuid();
        var day = new DateOnly(2026, 11, 20);
        var meals = new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

        await service.SetMealCompletionAsync(new SetMealCompletionCommand(
            userId, "user@example.com", day, meals[0], true, PlannedMealCount: 3));

        var summary = await service.GetProgressSummaryAsync(userId, day, day);
        Assert.Equal(3, summary.PlannedMeals);
        Assert.Equal(1, summary.CompletedMeals);
        Assert.Empty(notifications.Requests);

        await service.SetMealCompletionAsync(new SetMealCompletionCommand(
            userId, "user@example.com", day, meals[1], true, PlannedMealCount: 3));
        Assert.Empty(notifications.Requests);

        await service.SetMealCompletionAsync(new SetMealCompletionCommand(
            userId, "user@example.com", day, meals[2], true, PlannedMealCount: 3));
        Assert.Single(notifications.Requests);
    }

    [Fact]
    public async Task SetMealCompletionAsync_rejects_an_out_of_range_planned_meal_count()
    {
        await using var dbContext = CreateTrackingDbContext();
        var service = new TrackingService(
            new TrackingRepository(dbContext),
            new RecordingNotificationClient(),
            NullLogger<TrackingService>.Instance);

        await Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() =>
            service.SetMealCompletionAsync(new SetMealCompletionCommand(
                Guid.NewGuid(), "user@example.com", PlanDay, Guid.NewGuid(), true, PlannedMealCount: 99)));
    }

    // ---- JWT signing key (issue: silent public default) ----

    [Fact]
    public void JwtSigningKeyResolver_refuses_missing_or_public_keys_outside_development()
    {
        var production = new FakeEnvironment(Environments.Production);

        Assert.Throws<InvalidOperationException>(() => JwtSigningKeyResolver.Resolve(Config(), production));
        Assert.Throws<InvalidOperationException>(() => JwtSigningKeyResolver.Resolve(
            Config(("Jwt:SigningKey", JwtSigningKeyResolver.DevelopmentKey)), production));
        Assert.Throws<InvalidOperationException>(() => JwtSigningKeyResolver.Resolve(
            Config(("Jwt:SigningKey", "too-short")), production));
    }

    [Fact]
    public void JwtSigningKeyResolver_accepts_private_keys_and_explicit_development_use()
    {
        var production = new FakeEnvironment(Environments.Production);
        var privateKey = new string('k', 40);

        Assert.Equal(privateKey, JwtSigningKeyResolver.Resolve(Config(("Jwt:SigningKey", privateKey)), production));
        Assert.Equal(
            JwtSigningKeyResolver.DevelopmentKey,
            JwtSigningKeyResolver.Resolve(
                Config(("Jwt:SigningKey", JwtSigningKeyResolver.DevelopmentKey), ("Jwt:AllowDevelopmentKey", "true")),
                production));
        Assert.Equal(
            JwtSigningKeyResolver.DevelopmentKey,
            JwtSigningKeyResolver.Resolve(Config(), new FakeEnvironment(Environments.Development)));
    }

    // ---- helpers ----

    private static async Task<(MealPlanningService Service, RecordingOutbox Outbox, Guid UserId, Guid PlanId, Guid ItemId, Guid FoodId)>
        CreatePlanWithOneItemAsync()
    {
        var foodId = Guid.NewGuid();
        var catalog = new FakeDietKnowledgeCatalogClient();
        catalog.ExistingFoodIds.Add(foodId);
        var outbox = new RecordingOutbox();
        var service = new MealPlanningService(
            new InMemoryMealPlanRepository(), catalog, outbox,
            new FixedTimeProvider(new DateTimeOffset(2026, 11, 1, 0, 0, 0, TimeSpan.Zero)));
        var userId = Guid.NewGuid();

        var plan = await service.CreateMealPlanAsync(new CreateMealPlanCommand(
            userId,
            "user@example.com",
            "Week",
            PlanDay,
            PlanDay.AddDays(3),
            [new CreateMealPlanItemCommand(PlanDay, "Breakfast", null, foodId, "", null)]));

        return (service, outbox, userId, plan.Id, plan.Items.Single().Id, foodId);
    }

    private static UpdateMealPlanItemCommand UpdateItem(Guid userId, Guid foodId, DateOnly date, string notes)
    {
        return new UpdateMealPlanItemCommand(userId, "user@example.com", date, "Breakfast", null, foodId, notes, null);
    }
    private static ReminderRequestedMessage SampleReminder() => new(
        Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "user@example.com",
        DateTimeOffset.UtcNow.AddHours(2), "Meal reminder", "Body", Guid.NewGuid());

    private static MealPlanningDbContext CreateMealPlanningDbContext() =>
        new(new DbContextOptionsBuilder<MealPlanningDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    private static TrackingDbContext CreateTrackingDbContext() =>
        new(new DbContextOptionsBuilder<TrackingDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    private static IConfiguration Config(params (string Key, string Value)[] values) =>
        new ConfigurationBuilder()
            .AddInMemoryCollection(values.ToDictionary(item => item.Key, item => (string?)item.Value))
            .Build();

    private sealed class FakeEnvironment(string name) : IHostEnvironment
    {
        public string EnvironmentName { get; set; } = name;

        public string ApplicationName { get; set; } = "Tests";

        public string ContentRootPath { get; set; } = ".";

        public Microsoft.Extensions.FileProviders.IFileProvider ContentRootFileProvider { get; set; } = null!;
    }

    private sealed class CapturingReminderPublisher : IReminderPublisher
    {
        public bool Fail { get; set; }

        public List<ReminderRequestedMessage> Messages { get; } = [];

        public Task PublishAsync(ReminderRequestedMessage message, CancellationToken cancellationToken = default)
        {
            if (Fail)
            {
                throw new InvalidOperationException("Broker is down.");
            }

            Messages.Add(message);
            return Task.CompletedTask;
        }
    }

    private sealed class CapturingRecommendationPublisher : IRecommendationRequestPublisher
    {
        public List<RecommendationRequestMessage> Messages { get; } = [];

        public Task PublishAsync(RecommendationRequestMessage message, CancellationToken cancellationToken = default)
        {
            Messages.Add(message);
            return Task.CompletedTask;
        }
    }
}
