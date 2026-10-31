using System.Collections.Concurrent;
using LongevityDiet.DietKnowledge.Application.Abstractions;
using LongevityDiet.DietKnowledge.Application.Models;

namespace LongevityDiet.DietKnowledge.Application.Services;

// In-memory and per process: it only needs to outlive one request so the page can poll it.
public sealed class GenerationProgressTracker : IGenerationProgressTracker
{
    private readonly ConcurrentDictionary<Guid, GenerationRunProgressModel> runs = new();

    public void Start(Guid documentId)
    {
        runs[documentId] = new GenerationRunProgressModel(true, 0, 0);
    }

    public void Report(Guid documentId, int processedChunks, int totalChunks)
    {
        runs[documentId] = new GenerationRunProgressModel(true, processedChunks, totalChunks);
    }

    public void Finish(Guid documentId)
    {
        runs.TryRemove(documentId, out _);
    }

    public GenerationRunProgressModel Get(Guid documentId)
    {
        return runs.TryGetValue(documentId, out var run) ? run : new GenerationRunProgressModel(false, 0, 0);
    }
}
