using JeFile.Dashboard.Features.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System.Threading.Tasks;

[Route("~/dashboard/map/monitoring/list")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly IClusterClient _clusterClient;

    public DashboardController(IClusterClient clusterClient)
    {
        _clusterClient = clusterClient;
    }

    [HttpGet]
    public async Task<IActionResult> MapMonitoringList()
    {
        var grain = _clusterClient.GetGrain<IDashboardMapMonitoringListGrain>(Guid.NewGuid());
        var result = await grain.GetMonitoringListAsync();
        return Ok(result);
    }
}
