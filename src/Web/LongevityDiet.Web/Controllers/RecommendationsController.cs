using LongevityDiet.Web.Models;
using LongevityDiet.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LongevityDiet.Web.Controllers;

public sealed class RecommendationsController(ApiGatewayClient api, UserSession session)
    : AppController(api, session)
{
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var guard = RequireUser();
        if (guard is not null)
        {
            return guard;
        }

        var requests = await Api.GetAsync<PagedResponse<RecommendationRequestResponse>>(
            "/meal-planning/api/meal-recommendations?pageNumber=1&pageSize=20",
            Token,
            cancellationToken);

        return View(new RecommendationsIndexViewModel
        {
            Requests = requests.Data ?? KnowledgeIndexViewModel.Empty<RecommendationRequestResponse>()
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind(Prefix = "Form")] RecommendationForm form,
        CancellationToken cancellationToken)
    {
        var guard = RequireUser();
        if (guard is not null)
        {
            return guard;
        }

        if (!ModelState.IsValid)
        {
            return View("Index", new RecommendationsIndexViewModel
            {
                Form = form,
                Requests = KnowledgeIndexViewModel.Empty<RecommendationRequestResponse>()
            });
        }

        var result = await Api.PostAsync<RecommendationRequestResponse>(
            "/meal-planning/api/meal-recommendations",
            new
            {
                preferenceTags = SplitTags(form.PreferenceTagsText),
                form.Days
            },
            Token,
            cancellationToken);
        AddResultMessage(result, "Recommendation request sent.");

        return result.Succeeded && result.Data is not null
            ? RedirectToAction(nameof(Details), new { id = result.Data.Id })
            : RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        var guard = RequireUser();
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
    public async Task<IActionResult> Accept(
        Guid id,
        [Bind(Prefix = "AcceptForm")] AcceptRecommendationForm form,
        CancellationToken cancellationToken)
    {
        var guard = RequireUser();
        if (guard is not null)
        {
            return guard;
        }

        var hasFood = form.FoodId.HasValue && form.FoodId.Value != Guid.Empty;
        var hasRecipe = form.RecipeId.HasValue && form.RecipeId.Value != Guid.Empty;
        if (hasFood == hasRecipe)
        {
            ModelState.AddModelError(nameof(form.FoodId), "Choose exactly one approved food or recipe.");
        }

        if (!ModelState.IsValid)
        {
            var model = await BuildDetailsModelAsync(id, cancellationToken);
            if (model is null)
            {
                return RedirectToAction(nameof(Index));
            }

            model.AcceptForm = form;
            return View("Details", model);
        }

        var result = await Api.PostAsync<MealPlanResponse>(
            $"/meal-planning/api/meal-recommendations/{id}/accept",
            new
            {
                form.Name,
                form.StartDate,
                form.EndDate,
                items = new[]
                {
                    new
                    {
                        form.PlannedDate,
                        form.MealSlot,
                        recipeId = form.RecipeId == Guid.Empty ? null : form.RecipeId,
                        foodId = form.FoodId == Guid.Empty ? null : form.FoodId,
                        form.Notes,
                        reminderAtUtc = (DateTimeOffset?)null
                    }
                }
            },
            Token,
            cancellationToken);
        AddResultMessage(result, "Recommendation accepted as a meal plan.");

        return result.Succeeded && result.Data is not null
            ? RedirectToAction("Details", "MealPlans", new { id = result.Data.Id })
            : RedirectToAction(nameof(Details), new { id });
    }

    private async Task<RecommendationDetailsViewModel?> BuildDetailsModelAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await Api.GetAsync<RecommendationRequestResponse>(
            $"/meal-planning/api/meal-recommendations/{id}",
            Token,
            cancellationToken);
        if (!result.Succeeded || result.Data is null)
        {
            TempData["Error"] = FormatErrors(result.Message, result.Errors);
            return null;
        }

        return new RecommendationDetailsViewModel
        {
            Request = result.Data,
            AcceptForm = new AcceptRecommendationForm
            {
                Name = $"Accepted recommendation {DateTime.Today:yyyy-MM-dd}",
                StartDate = DateOnly.FromDateTime(DateTime.Today),
                EndDate = DateOnly.FromDateTime(DateTime.Today.AddDays(Math.Max(1, result.Data.Days) - 1)),
                PlannedDate = DateOnly.FromDateTime(DateTime.Today),
                MealSlot = "Breakfast"
            },
            Foods = await LoadFoodsAsync(cancellationToken: cancellationToken),
            Recipes = await LoadRecipesAsync(cancellationToken: cancellationToken)
        };
    }
}
