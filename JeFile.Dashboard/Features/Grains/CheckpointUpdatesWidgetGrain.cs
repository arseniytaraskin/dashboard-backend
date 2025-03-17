using System;
using JeFile.Dashboard.Core.Models;
using JeFile.Dashboard.Features.InterfacesGrain;

namespace JeFile.Dashboard.Features.Grains;

public class CheckpointUpdatesWidgetGrain : Grain, ICheckpointUpdatesWidgetGrain
{
    private int _checkpointsUpdatesCount;

    public Task<int> GetCheckpointsUpdatesCountAsync()
    {
        return Task.FromResult(_checkpointsUpdatesCount);
    }

    public Task RefreshAsync(MonitoringLineModel line, DateTime refreshTime)
    {
    
        if (line.LineId != this.GetPrimaryKey())
        {
            throw new ArgumentException("Неверный LineId");
        }

        // Обновляние данных
        _checkpointsUpdatesCount = line.Checkpoints.Select(x => x.UpdatesCount).Sum();

        return Task.CompletedTask;
    }
}
