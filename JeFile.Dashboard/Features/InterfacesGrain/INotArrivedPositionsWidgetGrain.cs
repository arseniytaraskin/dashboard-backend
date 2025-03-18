using System;
using JeFile.Dashboard.Core.Models;
using JeFile.Dashboard.Features.Model;

namespace JeFile.Dashboard.Features.InterfacesGrain;

public interface INotArrivedPositionsWidgetGrain : IGrainWithGuidCompoundKey
{
    Task<NotArrivedPositions> GetNotArrivedPositionsAsync();
    Task RefreshAsync(MonitoringLineModel line, DateTime refreshTime);
}
