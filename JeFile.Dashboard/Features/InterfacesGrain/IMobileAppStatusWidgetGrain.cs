using System;
using JeFile.Dashboard.Core.Models;
using JeFile.Dashboard.Features.Model;

namespace JeFile.Dashboard.Features.InterfacesGrain;

public interface IMobileAppStatusWidgetGrain : IGrainWithGuidCompoundKey
{
    Task<MobileAppStatus> GetMobileAppStatusAsync();
    Task RefreshAsync(MonitoringLineModel line, DateTime refreshTime);
}
