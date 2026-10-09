using LongevityDiet.ApiDefaults.Api;
using LongevityDiet.MealPlanning.Application.Models;

namespace LongevityDiet.MealPlanning.Api.Contracts;

internal static class MealPlanningContractMapper
{
    public static PagedResponse<TResponse> ToPagedResponse<TModel, TResponse>(
        PagedResult<TModel> result,
        Func<TModel, TResponse> mapper)
    {
        return new PagedResponse<TResponse>(
            result.Items.Select(mapper).ToArray(),
            result.PageNumber,
            result.PageSize,
            result.TotalCount);
    }

    public static MealPlanQuery ToQuery(this MealPlanListRequest request)
    {
        return new MealPlanQuery(
            request.PageNumber,
            request.PageSize,
            request.Search,
            request.FromDate,
            request.ToDate,
            request.SortBy,
            request.SortDirection);
    }

    public static RecommendationRequestQuery ToQuery(
        this RecommendationRequestListRequest request)
    {
        return new RecommendationRequestQuery(
            request.PageNumber,
            request.PageSize,
            request.Status);
    }

    public static CreateMealPlanCommand ToCommand(
        this CreateMealPlanRequest request,
        Guid userId,
        string userEmail)
    {
        return new CreateMealPlanCommand(
            userId,
            userEmail,
            request.Name,
            request.StartDate,
            request.EndDate,
            request.Items.Select(ToCommand).ToArray());
    }

    public static UpdateMealPlanCommand ToCommand(
        this UpdateMealPlanRequest request,
        Guid userId,
        string userEmail)
    {
        return new UpdateMealPlanCommand(
            userId,
            userEmail,
            request.Name,
            request.StartDate,
            request.EndDate);
    }

    public static AddMealPlanItemCommand ToAddCommand(
        this CreateMealPlanItemRequest request,
        Guid userId,
        string userEmail)
    {
        return new AddMealPlanItemCommand(
            userId,
            userEmail,
            request.PlannedDate,
            request.MealSlot,
            request.RecipeId,
            request.FoodId,
            request.Notes,
            request.ReminderAtUtc);
    }

    public static UpdateMealPlanItemCommand ToUpdateCommand(
        this UpdateMealPlanItemRequest request,
        Guid userId,
        string userEmail)
    {
        return new UpdateMealPlanItemCommand(
            userId,
            userEmail,
            request.PlannedDate,
            request.MealSlot,
            request.RecipeId,
            request.FoodId,
            request.Notes,
            request.ReminderAtUtc);
    }

    public static CreateRecommendationRequestCommand ToCommand(
        this CreateRecommendationRequest request,
        Guid userId)
    {
        return new CreateRecommendationRequestCommand(
            userId,
            request.PreferenceTags,
            request.Days);
    }

    public static AcceptRecommendationCommand ToCommand(
        this AcceptRecommendationRequest request,
        Guid userId,
        string userEmail)
    {
        return new AcceptRecommendationCommand(
            userId,
            userEmail,
            request.Name,
            request.StartDate,
            request.EndDate,
            request.Items.Select(ToCommand).ToArray());
    }

    public static MealPlanResponse ToResponse(this MealPlanModel model)
    {
        return new MealPlanResponse(
            model.Id,
            model.UserId,
            model.Name,
            model.StartDate,
            model.EndDate,
            model.Items.Select(ToResponse).ToArray(),
            model.CreatedAtUtc,
            model.UpdatedAtUtc);
    }

    public static MealPlanItemResponse ToResponse(this MealPlanItemModel model)
    {
        return new MealPlanItemResponse(
            model.Id,
            model.PlannedDate,
            model.MealSlot,
            model.RecipeId,
            model.FoodId,
            model.Notes,
            model.ReminderAtUtc,
            model.CreatedAtUtc,
            model.UpdatedAtUtc);
    }

    public static RecommendationRequestResponse ToResponse(
        this RecommendationRequestModel model)
    {
        return new RecommendationRequestResponse(
            model.Id,
            model.UserId,
            model.PreferenceTags,
            model.Days,
            model.Status,
            model.SuggestedMealTitles,
            model.Disclaimer,
            model.AcceptedMealPlanId,
            model.RequestedAtUtc,
            model.CompletedAtUtc,
            model.UpdatedAtUtc);
    }

    private static CreateMealPlanItemCommand ToCommand(CreateMealPlanItemRequest request)
    {
        return new CreateMealPlanItemCommand(
            request.PlannedDate,
            request.MealSlot,
            request.RecipeId,
            request.FoodId,
            request.Notes,
            request.ReminderAtUtc);
    }
}
