namespace LongevityDiet.MealPlanning.Application.Models;

public sealed record RecommendationRequestQuery(
    int PageNumber,
    int PageSize,
    string? Status);

public sealed record RecommendationRequestModel(
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

public sealed record CreateRecommendationRequestCommand(
    Guid UserId,
    IReadOnlyCollection<string> PreferenceTags,
    int Days);

public sealed record AcceptRecommendationCommand(
    Guid UserId,
    string UserEmail,
    string Name,
    DateOnly StartDate,
    DateOnly EndDate,
    IReadOnlyCollection<CreateMealPlanItemCommand> Items);
