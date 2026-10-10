using System.ComponentModel.DataAnnotations;

namespace LongevityDiet.DietKnowledge.Api.Contracts;

public sealed record BookSourceDocumentListRequest : ListRequest
{
    [MaxLength(40)]
    public string? Status { get; init; }
}

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

public sealed record RejectKnowledgeCandidateRequest
{
    [MaxLength(500)]
    public string? Reason { get; init; }
}
