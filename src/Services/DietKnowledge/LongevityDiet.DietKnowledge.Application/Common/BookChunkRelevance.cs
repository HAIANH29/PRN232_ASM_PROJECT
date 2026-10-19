using System.Text.RegularExpressions;

namespace LongevityDiet.DietKnowledge.Application.Common;

// Heuristics that keep PDF boilerplate, quotes, acknowledgments and clinical-treatment chapters
// away from candidate generation. The platform is educational and must not cover disease treatment.
public static partial class BookChunkRelevance
{
    private const int MinimumUsefulCharacters = 150;
    private const int MedicalTermThreshold = 2;

    private static readonly string[] OutOfScopeMarkers =
    [
        "Quotes From Pages",
        "Best Quotes from",
        "Acknowledgments"
    ];

    private static readonly string[] MedicalTerms =
    [
        "cancer",
        "chemotherapy",
        "diabetes",
        "cardiovascular",
        "alzheimer",
        "dementia",
        "tumor",
        "autoimmune",
        "multiple sclerosis",
        "arthritis",
        "crohn"
    ];

    public static string CleanText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return string.Empty;
        }

        var cleaned = BoilerplateRegex().Replace(text, " ");
        return WhitespaceRegex().Replace(cleaned, " ").Trim();
    }

    public static bool IsRelevant(string text)
    {
        var cleaned = CleanText(text);
        if (cleaned.Length < MinimumUsefulCharacters)
        {
            return false;
        }

        if (OutOfScopeMarkers.Any(marker => text.Contains(marker, StringComparison.OrdinalIgnoreCase)))
        {
            return false;
        }

        var medicalHits = MedicalTerms.Count(term =>
            cleaned.Contains(term, StringComparison.OrdinalIgnoreCase));
        return medicalHits < MedicalTermThreshold;
    }

    // Wider than the chunk filter: a candidate mentioning any of these is never approved in bulk.
    private static readonly string[] BulkApprovalBlockedTerms =
    [
        .. MedicalTerms,
        "disease",
        "treatment",
        "treat ",
        "cure",
        "diagnos",
        "predict",
        "lifespan",
        "life span"
    ];

    public static bool IsSafeForBulkApproval(string title, string summary)
    {
        var text = $"{title} {summary}";
        return !BulkApprovalBlockedTerms.Any(term =>
            text.Contains(term, StringComparison.OrdinalIgnoreCase));
    }

    [GeneratedRegex(
        @"Scan to Download|Install Bookey App to Unlock Full Text and\s*Audio|View on Bookey Website and Generate Beautiful Quote Images",
        RegexOptions.IgnoreCase)]
    private static partial Regex BoilerplateRegex();

    [GeneratedRegex(@"\s+")]
    private static partial Regex WhitespaceRegex();
}
