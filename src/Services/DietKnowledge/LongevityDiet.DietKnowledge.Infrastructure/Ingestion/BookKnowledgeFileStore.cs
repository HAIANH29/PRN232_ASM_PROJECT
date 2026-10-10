using LongevityDiet.DietKnowledge.Application.Abstractions;
using Microsoft.Extensions.Options;

namespace LongevityDiet.DietKnowledge.Infrastructure.Ingestion;

public sealed class BookKnowledgeFileStore(
    IOptions<BookKnowledgeIngestionOptions> options) : IBookKnowledgeFileStore
{
    private readonly BookKnowledgeIngestionOptions ingestionOptions = options.Value;

    public async Task<string> SaveAsync(
        Stream content,
        string storedFileName,
        CancellationToken cancellationToken = default)
    {
        var root = ResolveRoot(ingestionOptions.StoragePath);
        Directory.CreateDirectory(root);

        var safeFileName = Path.GetFileName(storedFileName);
        var path = Path.Combine(root, safeFileName);

        await using var output = new FileStream(
            path,
            FileMode.CreateNew,
            FileAccess.Write,
            FileShare.None,
            bufferSize: 81920,
            useAsync: true);
        await content.CopyToAsync(output, cancellationToken);

        return path;
    }

    private static string ResolveRoot(string configuredPath)
    {
        var path = string.IsNullOrWhiteSpace(configuredPath)
            ? "storage/book-sources"
            : configuredPath.Trim();

        return Path.IsPathRooted(path)
            ? path
            : Path.Combine(AppContext.BaseDirectory, path);
    }
}
