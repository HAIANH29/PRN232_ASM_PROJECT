namespace LongevityDiet.Identity.Domain.Entities;

// Stored in the UserProfiles table: the user's own eating preferences. The Recommendation flow
// can use these tags; they are never medical information.
public sealed class UserPreferenceProfile
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public User? User { get; set; }

    // Comma separated, normalized by the Application layer.
    public string PreferenceTags { get; set; } = string.Empty;

    public string Notes { get; set; } = string.Empty;

    public DateTimeOffset UpdatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}
