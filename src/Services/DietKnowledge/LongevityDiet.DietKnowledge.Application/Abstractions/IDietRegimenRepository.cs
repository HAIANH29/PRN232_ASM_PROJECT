using LongevityDiet.DietKnowledge.Application.Models;
using LongevityDiet.DietKnowledge.Domain.Entities;

namespace LongevityDiet.DietKnowledge.Application.Abstractions;

public interface IDietRegimenRepository
{
    Task<PagedResult<DietRegimen>> ListAsync(
        DietRegimenQuery query,
        CancellationToken cancellationToken = default);

    Task<DietRegimen?> GetByIdAsync(
        Guid id,
        bool includeInactive,
        bool approvedOnly,
        CancellationToken cancellationToken = default);

    Task<bool> NameExistsAsync(
        string name,
        Guid? excludedId = null,
        CancellationToken cancellationToken = default);

    Task<int> CountGuidelinesAsync(
        IReadOnlyCollection<Guid> guidelineIds,
        CancellationToken cancellationToken = default);

    Task<int> CountRecipesAsync(
        IReadOnlyCollection<Guid> recipeIds,
        CancellationToken cancellationToken = default);

    Task AddAsync(DietRegimen regimen, CancellationToken cancellationToken = default);

    // Replaces the regimen's guideline links and meals with the requested ones.
    void ReplaceChildren(
        DietRegimen regimen,
        IReadOnlyCollection<DietRegimenGuideline> guidelines,
        IReadOnlyCollection<DietRegimenMeal> meals);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
