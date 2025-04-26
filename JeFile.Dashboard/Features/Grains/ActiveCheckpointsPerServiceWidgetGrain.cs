using System;
using JeFile.Dashboard.Core.Models;
using JeFile.Dashboard.Features.InterfacesGrain;
using JeFile.Dashboard.Features.Model;
using JeFile.Dashboard.Infrastructure.States;
using Orleans.Providers;

namespace JeFile.Dashboard.Features.Grains;

[StorageProvider(ProviderName = "ActiveCheckpointsStore")] // Хранилище состояния в Azurite
public class ActiveCheckpointsPerServiceWidgetGrain : Grain<ServicesCheckpointsState>, IActiveCheckpointsPerServiceWidgetGrain
{
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

        
        State.Services = services.Select(s => new ServiceDetailsState
        {
            ServiceName = s.ServiceName,
            ActiveCheckpointsNames = s.ActiveCheckpointsNames
        }).ToArray();


        await WriteStateAsync(); 
    }
}
