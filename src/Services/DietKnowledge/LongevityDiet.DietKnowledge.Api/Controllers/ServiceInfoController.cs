using LongevityDiet.DietKnowledge.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace LongevityDiet.DietKnowledge.Api.Controllers;

[ApiController]
[Route("api/service-info")]
public sealed class ServiceInfoController(IDietKnowledgeService dietKnowledgeService) : ControllerBase
{
    [HttpGet]
    public ActionResult<DietKnowledgeServiceStatus> Get()
    {
        return Ok(dietKnowledgeService.GetStatus());
    }
}
