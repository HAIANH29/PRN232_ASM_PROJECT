using System.ComponentModel.DataAnnotations;

namespace LongevityDiet.Identity.Api.Contracts;

public sealed record UpdatePreferencesRequest
{
    [MaxLength(20)]
    public IReadOnlyCollection<string> PreferenceTags { get; init; } = Array.Empty<string>();

    [MaxLength(1000)]
    public string? Notes { get; init; }
}
