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
        var guard = RequireMember();
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
        var guard = RequireMember();
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
                language = Translator.IsVietnamese ? "vi" : "en",
                form.Days
            },
            Token,
            cancellationToken);
        AddResultMessage(result, "Request sent. Your meal ideas will show up on this page in a few seconds.");

        return result.Succeeded && result.Data is not null
            ? RedirectToAction(nameof(Details), new { id = result.Data.Id })
            : RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        var guard = RequireMember();
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
        var guard = RequireMember();
        if (guard is not null)
        {
            return guard;
        }

        var chosen = form.Meals.Where(row => row.Include).ToList();
        if (chosen.Count == 0)
        {
            ModelState.AddModelError(string.Empty, Translator.Lookup("Tick at least one meal to add to your plan."));
        }

        var items = new List<object>();
        foreach (var row in chosen)
        {
            if (!TryParseChoice(row.Choice, out var recipeId, out var foodId))
            {
                ModelState.AddModelError(
                    string.Empty,
                    string.Format(Translator.Lookup("Pick a recipe or food from the catalog for \"{0}\", or untick it."), row.Title));
                continue;
            }

            items.Add(new
            {
                row.PlannedDate,
                row.MealSlot,
                recipeId,
                foodId,
                notes = string.IsNullOrWhiteSpace(row.Title) ? null : string.Format(Translator.Lookup("Suggested: {0}"), ContentTranslations.Translate(row.Title)),
                reminderAtUtc = (DateTimeOffset?)null
            });
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
                startDate = chosen.Min(row => row.PlannedDate),
                endDate = chosen.Max(row => row.PlannedDate),
                items
            },
            Token,
            cancellationToken);
        AddResultMessage(result, "Done! Your meal plan was created from the suggestions.");

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

        var foods = await LoadFoodsAsync(cancellationToken: cancellationToken);
        var recipes = await LoadRecipesAsync(cancellationToken: cancellationToken);
        var slots = new[] { "Breakfast", "Lunch", "Dinner" };
        var today = DateOnly.FromDateTime(DateTime.Today);

        return new RecommendationDetailsViewModel
        {
            Request = result.Data,
            AcceptForm = new AcceptRecommendationForm
            {
                Name = string.Format(Translator.Lookup("My plan from suggestions {0}"), DateTime.Today.ToString("dd MMM", Translator.FormatCulture)),
                Meals = result.Data.SuggestedMealTitles
                    .Select((title, index) => new AcceptMealRow
                    {
                        Title = title,
                        PlannedDate = today.AddDays(index),
                        MealSlot = slots[index % slots.Length],
                        Choice = MatchCatalogChoice(title, recipes, foods)
                    })
                    .ToList()
            },
            Foods = foods,
            Recipes = recipes
        };
    }

    private static bool TryParseChoice(string? choice, out Guid? recipeId, out Guid? foodId)
    {
        recipeId = null;
        foodId = null;
        var parts = (choice ?? string.Empty).Split(':', 2);
        if (parts.Length != 2 || !Guid.TryParse(parts[1], out var id) || id == Guid.Empty)
        {
            return false;
        }

        switch (parts[0])
        {
            case "recipe":
                recipeId = id;
                return true;
            case "food":
                foodId = id;
                return true;
            default:
                return false;
        }
    }

    // Suggestions are free-text titles, so pre-select the closest catalog item (English or Vietnamese name).
    private static string? MatchCatalogChoice(
        string title,
        IReadOnlyCollection<RecipeResponse> recipes,
        IReadOnlyCollection<FoodResponse> foods)
    {
        var cleaned = title.Contains('(') ? title[..title.IndexOf('(')].Trim() : title.Trim();
        var wanted = Words(cleaned);

        var best = (Choice: (string?)null, Score: 0.0);
        void Consider(string prefix, Guid id, string name)
        {
            var score = new[] { name, ContentTranslations.Translate(name) }
                .Select(candidate => Score(wanted, cleaned, candidate))
                .Max();
            // Recipes win ties over single foods because they are the more specific match.
            if (score > best.Score)
            {
                best = ($"{prefix}:{id}", score);
            }
        }

        foreach (var recipe in recipes)
        {
            Consider("recipe", recipe.Id, recipe.Name);
        }

        foreach (var food in foods)
        {
            Consider("food", food.Id, food.Name);
        }

        return best.Score >= 0.5 ? best.Choice : null;
    }

    private static double Score(HashSet<string> wanted, string cleanedTitle, string candidate)
    {
        if (candidate.Equals(cleanedTitle, StringComparison.OrdinalIgnoreCase))
        {
            return 1.0;
        }

        if (cleanedTitle.Contains(candidate, StringComparison.OrdinalIgnoreCase) ||
            candidate.Contains(cleanedTitle, StringComparison.OrdinalIgnoreCase))
        {
            return 0.9;
        }

        var other = Words(candidate);
        if (wanted.Count == 0 || other.Count == 0)
        {
            return 0;
        }

        var shared = wanted.Intersect(other).Count();
        return (double)shared / wanted.Union(other).Count();
    }

    private static HashSet<string> Words(string text) =>
        text.ToLowerInvariant()
            .Split([' ', ',', '-', '/'], StringSplitOptions.RemoveEmptyEntries)
            .Where(word => word.Length > 1)
            .ToHashSet();
}
