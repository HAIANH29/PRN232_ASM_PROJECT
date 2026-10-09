using System.ComponentModel.DataAnnotations;

namespace LongevityDiet.MealPlanning.Api.Contracts;

public sealed record RecommendationRequestListRequest
{
    [Range(1, int.MaxValue)]
    public int PageNumber { get; init; } = 1;

    [Range(1, 100)]
    public int PageSize { get; init; } = 20;

    [MaxLength(40)]
    public string? Status { get; init; }
}

public sealed record RecommendationRequestResponse(
    Guid Id,
    Guid UserId,
    IReadOnlyCollection<string> PreferenceTags,
    int Days,
    string Status,
    IReadOnlyCollection<string> SuggestedMealTitles,
    string Disclaimer,
    Guid? AcceptedMealPlanId,
    DateTimeOffset RequestedAtUtc,
    DateTimeOffset? CompletedAtUtc,
    DateTimeOffset? UpdatedAtUtc);

public sealed record CreateRecommendationRequest
{
    public IReadOnlyCollection<string> PreferenceTags { get; init; } =
        Array.Empty<string>();

    [Range(1, 14)]
    public int Days { get; init; } = 7;
}

public sealed record AcceptRecommendationRequest
{
    [Required]
    [MaxLength(200)]
    public string Name { get; init; } = string.Empty;

    [Required]
    public DateOnly StartDate { get; init; }

    [Required]
    public DateOnly EndDate { get; init; }

    [Required]
    [MinLength(1)]
    public IReadOnlyCollection<CreateMealPlanItemRequest> Items { get; init; } =
        Array.Empty<CreateMealPlanItemRequest>();
}
