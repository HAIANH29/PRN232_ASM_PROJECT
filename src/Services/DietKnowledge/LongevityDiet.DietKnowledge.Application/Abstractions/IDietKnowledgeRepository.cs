using LongevityDiet.DietKnowledge.Domain.Entities;
using LongevityDiet.DietKnowledge.Application.Models;

namespace LongevityDiet.DietKnowledge.Application.Abstractions;

public interface IDietKnowledgeRepository
{
    Task<PagedResult<DietGuideline>> ListGuidelinesAsync(
        DietGuidelineQuery query,
        CancellationToken cancellationToken = default);

    Task<DietGuideline?> GetGuidelineByIdAsync(
        Guid id,
        bool includeInactive,
        bool approvedOnly,
        CancellationToken cancellationToken = default);

    Task<bool> GuidelineTitleExistsAsync(
        string title,
        Guid? excludedId = null,
        CancellationToken cancellationToken = default);

    Task AddGuidelineAsync(
        DietGuideline guideline,
        CancellationToken cancellationToken = default);

    Task<PagedResult<Food>> ListFoodsAsync(
        FoodQuery query,
        CancellationToken cancellationToken = default);

    Task<Food?> GetFoodByIdAsync(
        Guid id,
        bool includeInactive,
        bool approvedOnly,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Guid>> GetActiveFoodIdsAsync(
        IReadOnlyCollection<Guid> foodIds,
        CancellationToken cancellationToken = default);

    Task<bool> FoodNameExistsAsync(
        string name,
        Guid? excludedId = null,
        CancellationToken cancellationToken = default);

    Task AddFoodAsync(
        Food food,
        CancellationToken cancellationToken = default);

    Task<PagedResult<Recipe>> ListRecipesAsync(
        RecipeQuery query,
        CancellationToken cancellationToken = default);

    Task<Recipe?> GetRecipeByIdAsync(
        Guid id,
        bool includeInactive,
        bool approvedOnly,
        CancellationToken cancellationToken = default);

    Task<bool> RecipeNameExistsAsync(
        string name,
        Guid? excludedId = null,
        CancellationToken cancellationToken = default);

    Task AddRecipeAsync(
        Recipe recipe,
        CancellationToken cancellationToken = default);

    void ReplaceRecipeIngredients(
        Recipe recipe,
        IReadOnlyCollection<RecipeIngredient> ingredients);

    Task<PagedResult<BookSourceDocument>> ListBookSourceDocumentsAsync(
        BookSourceDocumentQuery query,
        CancellationToken cancellationToken = default);

    Task<BookSourceDocument?> GetBookSourceDocumentByIdAsync(
        Guid id,
        bool includeChunks = false,
        bool includeCandidates = false,
        CancellationToken cancellationToken = default);

    Task AddBookSourceDocumentAsync(
        BookSourceDocument document,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<BookSourceChunk>> ListBookSourceChunksAsync(
        Guid documentId,
        CancellationToken cancellationToken = default);

    Task AddBookSourceChunksAsync(
        IReadOnlyCollection<BookSourceChunk> chunks,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<KnowledgeCandidate>> ListKnowledgeCandidatesAsync(
        Guid documentId,
        CancellationToken cancellationToken = default);

    Task<KnowledgeCandidate?> GetKnowledgeCandidateByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task AddKnowledgeCandidatesAsync(
        IReadOnlyCollection<KnowledgeCandidate> candidates,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
