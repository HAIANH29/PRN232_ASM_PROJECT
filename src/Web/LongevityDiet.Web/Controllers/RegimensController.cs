using LongevityDiet.Web.Models;
using LongevityDiet.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LongevityDiet.Web.Controllers;

// Members browse the diet regimens (for example a multi-day menu) taken from approved book content.
public sealed class RegimensController(ApiGatewayClient api, UserSession session)
    : AppController(api, session)
{
    [HttpGet]
    public async Task<IActionResult> Index(
        string? search,
        int page = 1,
        CancellationToken cancellationToken = default)
    {
        if (Session.IsAdmin)
        {
            return RedirectToAction("Index", "AdminRegimens");
        }

        var path = ApiGatewayClient.BuildQuery(
            "/diet-knowledge/api/diet-regimens",
            ("pageNumber", Math.Max(1, page)),
            ("pageSize", 12),
            ("search", search));
        var result = await Api.GetAsync<PagedResponse<DietRegimenResponse>>(
            path,
            cancellationToken: cancellationToken);

        return View(new RegimensIndexViewModel
        {
            Search = search,
            Regimens = result.Data ?? KnowledgeIndexViewModel.Empty<DietRegimenResponse>()
        });
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        if (Session.IsAdmin)
        {
            return RedirectToAction("Index", "AdminRegimens");
        }

        var result = await Api.GetAsync<DietRegimenResponse>(
            $"/diet-knowledge/api/diet-regimens/{id}",
            cancellationToken: cancellationToken);
        if (!result.Succeeded || result.Data is null)
        {
            TempData["Error"] = FormatErrors(result.Message, result.Errors);
            return RedirectToAction(nameof(Index));
        }

        var model = new RegimenDetailsViewModel
        {
            Regimen = result.Data,
            StartForm = new StartDietTrackingForm
            {
                DietRegimenId = id,
                StartDate = DateOnly.FromDateTime(DateTime.Today),
                EndDate = DateOnly.FromDateTime(DateTime.Today).AddDays(Math.Max(1, result.Data.DurationDays) - 1)
            }
        };

        if (Session.IsSignedIn)
        {
            model.CanStartTracking = true;
            var active = await Api.GetAsync<PagedResponse<UserDietTrackingResponse>>(
                "/tracking/api/diet-trackings?status=Active&pageSize=1",
                Token,
                cancellationToken);
            model.ActiveTracking = active.Data?.Items.FirstOrDefault();
            model.MealPlans = (await LoadMealPlansAsync(cancellationToken)).Items;
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> StartTracking(
        [Bind(Prefix = "StartForm")] StartDietTrackingForm form,
        CancellationToken cancellationToken)
    {
        var guard = RequireMember();
        if (guard is not null)
        {
            return guard;
        }

        var result = await Api.PostAsync<UserDietTrackingResponse>(
            "/tracking/api/diet-trackings",
            new { form.DietRegimenId, form.MealPlanId, form.StartDate, form.EndDate },
            Token,
            cancellationToken);
        AddResultMessage(result, "Diet tracking started.");

        return result.Succeeded
            ? RedirectToAction("Index", "Tracking")
            : form.DietRegimenId.HasValue
                ? RedirectToAction(nameof(Details), new { id = form.DietRegimenId })
                : RedirectToAction("Index", "Tracking");
    }
}
