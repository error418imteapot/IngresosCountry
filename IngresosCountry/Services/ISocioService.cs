using IngresosCountry.Models;

namespace IngresosCountry.Services
{
    public interface ISocioService
    {
        Task<List<Socio>> GetAllAsync(string? estado = null, string? busqueda = null);
        Task<Socio?> GetByIdAsync(int id);
        Task<Socio?> GetByMembresiaAsync(string numeroMembresia);
        Task<int> CreateAsync(Socio socio);
        Task UpdateAsync(Socio socio);
        Task DeleteAsync(int id);
    }
}