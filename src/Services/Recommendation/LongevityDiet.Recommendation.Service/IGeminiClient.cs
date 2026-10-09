namespace LongevityDiet.Recommendation.Service;

public interface IGeminiClient
{
    bool IsEnabled { get; }

    Task<string?> GenerateTextAsync(string prompt, CancellationToken cancellationToken = default);
}
