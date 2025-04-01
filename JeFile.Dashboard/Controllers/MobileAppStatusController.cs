using System;
using JeFile.Dashboard.Core.enums;
using JeFile.Dashboard.Core.Models;
using JeFile.Dashboard.Features.InterfacesGrain;
using Microsoft.AspNetCore.Mvc;

namespace JeFile.Dashboard.Controllers;

[ApiController]
[Route("api/mobileAppStatus")]
public class MobileAppStatusController : ControllerBase
{
    private readonly IGrainFactory _grainFactory;

    public MobileAppStatusController(IGrainFactory grainFactory)
    {
        _grainFactory = grainFactory;
    }

    [HttpPost("refresh/{lineId}")]
    public async Task<IActionResult> RefreshStatus(Guid lineId, [FromBody] MonitoringLineModel line)
    {
        try
        {
            var grain = _grainFactory.GetGrain<IMobileAppStatusWidgetGrain>(
                lineId,
                WidgetType.MobileAppStatus.ToString()
            );

            await grain.RefreshAsync(line, DateTime.UtcNow);
            return Ok("Mobile app status updated successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error updating status: {ex.Message}");
        }
    }

    [HttpGet("status/{lineId}")]
    public async Task<IActionResult> GetStatus(Guid lineId)
    {
        try
        {
            var grain = _grainFactory.GetGrain<IMobileAppStatusWidgetGrain>(
                lineId,
                WidgetType.MobileAppStatus.ToString()
            );

            var status = await grain.GetMobileAppStatusAsync();
            return Ok(status);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error getting status: {ex.Message}");
        }
    }
}
