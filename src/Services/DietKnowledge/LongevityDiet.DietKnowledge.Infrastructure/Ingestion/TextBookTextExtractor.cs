using LongevityDiet.DietKnowledge.Application.Abstractions;
using LongevityDiet.DietKnowledge.Application.Models;

namespace LongevityDiet.DietKnowledge.Infrastructure.Ingestion;

public sealed class TextBookTextExtractor : IBookTextExtractor
{
    public async Task<IReadOnlyCollection<BookPageText>> ExtractPagesAsync(
        string path,
        CancellationToken cancellationToken = default)
    {
        // Plain text has no pages; the whole file is one page and the chunker splits it by size.
        var text = await File.ReadAllTextAsync(path, cancellationToken);
        var normalized = PdfBookTextExtractor.Normalize(text);

        return string.IsNullOrWhiteSpace(normalized)
            ? []
            : [new BookPageText(1, normalized)];
    }
}
