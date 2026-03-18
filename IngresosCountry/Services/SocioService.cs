using IngresosCountry.Data;
using IngresosCountry.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace IngresosCountry.Services
{
    public class SocioService : ISocioService
    {
        private readonly DatabaseConnection _db;

        public SocioService(DatabaseConnection db)
        {
            _db = db;
        }

        public async Task<List<Socio>> GetAllAsync(string? estado = null, string? busqueda = null)
        {
            var socios = new List<Socio>();
            using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("sp_Socios_GetAll", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Estado", (object?)estado ?? DBNull.Value);
            command.Parameters.AddWithValue("@Busqueda", (object?)busqueda ?? DBNull.Value);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                socios.Add(MapSocio(reader));
            }
            return socios;
        }

        public async Task<Socio?> GetByIdAsync(int id)
        {
            using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("sp_Socios_GetById", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapSocio(reader);
            }
            return null;
        }

        public async Task<Socio?> GetByMembresiaAsync(string numeroMembresia)
        {
            using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("sp_Socios_GetByMembresia", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@NumeroMembresia", numeroMembresia);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapSocio(reader);
            }
            return null;
        }

        public async Task<int> CreateAsync(Socio socio)
        {
            using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("sp_Socios_Insert", connection);
            command.CommandType = CommandType.StoredProcedure;
            AddSocioParameters(command, socio);

            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task UpdateAsync(Socio socio)
        {
            using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("sp_Socios_Update", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Id", socio.Id);
            AddSocioParameters(command, socio);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("sp_Socios_Delete", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }

        private static void AddSocioParameters(SqlCommand command, Socio socio)
        {
            command.Parameters.AddWithValue("@NumeroMembresia", socio.NumeroMembresia);
            command.Parameters.AddWithValue("@Nombre", socio.Nombre);
            command.Parameters.AddWithValue("@Apellido", socio.Apellido);
            command.Parameters.AddWithValue("@DocumentoIdentidad", (object?)socio.DocumentoIdentidad ?? DBNull.Value);
            command.Parameters.AddWithValue("@Email", (object?)socio.Email ?? DBNull.Value);
            command.Parameters.AddWithValue("@Telefono", (object?)socio.Telefono ?? DBNull.Value);
            command.Parameters.AddWithValue("@Direccion", (object?)socio.Direccion ?? DBNull.Value);
            command.Parameters.AddWithValue("@PaisId", (object?)socio.PaisId ?? DBNull.Value);
            command.Parameters.AddWithValue("@FechaNacimiento", (object?)socio.FechaNacimiento ?? DBNull.Value);
            command.Parameters.AddWithValue("@TipoMembresia", (object?)socio.TipoMembresia ?? DBNull.Value);
            command.Parameters.AddWithValue("@Estado", socio.Estado);
            command.Parameters.AddWithValue("@FotoUrl", (object?)socio.FotoUrl ?? DBNull.Value);
            command.Parameters.AddWithValue("@Notas", (object?)socio.Notas ?? DBNull.Value);
        }

        private static Socio MapSocio(SqlDataReader reader)
        {
            return new Socio
            {
                Id = reader.GetInt32("Id"),
                NumeroMembresia = reader.GetString("NumeroMembresia"),
                Nombre = reader.GetString("Nombre"),
                Apellido = reader.GetString("Apellido"),
                DocumentoIdentidad = reader.IsDBNull("DocumentoIdentidad") ? null : reader.GetString("DocumentoIdentidad"),
                Email = reader.IsDBNull("Email") ? null : reader.GetString("Email"),
                Telefono = reader.IsDBNull("Telefono") ? null : reader.GetString("Telefono"),
                Direccion = reader.IsDBNull("Direccion") ? null : reader.GetString("Direccion"),
                PaisId = reader.IsDBNull("PaisId") ? null : reader.GetInt32("PaisId"),
                PaisNombre = reader.IsDBNull("PaisNombre") ? null : reader.GetString("PaisNombre"),
                FechaNacimiento = reader.IsDBNull("FechaNacimiento") ? null : reader.GetDateTime("FechaNacimiento"),
                FechaIngreso = reader.GetDateTime("FechaIngreso"),
                TipoMembresia = reader.IsDBNull("TipoMembresia") ? null : reader.GetString("TipoMembresia"),
                Estado = reader.GetString("Estado"),
                FotoUrl = reader.IsDBNull("FotoUrl") ? null : reader.GetString("FotoUrl"),
                Notas = reader.IsDBNull("Notas") ? null : reader.GetString("Notas"),
                Activo = reader.GetBoolean("Activo")
            };
        }
    }
}