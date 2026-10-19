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
            ModelState.AddModelError("Upload.File", Translator.Lookup("Choose a PDF file."));
        }
        else if (!Path.GetExtension(form.File.FileName)
            .Equals(".pdf", StringComparison.OrdinalIgnoreCase))
        {
            ModelState.AddModelError("Upload.File", Translator.Lookup("Only PDF files can be uploaded."));
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
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var guard = RequireAdmin();
        if (guard is not null)
        {
            return guard;
        }

        var result = await Api.DeleteAsync(
            $"/diet-knowledge/api/admin/book-sources/{id}",
            Token,
            cancellationToken);

        AddResultMessage(result, "Book source deleted. Knowledge already approved from it was kept.");
        return RedirectToAction(nameof(Index));
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
    public async Task<IActionResult> ApproveAllCandidates(Guid id, CancellationToken cancellationToken)
    {
        var guard = RequireAdmin();
        if (guard is not null)
        {
            return guard;
        }

        var result = await Api.PostAsync<ApproveAllCandidatesResponse>(
            $"/diet-knowledge/api/admin/book-sources/{id}/candidates/approve-all",
            new { },
            Token,
            cancellationToken);

        if (result.Succeeded && result.Data is not null)
        {
            var message = string.Format(
                System.Globalization.CultureInfo.CurrentCulture,
                Translator.Lookup(
                    "Approved {0} candidates. {1} left for manual review (placeholders or disease wording), {2} could not be approved."),
                result.Data.Approved,
                result.Data.LeftForManualReview,
                result.Data.Failed);
            return Respond(true, message, id);
        }

        return Respond(result, "Candidates approved.", id);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RejectPlaceholders(Guid id, CancellationToken cancellationToken)
    {
        var guard = RequireAdmin();
        if (guard is not null)
        {
            return guard;
        }

        var result = await Api.PostAsync<RejectPlaceholdersResponse>(
            $"/diet-knowledge/api/admin/book-sources/{id}/candidates/reject-placeholders",
            new { },
            Token,
            cancellationToken);

        if (result.Succeeded && result.Data is not null)
        {
            var message = string.Format(
                System.Globalization.CultureInfo.CurrentCulture,
                Translator.Lookup("Rejected {0} placeholder candidates."),
                result.Data.Rejected);
            return Respond(true, message, id);
        }

        return Respond(result, "Placeholder candidates rejected.", id);
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

        return Respond(result, "Candidate approved into managed knowledge.", documentId, result.Data?.Status);
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

        return Respond(result, "Candidate rejected.", documentId, result.Data?.Status);
    }

    // Review buttons are submitted with fetch (X-Requested-With: fetch) and get JSON back so the
    // page does not reload; without that header the classic redirect with a flash message is used.
    private bool IsFetchRequest => Request.Headers["X-Requested-With"] == "fetch";

    private IActionResult Respond<T>(
        ApiCallResult<T> result,
        string successMessage,
        Guid documentId,
        string? status = null)
    {
        var message = result.Succeeded
            ? Translator.Lookup(successMessage)
            : FormatErrors(result.Message, result.Errors);
        return Respond(result.Succeeded, message, documentId, status);
    }

    private IActionResult Respond(bool succeeded, string message, Guid documentId, string? status = null)
    {
        if (IsFetchRequest)
        {
            return Json(new
            {
                succeeded,
                message,
                status,
                statusLabel = status is null ? null : Translator.Lookup(status)
            });
        }

        TempData[succeeded ? "Success" : "Error"] = message;
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

        var progress = await Api.GetAsync<KnowledgeGenerationProgressResponse>(
            $"/diet-knowledge/api/admin/book-sources/{id}/candidates/progress",
            Token,
            cancellationToken);

        return new AdminBookSourceDetailsViewModel
        {
            Document = document.Data,
            Chunks = chunks.Data ?? Array.Empty<BookSourceChunkResponse>(),
            Candidates = candidates.Data ?? Array.Empty<KnowledgeCandidateResponse>(),
            Progress = progress.Data ?? new KnowledgeGenerationProgressResponse(0, 0, 0, 0, 0)
        };
    }
}
