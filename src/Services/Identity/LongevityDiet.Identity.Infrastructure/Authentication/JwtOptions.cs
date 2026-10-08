namespace LongevityDiet.Identity.Infrastructure.Authentication;

public sealed class JwtOptions
{
    public string Issuer { get; set; } = "LongevityDiet.Identity";

    public string Audience { get; set; } = "LongevityDiet.Platform";

    public string SigningKey { get; set; } = "development-only-replace-with-secret";

    public int AccessTokenMinutes { get; set; } = 120;
}
