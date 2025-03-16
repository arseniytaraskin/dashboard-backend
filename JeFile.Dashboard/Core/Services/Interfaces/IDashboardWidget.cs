using System;
using JeFile.Dashboard.Core.Models;

namespace JeFile.Dashboard.Core.Services.Interfaces;

public interface IDashboardWidget
{
    Type WidgetDataType { get; }
    DashboardWidgetIndicatorType? IndicatorType { get; }
    bool Persistable { get; }
    TimeSpan RefreshTimeout { get; }
    Task<object> Refresh(MonitoringLineModel line, DateTime refreshTime, object? oldWidgetData);
    DashboardAttentionLevel GetAttentionLevel(object widgetData);
}
