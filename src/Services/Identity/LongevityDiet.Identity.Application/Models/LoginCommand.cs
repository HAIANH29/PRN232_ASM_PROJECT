namespace LongevityDiet.Identity.Application.Models;

public sealed record LoginCommand(
    string Email,
    string Password);
