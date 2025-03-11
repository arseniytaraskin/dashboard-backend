namespace JeFile.Dashboard.Core.Services.Interfaces
{
    public interface IDashboardMonitoringGroup
    {
        long Id { get; }
        string Name { get; }
        int CategoryId { get; }
        string LineType { get; }
        IReadOnlyList<ILineDashboard> Dashboards { get; }
        double DisplayLatitude { get; }
        double DisplayLongitude { get; }
        DashboardAttentionLevel? AttentionLevel { get; }
        int DisplayOrder { get; }
        IDashboardGroupMap Map { get; }
    }
}