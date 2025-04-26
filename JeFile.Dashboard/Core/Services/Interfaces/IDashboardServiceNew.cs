using System;
using JeFile.Dashboard.Core.Models;
using JeFile.Dashboard.Infrastructure.Data;

namespace JeFile.Dashboard.Core.Services.Interfaces;

public interface IDashboardServiceNew
{
Task<DashboardWidgetsData> GetDashboardDataAsync(Guid lineId);
Task RefreshDashboardDataAsync(Guid lineId, MonitoringLineModel line, DateTime refreshTime);
}
