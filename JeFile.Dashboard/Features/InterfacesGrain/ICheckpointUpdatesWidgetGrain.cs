using System;
using JeFile.Dashboard.Core.Models;

namespace JeFile.Dashboard.Features.InterfacesGrain;

public interface ICheckpointUpdatesWidgetGrain : IGrainWithGuidKey
{
    Task<int> GetCheckpointsUpdatesCountAsync();
    Task RefreshAsync(MonitoringLineModel line, DateTime refreshTime);
}
