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


    [Id(3)]
    public List<CheckpointModel> Checkpoints { get; set; } = new();

    [Id(4)]
    public List<WorkingServicePoint> WorkingServicePoints { get; set; } = new();
}
