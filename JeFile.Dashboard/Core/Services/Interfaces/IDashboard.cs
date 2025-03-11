namespace JeFile.Dashboard.Core.Services.Interfaces
{
    public interface IDashboard
    {
        IReadOnlyList<IDashboardMonitoringGroup> MonitoringGroups { get; }
    }
}