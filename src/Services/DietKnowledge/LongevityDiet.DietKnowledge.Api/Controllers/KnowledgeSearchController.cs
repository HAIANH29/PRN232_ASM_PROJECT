using System.ComponentModel.DataAnnotations;
using LongevityDiet.ApiDefaults.Api;
using LongevityDiet.DietKnowledge.Application.Models;
using LongevityDiet.DietKnowledge.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LongevityDiet.DietKnowledge.Api.Controllers;

public sealed record KnowledgeSearchRequest
{
    [Required]
    [MaxLength(300)]
    public string Q { get; init; } = string.Empty;

    // Guideline, Food or Recipe; omitted searches all three.
    [MaxLength(40)]
    public string? Type { get; init; }

    [Range(1, 50)]
    public int Limit { get; init; } = 10;
}

public sealed record KnowledgeSearchHitResponse(
    string SourceType,
    Guid SourceId,
    string Title,
    string Snippet,
    double Score);

public sealed record KnowledgeIndexResultResponse(
    Guid RunId,
    int Indexed,
    int Unchanged,
    int Removed,
    int Failed,
    string Status);

public sealed record KnowledgeIndexRunResponse(
    Guid Id,
    DateTimeOffset StartedAtUtc,
    DateTimeOffset? CompletedAtUtc,
    int ItemsIndexed,
    int ItemsFailed,
    string Status);

public sealed record KnowledgeIndexStatusResponse(
    bool VectorDbConfigured,
    string EmbeddingModel,
    int Dimensions,
    int IndexedItems,
    KnowledgeIndexRunResponse? LatestRun);

// Semantic search over active Approved knowledge only (the only content that is ever embedded).
[ApiController]
[AllowAnonymous]
[Route("api/knowledge")]
public sealed class KnowledgeSearchController(IKnowledgeIndexService indexService) : ControllerBase
{
    [HttpGet("search")]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<KnowledgeSearchHitResponse>>>> Search(
        [FromQuery] KnowledgeSearchRequest request,
        CancellationToken cancellationToken)
    {
        var hits = await indexService.SearchAsync(request.Q, request.Type, request.Limit, cancellationToken);

        return Ok(ApiResponse<IReadOnlyCollection<KnowledgeSearchHitResponse>>.Success(
            hits.Select(hit => new KnowledgeSearchHitResponse(
                hit.SourceType,
                hit.SourceId,
                hit.Title,
                hit.Snippet,
                hit.Score)).ToArray(),
            "Search completed.",
            HttpContext.TraceIdentifier));
    }
}

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/admin/knowledge-index")]
public sealed class AdminKnowledgeIndexController(IKnowledgeIndexService indexService) : ControllerBase
{
    [HttpGet("status")]
    public async Task<ActionResult<ApiResponse<KnowledgeIndexStatusResponse>>> Status(
        CancellationToken cancellationToken)
    {
        var status = await indexService.GetStatusAsync(cancellationToken);

        return Ok(ApiResponse<KnowledgeIndexStatusResponse>.Success(
            ToResponse(status),
            "Knowledge index status loaded.",
            HttpContext.TraceIdentifier));
    }

    [HttpPost("rebuild")]
    public async Task<ActionResult<ApiResponse<KnowledgeIndexResultResponse>>> Rebuild(
        CancellationToken cancellationToken)
    {
        var result = await indexService.ReindexAsync(cancellationToken);

        return Ok(ApiResponse<KnowledgeIndexResultResponse>.Success(
            new KnowledgeIndexResultResponse(
                result.RunId,
                result.Indexed,
                result.Unchanged,
                result.Removed,
                result.Failed,
                result.Status),
            "Knowledge index updated.",
            HttpContext.TraceIdentifier));
    }

    private static KnowledgeIndexStatusResponse ToResponse(KnowledgeIndexStatusModel status)
    {
        return new KnowledgeIndexStatusResponse(
            status.VectorDbConfigured,
            status.EmbeddingModel,
            status.Dimensions,
            status.IndexedItems,
            status.LatestRun is null
                ? null
                : new KnowledgeIndexRunResponse(
                    status.LatestRun.Id,
                    status.LatestRun.StartedAtUtc,
                    status.LatestRun.CompletedAtUtc,
                    status.LatestRun.ItemsIndexed,
                    status.LatestRun.ItemsFailed,
                    status.LatestRun.Status));
    }
}
