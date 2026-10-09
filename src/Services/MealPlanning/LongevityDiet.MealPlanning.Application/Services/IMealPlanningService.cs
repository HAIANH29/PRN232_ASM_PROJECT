using LongevityDiet.MealPlanning.Application.Models;

namespace LongevityDiet.MealPlanning.Application.Services;

public interface IMealPlanningService
{
    MealPlanningServiceStatus GetStatus();

    Task<PagedResult<MealPlanModel>> ListMealPlansAsync(
        Guid userId,
        MealPlanQuery query,
        CancellationToken cancellationToken = default);

    Task<MealPlanModel> GetMealPlanAsync(
        Guid mealPlanId,
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<MealPlanModel> CreateMealPlanAsync(
        CreateMealPlanCommand command,
        CancellationToken cancellationToken = default);

    Task<MealPlanModel> UpdateMealPlanAsync(
        Guid mealPlanId,
        UpdateMealPlanCommand command,
        CancellationToken cancellationToken = default);

    Task DeleteMealPlanAsync(
        Guid mealPlanId,
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<MealPlanItemModel> AddMealPlanItemAsync(
        Guid mealPlanId,
        AddMealPlanItemCommand command,
        CancellationToken cancellationToken = default);

    Task<MealPlanItemModel> UpdateMealPlanItemAsync(
        Guid mealPlanId,
        Guid itemId,
        UpdateMealPlanItemCommand command,
        CancellationToken cancellationToken = default);

    Task DeleteMealPlanItemAsync(
        Guid mealPlanId,
        Guid itemId,
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<RecommendationRequestModel> RequestRecommendationAsync(
        CreateRecommendationRequestCommand command,
        CancellationToken cancellationToken = default);

    Task<PagedResult<RecommendationRequestModel>> ListRecommendationRequestsAsync(
        Guid userId,
        RecommendationRequestQuery query,
        CancellationToken cancellationToken = default);

    Task<RecommendationRequestModel> GetRecommendationRequestAsync(
        Guid requestId,
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<MealPlanModel> AcceptRecommendationAsync(
        Guid requestId,
        AcceptRecommendationCommand command,
        CancellationToken cancellationToken = default);
}
