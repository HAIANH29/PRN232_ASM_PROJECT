using LongevityDiet.Web.Models;
using LongevityDiet.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LongevityDiet.Web.Controllers;

public sealed class AdminKnowledgeController(ApiGatewayClient api, UserSession session)
    : AppController(api, session)
{
    [HttpGet]
    public async Task<IActionResult> Index(
        string? status,
        int guidelinePage = 1,
        int foodPage = 1,
        int recipePage = 1,
        CancellationToken cancellationToken = default)
    {
        var guard = RequireAdmin();
        if (guard is not null)
        {
            return guard;
        }

        return View(await BuildIndexModelAsync(cancellationToken, status, guidelinePage, foodPage, recipePage));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAll(CancellationToken cancellationToken)
    {
        var guard = RequireAdmin();
        if (guard is not null)
        {
            return guard;
        }

        var result = await Api.DeleteAsync(
            "/diet-knowledge/api/admin/knowledge?confirm=true",
            Token,
            cancellationToken);
        AddResultMessage(result, "All knowledge deleted.");
        return RedirectToAction(nameof(Index));
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
            ModelState.AddModelError(nameof(form.IngredientFoodId), Translator.Lookup("Choose an approved food ingredient."));
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

        ViewBag.NutritionFacts = result.Data.NutritionFacts ?? Array.Empty<NutritionFactResponse>();
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

        ViewBag.Steps = result.Data.Steps ?? Array.Empty<RecipeStepResponse>();
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
            ModelState.AddModelError(nameof(form.IngredientFoodId), Translator.Lookup("Choose an approved food ingredient."));
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
    public async Task<IActionResult> RebuildIndex(CancellationToken cancellationToken)
    {
        var guard = RequireAdmin();
        if (guard is not null)
        {
            return guard;
        }

        var result = await Api.PostAsync<KnowledgeIndexResultResponse>(
            "/diet-knowledge/api/admin/knowledge-index/rebuild",
            new { },
            Token,
            cancellationToken);

        if (result.Succeeded && result.Data is not null)
        {
            TempData["Success"] = Translator.Lookup("Knowledge index updated.") +
                $" {result.Data.Indexed} / {result.Data.Unchanged} / {result.Data.Removed} / {result.Data.Failed}";
        }
        else
        {
            TempData["Error"] = FormatErrors(result.Message, result.Errors);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddNutritionFact(
        Guid id,
        NutritionFactForm form,
        CancellationToken cancellationToken)
    {
        var guard = RequireAdmin();
        if (guard is not null)
        {
            return guard;
        }

        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Check the nutrition values: a serving size is required and numbers cannot be negative.";
            return RedirectToAction(nameof(EditFood), new { id });
        }

        return await SaveFoodFactsAsync(
            id,
            facts => facts.Where(fact => !string.Equals(fact.ServingSize, form.ServingSize.Trim(), StringComparison.OrdinalIgnoreCase))
                .Append(new NutritionFactBody(form.ServingSize.Trim(), form.EnergyKcal, form.ProteinG, form.CarbohydrateG, form.FatG, form.FiberG, form.SourceReference ?? string.Empty))
                .ToList(),
            "Nutrition fact saved.",
            cancellationToken);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveNutritionFact(Guid id, Guid factId, CancellationToken cancellationToken)
    {
        var guard = RequireAdmin();
        if (guard is not null)
        {
            return guard;
        }

        return await SaveFoodFactsAsync(
            id,
            _ => null,
            "Nutrition fact removed.",
            cancellationToken,
            removeFactId: factId);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddRecipeStep(Guid id, RecipeStepForm form, CancellationToken cancellationToken)
    {
        var guard = RequireAdmin();
        if (guard is not null)
        {
            return guard;
        }

        if (!ModelState.IsValid)
        {
            TempData["Error"] = "A step needs a number of 1 or more and an instruction.";
            return RedirectToAction(nameof(EditRecipe), new { id });
        }

        return await SaveRecipeStepsAsync(
            id,
            steps => steps.Where(step => step.StepNumber != form.StepNumber)
                .Append(new RecipeStepBody(form.StepNumber, form.Instruction.Trim()))
                .ToList(),
            "Recipe step saved.",
            cancellationToken);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveRecipeStep(Guid id, int stepNumber, CancellationToken cancellationToken)
    {
        var guard = RequireAdmin();
        if (guard is not null)
        {
            return guard;
        }

        return await SaveRecipeStepsAsync(
            id,
            steps => steps.Where(step => step.StepNumber != stepNumber).ToList(),
            "Recipe step removed.",
            cancellationToken);
    }

    // The API replaces a food's nutrition facts as a whole, so load the food, change the list and save it again.
    private async Task<IActionResult> SaveFoodFactsAsync(
        Guid id,
        Func<List<NutritionFactBody>, List<NutritionFactBody>?> change,
        string successMessage,
        CancellationToken cancellationToken,
        Guid? removeFactId = null)
    {
        var current = await Api.GetAsync<FoodResponse>(
            $"/diet-knowledge/api/admin/foods/{id}",
            Token,
            cancellationToken);
        if (!current.Succeeded || current.Data is null)
        {
            TempData["Error"] = FormatErrors(current.Message, current.Errors);
            return RedirectToAction(nameof(Index));
        }

        var food = current.Data;
        var facts = (food.NutritionFacts ?? Array.Empty<NutritionFactResponse>())
            .Where(fact => fact.Id != removeFactId)
            .Select(fact => new NutritionFactBody(fact.ServingSize, fact.EnergyKcal, fact.ProteinG, fact.CarbohydrateG, fact.FatG, fact.FiberG, fact.SourceReference))
            .ToList();
        facts = change(facts) ?? facts;

        var result = await Api.PutAsync<FoodResponse>(
            $"/diet-knowledge/api/admin/foods/{id}",
            new
            {
                food.Name,
                food.Category,
                food.CompatibilityNotes,
                food.SourceTitle,
                food.SourceChapter,
                food.SourcePage,
                food.SourceReference,
                food.ReviewStatus,
                food.ReviewedBy,
                NutritionFacts = facts
            },
            Token,
            cancellationToken);
        AddResultMessage(result, successMessage);
        return RedirectToAction(nameof(EditFood), new { id });
    }

    // Same idea for recipe steps; every existing ingredient is sent back so none is lost.
    private async Task<IActionResult> SaveRecipeStepsAsync(
        Guid id,
        Func<List<RecipeStepBody>, List<RecipeStepBody>> change,
        string successMessage,
        CancellationToken cancellationToken)
    {
        var current = await Api.GetAsync<RecipeResponse>(
            $"/diet-knowledge/api/admin/recipes/{id}",
            Token,
            cancellationToken);
        if (!current.Succeeded || current.Data is null)
        {
            TempData["Error"] = FormatErrors(current.Message, current.Errors);
            return RedirectToAction(nameof(Index));
        }

        var recipe = current.Data;
        var steps = change((recipe.Steps ?? Array.Empty<RecipeStepResponse>())
            .Select(step => new RecipeStepBody(step.StepNumber, step.Instruction))
            .ToList());

        var result = await Api.PutAsync<RecipeResponse>(
            $"/diet-knowledge/api/admin/recipes/{id}",
            new
            {
                recipe.Name,
                recipe.Description,
                recipe.SourceTitle,
                recipe.SourceChapter,
                recipe.SourcePage,
                recipe.SourceReference,
                recipe.ReviewStatus,
                recipe.ReviewedBy,
                Category = recipe.Category ?? string.Empty,
                Ingredients = recipe.Ingredients.Select(ingredient => new RecipeIngredientRequest(ingredient.FoodId, ingredient.QuantityText)).ToArray(),
                Steps = steps
            },
            Token,
            cancellationToken);
        AddResultMessage(result, successMessage);
        return RedirectToAction(nameof(EditRecipe), new { id });
    }

    private sealed record NutritionFactBody(
        string ServingSize,
        decimal EnergyKcal,
        decimal ProteinG,
        decimal CarbohydrateG,
        decimal FatG,
        decimal FiberG,
        string SourceReference);

    private sealed record RecipeStepBody(int StepNumber, string Instruction);

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

    private async Task<AdminKnowledgeViewModel> BuildIndexModelAsync(
        CancellationToken cancellationToken,
        string? status = null,
        int guidelinePage = 1,
        int foodPage = 1,
        int recipePage = 1)
    {
        // Only approved content is listed by default; unapproved items are reviewed from the book sources.
        var reviewFilter = status switch
        {
            "NeedsReview" => "NeedsReview",
            "all" => "all",
            _ => "Approved"
        };
        var statusQuery = reviewFilter == "all" ? string.Empty : $"&reviewStatus={reviewFilter}";

        var guidelines = await Api.GetAsync<PagedResponse<DietGuidelineResponse>>(
            $"/diet-knowledge/api/admin/diet-guidelines?pageNumber={Math.Max(1, guidelinePage)}&pageSize=25&includeInactive=true{statusQuery}",
            Token,
            cancellationToken);
        var foods = await Api.GetAsync<PagedResponse<FoodResponse>>(
            $"/diet-knowledge/api/admin/foods?pageNumber={Math.Max(1, foodPage)}&pageSize=25&includeInactive=true{statusQuery}",
            Token,
            cancellationToken);
        var recipes = await Api.GetAsync<PagedResponse<RecipeResponse>>(
            $"/diet-knowledge/api/admin/recipes?pageNumber={Math.Max(1, recipePage)}&pageSize=25&includeInactive=true{statusQuery}",
            Token,
            cancellationToken);

        var indexStatus = await Api.GetAsync<KnowledgeIndexStatusResponse>(
            "/diet-knowledge/api/admin/knowledge-index/status",
            Token,
            cancellationToken);
        var categories = await Api.GetAsync<List<CategoryResponse>>(
            "/diet-knowledge/api/categories",
            cancellationToken: cancellationToken);

        return new AdminKnowledgeViewModel
        {
            IndexStatus = indexStatus.Data,
            Categories = categories.Data ?? new List<CategoryResponse>(),
            ReviewFilter = reviewFilter,
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
        form.ReviewedBy,
        Category = form.Category ?? string.Empty
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
        Category = form.Category ?? string.Empty,
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
        ReviewedBy = item.ReviewedBy,
        Category = item.Category
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
            IngredientQuantityText = ingredient?.QuantityText ?? "1 serving",
            Category = item.Category
        };
    }
}
