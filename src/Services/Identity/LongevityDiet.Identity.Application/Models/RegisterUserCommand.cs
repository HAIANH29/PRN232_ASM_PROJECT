namespace LongevityDiet.Identity.Application.Models;

public sealed record RegisterUserCommand(
    string Email,
    string Password,
    string DisplayName);
