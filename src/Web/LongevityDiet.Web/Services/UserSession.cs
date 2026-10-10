using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace LongevityDiet.Web.Services;

public sealed class UserSession(IHttpContextAccessor httpContextAccessor)
{
    private const string TokenCookie = "ldp_access_token";
    private const string EmailCookie = "ldp_email";
    private const string DisplayNameCookie = "ldp_display_name";
    private const string RolesCookie = "ldp_roles";

    public string? Token => Context.Request.Cookies[TokenCookie];

    public bool IsSignedIn => !string.IsNullOrWhiteSpace(Token);

    public string Email => Context.Request.Cookies[EmailCookie] ?? GetClaim(ClaimTypes.Email) ?? string.Empty;

    public string DisplayName => Context.Request.Cookies[DisplayNameCookie] ?? GetClaim(ClaimTypes.Name) ?? Email;

    public IReadOnlyCollection<string> Roles
    {
        get
        {
            var cookie = Context.Request.Cookies[RolesCookie];
            if (!string.IsNullOrWhiteSpace(cookie))
            {
                return cookie.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            }

            var role = GetClaim(ClaimTypes.Role);
            return string.IsNullOrWhiteSpace(role) ? Array.Empty<string>() : [role];
        }
    }

    public bool IsAdmin => Roles.Any(role => role.Equals("Admin", StringComparison.OrdinalIgnoreCase));

    public void SignIn(string token, string email, string displayName, IReadOnlyCollection<string> roles, DateTimeOffset expiresAtUtc)
    {
        var options = new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Lax,
            Secure = Context.Request.IsHttps,
            Expires = expiresAtUtc
        };

        Context.Response.Cookies.Append(TokenCookie, token, options);
        Context.Response.Cookies.Append(EmailCookie, email, options);
        Context.Response.Cookies.Append(DisplayNameCookie, displayName, options);
        Context.Response.Cookies.Append(RolesCookie, string.Join(',', roles), options);
    }

    public void SignOut()
    {
        Context.Response.Cookies.Delete(TokenCookie);
        Context.Response.Cookies.Delete(EmailCookie);
        Context.Response.Cookies.Delete(DisplayNameCookie);
        Context.Response.Cookies.Delete(RolesCookie);
    }

    private HttpContext Context =>
        httpContextAccessor.HttpContext ?? throw new InvalidOperationException("HTTP context is not available.");

    private string? GetClaim(string claimType)
    {
        var token = Token;
        if (string.IsNullOrWhiteSpace(token))
        {
            return null;
        }

        var parts = token.Split('.');
        if (parts.Length < 2)
        {
            return null;
        }

        try
        {
            var json = Encoding.UTF8.GetString(Base64UrlDecode(parts[1]));
            using var document = JsonDocument.Parse(json);
            if (document.RootElement.TryGetProperty(claimType, out var claim))
            {
                return claim.ValueKind == JsonValueKind.Array
                    ? claim.EnumerateArray().FirstOrDefault().GetString()
                    : claim.GetString();
            }
        }
        catch
        {
            return null;
        }

        return null;
    }

    private static byte[] Base64UrlDecode(string value)
    {
        var padded = value.Replace('-', '+').Replace('_', '/');
        padded = padded.PadRight(padded.Length + (4 - padded.Length % 4) % 4, '=');
        return Convert.FromBase64String(padded);
    }
}
