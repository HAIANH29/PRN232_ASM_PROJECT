using LongevityDiet.DietKnowledge.Application.Models;

namespace LongevityDiet.DietKnowledge.Application.Abstractions;

// Live progress of the generation run a document is currently going through (one admin click).
public interface IGenerationProgressTracker
{
    void Start(Guid documentId);

    void Report(Guid documentId, int processedChunks, int totalChunks);

    void Finish(Guid documentId);

    GenerationRunProgressModel Get(Guid documentId);
}
