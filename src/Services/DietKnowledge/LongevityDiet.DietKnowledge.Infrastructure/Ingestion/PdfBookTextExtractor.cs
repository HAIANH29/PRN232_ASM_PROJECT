using LongevityDiet.DietKnowledge.Application.Abstractions;
using LongevityDiet.DietKnowledge.Application.Models;
using UglyToad.PdfPig;

namespace LongevityDiet.DietKnowledge.Infrastructure.Ingestion;

public sealed class PdfBookTextExtractor : IBookTextExtractor
{
    public Task<IReadOnlyCollection<BookPageText>> ExtractPagesAsync(
        string path,
        CancellationToken cancellationToken = default)
    {
        return Task.Run<IReadOnlyCollection<BookPageText>>(
            () =>
            {
                using var document = PdfDocument.Open(path);
                return document.GetPages()
                    .Select(page => new BookPageText(page.Number, Normalize(page.Text)))
                    .Where(page => !string.IsNullOrWhiteSpace(page.Text))
                    .ToArray();
            },
            cancellationToken);
    }

    private static string Normalize(string text)
    {
        return string.Join(
            " ",
            (text ?? string.Empty)
                .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));
    }
}
