using LongevityDiet.DietKnowledge.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LongevityDiet.DietKnowledge.Infrastructure.Persistence;

public sealed class DietKnowledgeDbContext(DbContextOptions<DietKnowledgeDbContext> options) : DbContext(options)
{
    public DbSet<DietGuideline> DietGuidelines => Set<DietGuideline>();

    public DbSet<Food> Foods => Set<Food>();

    public DbSet<Recipe> Recipes => Set<Recipe>();

    public DbSet<RecipeIngredient> RecipeIngredients => Set<RecipeIngredient>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DietGuideline>(entity =>
        {
            entity.HasKey(guideline => guideline.Id);
            entity.Property(guideline => guideline.Title).HasMaxLength(200).IsRequired();
            entity.Property(guideline => guideline.Summary).HasMaxLength(2000).IsRequired();
            entity.Property(guideline => guideline.SourceNote).HasMaxLength(500);
            entity.Property(guideline => guideline.SourceTitle).HasMaxLength(200).IsRequired();
            entity.Property(guideline => guideline.SourceChapter).HasMaxLength(200).IsRequired();
            entity.Property(guideline => guideline.SourcePage).HasMaxLength(80).IsRequired();
            entity.Property(guideline => guideline.SourceReference).HasMaxLength(500).IsRequired();
            entity.Property(guideline => guideline.ReviewStatus).HasMaxLength(40).IsRequired();
            entity.Property(guideline => guideline.ReviewedBy).HasMaxLength(120).IsRequired();
            entity.Property(guideline => guideline.CreatedAtUtc).IsRequired();
            entity.HasIndex(guideline => guideline.Title);
            entity.HasIndex(guideline => guideline.ReviewStatus);
        });

        modelBuilder.Entity<Food>(entity =>
        {
            entity.HasKey(food => food.Id);
            entity.Property(food => food.Name).HasMaxLength(160).IsRequired();
            entity.Property(food => food.Category).HasMaxLength(120).IsRequired();
            entity.Property(food => food.CompatibilityNotes).HasMaxLength(1000);
            entity.Property(food => food.SourceTitle).HasMaxLength(200).IsRequired();
            entity.Property(food => food.SourceChapter).HasMaxLength(200).IsRequired();
            entity.Property(food => food.SourcePage).HasMaxLength(80).IsRequired();
            entity.Property(food => food.SourceReference).HasMaxLength(500).IsRequired();
            entity.Property(food => food.ReviewStatus).HasMaxLength(40).IsRequired();
            entity.Property(food => food.ReviewedBy).HasMaxLength(120).IsRequired();
            entity.Property(food => food.CreatedAtUtc).IsRequired();
            entity.HasIndex(food => food.Name);
            entity.HasIndex(food => food.Category);
            entity.HasIndex(food => food.ReviewStatus);
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(recipe => recipe.Id);
            entity.Property(recipe => recipe.Name).HasMaxLength(200).IsRequired();
            entity.Property(recipe => recipe.Description).HasMaxLength(2000);
            entity.Property(recipe => recipe.SourceTitle).HasMaxLength(200).IsRequired();
            entity.Property(recipe => recipe.SourceChapter).HasMaxLength(200).IsRequired();
            entity.Property(recipe => recipe.SourcePage).HasMaxLength(80).IsRequired();
            entity.Property(recipe => recipe.SourceReference).HasMaxLength(500).IsRequired();
            entity.Property(recipe => recipe.ReviewStatus).HasMaxLength(40).IsRequired();
            entity.Property(recipe => recipe.ReviewedBy).HasMaxLength(120).IsRequired();
            entity.Property(recipe => recipe.CreatedAtUtc).IsRequired();
            entity.HasIndex(recipe => recipe.Name);
            entity.HasIndex(recipe => recipe.ReviewStatus);
        });

        modelBuilder.Entity<RecipeIngredient>(entity =>
        {
            entity.HasKey(ingredient => ingredient.Id);
            entity.Property(ingredient => ingredient.QuantityText).HasMaxLength(120);
            entity.HasOne(ingredient => ingredient.Recipe)
                .WithMany(recipe => recipe.Ingredients)
                .HasForeignKey(ingredient => ingredient.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(ingredient => ingredient.Food)
                .WithMany()
                .HasForeignKey(ingredient => ingredient.FoodId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(ingredient => new { ingredient.RecipeId, ingredient.FoodId }).IsUnique();
        });
    }
}
