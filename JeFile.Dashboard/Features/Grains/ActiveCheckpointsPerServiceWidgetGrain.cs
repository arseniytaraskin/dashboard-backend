using System;
using Azure.Data.Tables;
using Npgsql;
using JeFile.Dashboard.Core.Models;
using JeFile.Dashboard.Features.InterfacesGrain;
using JeFile.Dashboard.Features.Model;
using JeFile.Dashboard.Infrastructure.States;
using Orleans.Providers;

namespace JeFile.Dashboard.Features.Grains;

[StorageProvider(ProviderName = "ActiveCheckpointsStore")] // Хранилище состояния в Azurite
public class ActiveCheckpointsPerServiceWidgetGrain : Grain<ServicesCheckpointsState>, IActiveCheckpointsPerServiceWidgetGrain
{

    private readonly TableServiceClient _tableServiceClient;
    private const string TableName = "testActiveCheckpointsWidget";
    private IDisposable? _timer;

    public ActiveCheckpointsPerServiceWidgetGrain(TableServiceClient tableServiceClient)
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
            TimeSpan.FromSeconds(5));

        await base.OnActivateAsync(cancellationToken);
    }

    private async Task RecalculateDataAsync()
{
    await ReadStateAsync();

    try
    {
        var services = new List<ServiceDetails>();
        var points = new List<string>();

        using var connection = new NpgsqlConnection("connection_string");
        await connection.OpenAsync();

        var servicesQuery = "SELECT id, name, display_order FROM services ORDER BY display_order";
        using var servicesCommand = new NpgsqlCommand(servicesQuery, connection);
        using var servicesReader = await servicesCommand.ExecuteReaderAsync();

        while (await servicesReader.ReadAsync())
        {
            var serviceId = servicesReader.GetGuid(0);
            var serviceName = servicesReader.GetString(1);

            using var checkpointsConnection = new NpgsqlConnection("connection_string");
            await checkpointsConnection.OpenAsync();

            var checkpointsQuery = @"
                SELECT cp.name 
                FROM checkpoints cp
                JOIN service_checkpoints sc ON cp.id = sc.checkpoint_id
                WHERE sc.service_id = @serviceId AND cp.is_active = true";

            using var checkpointsCommand = new NpgsqlCommand(checkpointsQuery, checkpointsConnection);
            checkpointsCommand.Parameters.AddWithValue("@serviceId", serviceId);
            using var checkpointsReader = await checkpointsCommand.ExecuteReaderAsync();

            while (await checkpointsReader.ReadAsync())
            {
                points.Add(checkpointsReader.GetString(0));
            }

            services.Add(new ServiceDetails
            {
                ServiceName = serviceName,
                ActiveCheckpointsNames = points.ToArray()
            });
            points.Clear();
        }

        State.Services = services.Select(s => new ServiceDetailsState
        {
            ServiceName = s.ServiceName,
            ActiveCheckpointsNames = s.ActiveCheckpointsNames
        }).ToArray();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error recalculating data: {ex.Message}");
    }

    await WriteStateAsync();
}

    public async Task<ServicesCheckpoints> GetServicesCheckpointsAsync()
    {
        // Возвращаем текущее состояние
        return new ServicesCheckpoints
        {
            Services = State.Services.Select(s => new ServiceDetails
            {
                ServiceName = s.ServiceName,
                ActiveCheckpointsNames = s.ActiveCheckpointsNames
            }).ToArray()
        };
    }

    public async Task RefreshAsync(MonitoringLineModel line, DateTime refreshTime)
    {
        var services = new List<ServiceDetails>();

        var points = new List<string>();
        foreach (var service in line.Services.OrderBy(x => x.DisplayOrder))
        {
            foreach (var checkpoint in line.GetWorkingServicePoints(refreshTime))
            {
                if (checkpoint.EnabledServices.Contains(service.Id))
                    points.Add(checkpoint.Name);
            }
            services.Add(new ServiceDetails
            {
                ServiceName = service.Name,
                ActiveCheckpointsNames = points.ToArray(),
            });
            points.Clear();
        }

        // Обновляем состояние
        State.Services = services.Select(s => new ServiceDetailsState
        {
            ServiceName = s.ServiceName,
            ActiveCheckpointsNames = s.ActiveCheckpointsNames
        }).ToArray();


        await WriteStateAsync(); // Сохраняем изменения в Azurite
    }
}
