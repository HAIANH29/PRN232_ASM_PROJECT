using LongevityDiet.ApiDefaults.Api;
using LongevityDiet.MealPlanning.Api.Contracts;
using LongevityDiet.MealPlanning.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LongevityDiet.MealPlanning.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/meal-recommendations")]
public sealed class MealRecommendationsController(
    IMealPlanningService mealPlanningService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResponse<RecommendationRequestResponse>>>> List(
        [FromQuery] RecommendationRequestListRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mealPlanningService.ListRecommendationRequestsAsync(
            CurrentUser.GetUserId(User),
            request.ToQuery(),
            cancellationToken);

        var response = MealPlanningContractMapper.ToPagedResponse(
            result,
            item => item.ToResponse());

        return Ok(ApiResponse<PagedResponse<RecommendationRequestResponse>>.Success(
            response,
            "Recommendation requests loaded.",
            HttpContext.TraceIdentifier));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<RecommendationRequestResponse>>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await mealPlanningService.GetRecommendationRequestAsync(
            id,
            CurrentUser.GetUserId(User),
            cancellationToken);

        return Ok(ApiResponse<RecommendationRequestResponse>.Success(
            result.ToResponse(),
            "Recommendation request loaded.",
            HttpContext.TraceIdentifier));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<RecommendationRequestResponse>>> Create(
        CreateRecommendationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mealPlanningService.RequestRecommendationAsync(
            request.ToCommand(CurrentUser.GetUserId(User)),
            cancellationToken);

        return Accepted(
            $"/api/meal-recommendations/{result.Id}",
            ApiResponse<RecommendationRequestResponse>.Success(
                result.ToResponse(),
                "Recommendation request published.",
                HttpContext.TraceIdentifier));
    }

    [HttpPost("{id:guid}/accept")]
    public async Task<ActionResult<ApiResponse<MealPlanResponse>>> Accept(
        Guid id,
        AcceptRecommendationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mealPlanningService.AcceptRecommendationAsync(
            id,
            request.ToCommand(CurrentUser.GetUserId(User), CurrentUser.GetEmail(User)),
            cancellationToken);

        return Created(
            $"/api/meal-plans/{result.Id}",
            ApiResponse<MealPlanResponse>.Success(
                result.ToResponse(),
                "Recommendation accepted and meal plan saved.",
                HttpContext.TraceIdentifier));
    }
}
