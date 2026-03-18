using IngresosCountry.Models;

namespace IngresosCountry.Services
{
    public interface ICatalogService
    {
        Task<List<Pais>> GetPaisesAsync();
        Task<List<Area>> GetAreasAsync();
        Task<List<TipoVisita>> GetTiposVisitaAsync();
    }
}