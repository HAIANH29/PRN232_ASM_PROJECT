using LongevityDiet.ApiDefaults.Api;
using LongevityDiet.DietKnowledge.Api.Contracts;
using LongevityDiet.DietKnowledge.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LongevityDiet.DietKnowledge.Api.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/admin/knowledge")]
public sealed class AdminKnowledgeController(IDietKnowledgeService dietKnowledgeService) : ControllerBase
{
    // Permanent: removes all guidelines, foods and recipes. Needs ?confirm=true so a stray call cannot wipe the data.
    [HttpDelete]
    public async Task<ActionResult<ApiResponse<DeleteAllKnowledgeResponse>>> DeleteAll(
        [FromQuery] bool confirm,
        CancellationToken cancellationToken)
    {
        if (!confirm)
        {
            return BadRequest(ApiResponse<object?>.Failure(
                [new ApiError("Validation.Invalid", "Add confirm=true to delete all knowledge.", "confirm")],
                "Validation failed.",
                HttpContext.TraceIdentifier));
        }

        var result = await dietKnowledgeService.DeleteAllKnowledgeAsync(cancellationToken);

        return Ok(ApiResponse<DeleteAllKnowledgeResponse>.Success(
            new DeleteAllKnowledgeResponse(
                result.GuidelinesDeleted,
                result.FoodsDeleted,
                result.RecipesDeleted,
                result.CandidatesReset),
            "All knowledge deleted.",
            HttpContext.TraceIdentifier));
    }
}
