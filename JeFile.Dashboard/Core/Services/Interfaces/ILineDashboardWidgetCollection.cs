using System;
using Orleans;
using System.Threading.Tasks;
namespace JeFile.Dashboard.Core.Services.Interfaces;

public interface ILineDashboardWidgetCollection: IGrainWithIntegerKey
{
    Task<ILineDashboardWidget?> GetWidget(string widgetId);
    Task<T> GetWidgetData<T>();
    Task<int> GetCount();
    Task<IReadOnlyList<ILineDashboardWidget>> GetAllWidgets();    
}
