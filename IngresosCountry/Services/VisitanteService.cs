using IngresosCountry.Data;
using IngresosCountry.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace IngresosCountry.Services
{
    public class VisitanteService : IVisitanteService
    {
        private readonly DatabaseConnection _db;

        public VisitanteService(DatabaseConnection db)
        {
            _db = db;
        }

        public async Task<List<VisitanteNoSocio>> GetAllAsync()
        {
            var list = new List<VisitanteNoSocio>();
            using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("sp_VisitantesNoSocios_GetAll", connection);
            command.CommandType = CommandType.StoredProcedure;

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new VisitanteNoSocio
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                    Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                    DocumentoIdentidad = reader.IsDBNull(reader.GetOrdinal("DocumentoIdentidad")) ? null : reader.GetString(reader.GetOrdinal("DocumentoIdentidad")),
                    Empresa = reader.IsDBNull(reader.GetOrdinal("Empresa")) ? null : reader.GetString(reader.GetOrdinal("Empresa")),
                    TipoVisitaId = reader.IsDBNull(reader.GetOrdinal("TipoVisitaId")) ? null : reader.GetInt32(reader.GetOrdinal("TipoVisitaId")),
                    TipoVisitaNombre = reader.IsDBNull(reader.GetOrdinal("TipoVisitaNombre")) ? null : reader.GetString(reader.GetOrdinal("TipoVisitaNombre")),
                    PlacaVehiculo = reader.IsDBNull(reader.GetOrdinal("PlacaVehiculo")) ? null : reader.GetString(reader.GetOrdinal("PlacaVehiculo")),
                    PersonaDestino = reader.IsDBNull(reader.GetOrdinal("PersonaDestino")) ? null : reader.GetString(reader.GetOrdinal("PersonaDestino")),
                    AreaDestino = reader.IsDBNull(reader.GetOrdinal("AreaDestino")) ? null : reader.GetString(reader.GetOrdinal("AreaDestino")),
                    Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono")) ? null : reader.GetString(reader.GetOrdinal("Telefono")),
                    Notas = reader.IsDBNull(reader.GetOrdinal("Notas")) ? null : reader.GetString(reader.GetOrdinal("Notas"))
                });
            }

            return list;
        }

        public async Task<int> CreateAsync(VisitanteNoSocio visitante)
        {
            using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("sp_VisitantesNoSocios_Insert", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Nombre", visitante.Nombre);
            command.Parameters.AddWithValue("@Apellido", visitante.Apellido);
            command.Parameters.AddWithValue("@DocumentoIdentidad", (object?)visitante.DocumentoIdentidad ?? DBNull.Value);
            command.Parameters.AddWithValue("@Empresa", (object?)visitante.Empresa ?? DBNull.Value);
            command.Parameters.AddWithValue("@TipoVisitaId", (object?)visitante.TipoVisitaId ?? DBNull.Value);
            command.Parameters.AddWithValue("@OtroTipoVisitaId", (object?)visitante.OtroTipoVisitaId ?? DBNull.Value);
            command.Parameters.AddWithValue("@PlacaVehiculo", (object?)visitante.PlacaVehiculo ?? DBNull.Value);
            command.Parameters.AddWithValue("@PersonaDestino", (object?)visitante.PersonaDestino ?? DBNull.Value);
            command.Parameters.AddWithValue("@AreaDestino", (object?)visitante.AreaDestino ?? DBNull.Value);
            command.Parameters.AddWithValue("@Telefono", (object?)visitante.Telefono ?? DBNull.Value);
            command.Parameters.AddWithValue("@Notas", (object?)visitante.Notas ?? DBNull.Value);

            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }
    }
}