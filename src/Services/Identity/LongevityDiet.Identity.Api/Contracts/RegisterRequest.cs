using System.ComponentModel.DataAnnotations;

namespace LongevityDiet.Identity.Api.Contracts;

public sealed record RegisterRequest
{
    [Required]
    [EmailAddress]
    [MaxLength(256)]
    public string Email { get; init; } = string.Empty;

    [Required]
    [MinLength(8)]
    [MaxLength(100)]
    public string Password { get; init; } = string.Empty;

    [Required]
    [MinLength(2)]
    [MaxLength(120)]
    public string DisplayName { get; init; } = string.Empty;
}
