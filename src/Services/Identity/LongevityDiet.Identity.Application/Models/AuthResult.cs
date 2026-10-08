namespace LongevityDiet.Identity.Application.Models;

public sealed record AuthResult(
    string AccessToken,
    DateTimeOffset ExpiresAtUtc,
    UserProfile Profile);
