using System.Net;
using LongevityDiet.MealPlanning.Application.Abstractions;
using Microsoft.Extensions.Logging;

namespace LongevityDiet.MealPlanning.Infrastructure.DietKnowledge;

public sealed class HttpDietKnowledgeCatalogClient(
    HttpClient httpClient,
    ILogger<HttpDietKnowledgeCatalogClient> logger) : IDietKnowledgeCatalogClient
{
    public async Task<bool> FoodExistsAsync(
        Guid foodId,
        CancellationToken cancellationToken = default)
    {
        return await ResourceExistsAsync($"/api/foods/{foodId}", cancellationToken);
    }

    public async Task<bool> RecipeExistsAsync(
        Guid recipeId,
        CancellationToken cancellationToken = default)
    {
        return await ResourceExistsAsync($"/api/recipes/{recipeId}", cancellationToken);
    }

    private async Task<bool> ResourceExistsAsync(
        string path,
        CancellationToken cancellationToken)
    {
        using var response = await httpClient.GetAsync(path, cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }

        if (response.IsSuccessStatusCode)
        {
            return true;
        }

        logger.LogWarning(
            "Diet Knowledge lookup {Path} returned {StatusCode}.",
            path,
            (int)response.StatusCode);

        return false;
    }
}
