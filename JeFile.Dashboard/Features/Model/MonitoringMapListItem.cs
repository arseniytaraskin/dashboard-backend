namespace JeFile.Dashboard.Features.Model;

public class MonitoringMapListItem
{
    public long Id { get; set; }
    public string Name { get; set; }

    public double Latitude { get; set; }
    public double  Longitude { get; set; }
    public IndicatorStatus Status { get; set; }
}