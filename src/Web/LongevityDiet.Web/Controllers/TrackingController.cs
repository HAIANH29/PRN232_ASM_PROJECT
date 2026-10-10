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
        var guard = RequireUser();
        if (guard is not null)
        {
            return guard;
        }

        var date = trackingDate ?? DateOnly.FromDateTime(DateTime.Today);
        var start = periodStartDate ?? date;
        var end = periodEndDate ?? date;

        return View(await BuildModelAsync(date, start, end, cancellationToken));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveDaily(
        [Bind(Prefix = "Form")] TrackingForm form,
        CancellationToken cancellationToken)
    {
        var guard = RequireUser();
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
        var guard = RequireUser();
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
            new { form.IsCompleted },
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
            Recipes = await LoadRecipesAsync(cancellationToken: cancellationToken)
        };
    }
}
