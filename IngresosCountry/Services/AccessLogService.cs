using IngresosCountry.Data;
using IngresosCountry.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace IngresosCountry.Services
{
    public class AccessLogService : IAccessLogService
    {
        private readonly DatabaseConnection _db;

        public AccessLogService(DatabaseConnection db)
        {
            _db = db;
        }

        public async Task<List<AccessLog>> GetAllAsync(DateTime? fechaDesde = null, DateTime? fechaHasta = null,
            string? tipoVisitante = null, string? resultadoAcceso = null, int? areaId = null)
        {
            var list = new List<AccessLog>();
            using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("sp_Visitas_GetAll", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@FechaDesde", (object?)fechaDesde ?? DBNull.Value);
            command.Parameters.AddWithValue("@FechaHasta", (object?)fechaHasta ?? DBNull.Value);
            command.Parameters.AddWithValue("@TipoVisitante", (object?)tipoVisitante ?? DBNull.Value);
            command.Parameters.AddWithValue("@ResultadoAcceso", (object?)resultadoAcceso ?? DBNull.Value);
            command.Parameters.AddWithValue("@AreaId", (object?)areaId ?? DBNull.Value);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new AccessLog
                {
                    Id = reader.GetInt32("Id"),
                    TipoVisitante = reader.GetString("TipoVisitante"),
                    SocioId = reader.IsDBNull("SocioId") ? null : reader.GetInt32("SocioId"),
                    InvitadoId = reader.IsDBNull("InvitadoId") ? null : reader.GetInt32("InvitadoId"),
                    NoSocioId = reader.IsDBNull("NoSocioId") ? null : reader.GetInt32("NoSocioId"),
                    EventoId = reader.IsDBNull("EventoId") ? null : reader.GetInt32("EventoId"),
                    AreaId = reader.IsDBNull("AreaId") ? null : reader.GetInt32("AreaId"),
                    AreaNombre = reader.IsDBNull("AreaNombre") ? null : reader.GetString("AreaNombre"),
                    FechaEntrada = reader.GetDateTime("FechaEntrada"),
                    FechaSalida = reader.IsDBNull("FechaSalida") ? null : reader.GetDateTime("FechaSalida"),
                    ResultadoAcceso = reader.GetString("ResultadoAcceso"),
                    MotivoRechazo = reader.IsDBNull("MotivoRechazo") ? null : reader.GetString("MotivoRechazo"),
                    RegistradoPor = reader.IsDBNull("RegistradoPor") ? null : reader.GetInt32("RegistradoPor"),
                    RegistradoPorNombre = reader.IsDBNull("RegistradoPorNombre") ? null : reader.GetString("RegistradoPorNombre"),
                    PuntoAcceso = reader.IsDBNull("PuntoAcceso") ? null : reader.GetString("PuntoAcceso"),
                    VisitanteNombre = reader.IsDBNull("VisitanteNombre") ? null : reader.GetString("VisitanteNombre")
                });
            }
            return list;
        }

        public async Task<int> RegistrarEntradaAsync(RegistrarAccesoViewModel model, int? registradoPor = null)
        {
            using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("sp_Visitas_RegistrarEntrada", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@TipoVisitante", model.TipoVisitante);
            command.Parameters.AddWithValue("@SocioId", (object?)model.SocioId ?? DBNull.Value);
            command.Parameters.AddWithValue("@InvitadoId", (object?)model.InvitadoId ?? DBNull.Value);
            command.Parameters.AddWithValue("@NoSocioId", (object?)model.NoSocioId ?? DBNull.Value);
            command.Parameters.AddWithValue("@EventoId", (object?)model.EventoId ?? DBNull.Value);
            command.Parameters.AddWithValue("@AreaId", (object?)model.AreaId ?? DBNull.Value);
            command.Parameters.AddWithValue("@ResultadoAcceso", model.ResultadoAcceso);
            command.Parameters.AddWithValue("@MotivoRechazo", (object?)model.MotivoRechazo ?? DBNull.Value);
            command.Parameters.AddWithValue("@RegistradoPor", (object?)registradoPor ?? DBNull.Value);
            command.Parameters.AddWithValue("@PuntoAcceso", (object?)model.PuntoAcceso ?? DBNull.Value);
            command.Parameters.AddWithValue("@Notas", (object?)model.Notas ?? DBNull.Value);

            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task RegistrarSalidaAsync(int id)
        {
            using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("sp_Visitas_RegistrarSalida", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }
    }
}