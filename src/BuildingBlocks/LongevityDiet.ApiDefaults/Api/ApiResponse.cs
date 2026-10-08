namespace LongevityDiet.ApiDefaults.Api;

public sealed record ApiResponse<T>(
    bool Succeeded,
    T? Data,
    string? Message,
    IReadOnlyCollection<ApiError> Errors,
    string? TraceId)
{
    public static ApiResponse<T> Success(T data, string? message = null, string? traceId = null)
    {
        return new ApiResponse<T>(
            Succeeded: true,
            Data: data,
            Message: message,
            Errors: Array.Empty<ApiError>(),
            TraceId: traceId);
    }

    public static ApiResponse<T> Failure(
        IReadOnlyCollection<ApiError> errors,
        string? message = null,
        string? traceId = null)
    {
        return new ApiResponse<T>(
            Succeeded: false,
            Data: default,
            Message: message,
            Errors: errors,
            TraceId: traceId);
    }
}
