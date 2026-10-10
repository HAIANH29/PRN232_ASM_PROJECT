using LongevityDiet.DietKnowledge.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LongevityDiet.DietKnowledge.Infrastructure.Persistence;

public sealed class DietKnowledgeDbContext(DbContextOptions<DietKnowledgeDbContext> options) : DbContext(options)
{
    public DbSet<DietGuideline> DietGuidelines => Set<DietGuideline>();

    public DbSet<Food> Foods => Set<Food>();

    public DbSet<Recipe> Recipes => Set<Recipe>();

    public DbSet<RecipeIngredient> RecipeIngredients => Set<RecipeIngredient>();

    public DbSet<BookSourceDocument> BookSourceDocuments => Set<BookSourceDocument>();

    public DbSet<BookSourceChunk> BookSourceChunks => Set<BookSourceChunk>();

    public DbSet<KnowledgeCandidate> KnowledgeCandidates => Set<KnowledgeCandidate>();

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
