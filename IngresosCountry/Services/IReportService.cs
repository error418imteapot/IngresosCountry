using IngresosCountry.Models;

namespace IngresosCountry.Services
{
    public interface IReportService
    {
        Task<DashboardViewModel> GetDashboardAsync();
        Task<List<AccessReportItem>> GetAccessByDateAsync(DateTime fechaDesde, DateTime fechaHasta);
        Task<List<AccessLog>> GetDeniedAccessAsync(DateTime? fechaDesde = null, DateTime? fechaHasta = null);
    }
}