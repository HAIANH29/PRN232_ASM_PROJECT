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
        string? sort,
        int guidelinePage = 1,
        int foodPage = 1,
        int recipePage = 1,
        CancellationToken cancellationToken = default)
    {
        if (Session.IsAdmin)
        {
            return RedirectToAction("Index", "AdminKnowledge");
        }

        var model = new KnowledgeIndexViewModel
        {
            Search = search,
            Category = category,
            Sort = sort
        };

        var (sortBy, sortDirection) = sort switch
        {
            "name_desc" => ("name", "desc"),
            "newest" => ("createdAt", "desc"),
            "oldest" => ("createdAt", "asc"),
            _ => ("name", "asc")
        };

        var guidelinePath = ApiGatewayClient.BuildQuery(
            "/diet-knowledge/api/diet-guidelines",
            ("pageNumber", Math.Max(1, guidelinePage)),
            ("pageSize", 20),
            ("search", search),
            ("sortBy", sortBy == "createdAt" ? sortBy : "title"),
            ("sortDirection", sortDirection));
        var foodPath = ApiGatewayClient.BuildQuery(
            "/diet-knowledge/api/foods",
            ("pageNumber", Math.Max(1, foodPage)),
            ("pageSize", 30),
            ("search", search),
            ("category", category),
            ("sortBy", sortBy == "createdAt" ? sortBy : "name"),
            ("sortDirection", sortDirection));
        var recipePath = ApiGatewayClient.BuildQuery(
            "/diet-knowledge/api/recipes",
            ("pageNumber", Math.Max(1, recipePage)),
            ("pageSize", 30),
            ("search", search),
            ("sortBy", sortBy == "createdAt" ? sortBy : "name"),
            ("sortDirection", sortDirection));

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
