using System.Security.Authentication;
using System.Security.Claims;

namespace LongevityDiet.Tracking.Api.Controllers;

internal static class CurrentUser
{
    public static Guid GetUserId(ClaimsPrincipal user)
    {
        var value = user.FindFirstValue(ClaimTypes.NameIdentifier) ??
            user.FindFirstValue("sub");

        return Guid.TryParse(value, out var userId) && userId != Guid.Empty
            ? userId
            : throw new AuthenticationException("Authenticated user id is missing.");
    }

    public static string GetEmail(ClaimsPrincipal user)
    {
        var value = user.FindFirstValue(ClaimTypes.Email);
        return string.IsNullOrWhiteSpace(value)
            ? throw new AuthenticationException("Authenticated user email is missing.")
            : value;
    }
}
