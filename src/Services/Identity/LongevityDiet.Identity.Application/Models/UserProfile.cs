namespace LongevityDiet.Identity.Application.Models;

public sealed record UserProfile(
    Guid Id,
    string Email,
    string DisplayName,
    bool IsActive,
    IReadOnlyCollection<string> Roles,
    DateTimeOffset CreatedAtUtc,
    IReadOnlyCollection<string>? PreferenceTags = null,
    string? Notes = null);

public sealed record UpdatePreferencesCommand(
    IReadOnlyCollection<string> PreferenceTags,
    string? Notes);
