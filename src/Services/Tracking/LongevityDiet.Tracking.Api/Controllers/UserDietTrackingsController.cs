using System.ComponentModel.DataAnnotations;
using LongevityDiet.ApiDefaults.Api;
using LongevityDiet.Tracking.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LongevityDiet.Tracking.Api.Controllers;

public sealed record UserDietTrackingResponse(
    Guid Id,
    Guid? DietRegimenId,
    Guid? MealPlanId,
    DateOnly StartDate,
    DateOnly? EndDate,
    string Status,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc);

public sealed record StartUserDietTrackingRequest
{
    public Guid? DietRegimenId { get; init; }

    public Guid? MealPlanId { get; init; }

    [Required]
    public DateOnly StartDate { get; init; }

    public DateOnly? EndDate { get; init; }
}

public sealed record FinishUserDietTrackingRequest
{
    [Required]
    [RegularExpression("^(?i)(Completed|Cancelled)$", ErrorMessage = "Status must be Completed or Cancelled.")]
    public string Status { get; init; } = string.Empty;
}

public sealed record UserDietTrackingListRequest
{
    [Range(1, int.MaxValue)]
    public int PageNumber { get; init; } = 1;

    [Range(1, 100)]
    public int PageSize { get; init; } = 20;

    [MaxLength(40)]
    public string? Status { get; init; }
}

[ApiController]
[Authorize]
[Route("api/diet-trackings")]
public sealed class UserDietTrackingsController(IUserDietTrackingService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResponse<UserDietTrackingResponse>>>> List(
        [FromQuery] UserDietTrackingListRequest request,
        CancellationToken cancellationToken)
    {
        var result = await service.ListAsync(
            CurrentUser.GetUserId(User),
            new UserDietTrackingQuery(request.PageNumber, request.PageSize, request.Status),
            cancellationToken);

        var response = new PagedResponse<UserDietTrackingResponse>(
            result.Items.Select(ToResponse).ToArray(),
            result.PageNumber,
            result.PageSize,
            result.TotalCount);

        return Ok(ApiResponse<PagedResponse<UserDietTrackingResponse>>.Success(
            response,
            "Diet trackings loaded.",
            HttpContext.TraceIdentifier));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<UserDietTrackingResponse>>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await service.GetAsync(CurrentUser.GetUserId(User), id, cancellationToken);

        return Ok(ApiResponse<UserDietTrackingResponse>.Success(
            ToResponse(result),
            "Diet tracking loaded.",
            HttpContext.TraceIdentifier));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<UserDietTrackingResponse>>> Start(
        StartUserDietTrackingRequest request,
        CancellationToken cancellationToken)
    {
        var result = await service.StartAsync(
            new StartUserDietTrackingCommand(
                CurrentUser.GetUserId(User),
                request.DietRegimenId,
                request.MealPlanId,
                request.StartDate,
                request.EndDate),
            cancellationToken);

        return Created(
            $"/api/diet-trackings/{result.Id}",
            ApiResponse<UserDietTrackingResponse>.Success(
                ToResponse(result),
                "Diet tracking started.",
                HttpContext.TraceIdentifier));
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<ActionResult<ApiResponse<UserDietTrackingResponse>>> Finish(
        Guid id,
        FinishUserDietTrackingRequest request,
        CancellationToken cancellationToken)
    {
        var result = await service.FinishAsync(
            CurrentUser.GetUserId(User),
            id,
            request.Status,
            cancellationToken);

        return Ok(ApiResponse<UserDietTrackingResponse>.Success(
            ToResponse(result),
            "Diet tracking updated.",
            HttpContext.TraceIdentifier));
    }

    private static UserDietTrackingResponse ToResponse(UserDietTrackingModel model)
    {
        return new UserDietTrackingResponse(
            model.Id,
            model.DietRegimenId,
            model.MealPlanId,
            model.StartDate,
            model.EndDate,
            model.Status,
            model.CreatedAtUtc,
            model.UpdatedAtUtc);
    }
}
