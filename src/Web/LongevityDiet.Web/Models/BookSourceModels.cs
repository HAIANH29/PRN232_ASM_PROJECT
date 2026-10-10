using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace LongevityDiet.Web.Models;

public sealed record BookSourceDocumentResponse(
    Guid Id,
    string OriginalFileName,
    string StoredFileName,
    string ContentType,
    long FileSizeBytes,
    string Status,
    string UploadedBy,
    DateTimeOffset UploadedAtUtc,
    DateTimeOffset? ProcessedAtUtc,
    int ChunkCount,
    string ErrorMessage);

public sealed record BookSourceChunkResponse(
    Guid Id,
    Guid DocumentId,
    int ChunkIndex,
    int PageNumber,
    string Text,
    int CharacterCount,
    int TokenEstimate,
    DateTimeOffset CreatedAtUtc);

public sealed record KnowledgeCandidateResponse(
    Guid Id,
    Guid DocumentId,
    Guid? ChunkId,
    string CandidateType,
    string Title,
    string Summary,
    string SourceTitle,
    string SourceChapter,
    string SourcePage,
    string SourceReference,
    string Status,
    string GeneratedBy,
    DateTimeOffset CreatedAtUtc,
    string ReviewedBy,
    DateTimeOffset? ReviewedAtUtc,
    Guid? ApprovedKnowledgeId);

public sealed class BookSourceUploadForm
{
    [Required]
    public IFormFile? File { get; set; }
}

public sealed class AdminBookSourcesViewModel
{
    public BookSourceUploadForm Upload { get; set; } = new();

    public PagedResponse<BookSourceDocumentResponse> Documents { get; set; } =
        KnowledgeIndexViewModel.Empty<BookSourceDocumentResponse>();
}

public sealed class AdminBookSourceDetailsViewModel
{
    public BookSourceDocumentResponse Document { get; set; } = default!;

    public IReadOnlyCollection<BookSourceChunkResponse> Chunks { get; set; } =
        Array.Empty<BookSourceChunkResponse>();

    public IReadOnlyCollection<KnowledgeCandidateResponse> Candidates { get; set; } =
        Array.Empty<KnowledgeCandidateResponse>();
}
