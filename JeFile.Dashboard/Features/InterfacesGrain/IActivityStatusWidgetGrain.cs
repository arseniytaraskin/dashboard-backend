using System;
using JeFile.Dashboard.Core.Models;
using JeFile.Dashboard.Features.Model;

namespace JeFile.Dashboard.Features.InterfacesGrain;

public interface IActivityStatusWidgetGrain : IGrainWithGuidCompoundKey
{
    Task<ActivityStatus> GetActivityStatusAsync();
    Task RefreshAsync(MonitoringLineModel line, DateTime refreshTime);
}
