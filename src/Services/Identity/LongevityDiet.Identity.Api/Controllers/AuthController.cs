using System.Security.Claims;
using LongevityDiet.ApiDefaults.Api;
using LongevityDiet.Identity.Api.Contracts;
using LongevityDiet.Identity.Application.Models;
using LongevityDiet.Identity.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LongevityDiet.Identity.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(IIdentityService identityService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Register(
        RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var result = await identityService.RegisterAsync(
            new RegisterUserCommand(request.Email, request.Password, request.DisplayName),
            cancellationToken);

        return CreatedAtAction(
            nameof(GetProfile),
            null,
            ApiResponse<AuthResponse>.Success(
                MapAuthResponse(result),
                "Registration completed.",
                HttpContext.TraceIdentifier));
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Login(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var result = await identityService.LoginAsync(
            new LoginCommand(request.Email, request.Password),
            cancellationToken);

        return Ok(ApiResponse<AuthResponse>.Success(
            MapAuthResponse(result),
            "Login completed.",
            HttpContext.TraceIdentifier));
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<ActionResult<ApiResponse<UserProfileResponse>>> GetProfile(
        CancellationToken cancellationToken)
    {
        var profile = await identityService.GetProfileAsync(GetCurrentUserId(), cancellationToken);

        return Ok(ApiResponse<UserProfileResponse>.Success(
            MapProfileResponse(profile),
            "Profile loaded.",
            HttpContext.TraceIdentifier));
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpGet("admin-check")]
    public ActionResult<ApiResponse<string>> AdminCheck()
    {
        return Ok(ApiResponse<string>.Success(
            "Admin authorization is working.",
            "Admin check passed.",
            HttpContext.TraceIdentifier));
    }

    private Guid GetCurrentUserId()
    {
        var userIdText = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdText, out var userId))
        {
            throw new UnauthorizedAccessException("The token does not contain a valid user id.");
        }

        return userId;
    }

    private static AuthResponse MapAuthResponse(AuthResult result)
    {
        return new AuthResponse(
            result.AccessToken,
            result.ExpiresAtUtc,
            MapProfileResponse(result.Profile));
    }

    private static UserProfileResponse MapProfileResponse(UserProfile profile)
    {
        return new UserProfileResponse(
            profile.Id,
            profile.Email,
            profile.DisplayName,
            profile.IsActive,
            profile.Roles,
            profile.CreatedAtUtc);
    }
}
