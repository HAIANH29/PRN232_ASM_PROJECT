namespace LongevityDiet.DietKnowledge.Application.Models;

public sealed record BookSourceDocumentQuery(
    int PageNumber = 1,
    int PageSize = 20,
    string? Status = null);

public sealed record BookSourceDocumentModel(
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

public sealed record BookSourceChunkModel(
    Guid Id,
    Guid DocumentId,
    int ChunkIndex,
    int PageNumber,
    string Text,
    int CharacterCount,
    int TokenEstimate,
    DateTimeOffset CreatedAtUtc);

public sealed record KnowledgeCandidateModel(
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

public sealed record UploadBookSourceCommand(
    string OriginalFileName,
    string ContentType,
    long FileSizeBytes,
    Stream Content,
    string UploadedBy);

public sealed record GenerateKnowledgeCandidatesCommand(
    Guid DocumentId,
    string RequestedBy);

public sealed record ApproveKnowledgeCandidateCommand(
    Guid CandidateId,
    string ReviewedBy);

public sealed record RejectKnowledgeCandidateCommand(
    Guid CandidateId,
    string ReviewedBy,
    string? Reason);

public sealed record KnowledgeCandidateDraft(
    Guid? ChunkId,
    string CandidateType,
    string Title,
    string Summary,
    string SourceTitle,
    string SourceChapter,
    string SourcePage,
    string SourceReference,
    string GeneratedBy);

public sealed record BookPageText(
    int PageNumber,
    string Text);
