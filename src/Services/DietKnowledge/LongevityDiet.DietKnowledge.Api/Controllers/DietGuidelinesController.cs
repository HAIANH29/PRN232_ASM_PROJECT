using LongevityDiet.ApiDefaults.Api;
using LongevityDiet.DietKnowledge.Api.Contracts;
using LongevityDiet.DietKnowledge.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LongevityDiet.DietKnowledge.Api.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/diet-guidelines")]
public sealed class DietGuidelinesController(IDietKnowledgeService dietKnowledgeService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResponse<DietGuidelineResponse>>>> List(
        [FromQuery] DietGuidelineListRequest request,
        CancellationToken cancellationToken)
    {
        var result = await dietKnowledgeService.ListGuidelinesAsync(
            request.ToQuery(forceActiveOnly: true),
            cancellationToken);

        var response = DietKnowledgeContractMapper.ToPagedResponse(result, item => item.ToResponse());

        return Ok(ApiResponse<PagedResponse<DietGuidelineResponse>>.Success(
            response,
            "Diet guidelines loaded.",
            HttpContext.TraceIdentifier));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<DietGuidelineResponse>>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await dietKnowledgeService.GetGuidelineAsync(id, cancellationToken: cancellationToken);

        return Ok(ApiResponse<DietGuidelineResponse>.Success(
            result.ToResponse(),
            "Diet guideline loaded.",
            HttpContext.TraceIdentifier));
    }
}

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/admin/diet-guidelines")]
public sealed class AdminDietGuidelinesController(IDietKnowledgeService dietKnowledgeService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResponse<DietGuidelineResponse>>>> List(
        [FromQuery] DietGuidelineListRequest request,
        CancellationToken cancellationToken)
    {
        var result = await dietKnowledgeService.ListGuidelinesAsync(
            request.ToQuery(forceActiveOnly: false),
            cancellationToken);

        var response = DietKnowledgeContractMapper.ToPagedResponse(result, item => item.ToResponse());

        return Ok(ApiResponse<PagedResponse<DietGuidelineResponse>>.Success(
            response,
            "Diet guidelines loaded.",
            HttpContext.TraceIdentifier));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<DietGuidelineResponse>>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await dietKnowledgeService.GetGuidelineAsync(
            id,
            includeInactive: true,
            cancellationToken);

        return Ok(ApiResponse<DietGuidelineResponse>.Success(
            result.ToResponse(),
            "Diet guideline loaded.",
            HttpContext.TraceIdentifier));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<DietGuidelineResponse>>> Create(
        CreateDietGuidelineRequest request,
        CancellationToken cancellationToken)
    {
        var result = await dietKnowledgeService.CreateGuidelineAsync(request.ToCommand(), cancellationToken);

        return Created(
            $"/api/diet-guidelines/{result.Id}",
            ApiResponse<DietGuidelineResponse>.Success(
                result.ToResponse(),
                "Diet guideline created.",
                HttpContext.TraceIdentifier));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<DietGuidelineResponse>>> Update(
        Guid id,
        UpdateDietGuidelineRequest request,
        CancellationToken cancellationToken)
    {
        var result = await dietKnowledgeService.UpdateGuidelineAsync(id, request.ToCommand(), cancellationToken);

        return Ok(ApiResponse<DietGuidelineResponse>.Success(
            result.ToResponse(),
            "Diet guideline updated.",
            HttpContext.TraceIdentifier));
    }

    [HttpPatch("{id:guid}/activation")]
    public async Task<ActionResult<ApiResponse<DietGuidelineResponse>>> SetActivation(
        Guid id,
        SetContentActivationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await dietKnowledgeService.SetGuidelineActivationAsync(
            id,
            request.IsActive,
            cancellationToken);

        return Ok(ApiResponse<DietGuidelineResponse>.Success(
            result.ToResponse(),
            request.IsActive ? "Diet guideline activated." : "Diet guideline deactivated.",
            HttpContext.TraceIdentifier));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        await dietKnowledgeService.SetGuidelineActivationAsync(id, isActive: false, cancellationToken);
        return NoContent();
    }
}
