using System.ComponentModel.DataAnnotations;

namespace LongevityDiet.DietKnowledge.Api.Contracts;

public sealed record RecipeListRequest : ListRequest
{
    public Guid? FoodId { get; init; }

    public bool IncludeInactive { get; init; }
}

public sealed record RecipeResponse(
    Guid Id,
    string Name,
    string Description,
    bool IsActive,
    IReadOnlyCollection<RecipeIngredientResponse> Ingredients,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc);

public sealed record RecipeIngredientResponse(
    Guid Id,
    Guid FoodId,
    string? FoodName,
    string? FoodCategory,
    string QuantityText);

public sealed record RecipeIngredientRequest
{
    [Required]
    public Guid FoodId { get; init; }

    [MaxLength(120)]
    public string QuantityText { get; init; } = string.Empty;
}

public sealed record CreateRecipeRequest
{
    [Required]
    [MaxLength(200)]
    public string Name { get; init; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Description { get; init; } = string.Empty;

    [Required]
    [MinLength(1)]
    public IReadOnlyCollection<RecipeIngredientRequest> Ingredients { get; init; } =
        Array.Empty<RecipeIngredientRequest>();
}

public sealed record UpdateRecipeRequest
{
    [Required]
    [MaxLength(200)]
    public string Name { get; init; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Description { get; init; } = string.Empty;

    [Required]
    [MinLength(1)]
    public IReadOnlyCollection<RecipeIngredientRequest> Ingredients { get; init; } =
        Array.Empty<RecipeIngredientRequest>();
}
