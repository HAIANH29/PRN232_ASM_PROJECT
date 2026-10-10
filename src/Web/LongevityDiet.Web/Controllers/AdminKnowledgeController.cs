using LongevityDiet.Web.Models;
using LongevityDiet.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LongevityDiet.Web.Controllers;

public sealed class AdminKnowledgeController(ApiGatewayClient api, UserSession session)
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
    public async Task<IActionResult> CreateGuideline(
        [Bind(Prefix = "Guideline")] GuidelineForm form,
        CancellationToken cancellationToken)
    {
        var guard = RequireAdmin();
        if (guard is not null)
        {
            return guard;
        }

        if (!ModelState.IsValid)
        {
            var model = await BuildIndexModelAsync(cancellationToken);
            model.Guideline = form;
            return View("Index", model);
        }

        var result = await Api.PostAsync<DietGuidelineResponse>(
            "/diet-knowledge/api/admin/diet-guidelines",
            ToGuidelineBody(form),
            Token,
            cancellationToken);
        AddResultMessage(result, "Guideline saved.");
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateFood(
        [Bind(Prefix = "Food")] FoodForm form,
        CancellationToken cancellationToken)
    {
        var guard = RequireAdmin();
        if (guard is not null)
        {
            return guard;
        }

        if (!ModelState.IsValid)
        {
            var model = await BuildIndexModelAsync(cancellationToken);
            model.Food = form;
            return View("Index", model);
        }

        var result = await Api.PostAsync<FoodResponse>(
            "/diet-knowledge/api/admin/foods",
            ToFoodBody(form),
            Token,
            cancellationToken);
        AddResultMessage(result, "Food saved.");
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateRecipe(
        [Bind(Prefix = "Recipe")] RecipeForm form,
        CancellationToken cancellationToken)
    {
        var guard = RequireAdmin();
        if (guard is not null)
        {
            return guard;
        }

        if (form.IngredientFoodId == Guid.Empty)
        {
            ModelState.AddModelError(nameof(form.IngredientFoodId), "Choose an approved food ingredient.");
        }

        if (!ModelState.IsValid)
        {
            var model = await BuildIndexModelAsync(cancellationToken);
            model.Recipe = form;
            return View("Index", model);
        }

        var result = await Api.PostAsync<RecipeResponse>(
            "/diet-knowledge/api/admin/recipes",
            ToRecipeBody(form),
            Token,
            cancellationToken);
        AddResultMessage(result, "Recipe saved.");
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> EditGuideline(Guid id, CancellationToken cancellationToken)
    {
        var guard = RequireAdmin();
        if (guard is not null)
        {
            return guard;
        }

        var result = await Api.GetAsync<DietGuidelineResponse>(
            $"/diet-knowledge/api/admin/diet-guidelines/{id}",
            Token,
            cancellationToken);
        if (!result.Succeeded || result.Data is null)
        {
            TempData["Error"] = FormatErrors(result.Message, result.Errors);
            return RedirectToAction(nameof(Index));
        }

        return View(MapGuideline(result.Data));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditGuideline(Guid id, GuidelineForm form, CancellationToken cancellationToken)
    {
        var guard = RequireAdmin();
        if (guard is not null)
        {
            return guard;
        }

        if (!ModelState.IsValid)
        {
            return View(form);
        }

        var result = await Api.PutAsync<DietGuidelineResponse>(
            $"/diet-knowledge/api/admin/diet-guidelines/{id}",
            ToGuidelineBody(form),
            Token,
            cancellationToken);
        AddResultMessage(result, "Guideline updated.");
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> EditFood(Guid id, CancellationToken cancellationToken)
    {
        var guard = RequireAdmin();
        if (guard is not null)
        {
            return guard;
        }

        var result = await Api.GetAsync<FoodResponse>(
            $"/diet-knowledge/api/admin/foods/{id}",
            Token,
            cancellationToken);
        if (!result.Succeeded || result.Data is null)
        {
            TempData["Error"] = FormatErrors(result.Message, result.Errors);
            return RedirectToAction(nameof(Index));
        }

        return View(MapFood(result.Data));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditFood(Guid id, FoodForm form, CancellationToken cancellationToken)
    {
        var guard = RequireAdmin();
        if (guard is not null)
        {
            return guard;
        }

        if (!ModelState.IsValid)
        {
            return View(form);
        }

        var result = await Api.PutAsync<FoodResponse>(
            $"/diet-knowledge/api/admin/foods/{id}",
            ToFoodBody(form),
            Token,
            cancellationToken);
        AddResultMessage(result, "Food updated.");
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> EditRecipe(Guid id, CancellationToken cancellationToken)
    {
        var guard = RequireAdmin();
        if (guard is not null)
        {
            return guard;
        }

        ViewBag.Foods = await LoadFoodsAsync(admin: true, cancellationToken);
        var result = await Api.GetAsync<RecipeResponse>(
            $"/diet-knowledge/api/admin/recipes/{id}",
            Token,
            cancellationToken);
        if (!result.Succeeded || result.Data is null)
        {
            TempData["Error"] = FormatErrors(result.Message, result.Errors);
            return RedirectToAction(nameof(Index));
        }

        return View(MapRecipe(result.Data));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditRecipe(Guid id, RecipeForm form, CancellationToken cancellationToken)
    {
        var guard = RequireAdmin();
        if (guard is not null)
        {
            return guard;
        }

        if (form.IngredientFoodId == Guid.Empty)
        {
            ModelState.AddModelError(nameof(form.IngredientFoodId), "Choose an approved food ingredient.");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Foods = await LoadFoodsAsync(admin: true, cancellationToken);
            return View(form);
        }

        var result = await Api.PutAsync<RecipeResponse>(
            $"/diet-knowledge/api/admin/recipes/{id}",
            ToRecipeBody(form),
            Token,
            cancellationToken);
        AddResultMessage(result, "Recipe updated.");
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetActivation(string type, Guid id, bool isActive, CancellationToken cancellationToken)
    {
        var guard = RequireAdmin();
        if (guard is not null)
        {
            return guard;
        }

        var path = type switch
        {
            "guideline" => $"/diet-knowledge/api/admin/diet-guidelines/{id}/activation",
            "food" => $"/diet-knowledge/api/admin/foods/{id}/activation",
            "recipe" => $"/diet-knowledge/api/admin/recipes/{id}/activation",
            _ => string.Empty
        };

        if (string.IsNullOrWhiteSpace(path))
        {
            TempData["Error"] = "Unknown content type.";
            return RedirectToAction(nameof(Index));
        }

        var result = await Api.PatchAsync<object>(
            path,
            new { isActive },
            Token,
            cancellationToken);
        AddResultMessage(result, isActive ? "Content activated." : "Content deactivated.");
        return RedirectToAction(nameof(Index));
    }

    private async Task<AdminKnowledgeViewModel> BuildIndexModelAsync(CancellationToken cancellationToken)
    {
        var guidelines = await Api.GetAsync<PagedResponse<DietGuidelineResponse>>(
            "/diet-knowledge/api/admin/diet-guidelines?pageNumber=1&pageSize=25&includeInactive=true",
            Token,
            cancellationToken);
        var foods = await Api.GetAsync<PagedResponse<FoodResponse>>(
            "/diet-knowledge/api/admin/foods?pageNumber=1&pageSize=25&includeInactive=true",
            Token,
            cancellationToken);
        var recipes = await Api.GetAsync<PagedResponse<RecipeResponse>>(
            "/diet-knowledge/api/admin/recipes?pageNumber=1&pageSize=25&includeInactive=true",
            Token,
            cancellationToken);

        return new AdminKnowledgeViewModel
        {
            Guidelines = guidelines.Data ?? KnowledgeIndexViewModel.Empty<DietGuidelineResponse>(),
            Foods = foods.Data ?? KnowledgeIndexViewModel.Empty<FoodResponse>(),
            Recipes = recipes.Data ?? KnowledgeIndexViewModel.Empty<RecipeResponse>()
        };
    }

    private static object ToGuidelineBody(GuidelineForm form) => new
    {
        form.Title,
        form.Summary,
        form.SourceNote,
        form.SourceTitle,
        form.SourceChapter,
        form.SourcePage,
        form.SourceReference,
        form.ReviewStatus,
        form.ReviewedBy
    };

    private static object ToFoodBody(FoodForm form) => new
    {
        form.Name,
        form.Category,
        form.CompatibilityNotes,
        form.SourceTitle,
        form.SourceChapter,
        form.SourcePage,
        form.SourceReference,
        form.ReviewStatus,
        form.ReviewedBy
    };

    private static object ToRecipeBody(RecipeForm form) => new
    {
        form.Name,
        form.Description,
        form.SourceTitle,
        form.SourceChapter,
        form.SourcePage,
        form.SourceReference,
        form.ReviewStatus,
        form.ReviewedBy,
        ingredients = new[]
        {
            new RecipeIngredientRequest(
                form.IngredientFoodId,
                form.IngredientQuantityText)
        }
    };

    private static GuidelineForm MapGuideline(DietGuidelineResponse item) => new()
    {
        Id = item.Id,
        Title = item.Title,
        Summary = item.Summary,
        SourceNote = item.SourceNote,
        SourceTitle = item.SourceTitle,
        SourceChapter = item.SourceChapter,
        SourcePage = item.SourcePage,
        SourceReference = item.SourceReference,
        ReviewStatus = item.ReviewStatus,
        ReviewedBy = item.ReviewedBy
    };

    private static FoodForm MapFood(FoodResponse item) => new()
    {
        Id = item.Id,
        Name = item.Name,
        Category = item.Category,
        CompatibilityNotes = item.CompatibilityNotes,
        SourceTitle = item.SourceTitle,
        SourceChapter = item.SourceChapter,
        SourcePage = item.SourcePage,
        SourceReference = item.SourceReference,
        ReviewStatus = item.ReviewStatus,
        ReviewedBy = item.ReviewedBy
    };

    private static RecipeForm MapRecipe(RecipeResponse item)
    {
        var ingredient = item.Ingredients.FirstOrDefault();
        return new RecipeForm
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            SourceTitle = item.SourceTitle,
            SourceChapter = item.SourceChapter,
            SourcePage = item.SourcePage,
            SourceReference = item.SourceReference,
            ReviewStatus = item.ReviewStatus,
            ReviewedBy = item.ReviewedBy,
            IngredientFoodId = ingredient?.FoodId ?? Guid.Empty,
            IngredientQuantityText = ingredient?.QuantityText ?? "1 serving"
        };
    }
}
