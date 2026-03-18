using IngresosCountry.Models;

namespace IngresosCountry.Services
{
    public interface IAccessLogService
    {
        Task<List<AccessLog>> GetAllAsync(DateTime? fechaDesde = null, DateTime? fechaHasta = null,
            string? tipoVisitante = null, string? resultadoAcceso = null, int? areaId = null);
        Task<int> RegistrarEntradaAsync(RegistrarAccesoViewModel model, int? registradoPor = null);
        Task RegistrarSalidaAsync(int id);
    }
}