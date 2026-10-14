using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace LongevityDiet.Web.Controllers;

public sealed class LanguageController : Controller
{
    private static readonly string[] Supported = ["en", "vi"];

    [HttpGet]
    public IActionResult Set(string lang, string? returnUrl)
    {
        var selected = Supported.Contains(lang, StringComparer.OrdinalIgnoreCase)
            ? lang.ToLowerInvariant()
            : "en";

        // Only the UI language changes; formats (dates, numbers) stay invariant so form values keep working.
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture("en-US", selected)),
            new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                IsEssential = true,
                SameSite = SameSiteMode.Lax,
                HttpOnly = true
            });

        return !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
            ? LocalRedirect(returnUrl)
            : RedirectToAction("Index", "Home");
    }
}
