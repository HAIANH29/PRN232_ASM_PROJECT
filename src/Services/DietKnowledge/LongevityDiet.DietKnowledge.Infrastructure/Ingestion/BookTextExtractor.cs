using LongevityDiet.DietKnowledge.Application.Abstractions;
using LongevityDiet.DietKnowledge.Application.Models;

namespace LongevityDiet.DietKnowledge.Infrastructure.Ingestion;

// Picks the extractor from the stored file extension (.pdf or .txt).
public sealed class BookTextExtractor(
    PdfBookTextExtractor pdfExtractor,
    TextBookTextExtractor textExtractor) : IBookTextExtractor
{
    public Task<IReadOnlyCollection<BookPageText>> ExtractPagesAsync(
        string path,
        CancellationToken cancellationToken = default)
    {
        return string.Equals(Path.GetExtension(path), ".txt", StringComparison.OrdinalIgnoreCase)
            ? textExtractor.ExtractPagesAsync(path, cancellationToken)
            : pdfExtractor.ExtractPagesAsync(path, cancellationToken);
    }
}
