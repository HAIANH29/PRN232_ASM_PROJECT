using LongevityDiet.MealPlanning.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LongevityDiet.MealPlanning.Infrastructure.Persistence;

public sealed class MealPlanningDbContext(DbContextOptions<MealPlanningDbContext> options) : DbContext(options)
{
    public DbSet<MealPlan> MealPlans => Set<MealPlan>();

    public DbSet<MealPlanItem> MealPlanItems => Set<MealPlanItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MealPlan>(entity =>
        {
            entity.HasKey(plan => plan.Id);
            entity.Property(plan => plan.Name).HasMaxLength(200).IsRequired();
            entity.HasIndex(plan => plan.UserId);
        });

        modelBuilder.Entity<MealPlanItem>(entity =>
        {
            entity.HasKey(item => item.Id);
            entity.Property(item => item.MealSlot).HasMaxLength(40).IsRequired();
            entity.Property(item => item.Notes).HasMaxLength(1000);
            entity.HasOne(item => item.MealPlan)
                .WithMany(plan => plan.Items)
                .HasForeignKey(item => item.MealPlanId);
        });
    }
}
