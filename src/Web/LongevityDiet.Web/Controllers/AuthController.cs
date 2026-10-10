using LongevityDiet.Web.Models;
using LongevityDiet.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LongevityDiet.Web.Controllers;

public sealed class AuthController(ApiGatewayClient api, UserSession session)
    : AppController(api, session)
{
    [HttpGet]
    public IActionResult Login()
    {
        return View(new LoginForm());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginForm form, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(form);
        }

        var result = await Api.PostAsync<AuthResponse>(
            "/identity/api/auth/login",
            new { form.Email, form.Password },
            cancellationToken: cancellationToken);

        if (!result.Succeeded || result.Data is null)
        {
            ModelState.AddModelError(string.Empty, FormatErrors(result.Message, result.Errors));
            return View(form);
        }

        Session.SignIn(
            result.Data.AccessToken,
            result.Data.Profile.Email,
            result.Data.Profile.DisplayName,
            result.Data.Profile.Roles,
            result.Data.ExpiresAtUtc);

        TempData["Success"] = "Signed in.";
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View(new RegisterForm());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterForm form, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(form);
        }

        var result = await Api.PostAsync<AuthResponse>(
            "/identity/api/auth/register",
            new { form.Email, form.Password, form.DisplayName },
            cancellationToken: cancellationToken);

        if (!result.Succeeded || result.Data is null)
        {
            ModelState.AddModelError(string.Empty, FormatErrors(result.Message, result.Errors));
            return View(form);
        }

        Session.SignIn(
            result.Data.AccessToken,
            result.Data.Profile.Email,
            result.Data.Profile.DisplayName,
            result.Data.Profile.Roles,
            result.Data.ExpiresAtUtc);

        TempData["Success"] = "Account created.";
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        Session.SignOut();
        TempData["Success"] = "Signed out.";
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Profile(CancellationToken cancellationToken)
    {
        var guard = RequireUser();
        if (guard is not null)
        {
            return guard;
        }

        var result = await Api.GetAsync<UserProfileResponse>(
            "/identity/api/auth/profile",
            Token,
            cancellationToken);

        if (!result.Succeeded || result.Data is null)
        {
            TempData["Error"] = FormatErrors(result.Message, result.Errors);
            return RedirectToAction("Login");
        }

        return View(result.Data);
    }
}
