using System;
using JeFile.Dashboard.Core.enums;

namespace JeFile.Dashboard.Core.Models;
[GenerateSerializer]
public class MonitoringPositionModel
{
    //public static object MonitoringPositionState { get; internal set; }
    [Id(0)]
    public int PersonsQuantity { get; set; }
    [Id(1)]
    public MonitoringPositionIdentity Identity { get; set; }

    [Id(2)]
    public MonitoringPositionState State { get; set; }

    [Id(3)]
    public List<ServiceModel> Services { get; set; } = new();

    //[Id(4)]
    //public List<WorkingServicePoint> WorkingServicePoints { get; set; } = new();

    [Id(4)]
    public Guid? AssignedCheckpointId { get; set; }

    [Id(5)]
    public TimeSpan DurationFromServiceStart { get; set; } = TimeSpan.Zero;

    [Id(6)]
    public TimeSpan ExpectedServiceDuration { get; set; } = TimeSpan.Zero;

    [Id(7)]
    public int PriorityLevel { get; set; } = 0;
}
