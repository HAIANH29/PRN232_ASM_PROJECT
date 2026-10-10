namespace LongevityDiet.DietKnowledge.Application.Abstractions;

public interface IBookKnowledgeFileStore
{
    Task<string> SaveAsync(
        Stream content,
        string storedFileName,
        CancellationToken cancellationToken = default);
}
