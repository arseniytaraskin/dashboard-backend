using System;
using JeFile.Dashboard.Core.enums;
using JeFile.Dashboard.Core.Models;
using JeFile.Dashboard.Features.InterfacesGrain;
using Microsoft.AspNetCore.Mvc;
namespace JeFile.Dashboard.Controllers;
[ApiController]
[Route("api/lineStatistics")]
public class LineStatisticsController : ControllerBase
{
private readonly IGrainFactory _grainFactory;

public LineStatisticsController(IGrainFactory grainFactory)
{
    _grainFactory = grainFactory;
}

[HttpPost("refresh/{lineId}")]
public async Task<IActionResult> RefreshLine(Guid lineId, [FromBody] MonitoringLineModel line, DateTime refreshTime)
{
    var grain = _grainFactory.GetGrain<ILineStatisticsWidgetGrain>(
        lineId, 
        WidgetType.LineStatistics.ToString()
        );
    await grain.RefreshAsync(line, refreshTime);
    return Ok("Статистика линии успешно обновлена.");
}

[HttpGet("statistics/{lineId}")]
public async Task<IActionResult> GetStatistics(Guid lineId)
{
    var grain = _grainFactory.GetGrain<ILineStatisticsWidgetGrain>(
        lineId,
        WidgetType.LineStatistics.ToString()
        );
    var statistics = await grain.GetStatisticsAsync();
    return Ok(statistics);
}
}
