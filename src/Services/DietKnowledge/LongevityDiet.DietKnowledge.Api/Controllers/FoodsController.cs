using LongevityDiet.ApiDefaults.Api;
using LongevityDiet.DietKnowledge.Api.Contracts;
using LongevityDiet.DietKnowledge.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LongevityDiet.DietKnowledge.Api.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/foods")]
public sealed class FoodsController(IDietKnowledgeService dietKnowledgeService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResponse<FoodResponse>>>> List(
        [FromQuery] FoodListRequest request,
        CancellationToken cancellationToken)
    {
        var result = await dietKnowledgeService.ListFoodsAsync(
            request.ToQuery(forceActiveOnly: true),
            cancellationToken);

        var response = DietKnowledgeContractMapper.ToPagedResponse(result, item => item.ToResponse());

        return Ok(ApiResponse<PagedResponse<FoodResponse>>.Success(
            response,
            "Foods loaded.",
            HttpContext.TraceIdentifier));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<FoodResponse>>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await dietKnowledgeService.GetFoodAsync(id, cancellationToken: cancellationToken);

        return Ok(ApiResponse<FoodResponse>.Success(
            result.ToResponse(),
            "Food loaded.",
            HttpContext.TraceIdentifier));
    }
}

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/admin/foods")]
public sealed class AdminFoodsController(IDietKnowledgeService dietKnowledgeService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResponse<FoodResponse>>>> List(
        [FromQuery] FoodListRequest request,
        CancellationToken cancellationToken)
    {
        var result = await dietKnowledgeService.ListFoodsAsync(
            request.ToQuery(forceActiveOnly: false),
            cancellationToken);

        var response = DietKnowledgeContractMapper.ToPagedResponse(result, item => item.ToResponse());

        return Ok(ApiResponse<PagedResponse<FoodResponse>>.Success(
            response,
            "Foods loaded.",
            HttpContext.TraceIdentifier));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<FoodResponse>>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await dietKnowledgeService.GetFoodAsync(
            id,
            includeInactive: true,
            cancellationToken);

        return Ok(ApiResponse<FoodResponse>.Success(
            result.ToResponse(),
            "Food loaded.",
            HttpContext.TraceIdentifier));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<FoodResponse>>> Create(
        CreateFoodRequest request,
        CancellationToken cancellationToken)
    {
        var result = await dietKnowledgeService.CreateFoodAsync(request.ToCommand(), cancellationToken);

        return Created(
            $"/api/foods/{result.Id}",
            ApiResponse<FoodResponse>.Success(
                result.ToResponse(),
                "Food created.",
                HttpContext.TraceIdentifier));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<FoodResponse>>> Update(
        Guid id,
        UpdateFoodRequest request,
        CancellationToken cancellationToken)
    {
        var result = await dietKnowledgeService.UpdateFoodAsync(id, request.ToCommand(), cancellationToken);

        return Ok(ApiResponse<FoodResponse>.Success(
            result.ToResponse(),
            "Food updated.",
            HttpContext.TraceIdentifier));
    }

    [HttpPatch("{id:guid}/activation")]
    public async Task<ActionResult<ApiResponse<FoodResponse>>> SetActivation(
        Guid id,
        SetContentActivationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await dietKnowledgeService.SetFoodActivationAsync(
            id,
            request.IsActive,
            cancellationToken);

        return Ok(ApiResponse<FoodResponse>.Success(
            result.ToResponse(),
            request.IsActive ? "Food activated." : "Food deactivated.",
            HttpContext.TraceIdentifier));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        await dietKnowledgeService.SetFoodActivationAsync(id, isActive: false, cancellationToken);
        return NoContent();
    }
}
