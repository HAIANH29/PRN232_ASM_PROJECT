namespace LongevityDiet.Contracts.Messaging;

public sealed record RecommendationRequestMessage(
    Guid RequestId,
    Guid UserId,
    IReadOnlyCollection<string> PreferenceTags,
    int Days,
    DateTimeOffset RequestedAtUtc);
