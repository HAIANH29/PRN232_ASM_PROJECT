using LongevityDiet.ApiDefaults.Api;
using LongevityDiet.DietKnowledge.Api.Contracts;
using LongevityDiet.DietKnowledge.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LongevityDiet.DietKnowledge.Api.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/categories")]
public sealed class CategoriesController(IDietKnowledgeService dietKnowledgeService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<CategoryResponse>>>> List(
        [FromQuery] string? categoryType,
        CancellationToken cancellationToken)
    {
        var result = await dietKnowledgeService.ListCategoriesAsync(categoryType, cancellationToken);

        return Ok(ApiResponse<IReadOnlyCollection<CategoryResponse>>.Success(
            result.Select(category => category.ToResponse()).ToArray(),
            "Categories loaded.",
            HttpContext.TraceIdentifier));
    }
}
