namespace LongevityDiet.MealPlanning.Domain.Entities;

public sealed class MealRecommendationRequest
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string PreferenceTagsJson { get; set; } = "[]";

    public int Days { get; set; }

    public string Status { get; set; } = "Pending";

    public string SuggestedMealTitlesJson { get; set; } = "[]";

    public string Disclaimer { get; set; } = string.Empty;

    public Guid? AcceptedMealPlanId { get; set; }

    public DateTimeOffset RequestedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? CompletedAtUtc { get; set; }

    public DateTimeOffset? UpdatedAtUtc { get; set; }
}
