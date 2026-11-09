using System.ComponentModel.DataAnnotations;
using LongevityDiet.Identity.Application.Models;
using LongevityDiet.Identity.Application.Services;
using LongevityDiet.Tracking.Application.Models;
using LongevityDiet.Tracking.Application.Services;
using LongevityDiet.Tracking.Infrastructure.Persistence;
using LongevityDiet.Tracking.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace LongevityDiet.Core.Tests;

public sealed class UserProfileAndDietTrackingTests
{
    [Fact]
    public async Task UpdatePreferencesAsync_creates_profile_trims_dedupes_tags_and_shows_them_on_the_profile()
    {
        var repository = new InMemoryUserRepository();
        var service = new IdentityService(repository, new FakePasswordHasher(), new FakeJwtTokenGenerator());
        var registered = await service.RegisterAsync(
            new RegisterUserCommand("pref@example.com", "Pass@123456", "Pref User"));

        Assert.Empty(registered.Profile.PreferenceTags!);

        var updated = await service.UpdatePreferencesAsync(
            registered.Profile.Id,
            new UpdatePreferencesCommand([" vegetarian ", "Legumes", "VEGETARIAN", ""], "  likes soup "));

        Assert.Equal(["vegetarian", "Legumes"], updated.PreferenceTags!.ToArray());
        Assert.Equal("likes soup", updated.Notes);

        var reloaded = await service.GetProfileAsync(registered.Profile.Id);
        Assert.Equal(["vegetarian", "Legumes"], reloaded.PreferenceTags!.ToArray());
        Assert.NotNull(repository.Users.Single().Profile);

        await service.UpdatePreferencesAsync(
            registered.Profile.Id,
            new UpdatePreferencesCommand([], null));
        Assert.Empty((await service.GetProfileAsync(registered.Profile.Id)).PreferenceTags!);
    }

    [Fact]
    public async Task UpdatePreferencesAsync_rejects_commas_long_tags_too_many_tags_and_unknown_users()
    {
        var repository = new InMemoryUserRepository();
        var service = new IdentityService(repository, new FakePasswordHasher(), new FakeJwtTokenGenerator());
        var registered = await service.RegisterAsync(
            new RegisterUserCommand("pref2@example.com", "Pass@123456", "Pref User"));
        var userId = registered.Profile.Id;

        await Assert.ThrowsAsync<ValidationException>(() => service.UpdatePreferencesAsync(
            userId, new UpdatePreferencesCommand(["a,b"], null)));
        await Assert.ThrowsAsync<ValidationException>(() => service.UpdatePreferencesAsync(
            userId, new UpdatePreferencesCommand([new string('x', 41)], null)));
        await Assert.ThrowsAsync<ValidationException>(() => service.UpdatePreferencesAsync(
            userId, new UpdatePreferencesCommand(Enumerable.Range(0, 21).Select(i => $"tag{i}").ToArray(), null)));
        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.UpdatePreferencesAsync(
            Guid.NewGuid(), new UpdatePreferencesCommand(["ok"], null)));
    }

    [Fact]
    public async Task StartAsync_requires_a_target_valid_dates_and_only_one_active_tracking()
    {
        await using var dbContext = CreateDbContext();
        var service = new UserDietTrackingService(new UserDietTrackingRepository(dbContext));
        var userId = Guid.NewGuid();
        var start = new DateOnly(2026, 11, 1);

        await Assert.ThrowsAsync<ValidationException>(() =>
            service.StartAsync(new StartUserDietTrackingCommand(userId, null, null, start, null)));
        await Assert.ThrowsAsync<ValidationException>(() =>
            service.StartAsync(new StartUserDietTrackingCommand(userId, Guid.NewGuid(), null, start, start.AddDays(-1))));

        var first = await service.StartAsync(
            new StartUserDietTrackingCommand(userId, Guid.NewGuid(), null, start, start.AddDays(13)));
        Assert.Equal("Active", first.Status);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.StartAsync(new StartUserDietTrackingCommand(userId, null, Guid.NewGuid(), start, null)));

        // Another user can start their own.
        await service.StartAsync(
            new StartUserDietTrackingCommand(Guid.NewGuid(), null, Guid.NewGuid(), start, null));
    }

    [Fact]
    public async Task FinishAsync_changes_status_once_and_hides_other_users_trackings()
    {
        await using var dbContext = CreateDbContext();
        var service = new UserDietTrackingService(new UserDietTrackingRepository(dbContext));
        var userId = Guid.NewGuid();
        var tracking = await service.StartAsync(new StartUserDietTrackingCommand(
            userId, Guid.NewGuid(), null, new DateOnly(2026, 11, 1), null));

        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.GetAsync(Guid.NewGuid(), tracking.Id));
        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.FinishAsync(Guid.NewGuid(), tracking.Id, "Completed"));
        await Assert.ThrowsAsync<ValidationException>(() => service.FinishAsync(userId, tracking.Id, "Active"));

        var finished = await service.FinishAsync(userId, tracking.Id, "completed");
        Assert.Equal("Completed", finished.Status);
        Assert.NotNull(finished.UpdatedAtUtc);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.FinishAsync(userId, tracking.Id, "Cancelled"));

        // A finished tracking no longer blocks starting a new one.
        await service.StartAsync(new StartUserDietTrackingCommand(
            userId, null, Guid.NewGuid(), new DateOnly(2026, 12, 1), null));

        var completed = await service.ListAsync(userId, new UserDietTrackingQuery(1, 10, "Completed"));
        Assert.Equal(tracking.Id, Assert.Single(completed.Items).Id);
    }

    [Fact]
    public async Task Daily_tracking_and_progress_link_to_the_active_diet_tracking_covering_the_date()
    {
        await using var dbContext = CreateDbContext();
        var repository = new TrackingRepository(dbContext);
        var trackingService = new TrackingService(
            repository,
            new RecordingNotificationClient(),
            NullLogger<TrackingService>.Instance);
        var dietService = new UserDietTrackingService(new UserDietTrackingRepository(dbContext));
        var userId = Guid.NewGuid();
        var inside = new DateOnly(2026, 11, 5);
        var outside = new DateOnly(2026, 12, 25);

        var diet = await dietService.StartAsync(new StartUserDietTrackingCommand(
            userId, Guid.NewGuid(), null, new DateOnly(2026, 11, 1), new DateOnly(2026, 11, 14)));

        await trackingService.UpsertDailyTrackingAsync(new UpsertDailyTrackingCommand(userId, inside, "in"));
        await trackingService.UpsertDailyTrackingAsync(new UpsertDailyTrackingCommand(userId, outside, "out"));
        await trackingService.GetProgressSummaryAsync(userId, inside, inside);

        var days = await dbContext.DailyTrackings.AsNoTracking().OrderBy(day => day.TrackingDate).ToArrayAsync();
        Assert.Equal(diet.Id, days[0].UserDietTrackingId);
        Assert.Null(days[1].UserDietTrackingId);
        Assert.Equal(diet.Id, (await dbContext.ProgressSummaries.SingleAsync()).UserDietTrackingId);
    }

    private static TrackingDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<TrackingDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new TrackingDbContext(options);
    }
}
