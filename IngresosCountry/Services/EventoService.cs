using IngresosCountry.Data;
using IngresosCountry.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace IngresosCountry.Services
{
    public class EventoService : IEventoService
    {
        private readonly DatabaseConnection _db;

        public EventoService(DatabaseConnection db)
        {
            _db = db;
        }

        public async Task<List<Evento>> GetAllAsync(string? estado = null)
        {
            var list = new List<Evento>();
            using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("sp_Eventos_GetAll", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Estado", (object?)estado ?? DBNull.Value);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new Evento
                {
                    Id = reader.GetInt32("Id"),
                    Nombre = reader.GetString("Nombre"),
                    Descripcion = reader.IsDBNull("Descripcion") ? null : reader.GetString("Descripcion"),
                    FechaInicio = reader.GetDateTime("FechaInicio"),
                    FechaFin = reader.GetDateTime("FechaFin"),
                    Ubicacion = reader.IsDBNull("Ubicacion") ? null : reader.GetString("Ubicacion"),
                    Capacidad = reader.IsDBNull("Capacidad") ? null : reader.GetInt32("Capacidad"),
                    OrganizadorNombre = reader.IsDBNull("OrganizadorNombre") ? null : reader.GetString("OrganizadorNombre"),
                    Estado = reader.GetString("Estado"),
                    TotalParticipantes = reader.GetInt32("TotalParticipantes")
                });
            }
            return list;
        }

        public async Task<Evento?> GetByIdAsync(int id)
        {
            using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("sp_Eventos_GetById", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Evento
                {
                    Id = reader.GetInt32("Id"),
                    Nombre = reader.GetString("Nombre"),
                    Descripcion = reader.IsDBNull("Descripcion") ? null : reader.GetString("Descripcion"),
                    FechaInicio = reader.GetDateTime("FechaInicio"),
                    FechaFin = reader.GetDateTime("FechaFin"),
                    Ubicacion = reader.IsDBNull("Ubicacion") ? null : reader.GetString("Ubicacion"),
                    Capacidad = reader.IsDBNull("Capacidad") ? null : reader.GetInt32("Capacidad"),
                    OrganizadorNombre = reader.IsDBNull("OrganizadorNombre") ? null : reader.GetString("OrganizadorNombre"),
                    Estado = reader.GetString("Estado")
                };
            }
            return null;
        }

        public async Task<int> CreateAsync(Evento evento)
        {
            using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("sp_Eventos_Insert", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Nombre", evento.Nombre);
            command.Parameters.AddWithValue("@Descripcion", (object?)evento.Descripcion ?? DBNull.Value);
            command.Parameters.AddWithValue("@FechaInicio", evento.FechaInicio);
            command.Parameters.AddWithValue("@FechaFin", evento.FechaFin);
            command.Parameters.AddWithValue("@Ubicacion", (object?)evento.Ubicacion ?? DBNull.Value);
            command.Parameters.AddWithValue("@Capacidad", (object?)evento.Capacidad ?? DBNull.Value);
            command.Parameters.AddWithValue("@OrganizadorSocioId", (object?)evento.OrganizadorSocioId ?? DBNull.Value);

            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task UpdateAsync(Evento evento)
        {
            using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("sp_Eventos_Update", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Id", evento.Id);
            command.Parameters.AddWithValue("@Nombre", evento.Nombre);
            command.Parameters.AddWithValue("@Descripcion", (object?)evento.Descripcion ?? DBNull.Value);
            command.Parameters.AddWithValue("@FechaInicio", evento.FechaInicio);
            command.Parameters.AddWithValue("@FechaFin", evento.FechaFin);
            command.Parameters.AddWithValue("@Ubicacion", (object?)evento.Ubicacion ?? DBNull.Value);
            command.Parameters.AddWithValue("@Capacidad", (object?)evento.Capacidad ?? DBNull.Value);
            command.Parameters.AddWithValue("@OrganizadorSocioId", (object?)evento.OrganizadorSocioId ?? DBNull.Value);
            command.Parameters.AddWithValue("@Estado", evento.Estado);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<List<EventoParticipante>> GetParticipantesAsync(int eventoId)
        {
            var list = new List<EventoParticipante>();
            using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("sp_EventoParticipantes_GetByEvento", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@EventoId", eventoId);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new EventoParticipante
                {
                    Id = reader.GetInt32("Id"),
                    EventoId = reader.GetInt32("EventoId"),
                    Nombre = reader.GetString("Nombre"),
                    DocumentoIdentidad = reader.IsDBNull("DocumentoIdentidad") ? null : reader.GetString("DocumentoIdentidad"),
                    TipoParticipante = reader.GetString("TipoParticipante"),
                    SocioId = reader.IsDBNull("SocioId") ? null : reader.GetInt32("SocioId"),
                    InvitadoId = reader.IsDBNull("InvitadoId") ? null : reader.GetInt32("InvitadoId"),
                    Confirmado = reader.GetBoolean("Confirmado"),
                    NumeroMembresia = reader.IsDBNull("NumeroMembresia") ? null : reader.GetString("NumeroMembresia")
                });
            }
            return list;
        }

        public async Task<int> AddParticipanteAsync(EventoParticipante participante)
        {
            using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("sp_EventoParticipantes_Insert", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@EventoId", participante.EventoId);
            command.Parameters.AddWithValue("@Nombre", participante.Nombre);
            command.Parameters.AddWithValue("@DocumentoIdentidad", (object?)participante.DocumentoIdentidad ?? DBNull.Value);
            command.Parameters.AddWithValue("@TipoParticipante", participante.TipoParticipante);
            command.Parameters.AddWithValue("@SocioId", (object?)participante.SocioId ?? DBNull.Value);
            command.Parameters.AddWithValue("@InvitadoId", (object?)participante.InvitadoId ?? DBNull.Value);

            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }
    }
}