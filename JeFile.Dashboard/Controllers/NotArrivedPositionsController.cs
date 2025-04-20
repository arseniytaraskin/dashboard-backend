using System;
using JeFile.Dashboard.Core.enums;
using JeFile.Dashboard.Core.Models;
using JeFile.Dashboard.Features.InterfacesGrain;
using Microsoft.AspNetCore.Mvc;

namespace JeFile.Dashboard.Controllers;

[ApiController]
[Route("api/NotArrivedPositions")]
public class NotArrivedPositionsController : ControllerBase
{
    private readonly IGrainFactory _grainFactory;

    public NotArrivedPositionsController(IGrainFactory grainFactory)
    {
        _grainFactory = grainFactory;
    }

    // Обновление данных для виджета
    [HttpPost("refresh/{lineId}")]
    public async Task<IActionResult> RefreshLine(Guid lineId, [FromBody] MonitoringLineModel line)
    {
        try
        {
            // Получаем grain по составному ключу (lineId и тип виджета)
            var grain = _grainFactory.GetGrain<INotArrivedPositionsWidgetGrain>(
                lineId, // Идентификатор линии
                WidgetType.NotArrivedPositions.ToString() // Тип виджета (строка)
            );

            
            await grain.RefreshAsync(line, DateTime.UtcNow);

            return Ok("Данные виджета успешно обновлены.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ошибка при обновлении данных: {ex.Message}");
        }
    }

    // Получение статистики для виджета
    [HttpGet("statistics/{lineId}")]
    public async Task<IActionResult> GetStatistics(Guid lineId)
    {
        try
        {
            
            var grain = _grainFactory.GetGrain<INotArrivedPositionsWidgetGrain>(
                lineId, 
                WidgetType.NotArrivedPositions.ToString() // Тип виджета (строка)
            );

            // Получаем данные из grain
            var statistics = await grain.GetNotArrivedPositionsAsync();

            return Ok(statistics);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ошибка при получении данных: {ex.Message}");
        }
    }
}
