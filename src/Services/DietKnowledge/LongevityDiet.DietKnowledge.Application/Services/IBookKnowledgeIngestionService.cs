using LongevityDiet.DietKnowledge.Application.Models;

namespace LongevityDiet.DietKnowledge.Application.Services;

public interface IBookKnowledgeIngestionService
{
    Task<PagedResult<BookSourceDocumentModel>> ListDocumentsAsync(
        BookSourceDocumentQuery query,
        CancellationToken cancellationToken = default);

    Task<BookSourceDocumentModel> GetDocumentAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<BookSourceChunkModel>> ListChunksAsync(
        Guid documentId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<KnowledgeCandidateModel>> ListCandidatesAsync(
        Guid documentId,
        CancellationToken cancellationToken = default);

    Task<BookSourceDocumentModel> UploadAndChunkAsync(
        UploadBookSourceCommand command,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<KnowledgeCandidateModel>> GenerateCandidatesAsync(
        GenerateKnowledgeCandidatesCommand command,
        CancellationToken cancellationToken = default);

    Task<KnowledgeCandidateModel> ApproveCandidateAsync(
        ApproveKnowledgeCandidateCommand command,
        CancellationToken cancellationToken = default);

    Task<KnowledgeCandidateModel> RejectCandidateAsync(
        RejectKnowledgeCandidateCommand command,
        CancellationToken cancellationToken = default);
}
