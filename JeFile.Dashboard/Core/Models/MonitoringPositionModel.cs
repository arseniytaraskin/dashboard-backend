using System;
using JeFile.Dashboard.Core.enums;

namespace JeFile.Dashboard.Core.Models;
[GenerateSerializer]
public class MonitoringPositionModel
{
    [Id(0)]
    public int PersonsQuantity { get; set; }
    [Id(1)]
    public MonitoringPositionIdentity Identity { get; set; }

    [Id(2)]
    public MonitoringPositionState State { get; set; }

    [Id(3)]
    public List<ServiceModel> Services { get; set; } = new();
}
