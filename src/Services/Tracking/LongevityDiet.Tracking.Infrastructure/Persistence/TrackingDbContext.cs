using LongevityDiet.Tracking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LongevityDiet.Tracking.Infrastructure.Persistence;

public sealed class TrackingDbContext(DbContextOptions<TrackingDbContext> options) : DbContext(options)
{
    public DbSet<UserDietTracking> UserDietTrackings => Set<UserDietTracking>();

    public DbSet<DailyTracking> DailyTrackings => Set<DailyTracking>();

    public DbSet<MealTracking> MealTrackings => Set<MealTracking>();

    public DbSet<ProgressSummary> ProgressSummaries => Set<ProgressSummary>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserDietTracking>(entity =>
        {
            entity.HasKey(tracking => tracking.Id);
            entity.Property(tracking => tracking.Status).HasMaxLength(40).IsRequired();
            entity.Property(tracking => tracking.CreatedAtUtc).IsRequired();
            entity.HasIndex(tracking => new { tracking.UserId, tracking.Status });
            entity.HasIndex(tracking => tracking.DietRegimenId);
            entity.HasIndex(tracking => tracking.MealPlanId);
        });

        modelBuilder.Entity<DailyTracking>(entity =>
        {
            entity.HasOne(tracking => tracking.UserDietTracking)
                .WithMany(userDiet => userDiet.DailyTrackings)
                .HasForeignKey(tracking => tracking.UserDietTrackingId)
                .OnDelete(DeleteBehavior.SetNull);
            entity.HasKey(tracking => tracking.Id);
            entity.Property(tracking => tracking.Notes).HasMaxLength(1000);
            entity.Property(tracking => tracking.CreatedAtUtc).IsRequired();
            entity.HasIndex(tracking => new { tracking.UserId, tracking.TrackingDate }).IsUnique();
        });

        modelBuilder.Entity<MealTracking>(entity =>
        {
            entity.HasKey(tracking => tracking.Id);
            entity.Property(tracking => tracking.CreatedAtUtc).IsRequired();
            entity.HasOne(tracking => tracking.DailyTracking)
                .WithMany(daily => daily.Meals)
                .HasForeignKey(tracking => tracking.DailyTrackingId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(tracking => tracking.MealPlanItemId);
            entity.HasIndex(tracking => new { tracking.DailyTrackingId, tracking.MealPlanItemId })
                .IsUnique();
        });

        modelBuilder.Entity<ProgressSummary>(entity =>
        {
            entity.HasOne(summary => summary.UserDietTracking)
                .WithMany(userDiet => userDiet.ProgressSummaries)
                .HasForeignKey(summary => summary.UserDietTrackingId)
                .OnDelete(DeleteBehavior.SetNull);
            entity.HasKey(summary => summary.Id);
            entity.Property(summary => summary.CalculatedAtUtc).IsRequired();
            entity.HasIndex(summary => new { summary.UserId, summary.PeriodStartDate, summary.PeriodEndDate })
                .IsUnique();
        });
    }
}
