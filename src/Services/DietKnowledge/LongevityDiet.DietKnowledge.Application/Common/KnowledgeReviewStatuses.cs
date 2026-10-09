namespace LongevityDiet.DietKnowledge.Application.Common;

public static class KnowledgeReviewStatuses
{
    public const string NeedsReview = "NeedsReview";

    public const string Approved = "Approved";

    public const string Rejected = "Rejected";

    public static bool IsValid(string? status)
    {
        return string.Equals(status, NeedsReview, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(status, Approved, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(status, Rejected, StringComparison.OrdinalIgnoreCase);
    }

    public static string Normalize(string? status)
    {
        if (string.IsNullOrWhiteSpace(status))
        {
            return NeedsReview;
        }

        if (string.Equals(status, Approved, StringComparison.OrdinalIgnoreCase))
        {
            return Approved;
        }

        if (string.Equals(status, Rejected, StringComparison.OrdinalIgnoreCase))
        {
            return Rejected;
        }

        return NeedsReview;
    }
}
