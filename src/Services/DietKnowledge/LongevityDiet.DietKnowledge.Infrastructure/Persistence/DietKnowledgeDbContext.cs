using LongevityDiet.DietKnowledge.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LongevityDiet.DietKnowledge.Infrastructure.Persistence;

public sealed class DietKnowledgeDbContext(DbContextOptions<DietKnowledgeDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories => Set<Category>();

    public DbSet<DietGuideline> DietGuidelines => Set<DietGuideline>();

    public DbSet<Food> Foods => Set<Food>();

    public DbSet<Recipe> Recipes => Set<Recipe>();

    public DbSet<RecipeIngredient> RecipeIngredients => Set<RecipeIngredient>();

    public DbSet<NutritionFact> NutritionFacts => Set<NutritionFact>();

    public DbSet<RecipeStep> RecipeSteps => Set<RecipeStep>();

    public DbSet<DietRegimen> DietRegimens => Set<DietRegimen>();

    public DbSet<DietRegimenGuideline> DietRegimenGuidelines => Set<DietRegimenGuideline>();

    public DbSet<DietRegimenMeal> DietRegimenMeals => Set<DietRegimenMeal>();

    public DbSet<BookSourceDocument> BookSourceDocuments => Set<BookSourceDocument>();

    public DbSet<BookSourceChunk> BookSourceChunks => Set<BookSourceChunk>();

    public DbSet<KnowledgeCandidate> KnowledgeCandidates => Set<KnowledgeCandidate>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(category => category.Id);
            entity.Property(category => category.Name).HasMaxLength(120).IsRequired();
            entity.Property(category => category.CategoryType).HasMaxLength(40).IsRequired();
            entity.Property(category => category.Description).HasMaxLength(500).IsRequired();
            entity.Property(category => category.CreatedAtUtc).IsRequired();
            entity.HasIndex(category => new { category.CategoryType, category.Name }).IsUnique();
        });

        modelBuilder.Entity<DietGuideline>(entity =>
        {
            entity.HasOne(guideline => guideline.CategoryRef)
                .WithMany()
                .HasForeignKey(guideline => guideline.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
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
            entity.HasOne(food => food.CategoryRef)
                .WithMany()
                .HasForeignKey(food => food.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.Property(food => food.CompatibilityNotes).HasMaxLength(1000);
            entity.Property(food => food.SourceTitle).HasMaxLength(200).IsRequired();
            entity.Property(food => food.SourceChapter).HasMaxLength(200).IsRequired();
            entity.Property(food => food.SourcePage).HasMaxLength(80).IsRequired();
            entity.Property(food => food.SourceReference).HasMaxLength(500).IsRequired();
            entity.Property(food => food.ReviewStatus).HasMaxLength(40).IsRequired();
            entity.Property(food => food.ReviewedBy).HasMaxLength(120).IsRequired();
            entity.Property(food => food.CreatedAtUtc).IsRequired();
            entity.HasIndex(food => food.Name);
            entity.HasIndex(food => food.CategoryId);
            entity.HasIndex(food => food.ReviewStatus);
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasOne(recipe => recipe.CategoryRef)
                .WithMany()
                .HasForeignKey(recipe => recipe.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
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

        modelBuilder.Entity<NutritionFact>(entity =>
        {
            entity.HasKey(fact => fact.Id);
            entity.Property(fact => fact.ServingSize).HasMaxLength(120).IsRequired();
            entity.Property(fact => fact.EnergyKcal).HasPrecision(8, 2);
            entity.Property(fact => fact.ProteinG).HasPrecision(8, 2);
            entity.Property(fact => fact.CarbohydrateG).HasPrecision(8, 2);
            entity.Property(fact => fact.FatG).HasPrecision(8, 2);
            entity.Property(fact => fact.FiberG).HasPrecision(8, 2);
            entity.Property(fact => fact.SourceReference).HasMaxLength(500).IsRequired();
            entity.HasOne(fact => fact.Food)
                .WithMany(food => food.NutritionFacts)
                .HasForeignKey(fact => fact.FoodId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(fact => fact.FoodId);
        });

        modelBuilder.Entity<RecipeStep>(entity =>
        {
            entity.HasKey(step => step.Id);
            entity.Property(step => step.Instruction).HasMaxLength(2000).IsRequired();
            entity.HasOne(step => step.Recipe)
                .WithMany(recipe => recipe.Steps)
                .HasForeignKey(step => step.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(step => new { step.RecipeId, step.StepNumber }).IsUnique();
        });

        modelBuilder.Entity<DietRegimen>(entity =>
        {
            entity.HasKey(regimen => regimen.Id);
            entity.Property(regimen => regimen.Name).HasMaxLength(200).IsRequired();
            entity.Property(regimen => regimen.Description).HasMaxLength(2000).IsRequired();
            entity.Property(regimen => regimen.SourceTitle).HasMaxLength(200).IsRequired();
            entity.Property(regimen => regimen.SourceChapter).HasMaxLength(200).IsRequired();
            entity.Property(regimen => regimen.SourcePage).HasMaxLength(80).IsRequired();
            entity.Property(regimen => regimen.ReviewStatus).HasMaxLength(40).IsRequired();
            entity.Property(regimen => regimen.CreatedAtUtc).IsRequired();
            entity.HasIndex(regimen => regimen.Name).IsUnique();
            entity.HasIndex(regimen => regimen.ReviewStatus);
        });

        modelBuilder.Entity<DietRegimenGuideline>(entity =>
        {
            entity.HasKey(link => link.Id);
            entity.Property(link => link.Note).HasMaxLength(500).IsRequired();
            entity.HasOne(link => link.DietRegimen)
                .WithMany(regimen => regimen.Guidelines)
                .HasForeignKey(link => link.DietRegimenId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(link => link.Guideline)
                .WithMany()
                .HasForeignKey(link => link.GuidelineId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(link => new { link.DietRegimenId, link.GuidelineId }).IsUnique();
        });

        modelBuilder.Entity<DietRegimenMeal>(entity =>
        {
            entity.HasKey(meal => meal.Id);
            entity.Property(meal => meal.MealSlot).HasMaxLength(40).IsRequired();
            entity.Property(meal => meal.Notes).HasMaxLength(500).IsRequired();
            entity.HasOne(meal => meal.DietRegimen)
                .WithMany(regimen => regimen.Meals)
                .HasForeignKey(meal => meal.DietRegimenId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(meal => meal.Recipe)
                .WithMany()
                .HasForeignKey(meal => meal.RecipeId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(meal => new { meal.DietRegimenId, meal.DayNumber, meal.MealSlot });
        });

        modelBuilder.Entity<BookSourceDocument>(entity =>
        {
            entity.HasKey(document => document.Id);
            entity.Property(document => document.OriginalFileName).HasMaxLength(260).IsRequired();
            entity.Property(document => document.StoredFileName).HasMaxLength(120).IsRequired();
            entity.Property(document => document.ContentType).HasMaxLength(120).IsRequired();
            entity.Property(document => document.StoragePath).HasMaxLength(500).IsRequired();
            entity.Property(document => document.Status).HasMaxLength(40).IsRequired();
            entity.Property(document => document.UploadedBy).HasMaxLength(120).IsRequired();
            entity.Property(document => document.ErrorMessage).HasMaxLength(1000).IsRequired();
            entity.Property(document => document.UploadedAtUtc).IsRequired();
            entity.HasIndex(document => document.Status);
            entity.HasIndex(document => document.UploadedAtUtc);
        });

        modelBuilder.Entity<BookSourceChunk>(entity =>
        {
            entity.HasKey(chunk => chunk.Id);
            entity.Property(chunk => chunk.Text).HasMaxLength(8000).IsRequired();
            entity.Property(chunk => chunk.CreatedAtUtc).IsRequired();
            entity.HasOne(chunk => chunk.Document)
                .WithMany(document => document.Chunks)
                .HasForeignKey(chunk => chunk.DocumentId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(chunk => new { chunk.DocumentId, chunk.ChunkIndex }).IsUnique();
            entity.HasIndex(chunk => chunk.PageNumber);
        });

        modelBuilder.Entity<KnowledgeCandidate>(entity =>
        {
            entity.HasKey(candidate => candidate.Id);
            entity.Property(candidate => candidate.CandidateType).HasMaxLength(40).IsRequired();
            entity.Property(candidate => candidate.Title).HasMaxLength(200).IsRequired();
            entity.Property(candidate => candidate.Summary).HasMaxLength(2000).IsRequired();
            entity.Property(candidate => candidate.SourceTitle).HasMaxLength(200).IsRequired();
            entity.Property(candidate => candidate.SourceChapter).HasMaxLength(200).IsRequired();
            entity.Property(candidate => candidate.SourcePage).HasMaxLength(80).IsRequired();
            entity.Property(candidate => candidate.SourceReference).HasMaxLength(500).IsRequired();
            entity.Property(candidate => candidate.Status).HasMaxLength(40).IsRequired();
            entity.Property(candidate => candidate.GeneratedBy).HasMaxLength(120).IsRequired();
            entity.Property(candidate => candidate.ReviewedBy).HasMaxLength(120).IsRequired();
            entity.Property(candidate => candidate.IngredientsJson).HasMaxLength(4000).IsRequired();
            entity.Property(candidate => candidate.CreatedAtUtc).IsRequired();
            entity.HasOne(candidate => candidate.Document)
                .WithMany(document => document.Candidates)
                .HasForeignKey(candidate => candidate.DocumentId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(candidate => candidate.Chunk)
                .WithMany(chunk => chunk.Candidates)
                .HasForeignKey(candidate => candidate.ChunkId)
                .OnDelete(DeleteBehavior.SetNull);
            entity.HasIndex(candidate => candidate.DocumentId);
            entity.HasIndex(candidate => candidate.Status);
            entity.HasIndex(candidate => candidate.CandidateType);
        });
    }
}
