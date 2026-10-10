using LongevityDiet.DietKnowledge.Application.Models;

namespace LongevityDiet.DietKnowledge.Application.Abstractions;

public interface IBookTextExtractor
{
    Task<IReadOnlyCollection<BookPageText>> ExtractPagesAsync(
        string path,
        CancellationToken cancellationToken = default);
}
