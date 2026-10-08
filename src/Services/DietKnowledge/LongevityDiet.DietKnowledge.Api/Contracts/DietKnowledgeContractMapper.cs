using LongevityDiet.ApiDefaults.Api;
using LongevityDiet.DietKnowledge.Application.Models;

namespace LongevityDiet.DietKnowledge.Api.Contracts;

internal static class DietKnowledgeContractMapper
{
    public static PagedResponse<TResponse> ToPagedResponse<TModel, TResponse>(
        PagedResult<TModel> result,
        Func<TModel, TResponse> mapper)
    {
        return new PagedResponse<TResponse>(
            result.Items.Select(mapper).ToArray(),
            result.PageNumber,
            result.PageSize,
            result.TotalCount);
    }

    public static DietGuidelineQuery ToQuery(
        this DietGuidelineListRequest request,
        bool forceActiveOnly)
    {
        return new DietGuidelineQuery(
            request.PageNumber,
            request.PageSize,
            request.Search,
            IncludeInactive: !forceActiveOnly && request.IncludeInactive,
            request.SortBy,
            request.SortDirection);
    }

    public static FoodQuery ToQuery(
        this FoodListRequest request,
        bool forceActiveOnly)
    {
        return new FoodQuery(
            request.PageNumber,
            request.PageSize,
            request.Search,
            request.Category,
            IncludeInactive: !forceActiveOnly && request.IncludeInactive,
            request.SortBy,
            request.SortDirection);
    }

    public static RecipeQuery ToQuery(
        this RecipeListRequest request,
        bool forceActiveOnly)
    {
        return new RecipeQuery(
            request.PageNumber,
            request.PageSize,
            request.Search,
            request.FoodId,
            IncludeInactive: !forceActiveOnly && request.IncludeInactive,
            request.SortBy,
            request.SortDirection);
    }

    public static CreateDietGuidelineCommand ToCommand(this CreateDietGuidelineRequest request)
    {
        return new CreateDietGuidelineCommand(request.Title, request.Summary, request.SourceNote);
    }

    public static UpdateDietGuidelineCommand ToCommand(this UpdateDietGuidelineRequest request)
    {
        return new UpdateDietGuidelineCommand(request.Title, request.Summary, request.SourceNote);
    }

    public static CreateFoodCommand ToCommand(this CreateFoodRequest request)
    {
        return new CreateFoodCommand(request.Name, request.Category, request.CompatibilityNotes);
    }

    public static UpdateFoodCommand ToCommand(this UpdateFoodRequest request)
    {
        return new UpdateFoodCommand(request.Name, request.Category, request.CompatibilityNotes);
    }

    public static CreateRecipeCommand ToCommand(this CreateRecipeRequest request)
    {
        return new CreateRecipeCommand(
            request.Name,
            request.Description,
            request.Ingredients.Select(ToCommand).ToArray());
    }

    public static UpdateRecipeCommand ToCommand(this UpdateRecipeRequest request)
    {
        return new UpdateRecipeCommand(
            request.Name,
            request.Description,
            request.Ingredients.Select(ToCommand).ToArray());
    }

    public static DietGuidelineResponse ToResponse(this DietGuidelineModel model)
    {
        return new DietGuidelineResponse(
            model.Id,
            model.Title,
            model.Summary,
            model.SourceNote,
            model.IsActive,
            model.CreatedAtUtc,
            model.UpdatedAtUtc);
    }

    public static FoodResponse ToResponse(this FoodModel model)
    {
        return new FoodResponse(
            model.Id,
            model.Name,
            model.Category,
            model.CompatibilityNotes,
            model.IsActive,
            model.CreatedAtUtc,
            model.UpdatedAtUtc);
    }

    public static RecipeResponse ToResponse(this RecipeModel model)
    {
        return new RecipeResponse(
            model.Id,
            model.Name,
            model.Description,
            model.IsActive,
            model.Ingredients.Select(ToResponse).ToArray(),
            model.CreatedAtUtc,
            model.UpdatedAtUtc);
    }

    private static RecipeIngredientCommand ToCommand(RecipeIngredientRequest request)
    {
        return new RecipeIngredientCommand(request.FoodId, request.QuantityText);
    }

    private static RecipeIngredientResponse ToResponse(RecipeIngredientModel model)
    {
        return new RecipeIngredientResponse(
            model.Id,
            model.FoodId,
            model.FoodName,
            model.FoodCategory,
            model.QuantityText);
    }
}
