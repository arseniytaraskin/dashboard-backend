using System;
using Azure;
using Azure.Data.Tables;
using JeFile.Dashboard.Core.enums;
using JeFile.Dashboard.Core.Models;
using JeFile.Dashboard.Features.InterfacesGrain;
using JeFile.Dashboard.Features.Model;
using JeFile.Dashboard.Infrastructure.Data;
using Orleans.Providers;

namespace JeFile.Dashboard.Features.Grains;

[StorageProvider(ProviderName = "PriorityPositionsStore")]
public class PriorityPositionsWidgetGrain : Grain<PriorityPositionsState>, IPriorityPositionsWidgetGrain
{
    private IDisposable? _timer;

    private readonly TableServiceClient _tableServiceClient;
    private const string TableName = "test";

    public PriorityPositionsWidgetGrain(TableServiceClient tableServiceClient)
    {
        _tableServiceClient = tableServiceClient;
    }

    public class PriorityPositionsModel
    {
        public int Id { get; set; }
        public int TotalDayPriorityPositions { get; set; }
        public int TotalDayNotPriorityPositions { get; set; }
        public DateTime LastUpdated { get; set; }
    }


    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        _timer = RegisterTimer(
            _ => RecalculateDataAsync(),
            null,
            TimeSpan.FromSeconds(10),
            TimeSpan.FromMinutes(30));

        await base.OnActivateAsync(cancellationToken);
    }

    private async Task RecalculateDataAsync()
    {
        var grainId = "1"; 
        var tableClient = _tableServiceClient.GetTableClient("test");

        try
        {
            
            var entity = await tableClient.GetEntityAsync<TableEntity>(grainId, "PriorityPositions");

            if (entity != null)
            {
                
                var priorityFromDb = Convert.ToInt32(entity.Value["TotalDayPriorityPositions"]);
                var notPriorityFromDb = Convert.ToInt32(entity.Value["TotalDayNotPriorityPositions"]);

                
                State.TotalDayPrioriryPositions += priorityFromDb;
                State.TotalDayNotPrioriryPositions -= notPriorityFromDb;
            }
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            
            Console.WriteLine("Запись с указанным ID не найдена в таблице Azure.");
        }

        
        
        State.LastUpdated = DateTime.UtcNow;

        await WriteStateAsync();

    }


    public Task RefreshAsync(MonitoringLineModel line, DateTime refreshTime)
    {
        if (line?.Positions == null) return Task.CompletedTask;

        var notPriority = 0;
        var priority = 0;

        var allPositions = line.Positions
            .Concat(line.RemovedPositions ?? Enumerable.Empty<MonitoringPositionModel>());

        foreach (var position in allPositions)
        {
            if (position.State == MonitoringPositionState.Removed
                || position.State == MonitoringPositionState.Leaved
                || position.State == MonitoringPositionState.Unknown)
                continue;

            if (position.PriorityLevel > 0) priority++;
            else notPriority++;
        }

        State.TotalDayPrioriryPositions = priority;
        State.TotalDayNotPrioriryPositions = notPriority;
        State.LastUpdated = refreshTime;

        return WriteStateAsync();
    }

    public Task<PriorityPositions> GetPriorityPositionsAsync()
    {
        return Task.FromResult(new PriorityPositions
        {
            TotalDayPrioriryPositions = State.TotalDayPrioriryPositions,
            TotalDayNotPrioriryPositions = State.TotalDayNotPrioriryPositions
        });
    }

}