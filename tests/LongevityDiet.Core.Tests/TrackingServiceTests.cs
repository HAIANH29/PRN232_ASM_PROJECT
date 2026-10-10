using System.ComponentModel.DataAnnotations;
using LongevityDiet.Tracking.Application.Models;
using LongevityDiet.Tracking.Application.Services;
using LongevityDiet.Tracking.Infrastructure.Persistence;
using LongevityDiet.Tracking.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace LongevityDiet.Core.Tests;

public sealed class TrackingServiceTests
{
    [Fact]
    public async Task SetMealCompletionAsync_calculates_completed_daily_progress_and_notifies_once()
    {
        await using var dbContext = CreateTrackingDbContext();
        var notificationClient = new RecordingNotificationClient();
        var repository = new TrackingRepository(dbContext);
        var service = new TrackingService(
            repository,
            notificationClient,
            NullLogger<TrackingService>.Instance);
        var userId = Guid.NewGuid();
        var mealPlanItemId = Guid.NewGuid();
        var trackingDate = new DateOnly(2026, 10, 10);

        var dailyTracking = await service.SetMealCompletionAsync(new SetMealCompletionCommand(
            userId,
            "user@example.com",
            trackingDate,
            mealPlanItemId,
            IsCompleted: true));

        Assert.Single(dailyTracking.Meals);
        Assert.True(dailyTracking.Meals.Single().IsCompleted);
        Assert.Single(notificationClient.Requests);

        var summary = await service.GetProgressSummaryAsync(userId, trackingDate, trackingDate);
        Assert.Equal(1, summary.PlannedMeals);
        Assert.Equal(1, summary.CompletedMeals);
        Assert.Equal(1m, summary.CompletionRate);
        Assert.Equal(notificationClient.NotificationId, summary.LastNotificationId);

        await service.SetMealCompletionAsync(new SetMealCompletionCommand(
            userId,
            "user@example.com",
            trackingDate,
            mealPlanItemId,
            IsCompleted: true));

        Assert.Single(notificationClient.Requests);
    }

    [Fact]
    public async Task GetProgressSummaryAsync_rejects_invalid_period()
    {
        await using var dbContext = CreateTrackingDbContext();
        var service = new TrackingService(
            new TrackingRepository(dbContext),
            new RecordingNotificationClient(),
            NullLogger<TrackingService>.Instance);

        await Assert.ThrowsAsync<ValidationException>(() =>
            service.GetProgressSummaryAsync(
                Guid.NewGuid(),
                new DateOnly(2026, 10, 11),
                new DateOnly(2026, 10, 10)));
    }

    private static TrackingDbContext CreateTrackingDbContext()
    {
        var options = new DbContextOptionsBuilder<TrackingDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new TrackingDbContext(options);
    }
}
