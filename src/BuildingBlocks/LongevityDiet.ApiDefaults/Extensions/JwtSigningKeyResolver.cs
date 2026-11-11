using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace LongevityDiet.ApiDefaults.Extensions;

// Decides which JWT signing key a service may start with. The well-known development key is only
// accepted in the Development environment or when Jwt:AllowDevelopmentKey is explicitly true
// (the local Docker demo sets it), so a forgotten secret cannot silently ship to a real deployment.
public static class JwtSigningKeyResolver
{
    public const string DevelopmentKey = "development-only-replace-with-secret";

    private const int MinimumKeyBytes = 32;

    public static string Resolve(IConfiguration configuration, IHostEnvironment environment)
    {
        var configured = configuration["Jwt:SigningKey"];
        var allowDevelopmentKey = environment.IsDevelopment() ||
            bool.TryParse(configuration["Jwt:AllowDevelopmentKey"], out var allowed) && allowed;

        if (string.IsNullOrWhiteSpace(configured))
        {
            if (allowDevelopmentKey)
            {
                return DevelopmentKey;
            }

            throw new InvalidOperationException(
                "Jwt:SigningKey is not configured. Set Jwt__SigningKey to a secret of at least 32 characters shared by all services.");
        }

        var key = configured;
        if (string.Equals(key, DevelopmentKey, StringComparison.Ordinal))
        {
            if (!allowDevelopmentKey)
            {
                throw new InvalidOperationException(
                    "Jwt:SigningKey is still the public development key. Set Jwt__SigningKey to a private secret, or set Jwt__AllowDevelopmentKey=true for a local demo only.");
            }

            return key;
        }

        if (System.Text.Encoding.UTF8.GetByteCount(key) < MinimumKeyBytes)
        {
            throw new InvalidOperationException(
                $"Jwt:SigningKey must be at least {MinimumKeyBytes} bytes long.");
        }

        return key;
    }
}
