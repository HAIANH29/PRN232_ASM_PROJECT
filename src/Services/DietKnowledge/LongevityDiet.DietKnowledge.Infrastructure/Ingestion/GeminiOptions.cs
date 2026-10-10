namespace LongevityDiet.DietKnowledge.Infrastructure.Ingestion;

public sealed class GeminiOptions
{
    public const string SectionName = "Gemini";

    public bool Enabled { get; set; }

    public string Endpoint { get; set; } = "https://generativelanguage.googleapis.com";

    public string ApiKey { get; set; } = string.Empty;

    public string Model { get; set; } = "gemini-2.0-flash";

    public int TimeoutSeconds { get; set; } = 30;

    public double Temperature { get; set; } = 0.15;

    public int MaxOutputTokens { get; set; } = 768;
}
