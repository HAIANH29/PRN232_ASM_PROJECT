namespace LongevityDiet.ApiDefaults.Api;

public sealed record ApiError(
    string Code,
    string Message,
    string? Target = null);
