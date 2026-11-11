using System.Security.Cryptography;
using System.Text;
using LongevityDiet.MealPlanning.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LongevityDiet.MealPlanning.Api.Controllers;

// Service-to-service endpoint for the Notification Worker. It is protected by a shared key and the
// API Gateway refuses every /meal-planning/internal path, so it is not reachable from the Web.
[ApiController]
[AllowAnonymous]
[ApiExplorerSettings(IgnoreApi = true)]
[Route("internal/reminders")]
public sealed class InternalRemindersController(
    IMealPlanningService mealPlanningService,
    IConfiguration configuration) : ControllerBase
{
    public const string KeyHeader = "X-Internal-Key";

    [HttpGet("{mealPlanItemId:guid}")]
    public async Task<IActionResult> IsCurrent(
        Guid mealPlanItemId,
        [FromQuery] DateTimeOffset scheduledForUtc,
        CancellationToken cancellationToken)
    {
        if (!HasValidKey())
        {
            return Unauthorized();
        }

        var current = await mealPlanningService.IsReminderCurrentAsync(
            mealPlanItemId,
            scheduledForUtc,
            cancellationToken);

        return Ok(new { current });
    }

    private bool HasValidKey()
    {
        var expected = configuration["InternalApi:Key"];
        if (string.IsNullOrWhiteSpace(expected) ||
            !Request.Headers.TryGetValue(KeyHeader, out var provided))
        {
            return false;
        }

        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(expected),
            Encoding.UTF8.GetBytes(provided.ToString()));
    }
}
