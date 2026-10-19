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
    public async Task DeleteDocumentAsync_removes_document_chunks_candidates_and_stored_file()
    {
        var repository = new InMemoryDietKnowledgeRepository();
        var fileStore = new FakeBookKnowledgeFileStore("storage");
        var documentId = Guid.NewGuid();
        var otherDocumentId = Guid.NewGuid();
        repository.Documents.Add(new BookSourceDocument { Id = documentId, StoragePath = "storage/a.pdf" });
        repository.Documents.Add(new BookSourceDocument { Id = otherDocumentId, StoragePath = "storage/b.pdf" });
        repository.Chunks.Add(new BookSourceChunk { Id = Guid.NewGuid(), DocumentId = documentId, Text = "a" });
        repository.Chunks.Add(new BookSourceChunk { Id = Guid.NewGuid(), DocumentId = otherDocumentId, Text = "b" });
        repository.Candidates.Add(new KnowledgeCandidate { Id = Guid.NewGuid(), DocumentId = documentId });
        var service = new BookKnowledgeIngestionService(
            repository,
            fileStore,
            new FakeBookTextExtractor([]),
            new FakeBookTextChunker(),
            new FakeKnowledgeCandidateGenerator());

        await service.DeleteDocumentAsync(documentId);

        Assert.DoesNotContain(repository.Documents, document => document.Id == documentId);
        Assert.Single(repository.Documents);
        Assert.Single(repository.Chunks);
        Assert.Empty(repository.Candidates);
        Assert.Equal(["storage/a.pdf"], fileStore.DeletedPaths);
        Assert.True(repository.SaveChangesCount > 0);
    }

    [Fact]
    public async Task DeleteDocumentAsync_throws_when_document_is_missing()
    {
        var service = CreateService(new InMemoryDietKnowledgeRepository());

        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.DeleteDocumentAsync(Guid.NewGuid()));
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
            Text = UsefulText,
            CharacterCount = UsefulText.Length,
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
    public async Task GenerateCandidatesAsync_sends_only_pending_relevant_chunks_and_marks_them_processed()
    {
        var repository = new InMemoryDietKnowledgeRepository();
        var documentId = AddChunkedDocument(repository);
        var pending = AddChunk(repository, documentId, 1, UsefulText);
        var alreadyDone = AddChunk(repository, documentId, 2, UsefulText);
        alreadyDone.CandidatesGeneratedAtUtc = DateTimeOffset.UtcNow;
        var boilerplate = AddChunk(repository, documentId, 3, "Scan to Download");
        var generator = new FakeKnowledgeCandidateGenerator
        {
            Drafts = [Draft(pending.Id, KnowledgeCandidateTypes.Food, "Almonds")]
        };

        var result = await CreateService(repository, generator).GenerateCandidatesAsync(
            new GenerateKnowledgeCandidatesCommand(documentId, "admin"));

        Assert.Equal([pending.Id], generator.ReceivedChunks.Select(chunk => chunk.Id));
        Assert.Single(result);
        Assert.NotNull(pending.CandidatesGeneratedAtUtc);
        Assert.Null(boilerplate.CandidatesGeneratedAtUtc);
    }

    [Fact]
    public async Task GenerateCandidatesAsync_rejects_when_every_relevant_chunk_is_done()
    {
        var repository = new InMemoryDietKnowledgeRepository();
        var documentId = AddChunkedDocument(repository);
        AddChunk(repository, documentId, 1, UsefulText).CandidatesGeneratedAtUtc = DateTimeOffset.UtcNow;

        await Assert.ThrowsAsync<ValidationException>(() =>
            CreateService(repository).GenerateCandidatesAsync(
                new GenerateKnowledgeCandidatesCommand(documentId, "admin")));
    }

    [Fact]
    public async Task GenerateCandidatesAsync_keeps_chunks_pending_when_the_generator_processed_none()
    {
        var repository = new InMemoryDietKnowledgeRepository();
        var documentId = AddChunkedDocument(repository);
        var chunk = AddChunk(repository, documentId, 1, UsefulText);
        var generator = new FakeKnowledgeCandidateGenerator { ProcessedChunkIds = [] };

        await Assert.ThrowsAsync<ValidationException>(() =>
            CreateService(repository, generator).GenerateCandidatesAsync(
                new GenerateKnowledgeCandidatesCommand(documentId, "admin")));

        Assert.Null(chunk.CandidatesGeneratedAtUtc);
    }

    [Fact]
    public async Task GenerateCandidatesAsync_skips_titles_that_already_exist_for_the_document()
    {
        var repository = new InMemoryDietKnowledgeRepository();
        var documentId = AddChunkedDocument(repository);
        var chunk = AddChunk(repository, documentId, 1, UsefulText);
        repository.Candidates.Add(new KnowledgeCandidate
        {
            Id = Guid.NewGuid(),
            DocumentId = documentId,
            Title = "Plant-rich pattern"
        });
        var generator = new FakeKnowledgeCandidateGenerator
        {
            Drafts =
            [
                Draft(chunk.Id, KnowledgeCandidateTypes.DietGuideline, "plant-rich pattern"),
                Draft(chunk.Id, KnowledgeCandidateTypes.DietGuideline, "Twelve-hour eating window")
            ]
        };

        var result = await CreateService(repository, generator).GenerateCandidatesAsync(
            new GenerateKnowledgeCandidatesCommand(documentId, "admin"));

        Assert.Equal(["Twelve-hour eating window"], result.Select(candidate => candidate.Title));
    }

    [Fact]
    public async Task ApproveAllCandidatesAsync_approves_safe_items_and_leaves_flagged_ones()
    {
        var repository = new InMemoryDietKnowledgeRepository();
        var documentId = AddChunkedDocument(repository);
        AddPendingCandidate(repository, documentId, KnowledgeCandidateTypes.DietGuideline, "Plant-rich pattern", "Eat mostly legumes, vegetables and whole grains.");
        AddPendingCandidate(repository, documentId, KnowledgeCandidateTypes.Food, "Almonds", "Almonds provide calcium and vitamin E.");
        AddPendingCandidate(repository, documentId, KnowledgeCandidateTypes.DietGuideline, "Low protein and cancer", "Lower protein is linked to lower cancer rates.");
        AddPendingCandidate(repository, documentId, KnowledgeCandidateTypes.DietGuideline, "Review page 2 chunk 2", "Admin review required.", "LocalChunkFallback");

        var result = await CreateService(repository).ApproveAllCandidatesAsync(
            new ApproveAllKnowledgeCandidatesCommand(documentId, "admin@example.com"));

        Assert.Equal(new ApproveAllKnowledgeCandidatesResultModel(2, 2, 0), result);
        Assert.Single(repository.Guidelines);
        Assert.Single(repository.Foods);
        Assert.Equal(
            2,
            repository.Candidates.Count(candidate => candidate.Status == KnowledgeCandidateStatuses.NeedsReview));
    }

    [Fact]
    public async Task ApproveAllCandidatesAsync_counts_failures_and_keeps_going()
    {
        var repository = new InMemoryDietKnowledgeRepository();
        var documentId = AddChunkedDocument(repository);
        AddPendingCandidate(repository, documentId, KnowledgeCandidateTypes.Recipe, "Lentil stew", "Simple lentil stew.");
        AddPendingCandidate(repository, documentId, KnowledgeCandidateTypes.DietGuideline, "Eat legumes", "Eat legumes most days.");

        var result = await CreateService(repository).ApproveAllCandidatesAsync(
            new ApproveAllKnowledgeCandidatesCommand(documentId, "admin"));

        Assert.Equal(new ApproveAllKnowledgeCandidatesResultModel(1, 0, 1), result);
    }

    [Fact]
    public async Task RejectPlaceholderCandidatesAsync_rejects_only_pending_local_placeholders()
    {
        var repository = new InMemoryDietKnowledgeRepository();
        var documentId = AddChunkedDocument(repository);
        AddPendingCandidate(repository, documentId, KnowledgeCandidateTypes.DietGuideline, "Review page 1 chunk 1", "Admin review required.", "LocalChunkFallback");
        AddPendingCandidate(repository, documentId, KnowledgeCandidateTypes.DietGuideline, "Review page 2 chunk 2", "Admin review required.", "LocalChunkFallback");
        AddPendingCandidate(repository, documentId, KnowledgeCandidateTypes.DietGuideline, "Eat legumes", "Eat legumes most days.");

        var rejected = await CreateService(repository).RejectPlaceholderCandidatesAsync(
            new RejectPlaceholderCandidatesCommand(documentId, "admin"));

        Assert.Equal(2, rejected);
        Assert.Equal(
            ["Eat legumes"],
            repository.Candidates
                .Where(candidate => candidate.Status == KnowledgeCandidateStatuses.NeedsReview)
                .Select(candidate => candidate.Title));
    }

    private static void AddPendingCandidate(
        InMemoryDietKnowledgeRepository repository,
        Guid documentId,
        string type,
        string title,
        string summary,
        string generatedBy = "Gemini")
    {
        repository.Candidates.Add(new KnowledgeCandidate
        {
            Id = Guid.NewGuid(),
            DocumentId = documentId,
            CandidateType = type,
            Title = title,
            Summary = summary,
            SourceTitle = "The Longevity Diet",
            Status = KnowledgeCandidateStatuses.NeedsReview,
            GeneratedBy = generatedBy,
            CreatedAtUtc = DateTimeOffset.UtcNow
        });
    }

    [Fact]
    public async Task GetGenerationProgressAsync_counts_relevant_processed_and_skipped_chunks()
    {
        var repository = new InMemoryDietKnowledgeRepository();
        var documentId = AddChunkedDocument(repository);
        AddChunk(repository, documentId, 1, UsefulText).CandidatesGeneratedAtUtc = DateTimeOffset.UtcNow;
        AddChunk(repository, documentId, 2, UsefulText);
        AddChunk(repository, documentId, 3, "Scan to Download");

        var progress = await CreateService(repository).GetGenerationProgressAsync(documentId);

        Assert.Equal(new KnowledgeGenerationProgressModel(3, 2, 1, 1, 1), progress);
    }

    [Theory]
    [InlineData("Scan to Download", false)]
    [InlineData("Chapter 15 Acknowledgments. This chapter reflects on thirty years of the journey in exploring longevity, thanking mentors, collaborators, colleagues and friends along the way.", false)]
    [InlineData("Chapter 7 covers cancer prevention. Fasting before chemotherapy is explored in mice and in patients, with attention to side effects, protection of normal cells and tumor response over time.", false)]
    [InlineData(UsefulText, true)]
    public void BookChunkRelevance_filters_boilerplate_and_out_of_scope_text(string text, bool expected)
    {
        Assert.Equal(expected, BookChunkRelevance.IsRelevant(text));
    }

    [Fact]
    public void BookChunkRelevance_removes_download_boilerplate_from_text()
    {
        var cleaned = BookChunkRelevance.CleanText("Scan to DownloadEat more legumes. Install Bookey App to Unlock Full Text and Audio");

        Assert.Equal("Eat more legumes.", cleaned);
    }

    private const string UsefulText =
        "The Longevity Diet is mostly plant-based with limited fish, rich in unsaturated fats and complex carbohydrates. " +
        "Meals are eaten within a twelve-hour window, with two main meals and a snack.";

    private static Guid AddChunkedDocument(InMemoryDietKnowledgeRepository repository)
    {
        var documentId = Guid.NewGuid();
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
        return documentId;
    }

    private static BookSourceChunk AddChunk(
        InMemoryDietKnowledgeRepository repository,
        Guid documentId,
        int index,
        string text)
    {
        var chunk = new BookSourceChunk
        {
            Id = Guid.NewGuid(),
            DocumentId = documentId,
            ChunkIndex = index,
            PageNumber = index,
            Text = text,
            CharacterCount = text.Length,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };
        repository.Chunks.Add(chunk);
        return chunk;
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
