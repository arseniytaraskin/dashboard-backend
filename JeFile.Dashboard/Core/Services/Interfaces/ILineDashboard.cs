using System;
using Orleans;
using System.Threading.Tasks;
namespace JeFile.Dashboard.Core.Services.Interfaces;

public interface ILineDashboard: IGrainWithIntegerKey
{
    Task<long> GetPlaceId();
    Task<long> GetLineId();
    Task<string> GetLineName();
    Task<string> GetPlaceAddress();
    Task<string> GetPlaceCity();
    Task<double> GetPlaceLatitude();
    Task<double> GetPlaceLongitude();

    Task<DateTime?> GetOpenTimeUtc();
    Task<DateTime?> GetCloseTimeUtc();
    Task<ILineDashboardWidgetCollection> GetWidgets();
    Task<DashboardAttentionLevel?> GetAttentionLevel();
}
