using LongevityDiet.ApiDefaults.Api;
using LongevityDiet.DietKnowledge.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace LongevityDiet.DietKnowledge.Api.Controllers;

[ApiController]
[Route("api/service-info")]
public sealed class ServiceInfoController(IDietKnowledgeService dietKnowledgeService) : ControllerBase
{
    [HttpGet]
    public ActionResult<ApiResponse<DietKnowledgeServiceStatus>> Get()
    {
        return Ok(ApiResponse<DietKnowledgeServiceStatus>.Success(
            dietKnowledgeService.GetStatus(),
            "Diet Knowledge Service is running.",
            HttpContext.TraceIdentifier));
    }
}
