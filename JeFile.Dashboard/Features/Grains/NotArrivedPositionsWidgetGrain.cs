using System;
using JeFile.Dashboard.Core.enums;
using JeFile.Dashboard.Core.Models;
using JeFile.Dashboard.Features.InterfacesGrain;
using JeFile.Dashboard.Features.Model;

namespace JeFile.Dashboard.Features.Grains;

public class NotArrivedPositionsWidgetGrain : Grain, INotArrivedPositionsWidgetGrain
{
private NotArrivedPositions _notArrivedPositions = new();

public Task<NotArrivedPositions> GetNotArrivedPositionsAsync()
{
    return Task.FromResult(_notArrivedPositions);
}

public Task RefreshAsync(MonitoringLineModel line, DateTime refreshTime)
{
    var lineId = this.GetPrimaryKey(out string widgetTypeStr);

    
    if (!Enum.TryParse(widgetTypeStr, out WidgetType widgetType))
    {
        throw new ArgumentException("Неверный тип виджета");
    }

    
    var key = new WidgetKey(lineId, widgetType);

    
    if (key.LineId != line.LineId)
    {
        throw new ArgumentException("Неверный LineId");
    }

    var appointments = 0;
    var todayPositions = 0;
    var called = 0;
    var finished = 0;

    foreach (var position in line.RemovedPositions)
    {
        if (position.Identity == MonitoringPositionIdentity.Break)
            continue;

        if (position.State == MonitoringPositionState.Leaved
            || position.State == MonitoringPositionState.Unknown)
        {
            if (position.Identity == MonitoringPositionIdentity.Appointment)
                appointments++;
            else
                todayPositions++;
            continue;
        }

        if (position.State == MonitoringPositionState.Removed)
        {
            called++; 
        }

        if (position.State == MonitoringPositionState.ManualFinished
            || position.State == MonitoringPositionState.AutoFinished)
        {
            finished++;
        }
    }

    foreach (var position in line.Positions)
    {
        if (position.Identity == MonitoringPositionIdentity.Break)
            continue;

        if (position.State == MonitoringPositionState.ServiceStarted)
        {
            finished++;
        }
    }

    _notArrivedPositions = new NotArrivedPositions
    {
        NotArrivedCalledPositions = called,
        NotArrivedAppointments = appointments,
        NotArrivedTodayPositions = todayPositions,
        FinishedAndInServicePositions = finished
    };

    return Task.CompletedTask;
}
}
