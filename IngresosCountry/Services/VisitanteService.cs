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
                    Id = reader.GetInt32("Id"),
                    Nombre = reader.GetString("Nombre"),
                    Apellido = reader.GetString("Apellido"),
                    DocumentoIdentidad = reader.IsDBNull("DocumentoIdentidad") ? null : reader.GetString("DocumentoIdentidad"),
                    Empresa = reader.IsDBNull("Empresa") ? null : reader.GetString("Empresa"),
                    TipoVisitaId = reader.IsDBNull("TipoVisitaId") ? null : reader.GetInt32("TipoVisitaId"),
                    TipoVisitaNombre = reader.IsDBNull("TipoVisitaNombre") ? null : reader.GetString("TipoVisitaNombre"),
                    PlacaVehiculo = reader.IsDBNull("PlacaVehiculo") ? null : reader.GetString("PlacaVehiculo"),
                    PersonaDestino = reader.IsDBNull("PersonaDestino") ? null : reader.GetString("PersonaDestino"),
                    AreaDestino = reader.IsDBNull("AreaDestino") ? null : reader.GetString("AreaDestino"),
                    Telefono = reader.IsDBNull("Telefono") ? null : reader.GetString("Telefono"),
                    Notas = reader.IsDBNull("Notas") ? null : reader.GetString("Notas")
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