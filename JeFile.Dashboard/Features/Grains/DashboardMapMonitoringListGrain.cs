
using System.Threading.Tasks;
using Orleans;
using JeFile.Dashboard.Core.Services;
using JeFile.Dashboard.Core.Services.Interfaces;
using JeFile.Dashboard.Features.Model;
using JeFile.Dashboard.Features.Interfaces;

public class DashboardMapMonitoringListGrain : Grain, IDashboardMapMonitoringListGrain
{
    private readonly IDashboardService _dashboardService;

    public DashboardMapMonitoringListGrain(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public async Task<MonitoringMapListItem[]> GetMonitoringListAsync()
    {
        // Упрощенная логика: просто получаем данные из сервиса
        var companyId = "defaultCompanyId"; // Заглушка вместо реального ID компании
        var language = "en"; // Заглушка вместо реального языка
        var dashboard = await _dashboardService.GetCompanyDashboard(companyId, language, CancellationToken.None);

        if (dashboard == null)
            throw new InvalidOperationException("Dashboard not found");

        var result = new List<MonitoringMapListItem>();

        foreach (var group in dashboard.MonitoringGroups)
        {
            var mapItem = new MonitoringMapListItem()
            {
                Id = group.Id,
                Latitude = group.DisplayLatitude,
                Longitude = group.DisplayLongitude,
                Name = group.Name,
                // Status = group.AttentionLevel.ConvertToIndicatorStatusColor()
            };

            result.Add(mapItem);
        }

        return result.ToArray();
    }
}