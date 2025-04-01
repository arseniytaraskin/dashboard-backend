using System;
using JeFile.Dashboard.Core.enums;
using JeFile.Dashboard.Core.Models;
using JeFile.Dashboard.Features.InterfacesGrain;
using JeFile.Dashboard.Features.Model;
using JeFile.Dashboard.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace JeFile.Dashboard.Features.Grains;

public class MobileAppStatusWidgetGrain : Grain, IMobileAppStatusWidgetGrain
{
private readonly IServiceProvider _serviceProvider;
private MobileAppStatus _status = new() { Available = false };

public MobileAppStatusWidgetGrain(IServiceProvider serviceProvider)
{
    _serviceProvider = serviceProvider;
}

public Task<MobileAppStatus> GetMobileAppStatusAsync()
{
    return Task.FromResult(_status);
}

public async Task RefreshAsync(MonitoringLineModel line, DateTime refreshTime)
{
    var lineId = this.GetPrimaryKey(out string widgetTypeStr);

    if (!Enum.TryParse(widgetTypeStr, out WidgetType widgetType))
    {
        throw new ArgumentException("Invalid widget type");
    }

    using var scope = _serviceProvider.CreateScope();
    var lineRepository = scope.ServiceProvider.GetRequiredService<ILineRepository>();
    var lineMetadata = await lineRepository.FindAsync(line.LineId, default);

    _status = new MobileAppStatus
    {
        Available = lineMetadata.Mode.HasFlag(LineMode.Mobileapp)
    };
}
}
