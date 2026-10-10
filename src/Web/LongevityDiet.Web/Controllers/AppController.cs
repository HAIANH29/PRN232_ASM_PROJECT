using LongevityDiet.Web.Models;
using LongevityDiet.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LongevityDiet.Web.Controllers;

public abstract class AppController(ApiGatewayClient api, UserSession session) : Controller
{
    protected ApiGatewayClient Api { get; } = api;

    protected UserSession Session { get; } = session;

    protected string? Token => Session.Token;

    protected IActionResult? RequireUser()
    {
        if (Session.IsSignedIn)
        {
            return null;
        }

        TempData["Warning"] = "Please sign in to continue.";
        return RedirectToAction("Login", "Auth");
    }

    protected IActionResult? RequireAdmin()
    {
        var userResult = RequireUser();
        if (userResult is not null)
        {
            return userResult;
        }

        if (Session.IsAdmin)
        {
            return null;
        }

        TempData["Warning"] = "Admin access is required for that screen.";
        return RedirectToAction("Index", "Home");
    }

    protected void AddResultMessage(ApiCallResult result, string successMessage)
    {
        if (result.Succeeded)
        {
            TempData["Success"] = successMessage;
            return;
        }

        TempData["Error"] = FormatErrors(result.Message, result.Errors);
    }

    protected void AddResultMessage<T>(ApiCallResult<T> result, string successMessage)
    {
        if (result.Succeeded)
        {
            TempData["Success"] = successMessage;
            return;
        }

        TempData["Error"] = FormatErrors(result.Message, result.Errors);
    }

    protected async Task<IReadOnlyCollection<FoodResponse>> LoadFoodsAsync(
        bool admin = false,
        CancellationToken cancellationToken = default)
    {
        var path = admin
            ? "/diet-knowledge/api/admin/foods?pageNumber=1&pageSize=100&includeInactive=true"
            : "/diet-knowledge/api/foods?pageNumber=1&pageSize=100";
        var result = await Api.GetAsync<PagedResponse<FoodResponse>>(
            path,
            admin ? Token : null,
            cancellationToken);

        return result.Data?.Items ?? Array.Empty<FoodResponse>();
    }

    protected async Task<IReadOnlyCollection<RecipeResponse>> LoadRecipesAsync(
        bool admin = false,
        CancellationToken cancellationToken = default)
    {
        var path = admin
            ? "/diet-knowledge/api/admin/recipes?pageNumber=1&pageSize=100&includeInactive=true"
            : "/diet-knowledge/api/recipes?pageNumber=1&pageSize=100";
        var result = await Api.GetAsync<PagedResponse<RecipeResponse>>(
            path,
            admin ? Token : null,
            cancellationToken);

        return result.Data?.Items ?? Array.Empty<RecipeResponse>();
    }

    protected async Task<PagedResponse<MealPlanResponse>> LoadMealPlansAsync(
        CancellationToken cancellationToken = default)
    {
        var result = await Api.GetAsync<PagedResponse<MealPlanResponse>>(
            "/meal-planning/api/meal-plans?pageNumber=1&pageSize=50",
            Token,
            cancellationToken);

        return result.Data ?? KnowledgeIndexViewModel.Empty<MealPlanResponse>();
    }

    protected static string[] SplitTags(string? value)
    {
        return (value ?? string.Empty)
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(tag => !string.IsNullOrWhiteSpace(tag))
            .Take(20)
            .ToArray();
    }

    protected static string FormatErrors(
        string message,
        IReadOnlyCollection<ApiError> errors)
    {
        if (errors.Count == 0)
        {
            return message;
        }

        return $"{message} {string.Join(" ", errors.Select(error => error.Message))}";
    }
}
