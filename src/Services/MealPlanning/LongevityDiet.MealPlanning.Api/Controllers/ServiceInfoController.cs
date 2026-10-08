using LongevityDiet.ApiDefaults.Api;
using LongevityDiet.MealPlanning.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace LongevityDiet.MealPlanning.Api.Controllers;

[ApiController]
[Route("api/service-info")]
public sealed class ServiceInfoController(IMealPlanningService mealPlanningService) : ControllerBase
{
    [HttpGet]
    public ActionResult<ApiResponse<MealPlanningServiceStatus>> Get()
    {
        return Ok(ApiResponse<MealPlanningServiceStatus>.Success(
            mealPlanningService.GetStatus(),
            "Meal Planning Service is running.",
            HttpContext.TraceIdentifier));
    }
}
