using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;

namespace JeFile.Dashboard.Core.Services.Interfaces;

public interface IDashboard : IGrainWithGuidKey
{
    Task<IReadOnlyList<IDashboardMonitoringGroup>> GetMonitoringGroups();
}
