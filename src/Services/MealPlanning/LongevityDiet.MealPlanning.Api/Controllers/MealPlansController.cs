using LongevityDiet.ApiDefaults.Api;
using LongevityDiet.MealPlanning.Api.Contracts;
using LongevityDiet.MealPlanning.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LongevityDiet.MealPlanning.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/meal-plans")]
public sealed class MealPlansController(IMealPlanningService mealPlanningService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResponse<MealPlanResponse>>>> List(
        [FromQuery] MealPlanListRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mealPlanningService.ListMealPlansAsync(
            CurrentUser.GetUserId(User),
            request.ToQuery(),
            cancellationToken);

        var response = MealPlanningContractMapper.ToPagedResponse(
            result,
            item => item.ToResponse());

        return Ok(ApiResponse<PagedResponse<MealPlanResponse>>.Success(
            response,
            "Meal plans loaded.",
            HttpContext.TraceIdentifier));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<MealPlanResponse>>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await mealPlanningService.GetMealPlanAsync(
            id,
            CurrentUser.GetUserId(User),
            cancellationToken);

        return Ok(ApiResponse<MealPlanResponse>.Success(
            result.ToResponse(),
            "Meal plan loaded.",
            HttpContext.TraceIdentifier));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<MealPlanResponse>>> Create(
        CreateMealPlanRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mealPlanningService.CreateMealPlanAsync(
            request.ToCommand(CurrentUser.GetUserId(User), CurrentUser.GetEmail(User)),
            cancellationToken);

        return Created(
            $"/api/meal-plans/{result.Id}",
            ApiResponse<MealPlanResponse>.Success(
                result.ToResponse(),
                "Meal plan created.",
                HttpContext.TraceIdentifier));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<MealPlanResponse>>> Update(
        Guid id,
        UpdateMealPlanRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mealPlanningService.UpdateMealPlanAsync(
            id,
            request.ToCommand(CurrentUser.GetUserId(User), CurrentUser.GetEmail(User)),
            cancellationToken);

        return Ok(ApiResponse<MealPlanResponse>.Success(
            result.ToResponse(),
            "Meal plan updated.",
            HttpContext.TraceIdentifier));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await mealPlanningService.DeleteMealPlanAsync(
            id,
            CurrentUser.GetUserId(User),
            cancellationToken);

        return NoContent();
    }

    [HttpPost("{mealPlanId:guid}/items")]
    public async Task<ActionResult<ApiResponse<MealPlanItemResponse>>> AddItem(
        Guid mealPlanId,
        CreateMealPlanItemRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mealPlanningService.AddMealPlanItemAsync(
            mealPlanId,
            request.ToAddCommand(CurrentUser.GetUserId(User), CurrentUser.GetEmail(User)),
            cancellationToken);

        return Created(
            $"/api/meal-plans/{mealPlanId}/items/{result.Id}",
            ApiResponse<MealPlanItemResponse>.Success(
                result.ToResponse(),
                "Meal plan item created.",
                HttpContext.TraceIdentifier));
    }

    [HttpPut("{mealPlanId:guid}/items/{itemId:guid}")]
    public async Task<ActionResult<ApiResponse<MealPlanItemResponse>>> UpdateItem(
        Guid mealPlanId,
        Guid itemId,
        UpdateMealPlanItemRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mealPlanningService.UpdateMealPlanItemAsync(
            mealPlanId,
            itemId,
            request.ToUpdateCommand(CurrentUser.GetUserId(User), CurrentUser.GetEmail(User)),
            cancellationToken);

        return Ok(ApiResponse<MealPlanItemResponse>.Success(
            result.ToResponse(),
            "Meal plan item updated.",
            HttpContext.TraceIdentifier));
    }

    [HttpDelete("{mealPlanId:guid}/items/{itemId:guid}")]
    public async Task<IActionResult> DeleteItem(
        Guid mealPlanId,
        Guid itemId,
        CancellationToken cancellationToken)
    {
        await mealPlanningService.DeleteMealPlanItemAsync(
            mealPlanId,
            itemId,
            CurrentUser.GetUserId(User),
            cancellationToken);

        return NoContent();
    }
}
