using System;
using Orleans.Providers;
using Azure;
using Azure.Data.Tables;
using Orleans.Providers;
using JeFile.Dashboard.Features.States;
using JeFile.Dashboard.Features.InterfacesGrain;
using JeFile.Dashboard.Core.Models;
using JeFile.Dashboard.Core.enums;

namespace JeFile.Dashboard.Features.Grains;

[GenerateSerializer]
public class CheckpointsDetails
{
    /// <summary>
    /// Количество точек, задействованных в предварительной записи
    /// </summary>
    [Id(0)]
    public int PlannedStaffsCount { get; init; }
    /// <summary>
    /// Количество работающих точек без учета скрытых окон
    /// </summary>
    [Id(1)]
    public int WorkingPointsCount { get; init; }
    /// <summary>
    /// Количество скрытых окон
    /// </summary>
    [Id(2)]
    public int HiddenPoints { get; init; }
    /// <summary>
    /// Количество точек, которые в текущий момент вызвали или обслуживают заявителей по предварительной записи
    /// </summary>
    [Id(3)]
    public int PointsWithAppointmentInService { get; init; }
    /// <summary>
    /// Количество точек, которые в текущий момент вызвали или обслуживают заявителей, записавшихся в текущий день
    /// </summary>
    [Id(4)]
    public int PointsWithTodayPositionInService { get; init; }
    /// <summary>
    /// Количество точек, в которых оператор на перерыве
    /// </summary>
    [Id(5)]
    public int PointsWithBreakPositionInService { get; init; }
    /// <summary>
    /// Количество точек, которые работают, но никого не обслуживают и не вызвали заявитей
    /// </summary>
    [Id(6)]
    public int PointsWithNoPositionInService { get; init; }
    [Id(7)]
    public int TotalPositionsCalledOrInService { get; init; }
    [Id(8)]
    public int TotalPointsCount { get; init; }
}


[StorageProvider(ProviderName = "CheckpointsDetailsStore")]
public class CheckpointsDetailsWidgetGrain : Grain<CheckpointsDetailsState>, ICheckpointsDetailsWidgetGrain
{

    private readonly TableServiceClient _tableServiceClient;
    private const string TableName = "testCheckpointDetailsWidget";
    private IDisposable? _timer;

    public CheckpointsDetailsWidgetGrain(TableServiceClient tableServiceClient)
    {
        _tableServiceClient = tableServiceClient;
    }

    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        var tableClient = _tableServiceClient.GetTableClient(TableName);
        await tableClient.CreateIfNotExistsAsync();

        _timer = RegisterTimer(
            _ => RecalculateDataAsync(),
            null,
            TimeSpan.FromSeconds(1),
            TimeSpan.FromSeconds(30));

        await base.OnActivateAsync(cancellationToken);
    }

    private async Task RecalculateDataAsync()
    {
        await ReadStateAsync();

        var grainId = "1";
        var tableClient = _tableServiceClient.GetTableClient(TableName);

        try
        {
            var entity = await tableClient.GetEntityAsync<TableEntity>(grainId, "CheckpointsDetailsData");
            if (entity != null)
            {
                State.PlannedStaffsCount = entity.Value.GetInt32("PlannedStaffsCount") ?? State.PlannedStaffsCount;
                State.WorkingPointsCount = entity.Value.GetInt32("WorkingPointsCount") ?? State.WorkingPointsCount;
                State.HiddenPoints = entity.Value.GetInt32("HiddenPoints") ?? State.HiddenPoints;
                State.PointsWithAppointmentInService = entity.Value.GetInt32("PointsWithAppointmentInService") ?? State.PointsWithAppointmentInService;
                State.PointsWithTodayPositionInService = entity.Value.GetInt32("PointsWithTodayPositionInService") ?? State.PointsWithTodayPositionInService;
                State.PointsWithBreakPositionInService = entity.Value.GetInt32("PointsWithBreakPositionInService") ?? State.PointsWithBreakPositionInService;
                State.PointsWithNoPositionInService = entity.Value.GetInt32("PointsWithNoPositionInService") ?? State.PointsWithNoPositionInService;
                State.TotalPositionsCalledOrInService = entity.Value.GetInt32("TotalPositionsCalledOrInService") ?? State.TotalPositionsCalledOrInService;
                State.TotalPointsCount = entity.Value.GetInt32("TotalPointsCount") ?? State.TotalPointsCount;
            }
        }
        catch (RequestFailedException) { }
        catch (Exception) { }

        
        await WriteStateAsync();
    }

    public async Task<CheckpointsDetails> GetCheckpointsDetailsAsync()
    {
        // Возвращаем текущее состояние
        return new CheckpointsDetails
        {
            PlannedStaffsCount = State.PlannedStaffsCount,
            WorkingPointsCount = State.WorkingPointsCount,
            HiddenPoints = State.HiddenPoints,
            PointsWithAppointmentInService = State.PointsWithAppointmentInService,
            PointsWithTodayPositionInService = State.PointsWithTodayPositionInService,
            PointsWithBreakPositionInService = State.PointsWithBreakPositionInService,
            PointsWithNoPositionInService = State.PointsWithNoPositionInService,
            TotalPositionsCalledOrInService = State.TotalPositionsCalledOrInService,
            TotalPointsCount = State.TotalPointsCount
        };
    }

    public async Task RefreshAsync(MonitoringLineModel line, DateTime refreshTime)
    {
        // Логика пересчета данных для состояния грейна
        var totalPositionsCalledAndInServiceCount = 0;
        var workingServicePoints = new HashSet<Guid>(line.GetWorkingServicePoints(refreshTime).Select(x => x.Id));
        var pointsWithNoPosition = new HashSet<Guid>(line.GetWorkingServicePoints(refreshTime).Select(x => x.Id));
        var appointments = 0;
        var breaks = 0;
        var todayPositions = 0;
        var hiddenPoints = 0;

        foreach (var position in GetCalledPositions(line))
        {
            totalPositionsCalledAndInServiceCount++;

            if (position.AssignedCheckpointId.HasValue)
                pointsWithNoPosition.Remove(position.AssignedCheckpointId.Value);

            if (position.AssignedCheckpointId.HasValue &&
                !workingServicePoints.Contains(position.AssignedCheckpointId.Value))
            {
                hiddenPoints++;
            }

            if (position.Identity == MonitoringPositionIdentity.Appointment)
            {
                appointments++;
                continue;
            }

            if (position.Identity == MonitoringPositionIdentity.Break)
            {
                breaks++;
                continue;
            }

            todayPositions++;
        }

        // Обновляем состояние
        State.PlannedStaffsCount = CalculateStaffsCheckpoints(line, refreshTime);
        State.PointsWithAppointmentInService = appointments;
        State.PointsWithBreakPositionInService = breaks;
        State.PointsWithTodayPositionInService = todayPositions;
        State.PointsWithNoPositionInService = pointsWithNoPosition.Count;
        State.HiddenPoints = hiddenPoints;
        State.WorkingPointsCount = workingServicePoints.Count;
        State.TotalPositionsCalledOrInService = totalPositionsCalledAndInServiceCount;
        State.TotalPointsCount = line.Checkpoints.Count;

        await WriteStateAsync(); // Сохраняем изменения в Azurite
    }

    private int CalculateStaffsCheckpoints(MonitoringLineModel line, DateTime refreshTime)
    {
        var todayStaff = line.StaffManagements
            .Where(x => (refreshTime - x.StartDate).TotalHours < 24)
            .ToList();

        if (todayStaff.Count == 0)
            return 0;

        var count = 0;
        foreach (var staff in todayStaff)
        {
            if (IsStaffActiveAtTime(staff, refreshTime))
                count += staff.NumberOfStaff;
        }
        return count;
    }

    private bool IsStaffActiveAtTime(MonitoringStaffManagementModel staff, DateTime refreshTime)
    {
        if (staff.OpenTime > refreshTime || staff.CloseTime < refreshTime)
            return false;
        if (!staff.PauseStart.HasValue || !staff.PauseEnd.HasValue)
            return true;
        if (staff.PauseStart.Value < refreshTime && staff.PauseEnd > refreshTime)
            return false;
        return true;
    }

    private IEnumerable<MonitoringPositionModel> GetCalledPositions(MonitoringLineModel line)
    {
        foreach (var position in line.Positions)
        {
            if (position.State == MonitoringPositionState.ServiceStarted
                || position.State == MonitoringPositionState.CalledToCheckPoint
                || position.State == MonitoringPositionState.MovingToCheckPoint
                || position.State == MonitoringPositionState.NearCheckPoint)
                yield return position;
        }
        yield break;
    }
}