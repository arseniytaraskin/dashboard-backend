using System;
using JeFile.Dashboard.Core.Models;
using JeFile.Dashboard.Core.Services;
using JeFile.Dashboard.Features.InterfacesGrain;
using JeFile.Dashboard.Features.Model;

namespace JeFile.Dashboard.Features.Grains;

public class ActivityStatusWidgetGrain : Grain, IActivityStatusWidgetGrain
{
private readonly WorktimeTable _worktimeTable;
private ActivityStatus _status = new();

public ActivityStatusWidgetGrain(WorktimeTable worktimeTable)
{
    _worktimeTable = worktimeTable;
}

public Task<ActivityStatus> GetActivityStatusAsync()
{
    return Task.FromResult(_status);
}

public Task RefreshAsync(MonitoringLineModel line, DateTime refreshTime)
{
    var isNotActive = line.NotActive;
    var timeTable = _worktimeTable.GetOfficeWorkTime(line.OfficeId);
    var isWorkingTime = CalculateWorkingTime(line, refreshTime, timeTable);

    _status = new ActivityStatus
    {
        NotActive = isNotActive,
        IsWorkingTime = isWorkingTime,
        AttentionLevel = isWorkingTime && isNotActive
            ? DashboardAttentionLevel.Alarm
            : DashboardAttentionLevel.Normal
    };

    return Task.CompletedTask;
}

private bool CalculateWorkingTime(MonitoringLineModel line, DateTime refreshTime, OfficeWorkTime? timeTable)
{
    var isWorkingTime = line.OperatingFrom <= refreshTime && line.OperatingTo > refreshTime;

    if (timeTable?.TimeTable.FirstOrDefault(x => x.DayOfWeek == refreshTime.DayOfWeek) is { } workingDay)
    {
        var openTime = line.DayStartTime + workingDay.OpenTime;
        var closeTime = line.DayStartTime + workingDay.CloseTime;
        isWorkingTime = openTime <= refreshTime && closeTime > refreshTime;
    }

    return timeTable != null ? isWorkingTime : false;
}
}
