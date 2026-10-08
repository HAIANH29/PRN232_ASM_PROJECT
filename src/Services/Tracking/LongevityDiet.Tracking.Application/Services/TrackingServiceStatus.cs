namespace LongevityDiet.Tracking.Application.Services;

public sealed record TrackingServiceStatus(
    string Service,
    string Database,
    IReadOnlyCollection<string> Owns);
