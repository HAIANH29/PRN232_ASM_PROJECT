using LongevityDiet.Web.Models;
using LongevityDiet.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LongevityDiet.Web.Controllers;

public sealed class AdminBookSourcesController(ApiGatewayClient api, UserSession session)
    : AppController(api, session)
{
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var guard = RequireAdmin();
        if (guard is not null)
        {
            return guard;
        }

        return View(await BuildIndexModelAsync(cancellationToken));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(
        [Bind(Prefix = "Upload")] BookSourceUploadForm form,
        CancellationToken cancellationToken)
    {
        var guard = RequireAdmin();
        if (guard is not null)
        {
            return guard;
        }

        if (form.File is null || form.File.Length == 0)
        {
            ModelState.AddModelError("Upload.File", "Choose a PDF file.");
        }
        else if (!Path.GetExtension(form.File.FileName)
            .Equals(".pdf", StringComparison.OrdinalIgnoreCase))
        {
            ModelState.AddModelError("Upload.File", "Only PDF files can be uploaded.");
        }

        if (!ModelState.IsValid)
        {
            var model = await BuildIndexModelAsync(cancellationToken);
            model.Upload = form;
            return View("Index", model);
        }

        var result = await Api.PostFileAsync<BookSourceDocumentResponse>(
            "/diet-knowledge/api/admin/book-sources",
            form.File!,
            "file",
            Token,
            cancellationToken);

        AddResultMessage(result, "Book PDF uploaded and chunked.");
        return result.Succeeded && result.Data is not null
            ? RedirectToAction(nameof(Details), new { id = result.Data.Id })
            : RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        var guard = RequireAdmin();
        if (guard is not null)
        {
            return guard;
        }

        var model = await BuildDetailsModelAsync(id, cancellationToken);
        if (model is null)
        {
            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GenerateCandidates(Guid id, CancellationToken cancellationToken)
    {
        var guard = RequireAdmin();
        if (guard is not null)
        {
            return guard;
        }

        var result = await Api.PostAsync<IReadOnlyCollection<KnowledgeCandidateResponse>>(
            $"/diet-knowledge/api/admin/book-sources/{id}/candidates/generate",
            new { },
            Token,
            cancellationToken);

        AddResultMessage(result, "Knowledge candidates generated.");
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApproveCandidate(
        Guid documentId,
        Guid candidateId,
        CancellationToken cancellationToken)
    {
        var guard = RequireAdmin();
        if (guard is not null)
        {
            return guard;
        }

        var result = await Api.PostAsync<KnowledgeCandidateResponse>(
            $"/diet-knowledge/api/admin/knowledge-candidates/{candidateId}/approve",
            new { },
            Token,
            cancellationToken);

        AddResultMessage(result, "Candidate approved into managed knowledge.");
        return RedirectToAction(nameof(Details), new { id = documentId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RejectCandidate(
        Guid documentId,
        Guid candidateId,
        CancellationToken cancellationToken)
    {
        var guard = RequireAdmin();
        if (guard is not null)
        {
            return guard;
        }

        var result = await Api.PostAsync<KnowledgeCandidateResponse>(
            $"/diet-knowledge/api/admin/knowledge-candidates/{candidateId}/reject",
            new { reason = "Rejected from admin review screen." },
            Token,
            cancellationToken);

        AddResultMessage(result, "Candidate rejected.");
        return RedirectToAction(nameof(Details), new { id = documentId });
    }

    private async Task<AdminBookSourcesViewModel> BuildIndexModelAsync(
        CancellationToken cancellationToken)
    {
        var documents = await Api.GetAsync<PagedResponse<BookSourceDocumentResponse>>(
            "/diet-knowledge/api/admin/book-sources?pageNumber=1&pageSize=50",
            Token,
            cancellationToken);

        return new AdminBookSourcesViewModel
        {
            Documents = documents.Data ?? KnowledgeIndexViewModel.Empty<BookSourceDocumentResponse>()
        };
    }

    private async Task<AdminBookSourceDetailsViewModel?> BuildDetailsModelAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var document = await Api.GetAsync<BookSourceDocumentResponse>(
            $"/diet-knowledge/api/admin/book-sources/{id}",
            Token,
            cancellationToken);
        if (!document.Succeeded || document.Data is null)
        {
            TempData["Error"] = FormatErrors(document.Message, document.Errors);
            return null;
        }

        var chunks = await Api.GetAsync<IReadOnlyCollection<BookSourceChunkResponse>>(
            $"/diet-knowledge/api/admin/book-sources/{id}/chunks",
            Token,
            cancellationToken);
        var candidates = await Api.GetAsync<IReadOnlyCollection<KnowledgeCandidateResponse>>(
            $"/diet-knowledge/api/admin/book-sources/{id}/candidates",
            Token,
            cancellationToken);

        return new AdminBookSourceDetailsViewModel
        {
            Document = document.Data,
            Chunks = chunks.Data ?? Array.Empty<BookSourceChunkResponse>(),
            Candidates = candidates.Data ?? Array.Empty<KnowledgeCandidateResponse>()
        };
    }
}
