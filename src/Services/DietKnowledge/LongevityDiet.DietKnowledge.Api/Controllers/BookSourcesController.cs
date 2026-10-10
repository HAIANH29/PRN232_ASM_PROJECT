using System.Security.Claims;
using LongevityDiet.ApiDefaults.Api;
using LongevityDiet.DietKnowledge.Api.Contracts;
using LongevityDiet.DietKnowledge.Application.Models;
using LongevityDiet.DietKnowledge.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LongevityDiet.DietKnowledge.Api.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/admin/book-sources")]
public sealed class BookSourcesController(IBookKnowledgeIngestionService ingestionService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResponse<BookSourceDocumentResponse>>>> List(
        [FromQuery] BookSourceDocumentListRequest request,
        CancellationToken cancellationToken)
    {
        var result = await ingestionService.ListDocumentsAsync(request.ToQuery(), cancellationToken);
        var response = DietKnowledgeContractMapper.ToPagedResponse(result, item => item.ToResponse());

        return Ok(ApiResponse<PagedResponse<BookSourceDocumentResponse>>.Success(
            response,
            "Book source documents loaded.",
            HttpContext.TraceIdentifier));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<BookSourceDocumentResponse>>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await ingestionService.GetDocumentAsync(id, cancellationToken);

        return Ok(ApiResponse<BookSourceDocumentResponse>.Success(
            result.ToResponse(),
            "Book source document loaded.",
            HttpContext.TraceIdentifier));
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(25 * 1024 * 1024)]
    public async Task<ActionResult<ApiResponse<BookSourceDocumentResponse>>> Upload(
        IFormFile? file,
        CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
        {
            return BadRequest(ApiResponse<object?>.Failure(
                [new ApiError("Validation.Invalid", "A PDF file is required.", "file")],
                "Validation failed.",
                HttpContext.TraceIdentifier));
        }

        await using var stream = file.OpenReadStream();
        var result = await ingestionService.UploadAndChunkAsync(
            new UploadBookSourceCommand(
                file.FileName,
                file.ContentType,
                file.Length,
                stream,
                CurrentUserName()),
            cancellationToken);

        return Created(
            $"/api/admin/book-sources/{result.Id}",
            ApiResponse<BookSourceDocumentResponse>.Success(
                result.ToResponse(),
                "Book source uploaded and chunked.",
                HttpContext.TraceIdentifier));
    }

    [HttpGet("{id:guid}/chunks")]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<BookSourceChunkResponse>>>> ListChunks(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await ingestionService.ListChunksAsync(id, cancellationToken);

        return Ok(ApiResponse<IReadOnlyCollection<BookSourceChunkResponse>>.Success(
            result.Select(item => item.ToResponse()).ToArray(),
            "Book source chunks loaded.",
            HttpContext.TraceIdentifier));
    }

    [HttpGet("{id:guid}/candidates")]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<KnowledgeCandidateResponse>>>> ListCandidates(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await ingestionService.ListCandidatesAsync(id, cancellationToken);

        return Ok(ApiResponse<IReadOnlyCollection<KnowledgeCandidateResponse>>.Success(
            result.Select(item => item.ToResponse()).ToArray(),
            "Knowledge candidates loaded.",
            HttpContext.TraceIdentifier));
    }

    [HttpPost("{id:guid}/candidates/generate")]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<KnowledgeCandidateResponse>>>> GenerateCandidates(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await ingestionService.GenerateCandidatesAsync(
            new GenerateKnowledgeCandidatesCommand(id, CurrentUserName()),
            cancellationToken);

        return Ok(ApiResponse<IReadOnlyCollection<KnowledgeCandidateResponse>>.Success(
            result.Select(item => item.ToResponse()).ToArray(),
            "Knowledge candidates generated for admin review.",
            HttpContext.TraceIdentifier));
    }

    private string CurrentUserName()
    {
        return User.FindFirstValue(ClaimTypes.Email) ??
            User.FindFirstValue(ClaimTypes.Name) ??
            "Admin";
    }
}

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/admin/knowledge-candidates")]
public sealed class KnowledgeCandidatesController(IBookKnowledgeIngestionService ingestionService)
    : ControllerBase
{
    [HttpPost("{id:guid}/approve")]
    public async Task<ActionResult<ApiResponse<KnowledgeCandidateResponse>>> Approve(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await ingestionService.ApproveCandidateAsync(
            new ApproveKnowledgeCandidateCommand(id, CurrentUserName()),
            cancellationToken);

        return Ok(ApiResponse<KnowledgeCandidateResponse>.Success(
            result.ToResponse(),
            "Knowledge candidate approved.",
            HttpContext.TraceIdentifier));
    }

    [HttpPost("{id:guid}/reject")]
    public async Task<ActionResult<ApiResponse<KnowledgeCandidateResponse>>> Reject(
        Guid id,
        RejectKnowledgeCandidateRequest request,
        CancellationToken cancellationToken)
    {
        var result = await ingestionService.RejectCandidateAsync(
            new RejectKnowledgeCandidateCommand(id, CurrentUserName(), request.Reason),
            cancellationToken);

        return Ok(ApiResponse<KnowledgeCandidateResponse>.Success(
            result.ToResponse(),
            "Knowledge candidate rejected.",
            HttpContext.TraceIdentifier));
    }

    private string CurrentUserName()
    {
        return User.FindFirstValue(ClaimTypes.Email) ??
            User.FindFirstValue(ClaimTypes.Name) ??
            "Admin";
    }
}
