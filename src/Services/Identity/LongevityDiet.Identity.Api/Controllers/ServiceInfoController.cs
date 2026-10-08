using LongevityDiet.Identity.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace LongevityDiet.Identity.Api.Controllers;

[ApiController]
[Route("api/service-info")]
public sealed class ServiceInfoController(IIdentityService identityService) : ControllerBase
{
    [HttpGet]
    public ActionResult<IdentityServiceStatus> Get()
    {
        return Ok(identityService.GetStatus());
    }
}
