using System;
using JeFile.Dashboard.Core.enums;
using JeFile.Dashboard.Core.Models;
using JeFile.Dashboard.Features.InterfacesGrain;
using Microsoft.AspNetCore.Mvc;

namespace JeFile.Dashboard.Controllers;

[ApiController]
[Route("api/priority-positions")]
public class PriorityPositionsController : ControllerBase
{
    private readonly IGrainFactory _grainFactory;

    public PriorityPositionsController(IGrainFactory grainFactory)
    {
        _grainFactory = grainFactory;
    }

    [HttpPost("refresh/{lineId}")]
    public async Task<IActionResult> Refresh(Guid lineId, [FromBody] MonitoringLineModel line)
    {
        var grain = _grainFactory.GetGrain<IPriorityPositionsWidgetGrain>(
            lineId,
            WidgetType.PriorityPositions.ToString()
        );
        await grain.RefreshAsync(line, DateTime.UtcNow);
        return Ok();
    }

    [HttpGet("statistics/{lineId}")]
    public async Task<IActionResult> GetStatistics(Guid lineId)
    {
        var grain = _grainFactory.GetGrain<IPriorityPositionsWidgetGrain>(
            lineId,
            WidgetType.PriorityPositions.ToString()
        );
        var stats = await grain.GetPriorityPositionsAsync();
        return Ok(stats);
    }
}
