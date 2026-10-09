using LongevityDiet.MealPlanning.Application.Abstractions;
using LongevityDiet.MealPlanning.Application.Models;
using LongevityDiet.MealPlanning.Domain.Entities;
using LongevityDiet.MealPlanning.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LongevityDiet.MealPlanning.Infrastructure.Repositories;

public sealed class MealPlanRepository(MealPlanningDbContext dbContext) : IMealPlanRepository
{
    public async Task<PagedResult<MealPlan>> ListMealPlansAsync(
        Guid userId,
        MealPlanQuery query,
        CancellationToken cancellationToken = default)
    {
        var source = dbContext.MealPlans
            .AsNoTracking()
            .Include(plan => plan.Items)
            .Where(plan => plan.UserId == userId);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.ToLower();
            source = source.Where(plan => plan.Name.ToLower().Contains(search));
        }

        if (query.FromDate.HasValue)
        {
            source = source.Where(plan => plan.EndDate >= query.FromDate.Value);
        }

        if (query.ToDate.HasValue)
        {
            source = source.Where(plan => plan.StartDate <= query.ToDate.Value);
        }

        source = ApplyMealPlanSort(source, query.SortBy, query.SortDirection);

        return await ToPagedResultAsync(source, query.PageNumber, query.PageSize, cancellationToken);
    }

    public Task<MealPlan?> GetMealPlanAsync(
        Guid mealPlanId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return dbContext.MealPlans
            .Include(plan => plan.Items)
            .FirstOrDefaultAsync(
                plan => plan.Id == mealPlanId && plan.UserId == userId,
                cancellationToken);
    }

    public async Task AddMealPlanAsync(
        MealPlan mealPlan,
        CancellationToken cancellationToken = default)
    {
        await dbContext.MealPlans.AddAsync(mealPlan, cancellationToken);
    }

    public void RemoveMealPlan(MealPlan mealPlan)
    {
        dbContext.MealPlans.Remove(mealPlan);
    }

    public void RemoveMealPlanItem(MealPlanItem mealPlanItem)
    {
        dbContext.MealPlanItems.Remove(mealPlanItem);
    }

    public async Task<PagedResult<MealRecommendationRequest>> ListRecommendationRequestsAsync(
        Guid userId,
        RecommendationRequestQuery query,
        CancellationToken cancellationToken = default)
    {
        var source = dbContext.MealRecommendationRequests
            .AsNoTracking()
            .Where(request => request.UserId == userId);

        if (!string.IsNullOrWhiteSpace(query.Status))
        {
            source = source.Where(request => request.Status == query.Status);
        }

        source = source.OrderByDescending(request => request.RequestedAtUtc);

        return await ToPagedResultAsync(source, query.PageNumber, query.PageSize, cancellationToken);
    }

    public Task<MealRecommendationRequest?> GetRecommendationRequestAsync(
        Guid requestId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return dbContext.MealRecommendationRequests
            .FirstOrDefaultAsync(
                request => request.Id == requestId && request.UserId == userId,
                cancellationToken);
    }

    public Task<MealRecommendationRequest?> GetRecommendationRequestForUpdateAsync(
        Guid requestId,
        CancellationToken cancellationToken = default)
    {
        return dbContext.MealRecommendationRequests
            .FirstOrDefaultAsync(request => request.Id == requestId, cancellationToken);
    }

    public async Task AddRecommendationRequestAsync(
        MealRecommendationRequest request,
        CancellationToken cancellationToken = default)
    {
        await dbContext.MealRecommendationRequests.AddAsync(request, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static IQueryable<MealPlan> ApplyMealPlanSort(
        IQueryable<MealPlan> source,
        string? sortBy,
        string? sortDirection)
    {
        var descending = string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase);

        return sortBy?.ToLowerInvariant() switch
        {
            "startdate" => descending
                ? source.OrderByDescending(plan => plan.StartDate).ThenBy(plan => plan.Name)
                : source.OrderBy(plan => plan.StartDate).ThenBy(plan => plan.Name),
            "enddate" => descending
                ? source.OrderByDescending(plan => plan.EndDate).ThenBy(plan => plan.Name)
                : source.OrderBy(plan => plan.EndDate).ThenBy(plan => plan.Name),
            "createdat" => descending
                ? source.OrderByDescending(plan => plan.CreatedAtUtc)
                : source.OrderBy(plan => plan.CreatedAtUtc),
            _ => descending
                ? source.OrderByDescending(plan => plan.Name)
                : source.OrderBy(plan => plan.Name)
        };
    }

    private static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        IQueryable<T> source,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var totalCount = await source.CountAsync(cancellationToken);
        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync(cancellationToken);

        return new PagedResult<T>(items, pageNumber, pageSize, totalCount);
    }
}
