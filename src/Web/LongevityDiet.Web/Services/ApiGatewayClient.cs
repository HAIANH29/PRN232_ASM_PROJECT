using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using LongevityDiet.Web.Models;

namespace LongevityDiet.Web.Services;

public sealed class ApiGatewayClient(HttpClient httpClient, ILogger<ApiGatewayClient> logger)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<ApiCallResult<T>> GetAsync<T>(
        string path,
        string? token = null,
        CancellationToken cancellationToken = default)
    {
        using var request = BuildRequest(HttpMethod.Get, path, token);
        return await SendAsync<T>(request, cancellationToken);
    }

    public async Task<ApiCallResult<T>> PostAsync<T>(
        string path,
        object body,
        string? token = null,
        CancellationToken cancellationToken = default)
    {
        using var request = BuildRequest(HttpMethod.Post, path, token, body);
        return await SendAsync<T>(request, cancellationToken);
    }

    public async Task<ApiCallResult<T>> PutAsync<T>(
        string path,
        object body,
        string? token = null,
        CancellationToken cancellationToken = default)
    {
        using var request = BuildRequest(HttpMethod.Put, path, token, body);
        return await SendAsync<T>(request, cancellationToken);
    }

    public async Task<ApiCallResult<T>> PatchAsync<T>(
        string path,
        object body,
        string? token = null,
        CancellationToken cancellationToken = default)
    {
        using var request = BuildRequest(HttpMethod.Patch, path, token, body);
        return await SendAsync<T>(request, cancellationToken);
    }

    public async Task<ApiCallResult> DeleteAsync(
        string path,
        string? token = null,
        CancellationToken cancellationToken = default)
    {
        using var request = BuildRequest(HttpMethod.Delete, path, token);
        using var response = await httpClient.SendAsync(request, cancellationToken);
        if (response.StatusCode == HttpStatusCode.NoContent)
        {
            return ApiCallResult.Success("Deleted.");
        }

        var result = await ReadEnvelopeAsync<object>(response, cancellationToken);
        return new ApiCallResult(
            response.IsSuccessStatusCode && (result?.Succeeded ?? true),
            result?.Message ?? DefaultMessage(response),
            result?.Errors ?? Array.Empty<ApiError>(),
            response.StatusCode);
    }

    private static HttpRequestMessage BuildRequest(
        HttpMethod method,
        string path,
        string? token,
        object? body = null)
    {
        var request = new HttpRequestMessage(method, path);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        if (body is not null)
        {
            request.Content = JsonContent.Create(body, options: JsonOptions);
        }

        return request;
    }

    private async Task<ApiCallResult<T>> SendAsync<T>(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        using var response = await httpClient.SendAsync(request, cancellationToken);
        var envelope = await ReadEnvelopeAsync<T>(response, cancellationToken);
        if (envelope is null)
        {
            return new ApiCallResult<T>(
                false,
                default,
                DefaultMessage(response),
                [new ApiError("HttpError", "The gateway response could not be read.")],
                response.StatusCode);
        }

        return new ApiCallResult<T>(
            response.IsSuccessStatusCode && envelope.Succeeded,
            envelope.Data,
            envelope.Message ?? DefaultMessage(response),
            envelope.Errors,
            response.StatusCode);
    }

    private async Task<ApiResponse<T>?> ReadEnvelopeAsync<T>(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        try
        {
            return await response.Content.ReadFromJsonAsync<ApiResponse<T>>(
                JsonOptions,
                cancellationToken);
        }
        catch (Exception exception)
        {
            var text = await response.Content.ReadAsStringAsync(cancellationToken);
            logger.LogWarning(
                exception,
                "Failed to parse gateway response {StatusCode}: {Body}",
                (int)response.StatusCode,
                Truncate(text));
            return null;
        }
    }

    public static string BuildQuery(
        string path,
        params (string Name, object? Value)[] values)
    {
        var query = values
            .Where(item => item.Value is not null && !string.IsNullOrWhiteSpace(item.Value.ToString()))
            .Select(item => $"{Uri.EscapeDataString(item.Name)}={Uri.EscapeDataString(Format(item.Value!))}")
            .ToArray();

        return query.Length == 0 ? path : $"{path}?{string.Join("&", query)}";
    }

    private static string Format(object value)
    {
        return value switch
        {
            DateOnly date => date.ToString("yyyy-MM-dd"),
            DateTimeOffset dateTime => dateTime.ToString("O"),
            bool boolean => boolean ? "true" : "false",
            _ => value.ToString() ?? string.Empty
        };
    }

    private static string DefaultMessage(HttpResponseMessage response)
    {
        return response.IsSuccessStatusCode
            ? "Request completed."
            : $"Request failed with HTTP {(int)response.StatusCode}.";
    }

    private static string Truncate(string value)
    {
        return value.Length <= 600 ? value : value[..600];
    }
}

public sealed record ApiCallResult<T>(
    bool Succeeded,
    T? Data,
    string Message,
    IReadOnlyCollection<ApiError> Errors,
    HttpStatusCode StatusCode);

public sealed record ApiCallResult(
    bool Succeeded,
    string Message,
    IReadOnlyCollection<ApiError> Errors,
    HttpStatusCode StatusCode)
{
    public static ApiCallResult Success(string message)
    {
        return new ApiCallResult(true, message, Array.Empty<ApiError>(), HttpStatusCode.OK);
    }
}
