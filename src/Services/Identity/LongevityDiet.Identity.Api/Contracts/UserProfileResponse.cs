namespace LongevityDiet.Identity.Api.Contracts;

public sealed record UserProfileResponse(
    Guid Id,
    string Email,
    string DisplayName,
    bool IsActive,
    IReadOnlyCollection<string> Roles,
    DateTimeOffset CreatedAtUtc);
