using JeFile.Dashboard.Features.Model;
using Orleans;

namespace JeFile.Dashboard.Features.Interfaces;

public interface IDashboardMapMonitoringListGrain : IGrainWithGuidKey
{
    Task<MonitoringMapListItem[]> GetMonitoringListAsync();
}
