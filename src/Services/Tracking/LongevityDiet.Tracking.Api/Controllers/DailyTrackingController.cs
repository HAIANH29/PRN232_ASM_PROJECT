using LongevityDiet.ApiDefaults.Api;
using LongevityDiet.Tracking.Api.Contracts;
using LongevityDiet.Tracking.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LongevityDiet.Tracking.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/daily-trackings")]
public sealed class DailyTrackingController(ITrackingService trackingService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResponse<DailyTrackingResponse>>>> List(
        [FromQuery] DailyTrackingListRequest request,
        CancellationToken cancellationToken)
    {
        var result = await trackingService.ListDailyTrackingAsync(
            CurrentUser.GetUserId(User),
            request.ToQuery(),
            cancellationToken);

        var response = TrackingContractMapper.ToPagedResponse(
            result,
            item => item.ToResponse());

        return Ok(ApiResponse<PagedResponse<DailyTrackingResponse>>.Success(
            response,
            "Daily tracking records loaded.",
            HttpContext.TraceIdentifier));
    }

    [HttpGet("{trackingDate}")]
    public async Task<ActionResult<ApiResponse<DailyTrackingResponse>>> GetByDate(
        DateOnly trackingDate,
        CancellationToken cancellationToken)
    {
        var result = await trackingService.GetDailyTrackingAsync(
            CurrentUser.GetUserId(User),
            trackingDate,
            cancellationToken);

        return Ok(ApiResponse<DailyTrackingResponse>.Success(
            result.ToResponse(),
            "Daily tracking loaded.",
            HttpContext.TraceIdentifier));
    }

    [HttpPut("{trackingDate}")]
    public async Task<ActionResult<ApiResponse<DailyTrackingResponse>>> Upsert(
        DateOnly trackingDate,
        UpsertDailyTrackingRequest request,
        CancellationToken cancellationToken)
    {
        var result = await trackingService.UpsertDailyTrackingAsync(
            request.ToCommand(CurrentUser.GetUserId(User), trackingDate),
            cancellationToken);

        return Ok(ApiResponse<DailyTrackingResponse>.Success(
            result.ToResponse(),
            "Daily tracking saved.",
            HttpContext.TraceIdentifier));
    }

    [HttpDelete("{trackingDate}")]
    public async Task<IActionResult> Delete(
        DateOnly trackingDate,
        CancellationToken cancellationToken)
    {
        await trackingService.DeleteDailyTrackingAsync(
            CurrentUser.GetUserId(User),
            trackingDate,
            cancellationToken);

        return NoContent();
    }

    [HttpPut("{trackingDate}/meals/{mealPlanItemId:guid}")]
    public async Task<ActionResult<ApiResponse<DailyTrackingResponse>>> SetMealCompletion(
        DateOnly trackingDate,
        Guid mealPlanItemId,
        SetMealCompletionRequest request,
        CancellationToken cancellationToken)
    {
        var result = await trackingService.SetMealCompletionAsync(
            request.ToCommand(
                CurrentUser.GetUserId(User),
                CurrentUser.GetEmail(User),
                trackingDate,
                mealPlanItemId),
            cancellationToken);

        return Ok(ApiResponse<DailyTrackingResponse>.Success(
            result.ToResponse(),
            request.IsCompleted ? "Meal marked completed." : "Meal marked not completed.",
            HttpContext.TraceIdentifier));
    }
}
