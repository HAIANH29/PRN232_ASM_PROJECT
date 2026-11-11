using System.Net;
using LongevityDiet.Web.Models;
using LongevityDiet.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LongevityDiet.Web.Controllers;

public sealed class TrackingController(ApiGatewayClient api, UserSession session)
    : AppController(api, session)
{
    [HttpGet]
    public async Task<IActionResult> Index(
        DateOnly? trackingDate,
        DateOnly? periodStartDate,
        DateOnly? periodEndDate,
        CancellationToken cancellationToken)
    {
        var guard = RequireMember();
        if (guard is not null)
        {
            return guard;
        }

        var date = trackingDate ?? DateOnly.FromDateTime(DateTime.Today);
        // Default progress window: the calendar week (Monday-Sunday) of the selected day.
        var weekStart = date.AddDays(-(((int)date.DayOfWeek + 6) % 7));
        var start = periodStartDate ?? weekStart;
        var end = periodEndDate ?? weekStart.AddDays(6);

        return View(await BuildModelAsync(date, start, end, cancellationToken));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveDaily(
        [Bind(Prefix = "Form")] TrackingForm form,
        CancellationToken cancellationToken)
    {
        var guard = RequireMember();
        if (guard is not null)
        {
            return guard;
        }

        var result = await Api.PutAsync<DailyTrackingResponse>(
            $"/tracking/api/daily-trackings/{form.TrackingDate:yyyy-MM-dd}",
            new { form.Notes },
            Token,
            cancellationToken);
        AddResultMessage(result, "Daily tracking saved.");
        return RedirectToAction(nameof(Index), new
        {
            trackingDate = form.TrackingDate,
            periodStartDate = form.PeriodStartDate,
            periodEndDate = form.PeriodEndDate
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetMeal(
        [Bind(Prefix = "Form")] TrackingForm form,
        CancellationToken cancellationToken)
    {
        var guard = RequireMember();
        if (guard is not null)
        {
            return guard;
        }

        if (!form.MealPlanItemId.HasValue || form.MealPlanItemId.Value == Guid.Empty)
        {
            TempData["Error"] = "Choose a meal plan item to update.";
            return RedirectToAction(nameof(Index), new { trackingDate = form.TrackingDate });
        }

        var result = await Api.PutAsync<DailyTrackingResponse>(
            $"/tracking/api/daily-trackings/{form.TrackingDate:yyyy-MM-dd}/meals/{form.MealPlanItemId}",
            new { form.IsCompleted, form.PlannedMealCount },
            Token,
            cancellationToken);
        AddResultMessage(result, form.IsCompleted ? "Meal marked completed." : "Meal marked not completed.");
        return RedirectToAction(nameof(Index), new
        {
            trackingDate = form.TrackingDate,
            periodStartDate = form.PeriodStartDate,
            periodEndDate = form.PeriodEndDate
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> StartDietTracking(
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
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> FinishDietTracking(
        Guid id,
        string status,
        CancellationToken cancellationToken)
    {
        var guard = RequireMember();
        if (guard is not null)
        {
            return guard;
        }

        var result = await Api.PatchAsync<UserDietTrackingResponse>(
            $"/tracking/api/diet-trackings/{id}/status",
            new { Status = status },
            Token,
            cancellationToken);
        AddResultMessage(
            result,
            string.Equals(status, "Completed", StringComparison.OrdinalIgnoreCase)
                ? "Diet tracking completed."
                : "Diet tracking cancelled.");
        return RedirectToAction(nameof(Index));
    }

    private async Task<TrackingIndexViewModel> BuildModelAsync(
        DateOnly trackingDate,
        DateOnly periodStart,
        DateOnly periodEnd,
        CancellationToken cancellationToken)
    {
        var daily = await Api.GetAsync<DailyTrackingResponse>(
            $"/tracking/api/daily-trackings/{trackingDate:yyyy-MM-dd}",
            Token,
            cancellationToken);
        var summary = await Api.GetAsync<ProgressSummaryResponse>(
            ApiGatewayClient.BuildQuery(
                "/tracking/api/progress-summaries",
                ("periodStartDate", periodStart),
                ("periodEndDate", periodEnd)),
            Token,
            cancellationToken);

        var dietTrackings = (await Api.GetAsync<PagedResponse<UserDietTrackingResponse>>(
            "/tracking/api/diet-trackings?pageSize=6",
            Token,
            cancellationToken)).Data?.Items ?? Array.Empty<UserDietTrackingResponse>();
        var regimens = (await Api.GetAsync<PagedResponse<DietRegimenResponse>>(
            "/diet-knowledge/api/diet-regimens?pageSize=100",
            cancellationToken: cancellationToken)).Data?.Items ?? Array.Empty<DietRegimenResponse>();

        return new TrackingIndexViewModel
        {
            Form = new TrackingForm
            {
                TrackingDate = trackingDate,
                PeriodStartDate = periodStart,
                PeriodEndDate = periodEnd,
                Notes = daily.Succeeded ? daily.Data?.Notes : string.Empty
            },
            DailyTracking = daily.StatusCode == HttpStatusCode.NotFound ? null : daily.Data,
            ProgressSummary = summary.Data,
            MealPlans = (await LoadMealPlansAsync(cancellationToken)).Items,
            Foods = await LoadFoodsAsync(cancellationToken: cancellationToken),
            Recipes = await LoadRecipesAsync(cancellationToken: cancellationToken),
            ActiveDietTracking = dietTrackings.FirstOrDefault(item => item.Status == "Active"),
            DietTrackingHistory = dietTrackings.Where(item => item.Status != "Active").ToArray(),
            Regimens = regimens,
            StartForm = new StartDietTrackingForm { StartDate = DateOnly.FromDateTime(DateTime.Today) }
        };
    }
}
