using LongevityDiet.DietKnowledge.Application.Abstractions;
using LongevityDiet.DietKnowledge.Application.Models;
using LongevityDiet.DietKnowledge.Domain.Entities;
using Microsoft.Extensions.Options;

namespace LongevityDiet.DietKnowledge.Infrastructure.Ingestion;

public sealed class BookTextChunker(
    IOptions<BookKnowledgeIngestionOptions> options) : IBookTextChunker
{
    private readonly BookKnowledgeIngestionOptions ingestionOptions = options.Value;

    public IReadOnlyCollection<BookSourceChunk> CreateChunks(
        Guid documentId,
        IReadOnlyCollection<BookPageText> pages)
    {
        var maxChunkCharacters = Math.Clamp(ingestionOptions.MaxChunkCharacters, 800, 7500);
        var overlapCharacters = Math.Clamp(
            ingestionOptions.ChunkOverlapCharacters,
            0,
            maxChunkCharacters / 3);
        var chunks = new List<BookSourceChunk>();
        var chunkIndex = 1;

        foreach (var page in pages.OrderBy(page => page.PageNumber))
        {
            var pageText = page.Text.Trim();
            var cursor = 0;

            while (cursor < pageText.Length)
            {
                var remaining = pageText.Length - cursor;
                var take = Math.Min(maxChunkCharacters, remaining);
                var end = cursor + take;

                if (end < pageText.Length)
                {
                    end = FindNaturalBreak(pageText, cursor, end, maxChunkCharacters);
                }

                var text = pageText[cursor..end].Trim();
                if (!string.IsNullOrWhiteSpace(text))
                {
                    chunks.Add(new BookSourceChunk
                    {
                        Id = Guid.NewGuid(),
                        DocumentId = documentId,
                        ChunkIndex = chunkIndex++,
                        PageNumber = page.PageNumber,
                        Text = text,
                        CharacterCount = text.Length,
                        TokenEstimate = Math.Max(1, text.Length / 4),
                        CreatedAtUtc = DateTimeOffset.UtcNow
                    });
                }

                if (end >= pageText.Length)
                {
                    break;
                }

                cursor = Math.Max(end - overlapCharacters, cursor + 1);
            }
        }

        return chunks;
    }

    private static int FindNaturalBreak(
        string text,
        int start,
        int proposedEnd,
        int maxChunkCharacters)
    {
        var minimumEnd = start + (int)(maxChunkCharacters * 0.6);
        for (var index = proposedEnd - 1; index > minimumEnd; index--)
        {
            if (text[index] is '.' or '!' or '?' or ';')
            {
                return index + 1;
            }
        }

        for (var index = proposedEnd - 1; index > minimumEnd; index--)
        {
            if (char.IsWhiteSpace(text[index]))
            {
                return index;
            }
        }

        return proposedEnd;
    }
}
