using System.ComponentModel.DataAnnotations;

namespace LongevityDiet.Web.Models;

public sealed class LoginForm
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}

public sealed class RegisterForm
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Display name")]
    public string DisplayName { get; set; } = string.Empty;
}

public sealed record AuthResponse(
    string AccessToken,
    DateTimeOffset ExpiresAtUtc,
    UserProfileResponse Profile);

public sealed record UserProfileResponse(
    Guid Id,
    string Email,
    string DisplayName,
    bool IsActive,
    IReadOnlyCollection<string> Roles,
    DateTimeOffset CreatedAtUtc);
