using System.ComponentModel.DataAnnotations;
using LongevityDiet.DietKnowledge.Application.Common;
using LongevityDiet.DietKnowledge.Application.Models;
using LongevityDiet.DietKnowledge.Application.Services;
using LongevityDiet.DietKnowledge.Domain.Entities;
using Xunit;

namespace LongevityDiet.Core.Tests;

public sealed class BookKnowledgeIngestionServiceTests
{
    [Fact]
    public async Task UploadAndChunkAsync_rejects_non_pdf_uploads()
    {
        var service = CreateService(new InMemoryDietKnowledgeRepository());

        await Assert.ThrowsAsync<ValidationException>(() =>
            service.UploadAndChunkAsync(new UploadBookSourceCommand(
                OriginalFileName: "book.txt",
                ContentType: "text/plain",
                FileSizeBytes: 12,
                Content: new MemoryStream("not a pdf"u8.ToArray()),
                UploadedBy: "admin@example.com")));
    }

    [Fact]
    public async Task GenerateCandidatesAsync_deduplicates_and_stores_usable_candidates()
    {
        var repository = new InMemoryDietKnowledgeRepository();
        var documentId = Guid.NewGuid();
        var chunkId = Guid.NewGuid();
        repository.Documents.Add(new BookSourceDocument
        {
            Id = documentId,
            OriginalFileName = "longevity.pdf",
            StoredFileName = "stored.pdf",
            ContentType = "application/pdf",
            FileSizeBytes = 100,
            StoragePath = "stored.pdf",
            Status = BookSourceDocumentStatuses.Chunked,
            UploadedBy = "admin",
            UploadedAtUtc = DateTimeOffset.UtcNow
        });
        repository.Chunks.Add(new BookSourceChunk
        {
            Id = chunkId,
            DocumentId = documentId,
            ChunkIndex = 1,
            PageNumber = 7,
            Text = "approved book text",
            CharacterCount = 18,
            TokenEstimate = 5,
            CreatedAtUtc = DateTimeOffset.UtcNow
        });

        var generator = new FakeKnowledgeCandidateGenerator
        {
            Drafts =
            [
                Draft(chunkId, KnowledgeCandidateTypes.DietGuideline, "Plant-rich pattern"),
                Draft(chunkId, KnowledgeCandidateTypes.DietGuideline, " plant-rich pattern "),
                Draft(chunkId, "InvalidType", "Ignored candidate")
            ]
        };

        var service = CreateService(repository, generator);
        var result = await service.GenerateCandidatesAsync(
            new GenerateKnowledgeCandidatesCommand(documentId, "admin"));

        Assert.Single(result);
        Assert.Single(repository.Candidates);
        Assert.Equal("Plant-rich pattern", repository.Candidates.Single().Title);
        Assert.Equal(KnowledgeCandidateStatuses.NeedsReview, repository.Candidates.Single().Status);
    }

    [Fact]
    public async Task ApproveCandidateAsync_creates_approved_guideline_from_reviewed_candidate()
    {
        var repository = new InMemoryDietKnowledgeRepository();
        var documentId = Guid.NewGuid();
        var candidate = new KnowledgeCandidate
        {
            Id = Guid.NewGuid(),
            DocumentId = documentId,
            CandidateType = KnowledgeCandidateTypes.DietGuideline,
            Title = "Time-restricted eating window",
            Summary = "Educational summary from the approved source.",
            SourceTitle = "The Longevity Diet",
            SourceChapter = "Diet pattern",
            SourcePage = "88",
            SourceReference = "",
            Status = KnowledgeCandidateStatuses.NeedsReview,
            GeneratedBy = "test",
            CreatedAtUtc = DateTimeOffset.UtcNow
        };

        repository.Documents.Add(new BookSourceDocument
        {
            Id = documentId,
            OriginalFileName = "longevity.pdf",
            StoredFileName = "stored.pdf",
            ContentType = "application/pdf",
            FileSizeBytes = 100,
            StoragePath = "stored.pdf",
            Status = BookSourceDocumentStatuses.Chunked,
            UploadedBy = "admin",
            UploadedAtUtc = DateTimeOffset.UtcNow
        });
        repository.Candidates.Add(candidate);

        var service = CreateService(repository);
        var result = await service.ApproveCandidateAsync(
            new ApproveKnowledgeCandidateCommand(candidate.Id, "admin@example.com"));

        Assert.Equal(KnowledgeCandidateStatuses.Approved, result.Status);
        Assert.Single(repository.Guidelines);
        var guideline = repository.Guidelines.Single();
        Assert.Equal("Time-restricted eating window", guideline.Title);
        Assert.Equal(KnowledgeReviewStatuses.Approved, guideline.ReviewStatus);
        Assert.Equal("admin@example.com", guideline.ReviewedBy);
        Assert.Equal(guideline.Id, result.ApprovedKnowledgeId);
    }

    private static BookKnowledgeIngestionService CreateService(
        InMemoryDietKnowledgeRepository repository,
        FakeKnowledgeCandidateGenerator? generator = null)
    {
        return new BookKnowledgeIngestionService(
            repository,
            new FakeBookKnowledgeFileStore("storage"),
            new FakeBookTextExtractor([new BookPageText(1, "The Longevity Diet text")]),
            new FakeBookTextChunker(),
            generator ?? new FakeKnowledgeCandidateGenerator());
    }

    private static KnowledgeCandidateDraft Draft(
        Guid chunkId,
        string type,
        string title)
    {
        return new KnowledgeCandidateDraft(
            chunkId,
            type,
            title,
            "Concise project-ready summary.",
            "The Longevity Diet",
            "Demo chapter",
            "7",
            "page 7",
            "test");
    }
}
