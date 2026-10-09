using LongevityDiet.ApiDefaults.Api;
using LongevityDiet.Tracking.Api.Contracts;
using LongevityDiet.Tracking.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LongevityDiet.Tracking.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/progress-summaries")]
public sealed class ProgressSummariesController(ITrackingService trackingService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<ProgressSummaryResponse>>> Get(
        [FromQuery] ProgressSummaryRequest request,
        CancellationToken cancellationToken)
    {
        var result = await trackingService.GetProgressSummaryAsync(
            CurrentUser.GetUserId(User),
            request.PeriodStartDate,
            request.PeriodEndDate,
            cancellationToken);

        return Ok(ApiResponse<ProgressSummaryResponse>.Success(
            result.ToResponse(),
            "Progress summary loaded.",
            HttpContext.TraceIdentifier));
    }
}
