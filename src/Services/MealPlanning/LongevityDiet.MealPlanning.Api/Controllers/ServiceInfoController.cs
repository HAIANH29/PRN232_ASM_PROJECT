using LongevityDiet.MealPlanning.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace LongevityDiet.MealPlanning.Api.Controllers;

[ApiController]
[Route("api/service-info")]
public sealed class ServiceInfoController(IMealPlanningService mealPlanningService) : ControllerBase
{
    [HttpGet]
    public ActionResult<MealPlanningServiceStatus> Get()
    {
        return Ok(mealPlanningService.GetStatus());
    }
}
