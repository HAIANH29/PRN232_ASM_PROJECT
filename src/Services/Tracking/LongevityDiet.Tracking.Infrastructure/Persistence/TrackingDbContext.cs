using LongevityDiet.Tracking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LongevityDiet.Tracking.Infrastructure.Persistence;

public sealed class TrackingDbContext(DbContextOptions<TrackingDbContext> options) : DbContext(options)
{
    public DbSet<DailyTracking> DailyTrackings => Set<DailyTracking>();

    public DbSet<MealTracking> MealTrackings => Set<MealTracking>();

    public DbSet<ProgressSummary> ProgressSummaries => Set<ProgressSummary>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DailyTracking>(entity =>
        {
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
            entity.HasKey(summary => summary.Id);
            entity.Property(summary => summary.CalculatedAtUtc).IsRequired();
            entity.HasIndex(summary => new { summary.UserId, summary.PeriodStartDate, summary.PeriodEndDate })
                .IsUnique();
        });
    }
}
