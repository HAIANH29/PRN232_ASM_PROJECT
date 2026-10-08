namespace LongevityDiet.Identity.Application.Abstractions;

public sealed record TokenResult(
    string AccessToken,
    DateTimeOffset ExpiresAtUtc);
