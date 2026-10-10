namespace LongevityDiet.DietKnowledge.Application.Common;

public static class BookSourceDocumentStatuses
{
    public const string Uploaded = "Uploaded";
    public const string Chunked = "Chunked";
    public const string Failed = "Failed";

    public static bool IsValid(string? status)
    {
        return string.Equals(status, Uploaded, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(status, Chunked, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(status, Failed, StringComparison.OrdinalIgnoreCase);
    }

    public static string Normalize(string status)
    {
        if (string.Equals(status, Uploaded, StringComparison.OrdinalIgnoreCase))
        {
            return Uploaded;
        }

        if (string.Equals(status, Chunked, StringComparison.OrdinalIgnoreCase))
        {
            return Chunked;
        }

        if (string.Equals(status, Failed, StringComparison.OrdinalIgnoreCase))
        {
            return Failed;
        }

        return Uploaded;
    }
}
