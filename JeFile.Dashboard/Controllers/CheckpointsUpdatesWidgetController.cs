using System;
using JeFile.Dashboard.Core.enums;
using JeFile.Dashboard.Core.Models;
using JeFile.Dashboard.Features.InterfacesGrain;
using Microsoft.AspNetCore.Mvc;
namespace JeFile.Dashboard.Controllers;

[ApiController]
[Route("api/checkpointsUpdatesWidget")]
public class CheckpointsUpdatesWidgetController : ControllerBase
{
    private readonly IGrainFactory _grainFactory;

public CheckpointsUpdatesWidgetController(IGrainFactory grainFactory)
{
    _grainFactory = grainFactory;
}

[HttpPost("refresh")]
public async Task<IActionResult> RefreshLine(Guid lineId, [FromBody] MonitoringLineModel line, DateTime refreshTime)
{
    try
    {
        
        var grain = _grainFactory.GetGrain<ICheckpointUpdatesWidgetGrain>(
            lineId,
            WidgetType.CheckpointUpdates.ToString());

        
        await grain.RefreshAsync(line, refreshTime);

        return Ok("Данные линии успешно обновлены.");
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Ошибка при обновлении данных линии: {ex.Message}");
    }
}

[HttpGet("updates-count/{lineId}")]
public async Task<IActionResult> GetUpdatesCount(Guid lineId)
{
    try
    {
        
        var grain = _grainFactory.GetGrain<ICheckpointUpdatesWidgetGrain>(
            lineId,
            WidgetType.CheckpointUpdates.ToString()
            );

        
        var updatesCount = await grain.GetCheckpointsUpdatesCountAsync();

        return Ok(new { LineId = lineId, UpdatesCount = updatesCount });
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Ошибка при получении количества обновлений: {ex.Message}");
    }
}
}
