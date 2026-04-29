// Backend.API/Controllers/AnalyticsController.cs
using Backend.Core.DTOs;
using Backend.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsServices _analyticsService;

    public AnalyticsController(IAnalyticsServices analyticsService)
    {
        _analyticsService = analyticsService;
    }

    [HttpGet("status-breakdown")]
    public async Task<ActionResult<IEnumerable<StatusBreakdownDto>>> GetStatusBreakdown()
    {
        var result = await _analyticsService.GetStatusBreakdownAsync();
        return Ok(result);
    }

    [HttpGet("weekly")]
    public async Task<ActionResult<IEnumerable<WeeklyApplicationsDto>>> GetWeekly(
        [FromQuery] int weeks = 4)
    {
        if (weeks < 1 || weeks > 52)
            return BadRequest("Weeks must be between 1 and 52.");

        var result = await _analyticsService.GetWeeklyApplicationsAsync(weeks);
        return Ok(result);
    }
}