using LongevityDiet.Web.Models;
using LongevityDiet.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LongevityDiet.Web.Controllers;

public sealed class KnowledgeController(ApiGatewayClient api, UserSession session)
    : AppController(api, session)
{
    [HttpGet]
    public async Task<IActionResult> Index(
        string? search,
        string? category,
        CancellationToken cancellationToken)
    {
        var model = new KnowledgeIndexViewModel
        {
            Search = search,
            Category = category
        };

        var guidelinePath = ApiGatewayClient.BuildQuery(
            "/diet-knowledge/api/diet-guidelines",
            ("pageNumber", 1),
            ("pageSize", 20),
            ("search", search),
            ("sortBy", "title"),
            ("sortDirection", "asc"));
        var foodPath = ApiGatewayClient.BuildQuery(
            "/diet-knowledge/api/foods",
            ("pageNumber", 1),
            ("pageSize", 30),
            ("search", search),
            ("category", category),
            ("sortBy", "name"),
            ("sortDirection", "asc"));
        var recipePath = ApiGatewayClient.BuildQuery(
            "/diet-knowledge/api/recipes",
            ("pageNumber", 1),
            ("pageSize", 30),
            ("search", search),
            ("sortBy", "name"),
            ("sortDirection", "asc"));

        model.Guidelines = (await Api.GetAsync<PagedResponse<DietGuidelineResponse>>(
            guidelinePath,
            cancellationToken: cancellationToken)).Data ?? KnowledgeIndexViewModel.Empty<DietGuidelineResponse>();
        model.Foods = (await Api.GetAsync<PagedResponse<FoodResponse>>(
            foodPath,
            cancellationToken: cancellationToken)).Data ?? KnowledgeIndexViewModel.Empty<FoodResponse>();
        model.Recipes = (await Api.GetAsync<PagedResponse<RecipeResponse>>(
            recipePath,
            cancellationToken: cancellationToken)).Data ?? KnowledgeIndexViewModel.Empty<RecipeResponse>();

        return View(model);
    }
}
