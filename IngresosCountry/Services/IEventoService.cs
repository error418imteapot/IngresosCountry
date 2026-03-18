using IngresosCountry.Models;

namespace IngresosCountry.Services
{
    public interface IEventoService
    {
        Task<List<Evento>> GetAllAsync(string? estado = null);
        Task<Evento?> GetByIdAsync(int id);
        Task<int> CreateAsync(Evento evento);
        Task UpdateAsync(Evento evento);
        Task<List<EventoParticipante>> GetParticipantesAsync(int eventoId);
        Task<int> AddParticipanteAsync(EventoParticipante participante);
    }
}