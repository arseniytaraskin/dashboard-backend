using System;
using JeFile.Dashboard.Core.Models;
using JeFile.Dashboard.Features.Model;

namespace JeFile.Dashboard.Features.InterfacesGrain;

public interface IPriorityPositionsWidgetGrain : IGrainWithGuidCompoundKey
{
    Task<PriorityPositions> GetPriorityPositionsAsync();
    Task RefreshAsync(MonitoringLineModel line, DateTime refreshTime);
}
