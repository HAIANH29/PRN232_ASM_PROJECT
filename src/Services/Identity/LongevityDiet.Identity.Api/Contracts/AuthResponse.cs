namespace LongevityDiet.Identity.Api.Contracts;

public sealed record AuthResponse(
    string AccessToken,
    DateTimeOffset ExpiresAtUtc,
    UserProfileResponse Profile);
