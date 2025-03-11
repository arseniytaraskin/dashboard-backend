namespace JeFile.Dashboard.Core.Services.Interfaces
{
    public interface ILineDashboard
    {
        long PlaceId { get; }
        long LineId { get; }
        string LineName { get; }
        string PlaceName { get; }
        string PlaceAddress { get; }
        string PlaceCity { get; }
        double PlaceLatitude { get; }
        double PlaceLongitude { get; }
        DateTime? OpenTimeUtc { get; }
        DateTime? CloseTimeUtc { get; }
        ILineDashboardWidgetCollection Widgets { get; }
        DashboardAttentionLevel? AttentionLevel { get; }
    }
}