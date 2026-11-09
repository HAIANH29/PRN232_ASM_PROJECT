using LongevityDiet.Web.Models;
using LongevityDiet.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LongevityDiet.Web.Controllers;

// Admin workspace for diet regimens. The API replaces guideline links and meals as a whole, so
// adding or removing one entry loads the regimen, changes the list and saves it again.
public sealed class AdminRegimensController(ApiGatewayClient api, UserSession session)
    : AppController(api, session)
{
    [HttpGet]
    public async Task<IActionResult> Index(int page = 1, CancellationToken cancellationToken = default)
    {
        var guard = RequireAdmin();
        if (guard is not null)
        {
            return guard;
        }

        return View(new AdminRegimensIndexViewModel
        {
            Regimens = await LoadRegimensPageAsync(page, cancellationToken)
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind(Prefix = "Form")] RegimenForm form,
        CancellationToken cancellationToken)
    {
        var guard = RequireAdmin();
        if (guard is not null)
        {
            return guard;
        }

        if (!ModelState.IsValid)
        {
            return View("Index", new AdminRegimensIndexViewModel
            {
                Regimens = await LoadRegimensPageAsync(1, cancellationToken),
                Form = form
            });
        }

        var result = await Api.PostAsync<DietRegimenResponse>(
            "/diet-knowledge/api/admin/diet-regimens",
            ToBody(form, [], []),
            Token,
            cancellationToken);
        AddResultMessage(result, "Diet regimen created.");

        return result.Succeeded && result.Data is not null
            ? RedirectToAction(nameof(Edit), new { id = result.Data.Id })
            : RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        var guard = RequireAdmin();
        if (guard is not null)
        {
            return guard;
        }

        var model = await BuildEditModelAsync(id, cancellationToken);
        return model is null ? RedirectToAction(nameof(Index)) : View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        Guid id,
        [Bind(Prefix = "Form")] RegimenForm form,
        CancellationToken cancellationToken)
    {
        var guard = RequireAdmin();
        if (guard is not null)
        {
            return guard;
        }

        if (!ModelState.IsValid)
        {
            var model = await BuildEditModelAsync(id, cancellationToken);
            if (model is null)
            {
                return RedirectToAction(nameof(Index));
            }

            model.Form = form;
            return View(model);
        }

        var current = await LoadAsync(id, cancellationToken);
        if (current is null)
        {
            return RedirectToAction(nameof(Index));
        }

        return await SaveAsync(id, form, GuidelinesOf(current), MealsOf(current), "Diet regimen updated.", cancellationToken);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddGuideline(
        Guid id,
        [Bind(Prefix = "GuidelineForm")] RegimenGuidelineForm form,
        CancellationToken cancellationToken)
    {
        var guard = RequireAdmin();
        if (guard is not null)
        {
            return guard;
        }

        var current = await LoadAsync(id, cancellationToken);
        if (current is null)
        {
            return RedirectToAction(nameof(Index));
        }

        if (!ModelState.IsValid || form.GuidelineId == Guid.Empty)
        {
            TempData["Error"] = "Choose an approved guideline.";
            return RedirectToAction(nameof(Edit), new { id });
        }

        var guidelines = GuidelinesOf(current)
            .Where(link => link.GuidelineId != form.GuidelineId)
            .Append(new RegimenGuidelineBody(form.GuidelineId, form.SortOrder, form.Note ?? string.Empty))
            .ToList();

        return await SaveAsync(id, ToForm(current), guidelines, MealsOf(current), "Guideline added to the regimen.", cancellationToken);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveGuideline(Guid id, Guid guidelineId, CancellationToken cancellationToken)
    {
        var guard = RequireAdmin();
        if (guard is not null)
        {
            return guard;
        }

        var current = await LoadAsync(id, cancellationToken);
        if (current is null)
        {
            return RedirectToAction(nameof(Index));
        }

        var guidelines = GuidelinesOf(current).Where(link => link.GuidelineId != guidelineId).ToList();
        return await SaveAsync(id, ToForm(current), guidelines, MealsOf(current), "Guideline removed from the regimen.", cancellationToken);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddMeal(
        Guid id,
        [Bind(Prefix = "MealForm")] RegimenMealForm form,
        CancellationToken cancellationToken)
    {
        var guard = RequireAdmin();
        if (guard is not null)
        {
            return guard;
        }

        var current = await LoadAsync(id, cancellationToken);
        if (current is null)
        {
            return RedirectToAction(nameof(Index));
        }

        if (!ModelState.IsValid || form.RecipeId == Guid.Empty)
        {
            TempData["Error"] = "Choose an approved recipe.";
            return RedirectToAction(nameof(Edit), new { id });
        }

        var meals = MealsOf(current)
            .Append(new RegimenMealBody(form.DayNumber, form.MealSlot, form.RecipeId, form.Notes ?? string.Empty))
            .ToList();

        return await SaveAsync(id, ToForm(current), GuidelinesOf(current), meals, "Meal added to the regimen.", cancellationToken);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveMeal(Guid id, Guid mealId, CancellationToken cancellationToken)
    {
        var guard = RequireAdmin();
        if (guard is not null)
        {
            return guard;
        }

        var current = await LoadAsync(id, cancellationToken);
        if (current is null)
        {
            return RedirectToAction(nameof(Index));
        }

        var meals = current.Meals
            .Where(meal => meal.Id != mealId)
            .Select(meal => new RegimenMealBody(meal.DayNumber, meal.MealSlot, meal.RecipeId, meal.Notes))
            .ToList();

        return await SaveAsync(id, ToForm(current), GuidelinesOf(current), meals, "Meal removed from the regimen.", cancellationToken);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetActivation(Guid id, bool isActive, CancellationToken cancellationToken)
    {
        var guard = RequireAdmin();
        if (guard is not null)
        {
            return guard;
        }

        var result = await Api.PatchAsync<DietRegimenResponse>(
            $"/diet-knowledge/api/admin/diet-regimens/{id}/activation",
            new { isActive },
            Token,
            cancellationToken);
        AddResultMessage(result, isActive ? "Diet regimen activated." : "Diet regimen deactivated.");
        return RedirectToAction(nameof(Index));
    }

    private async Task<IActionResult> SaveAsync(
        Guid id,
        RegimenForm form,
        IReadOnlyCollection<RegimenGuidelineBody> guidelines,
        IReadOnlyCollection<RegimenMealBody> meals,
        string successMessage,
        CancellationToken cancellationToken)
    {
        var result = await Api.PutAsync<DietRegimenResponse>(
            $"/diet-knowledge/api/admin/diet-regimens/{id}",
            ToBody(form, guidelines, meals),
            Token,
            cancellationToken);
        AddResultMessage(result, successMessage);
        return RedirectToAction(nameof(Edit), new { id });
    }

    private async Task<DietRegimenResponse?> LoadAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await Api.GetAsync<DietRegimenResponse>(
            $"/diet-knowledge/api/admin/diet-regimens/{id}",
            Token,
            cancellationToken);
        if (!result.Succeeded || result.Data is null)
        {
            TempData["Error"] = FormatErrors(result.Message, result.Errors);
            return null;
        }

        return result.Data;
    }

    private async Task<AdminRegimenEditViewModel?> BuildEditModelAsync(Guid id, CancellationToken cancellationToken)
    {
        var regimen = await LoadAsync(id, cancellationToken);
        if (regimen is null)
        {
            return null;
        }

        var guidelines = await Api.GetAsync<PagedResponse<DietGuidelineResponse>>(
            "/diet-knowledge/api/admin/diet-guidelines?pageNumber=1&pageSize=100&reviewStatus=Approved",
            Token,
            cancellationToken);
        var recipes = await Api.GetAsync<PagedResponse<RecipeResponse>>(
            "/diet-knowledge/api/admin/recipes?pageNumber=1&pageSize=100&reviewStatus=Approved",
            Token,
            cancellationToken);

        return new AdminRegimenEditViewModel
        {
            Regimen = regimen,
            Form = ToForm(regimen),
            Guidelines = guidelines.Data?.Items ?? Array.Empty<DietGuidelineResponse>(),
            Recipes = recipes.Data?.Items ?? Array.Empty<RecipeResponse>(),
            GuidelineForm = new RegimenGuidelineForm { SortOrder = regimen.Guidelines.Count + 1 },
            MealForm = new RegimenMealForm()
        };
    }

    private async Task<PagedResponse<DietRegimenResponse>> LoadRegimensPageAsync(
        int page,
        CancellationToken cancellationToken)
    {
        var result = await Api.GetAsync<PagedResponse<DietRegimenResponse>>(
            $"/diet-knowledge/api/admin/diet-regimens?pageNumber={Math.Max(1, page)}&pageSize=25&includeInactive=true",
            Token,
            cancellationToken);
        return result.Data ?? KnowledgeIndexViewModel.Empty<DietRegimenResponse>();
    }

    private static RegimenForm ToForm(DietRegimenResponse regimen) => new()
    {
        Id = regimen.Id,
        Name = regimen.Name,
        Description = regimen.Description,
        DurationDays = regimen.DurationDays,
        SourceTitle = regimen.SourceTitle,
        SourceChapter = regimen.SourceChapter,
        SourcePage = regimen.SourcePage,
        ReviewStatus = regimen.ReviewStatus
    };

    private static List<RegimenGuidelineBody> GuidelinesOf(DietRegimenResponse regimen) =>
        regimen.Guidelines
            .Select(link => new RegimenGuidelineBody(link.GuidelineId, link.SortOrder, link.Note))
            .ToList();

    private static List<RegimenMealBody> MealsOf(DietRegimenResponse regimen) =>
        regimen.Meals
            .Select(meal => new RegimenMealBody(meal.DayNumber, meal.MealSlot, meal.RecipeId, meal.Notes))
            .ToList();

    private static object ToBody(
        RegimenForm form,
        IReadOnlyCollection<RegimenGuidelineBody> guidelines,
        IReadOnlyCollection<RegimenMealBody> meals) => new
    {
        form.Name,
        form.Description,
        form.DurationDays,
        SourceTitle = string.IsNullOrWhiteSpace(form.SourceTitle) ? "The Longevity Diet" : form.SourceTitle,
        SourceChapter = form.SourceChapter ?? string.Empty,
        SourcePage = form.SourcePage ?? string.Empty,
        form.ReviewStatus,
        guidelines,
        meals
    };

    private sealed record RegimenGuidelineBody(Guid GuidelineId, int SortOrder, string Note);

    private sealed record RegimenMealBody(int DayNumber, string MealSlot, Guid RecipeId, string Notes);
}
