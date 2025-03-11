namespace JeFile.Dashboard.Core.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<IDashboard> GetCompanyDashboard(string companyId, string language, CancellationToken cancellationToken);
        Task<IDashboard?> GetTvDashboard(Guid dashboardId, CancellationToken cancellationToken);
    }
}