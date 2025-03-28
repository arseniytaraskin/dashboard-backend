using System;
using JeFile.Dashboard.Core.Models;
using JeFile.Dashboard.Features.Model;

namespace JeFile.Dashboard.Features.InterfacesGrain;

public interface IOutOfServiceWidgetGrain : IGrainWithGuidCompoundKey
{
    Task<OutOfService> GetOutOfServicePositionsCountAsync();
    Task RefreshAsync(MonitoringLineModel line, DateTime refreshTime);
}
