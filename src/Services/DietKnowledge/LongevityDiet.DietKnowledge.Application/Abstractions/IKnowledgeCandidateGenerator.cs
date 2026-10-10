using LongevityDiet.DietKnowledge.Application.Models;
using LongevityDiet.DietKnowledge.Domain.Entities;

namespace LongevityDiet.DietKnowledge.Application.Abstractions;

public interface IKnowledgeCandidateGenerator
{
    Task<IReadOnlyCollection<KnowledgeCandidateDraft>> GenerateAsync(
        BookSourceDocument document,
        IReadOnlyCollection<BookSourceChunk> chunks,
        CancellationToken cancellationToken = default);
}
