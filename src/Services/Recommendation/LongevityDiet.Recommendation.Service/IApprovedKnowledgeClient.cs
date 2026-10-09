namespace LongevityDiet.Recommendation.Service;

public interface IApprovedKnowledgeClient
{
    Task<ApprovedKnowledgeContext> GetApprovedKnowledgeAsync(CancellationToken cancellationToken = default);
}
