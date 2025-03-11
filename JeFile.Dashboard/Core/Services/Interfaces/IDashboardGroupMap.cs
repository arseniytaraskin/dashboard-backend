namespace JeFile.Dashboard.Core.Services.Interfaces
{
    public interface IDashboardGroupMap
    {
        string Url { get; }
        string Title { get; }
        double Latitude { get; }
        double Longitude { get; }
        double ZoomLevel { get; }
    }
}