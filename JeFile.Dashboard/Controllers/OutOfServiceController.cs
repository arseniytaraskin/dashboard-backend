using System;
using JeFile.Dashboard.Core.enums;
using JeFile.Dashboard.Core.Models;
using JeFile.Dashboard.Features.InterfacesGrain;
using Microsoft.AspNetCore.Mvc;
namespace JeFile.Dashboard.Controllers;
[ApiController]
[Route("api/OutOfService")]
public class OutOfServiceController : ControllerBase
{
    private readonly IGrainFactory _grainFactory;

    public OutOfServiceController(IGrainFactory grainFactory)
    {
        _grainFactory = grainFactory;
    }

    
    [HttpPost("refresh/{lineId}")]
    public async Task<IActionResult> RefreshLine(Guid lineId, [FromBody] MonitoringLineModel line)
    {
        try
        {
            
            var grain = _grainFactory.GetGrain<IOutOfServiceWidgetGrain>(
                lineId, 
                WidgetType.OutOfService.ToString() 
            );

            
            await grain.RefreshAsync(line, DateTime.UtcNow);

            return Ok("Данные виджета успешно обновлены.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ошибка при обновлении данных: {ex.Message}");
        }
    }

    
    [HttpGet("statistics/{lineId}")]
    public async Task<IActionResult> GetStatistics(Guid lineId)
    {
        try
        {
            
            var grain = _grainFactory.GetGrain<IOutOfServiceWidgetGrain>(
                lineId, 
                WidgetType.OutOfService.ToString() 
            );

            
            var statistics = await grain.GetOutOfServicePositionsCountAsync();

            return Ok(statistics);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ошибка при получении данных: {ex.Message}");
        }
    }
}

