namespace LongevityDiet.DietKnowledge.Infrastructure.Ingestion;

public sealed class BookKnowledgeIngestionOptions
{
    public const string SectionName = "BookKnowledgeIngestion";

    public string StoragePath { get; set; } = "storage/book-sources";

    public int MaxChunkCharacters { get; set; } = 2500;

    public int ChunkOverlapCharacters { get; set; } = 200;

    public int MaxChunksPerGeneration { get; set; } = 8;
}
