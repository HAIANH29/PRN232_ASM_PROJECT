using LongevityDiet.ApiDefaults.Api;
using LongevityDiet.Tracking.Application.Models;

namespace LongevityDiet.Tracking.Api.Contracts;

internal static class TrackingContractMapper
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

    public static DailyTrackingQuery ToQuery(this DailyTrackingListRequest request)
    {
        return new DailyTrackingQuery(
            request.PageNumber,
            request.PageSize,
            request.FromDate,
            request.ToDate);
    }

    public static UpsertDailyTrackingCommand ToCommand(
        this UpsertDailyTrackingRequest request,
        Guid userId,
        DateOnly trackingDate)
    {
        return new UpsertDailyTrackingCommand(
            userId,
            trackingDate,
            request.Notes);
    }

    public static SetMealCompletionCommand ToCommand(
        this SetMealCompletionRequest request,
        Guid userId,
        string userEmail,
        DateOnly trackingDate,
        Guid mealPlanItemId)
    {
        return new SetMealCompletionCommand(
            userId,
            userEmail,
            trackingDate,
            mealPlanItemId,
            request.IsCompleted);
    }

    public static DailyTrackingResponse ToResponse(this DailyTrackingModel model)
    {
        return new DailyTrackingResponse(
            model.Id,
            model.UserId,
            model.TrackingDate,
            model.Notes,
            model.Meals.Select(ToResponse).ToArray(),
            model.CreatedAtUtc,
            model.UpdatedAtUtc);
    }

    public static MealTrackingResponse ToResponse(this MealTrackingModel model)
    {
        return new MealTrackingResponse(
            model.Id,
            model.DailyTrackingId,
            model.MealPlanItemId,
            model.IsCompleted,
            model.CompletedAtUtc,
            model.CreatedAtUtc,
            model.UpdatedAtUtc);
    }

    public static ProgressSummaryResponse ToResponse(this ProgressSummaryModel model)
    {
        return new ProgressSummaryResponse(
            model.Id,
            model.UserId,
            model.PeriodStartDate,
            model.PeriodEndDate,
            model.PlannedMeals,
            model.CompletedMeals,
            model.CompletionRate,
            model.CalculatedAtUtc,
            model.LastNotificationId,
            model.LastNotificationSentAtUtc);
    }
}
