namespace LongevityDiet.Identity.Application.Services;

public sealed record IdentityServiceStatus(
    string Service,
    string Database,
    IReadOnlyCollection<string> Owns);
