namespace LongevityDiet.Contracts.Messaging;

public sealed record RecommendationResultMessage(
    Guid RequestId,
    Guid UserId,
    IReadOnlyCollection<string> SuggestedMealTitles,
    string Disclaimer,
    DateTimeOffset CompletedAtUtc);
