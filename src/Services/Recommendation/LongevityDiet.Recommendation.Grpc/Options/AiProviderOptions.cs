namespace LongevityDiet.Recommendation.Grpc.Options;

public sealed class AiProviderOptions
{
    public const string SectionName = "AiProvider";

    public bool Enabled { get; set; }

    public string ProviderName { get; set; } = "NotConfigured";

    public string Endpoint { get; set; } = string.Empty;
}
