using System.Threading.Tasks;
using Orleans;

namespace JeFile.Dashboard.Core.Services.Interfaces;

public interface IDashboardGroupMap : IGrainWithStringKey
{
        Task<string> GetUrl(); // получить Url 
        Task<string> GetTitle(); // получить название
        Task<double> GetLatitude(); // получить размер по ширине
        Task<double> GetLongitude(); // получить размер по длине
        Task<double> GetZoomLevel();
}
