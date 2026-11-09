using LongevityDiet.DietKnowledge.Application.Abstractions;
using LongevityDiet.DietKnowledge.Application.Common;
using LongevityDiet.DietKnowledge.Application.Models;
using LongevityDiet.DietKnowledge.Domain.Entities;
using LongevityDiet.DietKnowledge.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LongevityDiet.DietKnowledge.Infrastructure.Repositories;

public sealed class DietRegimenRepository(DietKnowledgeDbContext dbContext) : IDietRegimenRepository
{
    public async Task<PagedResult<DietRegimen>> ListAsync(
        DietRegimenQuery query,
        CancellationToken cancellationToken = default)
    {
        var source = WithDetails().AsNoTracking();

        if (!query.IncludeInactive)
        {
            source = source.Where(regimen => regimen.IsActive);
        }

        if (query.ApprovedOnly)
        {
            source = source.Where(regimen => regimen.ReviewStatus == KnowledgeReviewStatuses.Approved);
        }
        else if (!string.IsNullOrWhiteSpace(query.ReviewStatus))
        {
            source = source.Where(regimen => regimen.ReviewStatus == query.ReviewStatus);
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.ToLower();
            source = source.Where(regimen =>
                regimen.Name.ToLower().Contains(search) ||
                regimen.Description.ToLower().Contains(search));
        }

        source = source.OrderBy(regimen => regimen.Name);

        var total = await source.CountAsync(cancellationToken);
        var items = await source
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToArrayAsync(cancellationToken);

        return new PagedResult<DietRegimen>(items, query.PageNumber, query.PageSize, total);
    }

    public Task<DietRegimen?> GetByIdAsync(
        Guid id,
        bool includeInactive,
        bool approvedOnly,
        CancellationToken cancellationToken = default)
    {
        var query = WithDetails();

        if (!includeInactive)
        {
            query = query.Where(regimen => regimen.IsActive);
        }

        if (approvedOnly)
        {
            query = query.Where(regimen => regimen.ReviewStatus == KnowledgeReviewStatuses.Approved);
        }

        return query.FirstOrDefaultAsync(regimen => regimen.Id == id, cancellationToken);
    }

    public Task<bool> NameExistsAsync(
        string name,
        Guid? excludedId = null,
        CancellationToken cancellationToken = default)
    {
        var normalized = name.Trim().ToUpper();
        return dbContext.DietRegimens.AnyAsync(
            regimen => regimen.Name.ToUpper() == normalized &&
                (!excludedId.HasValue || regimen.Id != excludedId.Value),
            cancellationToken);
    }

    public Task<int> CountGuidelinesAsync(
        IReadOnlyCollection<Guid> guidelineIds,
        CancellationToken cancellationToken = default)
    {
        return dbContext.DietGuidelines.CountAsync(
            guideline => guideline.IsActive &&
                guideline.ReviewStatus == KnowledgeReviewStatuses.Approved &&
                guidelineIds.Contains(guideline.Id),
            cancellationToken);
    }

    public Task<int> CountRecipesAsync(
        IReadOnlyCollection<Guid> recipeIds,
        CancellationToken cancellationToken = default)
    {
        return dbContext.Recipes.CountAsync(
            recipe => recipe.IsActive &&
                recipe.ReviewStatus == KnowledgeReviewStatuses.Approved &&
                recipeIds.Contains(recipe.Id),
            cancellationToken);
    }

    public async Task AddAsync(DietRegimen regimen, CancellationToken cancellationToken = default)
    {
        await dbContext.DietRegimens.AddAsync(regimen, cancellationToken);
    }

    public void ReplaceChildren(
        DietRegimen regimen,
        IReadOnlyCollection<DietRegimenGuideline> guidelines,
        IReadOnlyCollection<DietRegimenMeal> meals)
    {
        dbContext.DietRegimenGuidelines.RemoveRange(regimen.Guidelines.ToArray());
        dbContext.DietRegimenMeals.RemoveRange(regimen.Meals.ToArray());

        foreach (var link in guidelines)
        {
            regimen.Guidelines.Add(link);
            dbContext.Entry(link).State = EntityState.Added;
        }

        foreach (var meal in meals)
        {
            regimen.Meals.Add(meal);
            dbContext.Entry(meal).State = EntityState.Added;
        }
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private IQueryable<DietRegimen> WithDetails()
    {
        return dbContext.DietRegimens
            .Include(regimen => regimen.Guidelines)
            .ThenInclude(link => link.Guideline)
            .Include(regimen => regimen.Meals)
            .ThenInclude(meal => meal.Recipe);
    }
}
