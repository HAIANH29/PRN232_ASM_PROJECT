using LongevityDiet.Tracking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LongevityDiet.Tracking.Infrastructure.Persistence;

public sealed class TrackingDbContext(DbContextOptions<TrackingDbContext> options) : DbContext(options)
{
    public DbSet<DailyTracking> DailyTrackings => Set<DailyTracking>();

    public DbSet<MealTracking> MealTrackings => Set<MealTracking>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DailyTracking>(entity =>
        {
            entity.HasKey(tracking => tracking.Id);
            entity.Property(tracking => tracking.Notes).HasMaxLength(1000);
            entity.HasIndex(tracking => new { tracking.UserId, tracking.TrackingDate }).IsUnique();
        });

        modelBuilder.Entity<MealTracking>(entity =>
        {
            entity.HasKey(tracking => tracking.Id);
            entity.HasOne(tracking => tracking.DailyTracking)
                .WithMany(daily => daily.Meals)
                .HasForeignKey(tracking => tracking.DailyTrackingId);
        });
    }
}
