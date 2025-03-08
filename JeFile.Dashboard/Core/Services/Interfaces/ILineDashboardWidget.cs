using System;
using Orleans;
using System.Threading.Tasks;

namespace JeFile.Dashboard.Core.Services.Interfaces;

public interface ILineDashboardWidget : IGrainWithStringKey
{
    Task<Type> GetDataType();
    Task<DateTime> GetLastUpdateTime();
    Task<object> GetData();

    Task<DashboardAttentionLevel> GetAttentionLevel();
    Task<DashboardWidgetIndicatorType?> GetIndicatorType(); 
}
