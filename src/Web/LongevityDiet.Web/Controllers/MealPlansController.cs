using LongevityDiet.Web.Models;
using LongevityDiet.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LongevityDiet.Web.Controllers;

public sealed class MealPlansController(ApiGatewayClient api, UserSession session)
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

        return View(new MealPlansIndexViewModel
        {
            MealPlans = await LoadMealPlansAsync(cancellationToken)
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind(Prefix = "Form")] MealPlanForm form,
        CancellationToken cancellationToken)
    {
        var guard = RequireUser();
        if (guard is not null)
        {
            return guard;
        }

        if (!ModelState.IsValid)
        {
            return View("Index", new MealPlansIndexViewModel
            {
                MealPlans = await LoadMealPlansAsync(cancellationToken),
                Form = form
            });
        }

        var result = await Api.PostAsync<MealPlanResponse>(
            "/meal-planning/api/meal-plans",
            new
            {
                form.Name,
                form.StartDate,
                form.EndDate,
                items = Array.Empty<object>()
            },
            Token,
            cancellationToken);
        AddResultMessage(result, "Meal plan created.");

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
    public async Task<IActionResult> Update(
        Guid id,
        [Bind(Prefix = "EditForm")] MealPlanForm form,
        CancellationToken cancellationToken)
    {
        var guard = RequireUser();
        if (guard is not null)
        {
            return guard;
        }

        if (!ModelState.IsValid)
        {
            var model = await BuildDetailsModelAsync(id, cancellationToken);
            if (model is null)
            {
                return RedirectToAction(nameof(Index));
            }

            model.EditForm = form;
            return View("Details", model);
        }

        var result = await Api.PutAsync<MealPlanResponse>(
            $"/meal-planning/api/meal-plans/{id}",
            new { form.Name, form.StartDate, form.EndDate },
            Token,
            cancellationToken);
        AddResultMessage(result, "Meal plan updated.");
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var guard = RequireUser();
        if (guard is not null)
        {
            return guard;
        }

        var result = await Api.DeleteAsync($"/meal-planning/api/meal-plans/{id}", Token, cancellationToken);
        AddResultMessage(result, "Meal plan deleted.");
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddItem(
        Guid id,
        [Bind(Prefix = "ItemForm")] MealPlanItemForm form,
        CancellationToken cancellationToken)
    {
        var guard = RequireUser();
        if (guard is not null)
        {
            return guard;
        }

        ValidateCatalogChoice(form.FoodId, form.RecipeId);
        if (!ModelState.IsValid)
        {
            var model = await BuildDetailsModelAsync(id, cancellationToken);
            if (model is null)
            {
                return RedirectToAction(nameof(Index));
            }

            model.ItemForm = form;
            return View("Details", model);
        }

        var result = await Api.PostAsync<MealPlanItemResponse>(
            $"/meal-planning/api/meal-plans/{id}/items",
            ToItemBody(form),
            Token,
            cancellationToken);
        AddResultMessage(result, "Meal item added.");
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteItem(Guid id, Guid itemId, CancellationToken cancellationToken)
    {
        var guard = RequireUser();
        if (guard is not null)
        {
            return guard;
        }

        var result = await Api.DeleteAsync(
            $"/meal-planning/api/meal-plans/{id}/items/{itemId}",
            Token,
            cancellationToken);
        AddResultMessage(result, "Meal item removed.");
        return RedirectToAction(nameof(Details), new { id });
    }

    private async Task<MealPlanDetailsViewModel?> BuildDetailsModelAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await Api.GetAsync<MealPlanResponse>(
            $"/meal-planning/api/meal-plans/{id}",
            Token,
            cancellationToken);
        if (!result.Succeeded || result.Data is null)
        {
            TempData["Error"] = FormatErrors(result.Message, result.Errors);
            return null;
        }

        return new MealPlanDetailsViewModel
        {
            MealPlan = result.Data,
            EditForm = new MealPlanForm
            {
                Name = result.Data.Name,
                StartDate = result.Data.StartDate,
                EndDate = result.Data.EndDate
            },
            ItemForm = new MealPlanItemForm
            {
                PlannedDate = result.Data.StartDate,
                MealSlot = "Breakfast"
            },
            Foods = await LoadFoodsAsync(cancellationToken: cancellationToken),
            Recipes = await LoadRecipesAsync(cancellationToken: cancellationToken)
        };
    }

    private void ValidateCatalogChoice(Guid? foodId, Guid? recipeId)
    {
        var hasFood = foodId.HasValue && foodId.Value != Guid.Empty;
        var hasRecipe = recipeId.HasValue && recipeId.Value != Guid.Empty;
        if (hasFood == hasRecipe)
        {
            ModelState.AddModelError(
                nameof(MealPlanItemForm.FoodId),
                "Choose exactly one approved food or recipe.");
        }
    }

    private static object ToItemBody(MealPlanItemForm form) => new
    {
        form.PlannedDate,
        form.MealSlot,
        recipeId = form.RecipeId == Guid.Empty ? null : form.RecipeId,
        foodId = form.FoodId == Guid.Empty ? null : form.FoodId,
        form.Notes,
        form.ReminderAtUtc
    };
}
