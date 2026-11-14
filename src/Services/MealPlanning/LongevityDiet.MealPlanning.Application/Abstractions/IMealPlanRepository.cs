using LongevityDiet.MealPlanning.Domain.Entities;
using LongevityDiet.MealPlanning.Application.Models;

namespace LongevityDiet.MealPlanning.Application.Abstractions;

public interface IMealPlanRepository
{
    Task<PagedResult<MealPlan>> ListMealPlansAsync(
        Guid userId,
        MealPlanQuery query,
        CancellationToken cancellationToken = default);

    Task<MealPlan?> GetMealPlanAsync(
        Guid mealPlanId,
        Guid userId,
        CancellationToken cancellationToken = default);

    Task AddMealPlanAsync(MealPlan mealPlan, CancellationToken cancellationToken = default);

    Task AddMealPlanItemAsync(MealPlanItem mealPlanItem, CancellationToken cancellationToken = default);

    void RemoveMealPlan(MealPlan mealPlan);

    void RemoveMealPlanItem(MealPlanItem mealPlanItem);

    Task<PagedResult<MealRecommendationRequest>> ListRecommendationRequestsAsync(
        Guid userId,
        RecommendationRequestQuery query,
        CancellationToken cancellationToken = default);

    Task<MealRecommendationRequest?> GetRecommendationRequestAsync(
        Guid requestId,
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<MealRecommendationRequest?> GetRecommendationRequestForUpdateAsync(
        Guid requestId,
        CancellationToken cancellationToken = default);

    void RemoveRecommendationRequest(MealRecommendationRequest request);

    Task AddRecommendationRequestAsync(
        MealRecommendationRequest request,
        CancellationToken cancellationToken = default);

    // Reminder time of one item regardless of owner; null when the item no longer exists.
    Task<DateTimeOffset?> GetReminderAtUtcAsync(
        Guid mealPlanItemId,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
