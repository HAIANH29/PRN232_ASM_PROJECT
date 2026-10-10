namespace LongevityDiet.Web.Models;

public sealed record ApiError(
    string Code,
    string Message,
    string? Target = null);

public sealed record ApiResponse<T>(
    bool Succeeded,
    T? Data,
    string? Message,
    IReadOnlyCollection<ApiError> Errors,
    string? TraceId);

public sealed record PagedResponse<T>(
    IReadOnlyCollection<T> Items,
    int PageNumber,
    int PageSize,
    int TotalCount)
{
    public int TotalPages => PageSize <= 0
        ? 0
        : (int)Math.Ceiling(TotalCount / (double)PageSize);
}
