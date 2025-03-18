using System;
using JeFile.Dashboard.Core.Models;
using JeFile.Dashboard.Features.Model;

namespace JeFile.Dashboard.Features.InterfacesGrain;

public interface ILineStatisticsWidgetGrain : IGrainWithGuidKey
{
    Task<LineStatistics> GetStatisticsAsync();
    Task RefreshAsync(MonitoringLineModel line, DateTime refreshTime);
}
