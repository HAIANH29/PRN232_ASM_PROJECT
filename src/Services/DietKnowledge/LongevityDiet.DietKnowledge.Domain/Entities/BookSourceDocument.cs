namespace LongevityDiet.DietKnowledge.Domain.Entities;

public sealed class BookSourceDocument
{
    public Guid Id { get; set; }

    public string OriginalFileName { get; set; } = string.Empty;

    public string StoredFileName { get; set; } = string.Empty;

    public string ContentType { get; set; } = string.Empty;

    public long FileSizeBytes { get; set; }

    public string StoragePath { get; set; } = string.Empty;

    public string Status { get; set; } = "Uploaded";

    public string UploadedBy { get; set; } = string.Empty;

    public DateTimeOffset UploadedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? ProcessedAtUtc { get; set; }

    public int ChunkCount { get; set; }

    public string ErrorMessage { get; set; } = string.Empty;

    public ICollection<BookSourceChunk> Chunks { get; } = new List<BookSourceChunk>();

    public ICollection<KnowledgeCandidate> Candidates { get; } = new List<KnowledgeCandidate>();
}
