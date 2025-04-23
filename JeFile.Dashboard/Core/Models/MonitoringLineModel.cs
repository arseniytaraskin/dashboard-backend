using System;

namespace JeFile.Dashboard.Core.Models;

[GenerateSerializer]
public class MonitoringLineModel
{
    [Id(0)]
    public Guid LineId { get; set; }

    [Id(1)]
    public List<MonitoringPositionModel> Positions { get; set; } = new();
    [Id(2)]
    public List<MonitoringPositionModel> RemovedPositions { get; set; } = new();

    // новое 
    [Id(3)]
    public List<CheckpointModel> Checkpoints { get; set; } = new();

    [Id(4)]
    public List<WorkingServicePoint> WorkingServicePoints { get; set; } = new();

    // для OverTimingWidgetGrain
    [Id(5)]
    public DateTime OperatingFrom { get; set; }

    [Id(6)]
    public DateTime OperatingTo { get; set; }

    // убрать
    [Id(7)]
    public bool NotActive { get; set; }

    // id офиса
    [Id(8)]
    public long OfficeId { get; set; }
    [Id(9)]
    public DateTime DayStartTime { get; set; }
}
