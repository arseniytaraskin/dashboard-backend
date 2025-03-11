using Orleans;
using Orleans.Serialization;

namespace JeFile.Dashboard.Features.Model;

[GenerateSerializer]
public class MonitoringMapListItem
{
    [Id(0)]
    public long Id { get; set; }
    [Id(1)]
    public string Name { get; set; }
    [Id(2)]
    public double Latitude { get; set; }
    [Id(3)]
    public double  Longitude { get; set; }
    [Id(4)]
    public IndicatorStatus Status { get; set; }
}