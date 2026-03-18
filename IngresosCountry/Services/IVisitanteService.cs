using IngresosCountry.Models;

namespace IngresosCountry.Services
{
    public interface IVisitanteService
    {
        Task<List<VisitanteNoSocio>> GetAllAsync();
        Task<int> CreateAsync(VisitanteNoSocio visitante);
    }
}