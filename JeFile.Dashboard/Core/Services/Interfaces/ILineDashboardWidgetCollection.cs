namespace JeFile.Dashboard.Core.Services.Interfaces
{
    public interface ILineDashboardWidgetCollection : IReadOnlyCollection<ILineDashboardWidget>
    {
        ILineDashboardWidget? this[string widgetId] { get; }
        T GetWidgetData<T>();
    }
}