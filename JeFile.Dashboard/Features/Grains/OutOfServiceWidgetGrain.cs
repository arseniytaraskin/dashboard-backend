using System;
using JeFile.Dashboard.Core.enums;
using JeFile.Dashboard.Core.Models;
using JeFile.Dashboard.Features.InterfacesGrain;
using JeFile.Dashboard.Features.Model;

namespace JeFile.Dashboard.Features.Grains;

public class OutOfServiceWidgetGrain  : Grain, IOutOfServiceWidgetGrain
{ 
    private OutOfService _outOfService = new();

    public Task<OutOfService> GetOutOfServicePositionsCountAsync()
    {
        return Task.FromResult(_outOfService);
    }

    public Task RefreshAsync(MonitoringLineModel line, DateTime refreshTime)
    {
        
        var lineId = this.GetPrimaryKey(out string widgetTypeStr);

        if (!Enum.TryParse(widgetTypeStr, out WidgetType widgetType))
        {
            throw new ArgumentException("Неверный тип виджета");
        }

        
        if (line.LineId != lineId)
        {
            throw new ArgumentException("Неверный LineId");
        }

        var services = new HashSet<long>(line.WorkingServicePoints.SelectMany(x => x.EnabledServices));

        var outOfServiceCount = 0;
        foreach (var position in line.Positions)
        {
            if (position.State != MonitoringPositionState.InLine)
                continue;

            if (position.Identity == MonitoringPositionIdentity.Break
             || position.Identity == MonitoringPositionIdentity.Unknown)
                continue;

            if (!services.IsSupersetOf(position.Services.Select(x => x.Id)))
                outOfServiceCount++;
        }

        _outOfService = new OutOfService
        {
            OutOfServicePositionsCount = outOfServiceCount
        };

        return Task.CompletedTask;
    }
}

