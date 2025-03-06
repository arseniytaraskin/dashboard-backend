using System.Collections.Generic;
using Orleans;
using System.Threading.Tasks;

namespace JeFile.Dashboard.Core.Services.Interfaces;

public interface IDashboardMonitoringGroup : IGrainWithIntegerKey
{
    Task<long> GetId();
    Task<string> GetName();
    Task<int> GetCategoryId();
    Task<string> GetLineType();
    Task<IReadOnlyList<ILineDashboard>> GetDashboards();
    Task<double> GetDisplayLatitude();
    Task<double> GetDisplayLongitude();
    Task<DashboardAttentionLevel?> GetAttentionLevel();
    Task<int> GetDisplayOrder();
    Task<IDashboardGroupMap> GetMap();

}
