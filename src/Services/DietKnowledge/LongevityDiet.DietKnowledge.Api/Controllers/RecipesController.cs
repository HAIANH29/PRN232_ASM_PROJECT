using LongevityDiet.ApiDefaults.Api;
using LongevityDiet.DietKnowledge.Api.Contracts;
using LongevityDiet.DietKnowledge.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LongevityDiet.DietKnowledge.Api.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/recipes")]
public sealed class RecipesController(IDietKnowledgeService dietKnowledgeService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResponse<RecipeResponse>>>> List(
        [FromQuery] RecipeListRequest request,
        CancellationToken cancellationToken)
    {
        var result = await dietKnowledgeService.ListRecipesAsync(
            request.ToQuery(forceActiveOnly: true),
            cancellationToken);

        var response = DietKnowledgeContractMapper.ToPagedResponse(result, item => item.ToResponse());

        return Ok(ApiResponse<PagedResponse<RecipeResponse>>.Success(
            response,
            "Recipes loaded.",
            HttpContext.TraceIdentifier));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<RecipeResponse>>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await dietKnowledgeService.GetRecipeAsync(id, cancellationToken: cancellationToken);

        return Ok(ApiResponse<RecipeResponse>.Success(
            result.ToResponse(),
            "Recipe loaded.",
            HttpContext.TraceIdentifier));
    }
}

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/admin/recipes")]
public sealed class AdminRecipesController(IDietKnowledgeService dietKnowledgeService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResponse<RecipeResponse>>>> List(
        [FromQuery] RecipeListRequest request,
        CancellationToken cancellationToken)
    {
        var result = await dietKnowledgeService.ListRecipesAsync(
            request.ToQuery(forceActiveOnly: false),
            cancellationToken);

        var response = DietKnowledgeContractMapper.ToPagedResponse(result, item => item.ToResponse());

        return Ok(ApiResponse<PagedResponse<RecipeResponse>>.Success(
            response,
            "Recipes loaded.",
            HttpContext.TraceIdentifier));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<RecipeResponse>>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await dietKnowledgeService.GetRecipeAsync(
            id,
            includeInactive: true,
            cancellationToken);

        return Ok(ApiResponse<RecipeResponse>.Success(
            result.ToResponse(),
            "Recipe loaded.",
            HttpContext.TraceIdentifier));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<RecipeResponse>>> Create(
        CreateRecipeRequest request,
        CancellationToken cancellationToken)
    {
        var result = await dietKnowledgeService.CreateRecipeAsync(request.ToCommand(), cancellationToken);

        return Created(
            $"/api/recipes/{result.Id}",
            ApiResponse<RecipeResponse>.Success(
                result.ToResponse(),
                "Recipe created.",
                HttpContext.TraceIdentifier));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<RecipeResponse>>> Update(
        Guid id,
        UpdateRecipeRequest request,
        CancellationToken cancellationToken)
    {
        var result = await dietKnowledgeService.UpdateRecipeAsync(id, request.ToCommand(), cancellationToken);

        return Ok(ApiResponse<RecipeResponse>.Success(
            result.ToResponse(),
            "Recipe updated.",
            HttpContext.TraceIdentifier));
    }

    [HttpPatch("{id:guid}/activation")]
    public async Task<ActionResult<ApiResponse<RecipeResponse>>> SetActivation(
        Guid id,
        SetContentActivationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await dietKnowledgeService.SetRecipeActivationAsync(
            id,
            request.IsActive,
            cancellationToken);

        return Ok(ApiResponse<RecipeResponse>.Success(
            result.ToResponse(),
            request.IsActive ? "Recipe activated." : "Recipe deactivated.",
            HttpContext.TraceIdentifier));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        await dietKnowledgeService.SetRecipeActivationAsync(id, isActive: false, cancellationToken);
        return NoContent();
    }
}
