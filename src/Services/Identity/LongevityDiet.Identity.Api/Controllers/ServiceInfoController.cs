using LongevityDiet.ApiDefaults.Api;
using LongevityDiet.Identity.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace LongevityDiet.Identity.Api.Controllers;

[ApiController]
[Route("api/service-info")]
public sealed class ServiceInfoController(IIdentityService identityService) : ControllerBase
{
    [HttpGet]
    public ActionResult<ApiResponse<IdentityServiceStatus>> Get()
    {
        return Ok(ApiResponse<IdentityServiceStatus>.Success(
            identityService.GetStatus(),
            "Identity Service is running.",
            HttpContext.TraceIdentifier));
    }
}
