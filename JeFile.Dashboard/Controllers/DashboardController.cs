using JeFile.Dashboard.Core.Models;
using JeFile.Dashboard.Core.Services.Interfaces;
using JeFile.Dashboard.Features.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System.Threading.Tasks;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardServiceNew _dashboardService;

    public DashboardController(IDashboardServiceNew dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("{lineId}")]
    public async Task<IActionResult> GetDashboardData(Guid lineId)
    {
        var data = await _dashboardService.GetDashboardDataAsync(lineId);
        return Ok(data);
    }

    [HttpPost("refresh/{lineId}")]
    public async Task<IActionResult> RefreshDashboard(Guid lineId,
        [FromBody] MonitoringLineModel line,
        DateTime refreshTime)
    {
        await _dashboardService.RefreshDashboardDataAsync(lineId, line, refreshTime);
        return Ok("Данные дашборда обновлены");
    }
}
