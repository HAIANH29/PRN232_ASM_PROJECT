using LongevityDiet.ApiDefaults.Api;
using LongevityDiet.DietKnowledge.Api.Contracts;
using LongevityDiet.DietKnowledge.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LongevityDiet.DietKnowledge.Api.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/diet-regimens")]
public sealed class DietRegimensController(IDietRegimenService regimenService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResponse<DietRegimenResponse>>>> List(
        [FromQuery] DietRegimenListRequest request,
        CancellationToken cancellationToken)
    {
        var result = await regimenService.ListAsync(
            request.ToQuery(forceActiveOnly: true),
            cancellationToken);

        var response = DietKnowledgeContractMapper.ToPagedResponse(result, item => item.ToResponse());

        return Ok(ApiResponse<PagedResponse<DietRegimenResponse>>.Success(
            response,
            "Diet regimens loaded.",
            HttpContext.TraceIdentifier));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<DietRegimenResponse>>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await regimenService.GetAsync(id, cancellationToken: cancellationToken);

        return Ok(ApiResponse<DietRegimenResponse>.Success(
            result.ToResponse(),
            "Diet regimen loaded.",
            HttpContext.TraceIdentifier));
    }
}

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/admin/diet-regimens")]
public sealed class AdminDietRegimensController(IDietRegimenService regimenService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResponse<DietRegimenResponse>>>> List(
        [FromQuery] DietRegimenListRequest request,
        CancellationToken cancellationToken)
    {
        var result = await regimenService.ListAsync(
            request.ToQuery(forceActiveOnly: false),
            cancellationToken);

        var response = DietKnowledgeContractMapper.ToPagedResponse(result, item => item.ToResponse());

        return Ok(ApiResponse<PagedResponse<DietRegimenResponse>>.Success(
            response,
            "Diet regimens loaded.",
            HttpContext.TraceIdentifier));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<DietRegimenResponse>>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await regimenService.GetAsync(id, includeInactive: true, cancellationToken);

        return Ok(ApiResponse<DietRegimenResponse>.Success(
            result.ToResponse(),
            "Diet regimen loaded.",
            HttpContext.TraceIdentifier));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<DietRegimenResponse>>> Create(
        SaveDietRegimenRequest request,
        CancellationToken cancellationToken)
    {
        var result = await regimenService.CreateAsync(request.ToCommand(), cancellationToken);

        return Created(
            $"/api/diet-regimens/{result.Id}",
            ApiResponse<DietRegimenResponse>.Success(
                result.ToResponse(),
                "Diet regimen created.",
                HttpContext.TraceIdentifier));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<DietRegimenResponse>>> Update(
        Guid id,
        SaveDietRegimenRequest request,
        CancellationToken cancellationToken)
    {
        var result = await regimenService.UpdateAsync(id, request.ToCommand(), cancellationToken);

        return Ok(ApiResponse<DietRegimenResponse>.Success(
            result.ToResponse(),
            "Diet regimen updated.",
            HttpContext.TraceIdentifier));
    }

    [HttpPatch("{id:guid}/activation")]
    public async Task<ActionResult<ApiResponse<DietRegimenResponse>>> SetActivation(
        Guid id,
        SetContentActivationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await regimenService.SetActivationAsync(id, request.IsActive, cancellationToken);

        return Ok(ApiResponse<DietRegimenResponse>.Success(
            result.ToResponse(),
            request.IsActive ? "Diet regimen activated." : "Diet regimen deactivated.",
            HttpContext.TraceIdentifier));
    }
}
