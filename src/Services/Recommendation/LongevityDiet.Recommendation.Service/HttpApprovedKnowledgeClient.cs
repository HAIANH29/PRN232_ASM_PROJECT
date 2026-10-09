using System.Net.Http.Json;
using System.Text.Json;

namespace LongevityDiet.Recommendation.Service;

public sealed class HttpApprovedKnowledgeClient(
    HttpClient httpClient,
    ILogger<HttpApprovedKnowledgeClient> logger) : IApprovedKnowledgeClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<ApprovedKnowledgeContext> GetApprovedKnowledgeAsync(
        CancellationToken cancellationToken = default)
    {
        var guidelines = await GetItemsAsync<DietGuidelineResponse>(
            "/api/diet-guidelines?pageNumber=1&pageSize=50&sortBy=title&sortDirection=asc",
            "diet guidelines",
            cancellationToken);
        var foods = await GetItemsAsync<FoodResponse>(
            "/api/foods?pageNumber=1&pageSize=100&sortBy=name&sortDirection=asc",
            "foods",
            cancellationToken);
        var recipes = await GetItemsAsync<RecipeResponse>(
            "/api/recipes?pageNumber=1&pageSize=100&sortBy=name&sortDirection=asc",
            "recipes",
            cancellationToken);

        return new ApprovedKnowledgeContext(
            guidelines
                .Where(item => item.IsApproved)
                .Select(item => new ApprovedDietGuideline(
                    item.Id,
                    Clean(item.Title),
                    Clean(item.Summary),
                    Clean(item.SourceChapter),
                    Clean(item.SourcePage),
                    Clean(item.SourceReference)))
                .ToArray(),
            foods
                .Where(item => item.IsApproved)
                .Select(item => new ApprovedFood(
                    item.Id,
                    Clean(item.Name),
                    Clean(item.Category),
                    Clean(item.CompatibilityNotes),
                    Clean(item.SourceChapter),
                    Clean(item.SourcePage),
                    Clean(item.SourceReference)))
                .ToArray(),
            recipes
                .Where(item => item.IsApproved)
                .Select(item => new ApprovedRecipe(
                    item.Id,
                    Clean(item.Name),
                    Clean(item.Description),
                    item.Ingredients.Select(ingredient => new ApprovedRecipeIngredient(
                        ingredient.FoodId,
                        Clean(ingredient.FoodName),
                        Clean(ingredient.FoodCategory),
                        Clean(ingredient.QuantityText))).ToArray(),
                    Clean(item.SourceChapter),
                    Clean(item.SourcePage),
                    Clean(item.SourceReference)))
                .ToArray());
    }

    private async Task<IReadOnlyCollection<T>> GetItemsAsync<T>(
        string path,
        string label,
        CancellationToken cancellationToken)
    {
        using var response = await httpClient.GetAsync(path, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            logger.LogWarning(
                "Diet Knowledge {Label} lookup returned HTTP {StatusCode}.",
                label,
                (int)response.StatusCode);
            return Array.Empty<T>();
        }

        var envelope = await response.Content.ReadFromJsonAsync<ApiEnvelope<PagedEnvelope<T>>>(
            JsonOptions,
            cancellationToken);

        if (envelope is { Succeeded: true, Data.Items: not null })
        {
            return envelope.Data.Items;
        }

        logger.LogWarning("Diet Knowledge {Label} response did not contain usable data.", label);
        return Array.Empty<T>();
    }

    private static string Clean(string? value)
    {
        return value?.Trim() ?? string.Empty;
    }

    private sealed record ApiEnvelope<T>(
        bool Succeeded,
        T? Data,
        string? Message);

    private sealed record PagedEnvelope<T>(
        IReadOnlyCollection<T> Items,
        int PageNumber,
        int PageSize,
        int TotalCount);

    private sealed record DietGuidelineResponse(
        Guid Id,
        string Title,
        string Summary,
        string SourceChapter,
        string SourcePage,
        string SourceReference,
        string ReviewStatus,
        bool IsActive)
    {
        public bool IsApproved =>
            IsActive && ReviewStatus.Equals("Approved", StringComparison.OrdinalIgnoreCase);
    }

    private sealed record FoodResponse(
        Guid Id,
        string Name,
        string Category,
        string CompatibilityNotes,
        string SourceChapter,
        string SourcePage,
        string SourceReference,
        string ReviewStatus,
        bool IsActive)
    {
        public bool IsApproved =>
            IsActive && ReviewStatus.Equals("Approved", StringComparison.OrdinalIgnoreCase);
    }

    private sealed record RecipeResponse(
        Guid Id,
        string Name,
        string Description,
        bool IsActive,
        string SourceChapter,
        string SourcePage,
        string SourceReference,
        string ReviewStatus,
        IReadOnlyCollection<RecipeIngredientResponse> Ingredients)
    {
        public bool IsApproved =>
            IsActive && ReviewStatus.Equals("Approved", StringComparison.OrdinalIgnoreCase);
    }

    private sealed record RecipeIngredientResponse(
        Guid FoodId,
        string? FoodName,
        string? FoodCategory,
        string QuantityText);
}
