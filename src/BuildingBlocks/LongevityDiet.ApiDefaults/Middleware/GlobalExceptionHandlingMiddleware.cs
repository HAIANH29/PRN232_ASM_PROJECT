using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;
using LongevityDiet.ApiDefaults.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LongevityDiet.ApiDefaults.Middleware;

public sealed class GlobalExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<GlobalExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            if (context.Response.HasStarted)
            {
                logger.LogWarning(exception, "The response has already started.");
                throw;
            }

            await WriteErrorResponseAsync(context, exception);
        }
    }

    private async Task WriteErrorResponseAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message, error) = MapException(exception);

        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            logger.LogError(exception, "Unhandled exception.");
        }
        else
        {
            logger.LogWarning(exception, "Handled request exception.");
        }

        context.Response.Clear();
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var response = ApiResponse<object?>.Failure(
            new[] { error },
            message,
            context.TraceIdentifier);

        await context.Response.WriteAsJsonAsync(response);
    }

    private static (int StatusCode, string Message, ApiError Error) MapException(Exception exception)
    {
        return exception switch
        {
            ValidationException validationException => (
                StatusCodes.Status400BadRequest,
                "Validation failed.",
                new ApiError("Validation.Invalid", validationException.Message)),

            ArgumentException argumentException => (
                StatusCodes.Status400BadRequest,
                "Invalid request.",
                new ApiError("Request.Invalid", argumentException.Message)),

            UnauthorizedAccessException => (
                StatusCodes.Status403Forbidden,
                "Access denied.",
                new ApiError("Authorization.Forbidden", "You do not have access to this resource.")),

            AuthenticationException authenticationException => (
                StatusCodes.Status401Unauthorized,
                "Authentication failed.",
                new ApiError("Authentication.InvalidCredentials", authenticationException.Message)),

            KeyNotFoundException keyNotFoundException => (
                StatusCodes.Status404NotFound,
                "Resource not found.",
                new ApiError("Resource.NotFound", keyNotFoundException.Message)),

            InvalidOperationException invalidOperationException => (
                StatusCodes.Status409Conflict,
                "Request could not be completed.",
                new ApiError("Request.Conflict", invalidOperationException.Message)),

            _ => (
                StatusCodes.Status500InternalServerError,
                "Unexpected server error.",
                new ApiError("Server.Unexpected", "An unexpected error occurred."))
        };
    }
}
