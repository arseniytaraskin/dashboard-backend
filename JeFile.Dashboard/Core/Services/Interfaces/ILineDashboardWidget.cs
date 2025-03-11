namespace JeFile.Dashboard.Core.Services.Interfaces
{
    public interface ILineDashboardWidget
    {
        string Id { get; }
        Type DataType { get; }
        DateTime LastUpdateTime { get; }
        object Data { get; }
        DashboardAttentionLevel AttentionLevel { get; }
        DashboardWidgetIndicatorType? IndicatorType { get; }
    }
}