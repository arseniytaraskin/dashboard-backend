using System;
using JeFile.Dashboard.Core.enums;
using JeFile.Dashboard.Core.Models;
using JeFile.Dashboard.Core.Services.Interfaces;
using JeFile.Dashboard.Features.InterfacesGrain;
using JeFile.Dashboard.Features.Model;
using JeFile.Dashboard.Infrastructure.Data;

namespace JeFile.Dashboard.Core;

public class DashboardService : IDashboardServiceNew
{
    private readonly IGrainFactory _grainFactory;

    public DashboardService(IGrainFactory grainFactory)
    {
        _grainFactory = grainFactory;
    }

    public async Task<DashboardWidgetsData> GetDashboardDataAsync(Guid lineId)
    {
        var statsGrain = _grainFactory.GetGrain<ILineStatisticsWidgetGrain>(lineId, WidgetType.LineStatistics.ToString());
        var checkpointsGrain = _grainFactory.GetGrain<ICheckpointUpdatesWidgetGrain>(lineId, WidgetType.CheckpointUpdates.ToString());
        var priorityPositionsGrain = _grainFactory.GetGrain<IPriorityPositionsWidgetGrain>(lineId, WidgetType.PriorityPositions.ToString());

        var mobileStatusAppGrain = _grainFactory.GetGrain<IMobileAppStatusWidgetGrain>(lineId, WidgetType.MobileAppStatus.ToString());
        var notArrivedPositionsGrain = _grainFactory.GetGrain<INotArrivedPositionsWidgetGrain>(lineId, WidgetType.NotArrivedPositions.ToString());
        var outOfServiceGrain = _grainFactory.GetGrain<IOutOfServiceWidgetGrain>(lineId, WidgetType.OutOfService.ToString());

        var tasks = new Task[]
        {
        statsGrain.GetStatisticsAsync(),
        checkpointsGrain.GetCheckpointsUpdatesCountAsync(),
        priorityPositionsGrain.GetPriorityPositionsAsync(),

        mobileStatusAppGrain.GetMobileAppStatusAsync(),
        notArrivedPositionsGrain.GetNotArrivedPositionsAsync(),
        outOfServiceGrain.GetOutOfServicePositionsCountAsync(),

        };

        await Task.WhenAll(tasks);

        return new DashboardWidgetsData
        {
            LineStatistics = ((Task<LineStatistics>)tasks[0]).Result,
            CheckpointsUpdatesCount = ((Task<int>)tasks[1]).Result,
            PriorityPositions = ((Task<PriorityPositions>)tasks[2]).Result,

            MobileAppStatus = ((Task<MobileAppStatus>)tasks[3]).Result,
            NotArrivedPositions = ((Task<NotArrivedPositions>)tasks[4]).Result,
            OutOfService = ((Task<OutOfService>)tasks[5]).Result,

            LastUpdated = DateTime.UtcNow
        };
    }

    public async Task RefreshDashboardDataAsync(Guid lineId, MonitoringLineModel line, DateTime refreshTime)
    {
        var statsGrain = _grainFactory.GetGrain<ILineStatisticsWidgetGrain>(lineId, WidgetType.LineStatistics.ToString());
        var checkpointsGrain = _grainFactory.GetGrain<ICheckpointUpdatesWidgetGrain>(lineId, WidgetType.CheckpointUpdates.ToString());
        var priorityPositionsGrain = _grainFactory.GetGrain<IPriorityPositionsWidgetGrain>(lineId, WidgetType.PriorityPositions.ToString());

        var mobileStatusAppGrain = _grainFactory.GetGrain<IMobileAppStatusWidgetGrain>(lineId, WidgetType.MobileAppStatus.ToString());
        var notArrivedPositionsGrain = _grainFactory.GetGrain<INotArrivedPositionsWidgetGrain>(lineId, WidgetType.NotArrivedPositions.ToString());
        var outOfServiceGrain = _grainFactory.GetGrain<IOutOfServiceWidgetGrain>(lineId, WidgetType.OutOfService.ToString());

        await Task.WhenAll(
            statsGrain.RefreshAsync(line, refreshTime),
            checkpointsGrain.RefreshAsync(line, refreshTime),
            priorityPositionsGrain.RefreshAsync(line, refreshTime),
            mobileStatusAppGrain.RefreshAsync(line, refreshTime),
            notArrivedPositionsGrain.RefreshAsync(line, refreshTime),
            outOfServiceGrain.RefreshAsync(line, refreshTime)
        );
    }

}
