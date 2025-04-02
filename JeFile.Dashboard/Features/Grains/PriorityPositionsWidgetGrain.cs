using System;
using JeFile.Dashboard.Core.enums;
using JeFile.Dashboard.Core.Models;
using JeFile.Dashboard.Features.InterfacesGrain;
using JeFile.Dashboard.Features.Model;

namespace JeFile.Dashboard.Features.Grains;

public class PriorityPositionsWidgetGrain : Grain, IPriorityPositionsWidgetGrain
{
    private PriorityPositions _priorityPositions = new();

    public Task<PriorityPositions> GetPriorityPositionsAsync()
    {
        return Task.FromResult(_priorityPositions);
    }

    public Task RefreshAsync(MonitoringLineModel line, DateTime refreshTime)
    {
        var notPriority = 0;
        var priority = 0;

        foreach (var position in line.Positions.Concat(line.RemovedPositions))
        {
            if (position.State == MonitoringPositionState.Removed
                || position.State == MonitoringPositionState.Leaved
                || position.State == MonitoringPositionState.Unknown)
                continue;

            if (position.PriorityLevel > 0)
                priority++;
            else
                notPriority++;
        }

        _priorityPositions = new PriorityPositions
        {
            TotalDayPrioriryPositions = priority,
            TotalDayNotPrioriryPositions = notPriority
        };

        return Task.CompletedTask;
    }
}
