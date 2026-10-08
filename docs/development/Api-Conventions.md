# API Conventions

This document defines the baseline REST API conventions for the public business APIs:

- Identity Service
- Diet Knowledge Service
- Meal Planning Service
- Tracking Service

The shared implementation lives in `src/BuildingBlocks/LongevityDiet.ApiDefaults`.

## Startup Pattern

Public REST APIs should register the shared defaults:

```csharp
builder.Services.AddLongevityPublicApiDefaults("Service Display Name");
```

They should apply the shared middleware/endpoints:

```csharp
app.UseLongevityPublicApiDefaults();
```

This keeps Swagger, JWT bearer setup, health checks, validation error responses, and global exception handling consistent across services.

## Success Response

Controllers should return `ApiResponse<T>` for normal JSON responses.

Shape:

```json
{
  "succeeded": true,
  "data": {},
  "message": "Optional message.",
  "errors": [],
  "traceId": "request-trace-id"
}
```

Use:

```csharp
return Ok(ApiResponse<MyDto>.Success(dto, "Optional message.", HttpContext.TraceIdentifier));
```

## Error Response

Validation and unhandled exceptions use the same envelope shape with `succeeded = false`.

Shape:

```json
{
  "succeeded": false,
  "data": null,
  "message": "Validation failed.",
  "errors": [
    {
      "code": "Validation.Invalid",
      "message": "The Name field is required.",
      "target": "name"
    }
  ],
  "traceId": "request-trace-id"
}
```

Do not expose stack traces or internal exception details to clients.

## Validation

Use request DTOs with `System.ComponentModel.DataAnnotations` attributes first. The shared API defaults convert invalid model state into the common error response.

Example:

```csharp
public sealed record CreateFoodRequest
{
    [Required]
    [MaxLength(160)]
    public string Name { get; init; } = string.Empty;
}
```

Service/application-level validation may throw `ValidationException` for cross-field or use-case rules.

## Pagination

List endpoints should accept `PageRequest` when pagination is needed.

Defaults:
- `PageNumber = 1`
- `PageSize = 20`
- maximum `PageSize = 100`

List endpoints should return `ApiResponse<PagedResponse<T>>`.

## Exception Mapping

The shared middleware currently maps common exceptions as follows:

| Exception | HTTP status |
| --- | --- |
| `ValidationException` | 400 |
| `ArgumentException` | 400 |
| `UnauthorizedAccessException` | 403 |
| `KeyNotFoundException` | 404 |
| `InvalidOperationException` | 409 |
| other exceptions | 500 |

Future custom domain/application exceptions should be added to the shared middleware instead of handled differently in each API.

## Swagger

Swagger is enabled for public REST APIs and includes a Bearer JWT security definition. Identity does not issue real JWTs yet; this prepares the API surface for Milestone 1.

## Package Management

Package versions are centrally managed in `Directory.Packages.props`.

Rules:
- Project files may add `PackageReference` entries without versions.
- New or upgraded package versions must be added to `Directory.Packages.props`.
- Do not pin package versions inside individual `.csproj` files.
