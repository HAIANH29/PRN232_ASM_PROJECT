using LongevityDiet.ApiDefaults.Api;
using LongevityDiet.Tracking.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace LongevityDiet.Tracking.Api.Controllers;

[ApiController]
[Route("api/service-info")]
public sealed class ServiceInfoController(ITrackingService trackingService) : ControllerBase
{
    [HttpGet]
    public ActionResult<ApiResponse<TrackingServiceStatus>> Get()
    {
        return Ok(ApiResponse<TrackingServiceStatus>.Success(
            trackingService.GetStatus(),
            "Tracking Service is running.",
            HttpContext.TraceIdentifier));
    }
}
