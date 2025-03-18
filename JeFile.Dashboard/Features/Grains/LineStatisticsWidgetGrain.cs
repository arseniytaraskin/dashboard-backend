using System;
using JeFile.Dashboard.Core.enums;
using JeFile.Dashboard.Core.Models;
using JeFile.Dashboard.Features.InterfacesGrain;
using JeFile.Dashboard.Features.Model;

namespace JeFile.Dashboard.Features.Grains;

public class LineStatisticsWidgetGrain : Grain, ILineStatisticsWidgetGrain
{
private LineStatistics _statistics = new();

public Task<LineStatistics> GetStatisticsAsync()
{
    return Task.FromResult(_statistics);
}

public Task RefreshAsync(MonitoringLineModel line, DateTime refreshTime)
{
    var finished = 0;
    var inService = 0;
    var inLine = 0;

    foreach (var position in line.Positions.Concat(line.RemovedPositions))
    {
        if (position.Identity == MonitoringPositionIdentity.Break)
            continue;

        if (position.State == MonitoringPositionState.Removed
            || position.State == MonitoringPositionState.Leaved
            || position.State == MonitoringPositionState.Unknown)
            continue;

        if (position.State == MonitoringPositionState.ManualFinished
            || position.State == MonitoringPositionState.AutoFinished)
        {
            finished++;
            continue;
        }

        if (position.State == MonitoringPositionState.ServiceStarted)
        {
            inService++;
            continue;
        }

        inLine++;
    }

    _statistics = new LineStatistics
    {
        PositionsInService = inService,
        PositionsInLine = inLine,
        FinishedPositions = finished,
    };

    return Task.CompletedTask;
}
}
