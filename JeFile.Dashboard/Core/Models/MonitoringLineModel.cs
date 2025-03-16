using System;

namespace JeFile.Dashboard.Core.Models;

public class MonitoringLineModel
{
    public List<MonitoringPositionModel> Positions { get; set; } = new();
    public List<MonitoringPositionModel> RemovedPositions { get; set; } = new();
}
