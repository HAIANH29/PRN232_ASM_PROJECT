using LongevityDiet.DietKnowledge.Application.Models;
using LongevityDiet.DietKnowledge.Domain.Entities;

namespace LongevityDiet.DietKnowledge.Application.Abstractions;

public interface IBookTextChunker
{
    IReadOnlyCollection<BookSourceChunk> CreateChunks(
        Guid documentId,
        IReadOnlyCollection<BookPageText> pages);
}
