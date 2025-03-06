using System.Threading.Tasks;
using Orleans;

namespace JeFile.Dashboard.Core.Services.Interfaces;

public interface IDashboardGroupMap : IGrainWithStringKey
{
        Task<string> GetUrl();
        Task<string> GetTitle();
        Task<double> GetLatitude();
        Task<double> GetLongitude();
        Task<double> GetZoomLevel();
}
