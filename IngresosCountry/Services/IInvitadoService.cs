using IngresosCountry.Models;

namespace IngresosCountry.Services
{
    public interface IInvitadoService
    {
        Task<List<Invitado>> GetAllAsync();
        Task<Invitado?> GetByIdAsync(int id);
        Task<int> CreateAsync(Invitado invitado);
        Task UpdateAsync(Invitado invitado);
        Task<List<Invitacion>> GetInvitacionesAsync();
        Task<List<Invitacion>> GetInvitacionesBySocioAsync(int socioId);
        Task<int> CreateInvitacionAsync(Invitacion invitacion);
        Task<Invitacion?> ValidateQRAsync(string codigoQR);
    }
}