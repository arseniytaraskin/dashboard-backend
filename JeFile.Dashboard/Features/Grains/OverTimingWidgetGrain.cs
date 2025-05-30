using System;
using Azure;
using Azure.Data.Tables;
using JeFile.Dashboard.Core.enums;
using JeFile.Dashboard.Core.Models;
using JeFile.Dashboard.Features.InterfacesGrain;
using JeFile.Dashboard.Features.Model;
using JeFile.Dashboard.Features.States;
using Orleans.Providers;

namespace JeFile.Dashboard.Features.Grains;

[StorageProvider(ProviderName = "OverTimingStore")]
public class OverTimingWidgetGrain : Grain<OverTimingState>, IOverTimingWidgetGrain
{
    private OverTiming _overTiming = new();

    private readonly TableServiceClient _tableServiceClient;
    private const string TableName = "testOverTimingWidget";

    private IDisposable? _timer;

    public OverTimingWidgetGrain(TableServiceClient tableServiceClient)
    {
        _tableServiceClient = tableServiceClient;
    }

    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        _timer = RegisterTimer(
            _ => RecalculateDataAsync(),
            null,
            TimeSpan.FromSeconds(1),
            TimeSpan.FromMinutes(10));

        await base.OnActivateAsync(cancellationToken);
    }

    public async Task RecalculateDataAsync()
    {
        await ReadStateAsync();

        var grainId = "1";
        var tableClient = _tableServiceClient.GetTableClient("testOverTimingWidget");

        try
        {
            // Получаем данные из таблицы Azure
            var entity = await tableClient.GetEntityAsync<TableEntity>(grainId, "OverTimingData");

            if (entity != null)
            {
                // Обновляем состояние гранулы на основе данных из таблицы
                State.TotalActivePoints = Convert.ToInt32(entity.Value["TotalActivePoints"]);
                State.OverTimePoints = Convert.ToInt32(entity.Value["OverTimePoints"]);
                State.WrostOverTimePointsPercent = Convert.ToInt32(entity.Value["WrostOverTimePointsPercent"]);
                State.MaxOverTime = Convert.ToInt32(entity.Value["MaxOverTime"]);
            }
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            Console.WriteLine("Запись с указанным ID не найдена в таблице Azure.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при чтении данных: {ex.Message}");
        }

        // Обновляем timestamp
        State.Time = DateTime.UtcNow;

        // Сохраняем состояние гранулы
        await WriteStateAsync();
    }



    public Task<OverTiming> GetOverTimingAsync()
    {
        return Task.FromResult(new OverTiming
        {
            TotalActivePoints = State.TotalActivePoints,
            OverTimePoints = State.OverTimePoints,
            WrostOverTimePointsPercent = State.WrostOverTimePointsPercent,
            Time = State.Time,
            MaxOverTime = State.MaxOverTime,
            History = State.History ?? Array.Empty<OverTimingHistoryItem>()
        });
    }



    public async Task RefreshAsync(MonitoringLineModel line, DateTime refreshTime)
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

        var overTimePoints = 0;
        var pointsInService = 0;
        var maxOvertime = 0;

        foreach (var position in line.Positions)
        {
            if (position.State != MonitoringPositionState.ServiceStarted
                && position.AssignedCheckpointId.HasValue)
                continue;

            pointsInService++;

            if (position.DurationFromServiceStart > position.ExpectedServiceDuration)
            {
                overTimePoints++;
                var overtime = (int)(position.DurationFromServiceStart - position.ExpectedServiceDuration).TotalMinutes;
                maxOvertime = Math.Max(overtime, maxOvertime);
            }
        }

        var percent = 0;
        if (pointsInService > 0)
        {
            percent = (overTimePoints * 100) / pointsInService;
        }

        var history = State.History;
        var wrostPercent = Math.Max(percent, State.WrostOverTimePointsPercent);
        if (ShouldCollectHistory(line, refreshTime))
        {
            if (history.Length == 0 || (refreshTime - history[history.Length - 1].Time).TotalMinutes >= 5)
            {
                history = history.Append(new OverTimingHistoryItem
                {
                    Time = refreshTime,
                    OverTimePointsPercent = wrostPercent
                }).ToArray();
                wrostPercent = 0;
            }
        }

        State.TotalActivePoints = pointsInService;
        State.OverTimePoints = overTimePoints;
        State.WrostOverTimePointsPercent = wrostPercent;
        State.Time = refreshTime;
        State.MaxOverTime = maxOvertime;
        State.History = history;

        // Сохраняем состояние в Azurite
        await WriteStateAsync();
    }


    private static bool ShouldCollectHistory(MonitoringLineModel line, DateTime refreshTime)
    {
        return line.OperatingFrom < refreshTime
            && line.OperatingTo > refreshTime.AddMinutes(10);
    }
}
