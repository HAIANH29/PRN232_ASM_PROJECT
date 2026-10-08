namespace LongevityDiet.Identity.Application.Models;

public sealed record UserProfile(
    Guid Id,
    string Email,
    string DisplayName,
    bool IsActive,
    IReadOnlyCollection<string> Roles,
    DateTimeOffset CreatedAtUtc);
