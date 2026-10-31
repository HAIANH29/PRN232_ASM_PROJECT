using LongevityDiet.DietKnowledge.Application.Models;
using LongevityDiet.DietKnowledge.Domain.Entities;

namespace LongevityDiet.DietKnowledge.Application.Abstractions;

public interface IKnowledgeCandidateGenerator
{
    // Receives only chunks that still need analysis and may handle just the first few of them.
    // ProcessedChunkIds lists the chunks that were actually analyzed so the caller can mark them done.
    // onProgress(processed, total) is called with the chunks handled so far in this run.
    Task<KnowledgeCandidateGenerationResult> GenerateAsync(
        BookSourceDocument document,
        IReadOnlyCollection<BookSourceChunk> chunks,
        CancellationToken cancellationToken = default,
        Action<int, int>? onProgress = null);
}
