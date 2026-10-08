using LongevityDiet.Tracking.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace LongevityDiet.Tracking.Api.Controllers;

[ApiController]
[Route("api/service-info")]
public sealed class ServiceInfoController(ITrackingService trackingService) : ControllerBase
{
    [HttpGet]
    public ActionResult<TrackingServiceStatus> Get()
    {
        return Ok(trackingService.GetStatus());
    }
}
