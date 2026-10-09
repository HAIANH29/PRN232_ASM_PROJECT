using LongevityDiet.MealPlanning.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LongevityDiet.MealPlanning.Infrastructure.Persistence;

public sealed class MealPlanningDbContext(DbContextOptions<MealPlanningDbContext> options) : DbContext(options)
{
    public DbSet<MealPlan> MealPlans => Set<MealPlan>();

    public DbSet<MealPlanItem> MealPlanItems => Set<MealPlanItem>();

    public DbSet<MealRecommendationRequest> MealRecommendationRequests =>
        Set<MealRecommendationRequest>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MealPlan>(entity =>
        {
            entity.HasKey(plan => plan.Id);
            entity.Property(plan => plan.Name).HasMaxLength(200).IsRequired();
            entity.Property(plan => plan.CreatedAtUtc).IsRequired();
            entity.HasIndex(plan => plan.UserId);
            entity.HasIndex(plan => new { plan.UserId, plan.StartDate, plan.EndDate });
        });

        modelBuilder.Entity<MealPlanItem>(entity =>
        {
            entity.HasKey(item => item.Id);
            entity.Property(item => item.MealSlot).HasMaxLength(40).IsRequired();
            entity.Property(item => item.Notes).HasMaxLength(1000);
            entity.Property(item => item.ReminderAtUtc).IsRequired();
            entity.Property(item => item.CreatedAtUtc).IsRequired();
            entity.HasOne(item => item.MealPlan)
                .WithMany(plan => plan.Items)
                .HasForeignKey(item => item.MealPlanId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(item => item.MealPlanId);
            entity.HasIndex(item => item.RecipeId);
            entity.HasIndex(item => item.FoodId);
            entity.HasIndex(item => item.PlannedDate);
        });

        modelBuilder.Entity<MealRecommendationRequest>(entity =>
        {
            entity.HasKey(request => request.Id);
            entity.Property(request => request.PreferenceTagsJson).HasMaxLength(1000).IsRequired();
            entity.Property(request => request.Days).IsRequired();
            entity.Property(request => request.Status).HasMaxLength(40).IsRequired();
            entity.Property(request => request.SuggestedMealTitlesJson).HasMaxLength(4000).IsRequired();
            entity.Property(request => request.Disclaimer).HasMaxLength(1000);
            entity.Property(request => request.RequestedAtUtc).IsRequired();
            entity.HasIndex(request => request.UserId);
            entity.HasIndex(request => request.Status);
            entity.HasIndex(request => request.AcceptedMealPlanId);
        });
    }
}
