using System.ComponentModel.DataAnnotations;
using LongevityDiet.DietKnowledge.Application.Abstractions;
using LongevityDiet.DietKnowledge.Application.Common;
using LongevityDiet.DietKnowledge.Application.Models;
using LongevityDiet.DietKnowledge.Domain.Entities;

namespace LongevityDiet.DietKnowledge.Application.Services;

public sealed class BookKnowledgeIngestionService(
    IDietKnowledgeRepository repository,
    IBookKnowledgeFileStore fileStore,
    IBookTextExtractor textExtractor,
    IBookTextChunker textChunker,
    IKnowledgeCandidateGenerator candidateGenerator) : IBookKnowledgeIngestionService
{
    private const long DefaultMaxUploadBytes = 25 * 1024 * 1024;

    public async Task<PagedResult<BookSourceDocumentModel>> ListDocumentsAsync(
        BookSourceDocumentQuery query,
        CancellationToken cancellationToken = default)
    {
        var normalized = Normalize(query);
        var result = await repository.ListBookSourceDocumentsAsync(normalized, cancellationToken);
        return new PagedResult<BookSourceDocumentModel>(
            result.Items.Select(MapDocument).ToArray(),
            result.PageNumber,
            result.PageSize,
            result.TotalCount);
    }

    public async Task<BookSourceDocumentModel> GetDocumentAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var document = await repository.GetBookSourceDocumentByIdAsync(id, cancellationToken: cancellationToken)
            ?? throw new KeyNotFoundException("Book source document was not found.");

        return MapDocument(document);
    }

    public async Task<IReadOnlyCollection<BookSourceChunkModel>> ListChunksAsync(
        Guid documentId,
        CancellationToken cancellationToken = default)
    {
        await EnsureDocumentExistsAsync(documentId, cancellationToken);
        var chunks = await repository.ListBookSourceChunksAsync(documentId, cancellationToken);
        return chunks.Select(MapChunk).ToArray();
    }

    public async Task<IReadOnlyCollection<KnowledgeCandidateModel>> ListCandidatesAsync(
        Guid documentId,
        CancellationToken cancellationToken = default)
    {
        await EnsureDocumentExistsAsync(documentId, cancellationToken);
        var candidates = await repository.ListKnowledgeCandidatesAsync(documentId, cancellationToken);
        return candidates.Select(MapCandidate).ToArray();
    }

    public async Task<BookSourceDocumentModel> UploadAndChunkAsync(
        UploadBookSourceCommand command,
        CancellationToken cancellationToken = default)
    {
        ValidateUpload(command);

        var documentId = Guid.NewGuid();
        var storedFileName = $"{documentId:N}.pdf";
        var storagePath = await fileStore.SaveAsync(
            command.Content,
            storedFileName,
            cancellationToken);

        var document = new BookSourceDocument
        {
            Id = documentId,
            OriginalFileName = Clean(command.OriginalFileName),
            StoredFileName = storedFileName,
            ContentType = string.IsNullOrWhiteSpace(command.ContentType)
                ? "application/pdf"
                : command.ContentType.Trim(),
            FileSizeBytes = command.FileSizeBytes,
            StoragePath = storagePath,
            Status = BookSourceDocumentStatuses.Uploaded,
            UploadedBy = CleanOptional(command.UploadedBy),
            UploadedAtUtc = DateTimeOffset.UtcNow
        };

        await repository.AddBookSourceDocumentAsync(document, cancellationToken);

        try
        {
            var pages = await textExtractor.ExtractPagesAsync(storagePath, cancellationToken);
            var chunks = textChunker.CreateChunks(document.Id, pages);

            if (chunks.Count == 0)
            {
                throw new ValidationException("No readable text could be extracted from this PDF.");
            }

            await repository.AddBookSourceChunksAsync(chunks, cancellationToken);
            document.ChunkCount = chunks.Count;
            document.Status = BookSourceDocumentStatuses.Chunked;
            document.ProcessedAtUtc = DateTimeOffset.UtcNow;
            document.ErrorMessage = string.Empty;
        }
        catch (Exception exception)
        {
            document.Status = BookSourceDocumentStatuses.Failed;
            document.ErrorMessage = Truncate(exception.Message, 1000);
            document.ProcessedAtUtc = DateTimeOffset.UtcNow;
        }

        await repository.SaveChangesAsync(cancellationToken);
        return MapDocument(document);
    }

    public async Task<IReadOnlyCollection<KnowledgeCandidateModel>> GenerateCandidatesAsync(
        GenerateKnowledgeCandidatesCommand command,
        CancellationToken cancellationToken = default)
    {
        var document = await repository.GetBookSourceDocumentByIdAsync(
                command.DocumentId,
                includeChunks: false,
                includeCandidates: false,
                cancellationToken)
            ?? throw new KeyNotFoundException("Book source document was not found.");

        if (document.Status != BookSourceDocumentStatuses.Chunked)
        {
            throw new ValidationException("Candidates can only be generated after the PDF is chunked.");
        }

        var chunks = await repository.ListBookSourceChunksAsync(document.Id, cancellationToken);
        if (chunks.Count == 0)
        {
            throw new ValidationException("This document has no chunks to analyze.");
        }

        var drafts = await candidateGenerator.GenerateAsync(document, chunks, cancellationToken);
        var candidates = drafts
            .Where(IsUsableDraft)
            .GroupBy(draft => Clean(draft.Title), StringComparer.OrdinalIgnoreCase)
            .Select(group => MapDraft(document, group.First()))
            .ToArray();

        if (candidates.Length == 0)
        {
            throw new ValidationException("No knowledge candidates were generated from this document.");
        }

        await repository.AddKnowledgeCandidatesAsync(candidates, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return candidates.Select(MapCandidate).ToArray();
    }

    public async Task<KnowledgeCandidateModel> ApproveCandidateAsync(
        ApproveKnowledgeCandidateCommand command,
        CancellationToken cancellationToken = default)
    {
        var candidate = await repository.GetKnowledgeCandidateByIdAsync(
                command.CandidateId,
                cancellationToken)
            ?? throw new KeyNotFoundException("Knowledge candidate was not found.");

        if (candidate.Status != KnowledgeCandidateStatuses.NeedsReview)
        {
            throw new ValidationException("Only candidates that still need review can be approved.");
        }

        var reviewedBy = Clean(command.ReviewedBy);
        var approvedKnowledgeId = await CreateApprovedKnowledgeAsync(
            candidate,
            reviewedBy,
            cancellationToken);

        candidate.Status = KnowledgeCandidateStatuses.Approved;
        candidate.ReviewedBy = reviewedBy;
        candidate.ReviewedAtUtc = DateTimeOffset.UtcNow;
        candidate.ApprovedKnowledgeId = approvedKnowledgeId;

        await repository.SaveChangesAsync(cancellationToken);
        return MapCandidate(candidate);
    }

    public async Task<KnowledgeCandidateModel> RejectCandidateAsync(
        RejectKnowledgeCandidateCommand command,
        CancellationToken cancellationToken = default)
    {
        var candidate = await repository.GetKnowledgeCandidateByIdAsync(
                command.CandidateId,
                cancellationToken)
            ?? throw new KeyNotFoundException("Knowledge candidate was not found.");

        if (candidate.Status != KnowledgeCandidateStatuses.NeedsReview)
        {
            throw new ValidationException("Only candidates that still need review can be rejected.");
        }

        candidate.Status = KnowledgeCandidateStatuses.Rejected;
        candidate.ReviewedBy = Clean(command.ReviewedBy);
        candidate.ReviewedAtUtc = DateTimeOffset.UtcNow;

        var reason = CleanOptional(command.Reason);
        if (!string.IsNullOrWhiteSpace(reason))
        {
            candidate.SourceReference = Truncate(
                $"{candidate.SourceReference} Rejected note: {reason}".Trim(),
                500);
        }

        await repository.SaveChangesAsync(cancellationToken);
        return MapCandidate(candidate);
    }

    private async Task<Guid> CreateApprovedKnowledgeAsync(
        KnowledgeCandidate candidate,
        string reviewedBy,
        CancellationToken cancellationToken)
    {
        var candidateType = KnowledgeCandidateTypes.Normalize(candidate.CandidateType);
        var title = Clean(candidate.Title);
        var sourceChapter = string.IsNullOrWhiteSpace(candidate.SourceChapter)
            ? "Uploaded book source"
            : candidate.SourceChapter.Trim();
        var sourcePage = CleanOptional(candidate.SourcePage);
        var sourceReference = string.IsNullOrWhiteSpace(candidate.SourceReference)
            ? $"Uploaded PDF source document {candidate.DocumentId}"
            : candidate.SourceReference.Trim();

        if (candidateType == KnowledgeCandidateTypes.DietGuideline)
        {
            if (await repository.GuidelineTitleExistsAsync(title, cancellationToken: cancellationToken))
            {
                throw new InvalidOperationException("A diet guideline with this candidate title already exists.");
            }

            var guideline = new DietGuideline
            {
                Id = Guid.NewGuid(),
                Title = title,
                Summary = Clean(candidate.Summary),
                SourceNote = "Approved from uploaded book-source candidate.",
                SourceTitle = CleanOrDefault(candidate.SourceTitle, "The Longevity Diet"),
                SourceChapter = sourceChapter,
                SourcePage = sourcePage,
                SourceReference = sourceReference,
                ReviewStatus = KnowledgeReviewStatuses.Approved,
                ReviewedBy = reviewedBy,
                ReviewedAtUtc = DateTimeOffset.UtcNow,
                IsActive = true,
                CreatedAtUtc = DateTimeOffset.UtcNow
            };

            await repository.AddGuidelineAsync(guideline, cancellationToken);
            return guideline.Id;
        }

        if (candidateType == KnowledgeCandidateTypes.Food)
        {
            if (await repository.FoodNameExistsAsync(title, cancellationToken: cancellationToken))
            {
                throw new InvalidOperationException("A food with this candidate title already exists.");
            }

            var food = new Food
            {
                Id = Guid.NewGuid(),
                Name = title,
                Category = "Book-derived",
                CompatibilityNotes = Clean(candidate.Summary),
                SourceTitle = CleanOrDefault(candidate.SourceTitle, "The Longevity Diet"),
                SourceChapter = sourceChapter,
                SourcePage = sourcePage,
                SourceReference = sourceReference,
                ReviewStatus = KnowledgeReviewStatuses.Approved,
                ReviewedBy = reviewedBy,
                ReviewedAtUtc = DateTimeOffset.UtcNow,
                IsActive = true,
                CreatedAtUtc = DateTimeOffset.UtcNow
            };

            await repository.AddFoodAsync(food, cancellationToken);
            return food.Id;
        }

        throw new ValidationException(
            "Recipe candidates must be recreated through the recipe admin screen so ingredients are reviewed explicitly.");
    }

    private async Task EnsureDocumentExistsAsync(
        Guid documentId,
        CancellationToken cancellationToken)
    {
        _ = await repository.GetBookSourceDocumentByIdAsync(documentId, cancellationToken: cancellationToken)
            ?? throw new KeyNotFoundException("Book source document was not found.");
    }

    private static KnowledgeCandidate MapDraft(
        BookSourceDocument document,
        KnowledgeCandidateDraft draft)
    {
        return new KnowledgeCandidate
        {
            Id = Guid.NewGuid(),
            DocumentId = document.Id,
            ChunkId = draft.ChunkId,
            CandidateType = KnowledgeCandidateTypes.Normalize(draft.CandidateType),
            Title = Truncate(Clean(draft.Title), 200),
            Summary = Truncate(Clean(draft.Summary), 2000),
            SourceTitle = CleanOrDefault(draft.SourceTitle, "The Longevity Diet"),
            SourceChapter = Truncate(CleanOptional(draft.SourceChapter), 200),
            SourcePage = Truncate(CleanOptional(draft.SourcePage), 80),
            SourceReference = Truncate(CleanOptional(draft.SourceReference), 500),
            Status = KnowledgeCandidateStatuses.NeedsReview,
            GeneratedBy = Truncate(CleanOrDefault(draft.GeneratedBy, "BookKnowledgeIngestion"), 120),
            CreatedAtUtc = DateTimeOffset.UtcNow
        };
    }

    private static bool IsUsableDraft(KnowledgeCandidateDraft draft)
    {
        return KnowledgeCandidateTypes.IsValid(draft.CandidateType) &&
            !string.IsNullOrWhiteSpace(draft.Title) &&
            !string.IsNullOrWhiteSpace(draft.Summary);
    }

    private static BookSourceDocumentQuery Normalize(BookSourceDocumentQuery query)
    {
        return query with
        {
            PageNumber = Math.Max(1, query.PageNumber),
            PageSize = Math.Clamp(query.PageSize, 1, 100),
            Status = NormalizeStatusFilter(query.Status)
        };
    }

    private static string NormalizeStatusFilter(string? status)
    {
        if (string.IsNullOrWhiteSpace(status))
        {
            return string.Empty;
        }

        if (!BookSourceDocumentStatuses.IsValid(status))
        {
            throw new ValidationException("Status must be Uploaded, Chunked, or Failed.");
        }

        return BookSourceDocumentStatuses.Normalize(status);
    }

    private static void ValidateUpload(UploadBookSourceCommand command)
    {
        if (command.Content.Length == 0 || command.FileSizeBytes == 0)
        {
            throw new ValidationException("A PDF file is required.");
        }

        if (command.FileSizeBytes > DefaultMaxUploadBytes)
        {
            throw new ValidationException("PDF uploads are limited to 25 MB.");
        }

        var extension = Path.GetExtension(command.OriginalFileName);
        if (!string.Equals(extension, ".pdf", StringComparison.OrdinalIgnoreCase))
        {
            throw new ValidationException("Only PDF files can be uploaded.");
        }
    }

    private static BookSourceDocumentModel MapDocument(BookSourceDocument document)
    {
        return new BookSourceDocumentModel(
            document.Id,
            document.OriginalFileName,
            document.StoredFileName,
            document.ContentType,
            document.FileSizeBytes,
            document.Status,
            document.UploadedBy,
            document.UploadedAtUtc,
            document.ProcessedAtUtc,
            document.ChunkCount,
            document.ErrorMessage);
    }

    private static BookSourceChunkModel MapChunk(BookSourceChunk chunk)
    {
        return new BookSourceChunkModel(
            chunk.Id,
            chunk.DocumentId,
            chunk.ChunkIndex,
            chunk.PageNumber,
            chunk.Text,
            chunk.CharacterCount,
            chunk.TokenEstimate,
            chunk.CreatedAtUtc);
    }

    private static KnowledgeCandidateModel MapCandidate(KnowledgeCandidate candidate)
    {
        return new KnowledgeCandidateModel(
            candidate.Id,
            candidate.DocumentId,
            candidate.ChunkId,
            candidate.CandidateType,
            candidate.Title,
            candidate.Summary,
            candidate.SourceTitle,
            candidate.SourceChapter,
            candidate.SourcePage,
            candidate.SourceReference,
            candidate.Status,
            candidate.GeneratedBy,
            candidate.CreatedAtUtc,
            candidate.ReviewedBy,
            candidate.ReviewedAtUtc,
            candidate.ApprovedKnowledgeId);
    }

    private static string Clean(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ValidationException("A required value is missing.");
        }

        return value.Trim();
    }

    private static string CleanOptional(string? value)
    {
        return value?.Trim() ?? string.Empty;
    }

    private static string CleanOrDefault(string? value, string defaultValue)
    {
        return string.IsNullOrWhiteSpace(value) ? defaultValue : value.Trim();
    }

    private static string Truncate(string value, int maxLength)
    {
        return value.Length <= maxLength ? value : value[..maxLength];
    }
}
