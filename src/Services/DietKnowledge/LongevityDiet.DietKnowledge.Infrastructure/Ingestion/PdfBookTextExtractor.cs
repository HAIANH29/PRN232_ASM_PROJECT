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
        // PDF text can contain NUL/control characters and unpaired surrogates, which PostgreSQL rejects.
        var cleaned = new string(RemoveInvalidCharacters(text ?? string.Empty).ToArray());

        return string.Join(
            " ",
            cleaned.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));
    }

    private static IEnumerable<char> RemoveInvalidCharacters(string text)
    {
        for (var i = 0; i < text.Length; i++)
        {
            var current = text[i];

            if (char.IsHighSurrogate(current) && i + 1 < text.Length && char.IsLowSurrogate(text[i + 1]))
            {
                yield return current;
                yield return text[++i];
            }
            else if (char.IsSurrogate(current))
            {
                continue;
            }
            else if (char.IsControl(current) && !char.IsWhiteSpace(current))
            {
                continue;
            }
            else
            {
                yield return current;
            }
        }
    }
}
