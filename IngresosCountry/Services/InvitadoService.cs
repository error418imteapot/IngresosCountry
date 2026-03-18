using IngresosCountry.Data;
using IngresosCountry.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace IngresosCountry.Services
{
    public class InvitadoService : IInvitadoService
    {
        private readonly DatabaseConnection _db;

        public InvitadoService(DatabaseConnection db)
        {
            _db = db;
        }

        public async Task<List<Invitado>> GetAllAsync()
        {
            var list = new List<Invitado>();
            using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("sp_Invitados_GetAll", connection);
            command.CommandType = CommandType.StoredProcedure;

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new Invitado
                {
                    Id = reader.GetInt32("Id"),
                    Nombre = reader.GetString("Nombre"),
                    Apellido = reader.GetString("Apellido"),
                    DocumentoIdentidad = reader.IsDBNull("DocumentoIdentidad") ? null : reader.GetString("DocumentoIdentidad"),
                    Telefono = reader.IsDBNull("Telefono") ? null : reader.GetString("Telefono"),
                    Email = reader.IsDBNull("Email") ? null : reader.GetString("Email")
                });
            }
            return list;
        }

        public async Task<Invitado?> GetByIdAsync(int id)
        {
            using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("sp_Invitados_GetById", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Invitado
                {
                    Id = reader.GetInt32("Id"),
                    Nombre = reader.GetString("Nombre"),
                    Apellido = reader.GetString("Apellido"),
                    DocumentoIdentidad = reader.IsDBNull("DocumentoIdentidad") ? null : reader.GetString("DocumentoIdentidad"),
                    Telefono = reader.IsDBNull("Telefono") ? null : reader.GetString("Telefono"),
                    Email = reader.IsDBNull("Email") ? null : reader.GetString("Email")
                };
            }
            return null;
        }

        public async Task<int> CreateAsync(Invitado invitado)
        {
            using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("sp_Invitados_Insert", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Nombre", invitado.Nombre);
            command.Parameters.AddWithValue("@Apellido", invitado.Apellido);
            command.Parameters.AddWithValue("@DocumentoIdentidad", (object?)invitado.DocumentoIdentidad ?? DBNull.Value);
            command.Parameters.AddWithValue("@Telefono", (object?)invitado.Telefono ?? DBNull.Value);
            command.Parameters.AddWithValue("@Email", (object?)invitado.Email ?? DBNull.Value);

            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task UpdateAsync(Invitado invitado)
        {
            using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("sp_Invitados_Update", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Id", invitado.Id);
            command.Parameters.AddWithValue("@Nombre", invitado.Nombre);
            command.Parameters.AddWithValue("@Apellido", invitado.Apellido);
            command.Parameters.AddWithValue("@DocumentoIdentidad", (object?)invitado.DocumentoIdentidad ?? DBNull.Value);
            command.Parameters.AddWithValue("@Telefono", (object?)invitado.Telefono ?? DBNull.Value);
            command.Parameters.AddWithValue("@Email", (object?)invitado.Email ?? DBNull.Value);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<List<Invitacion>> GetInvitacionesAsync()
        {
            var list = new List<Invitacion>();
            using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("sp_Invita_GetAll", connection);
            command.CommandType = CommandType.StoredProcedure;

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(MapInvitacion(reader));
            }
            return list;
        }

        public async Task<List<Invitacion>> GetInvitacionesBySocioAsync(int socioId)
        {
            var list = new List<Invitacion>();
            using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("sp_Invita_GetBySocio", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@SocioId", socioId);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(MapInvitacion(reader));
            }
            return list;
        }

        public async Task<int> CreateInvitacionAsync(Invitacion invitacion)
        {
            using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("sp_Invita_Insert", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@SocioId", invitacion.SocioId);
            command.Parameters.AddWithValue("@InvitadoId", invitacion.InvitadoId);
            command.Parameters.AddWithValue("@FechaInvitacion", invitacion.FechaInvitacion);
            command.Parameters.AddWithValue("@FechaExpiracion", (object?)invitacion.FechaExpiracion ?? DBNull.Value);
            command.Parameters.AddWithValue("@CodigoQR", (object?)invitacion.CodigoQR ?? DBNull.Value);
            command.Parameters.AddWithValue("@Notas", (object?)invitacion.Notas ?? DBNull.Value);

            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<Invitacion?> ValidateQRAsync(string codigoQR)
        {
            using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("sp_Invita_ValidateQR", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@CodigoQR", codigoQR);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapInvitacion(reader);
            }
            return null;
        }

        private static Invitacion MapInvitacion(SqlDataReader reader)
        {
            return new Invitacion
            {
                Id = reader.GetInt32("Id"),
                SocioId = reader.GetInt32("SocioId"),
                InvitadoId = reader.GetInt32("InvitadoId"),
                FechaInvitacion = reader.GetDateTime("FechaInvitacion"),
                FechaExpiracion = reader.IsDBNull("FechaExpiracion") ? null : reader.GetDateTime("FechaExpiracion"),
                CodigoQR = reader.IsDBNull("CodigoQR") ? null : reader.GetString("CodigoQR"),
                Estado = reader.GetString("Estado"),
                SocioNombre = reader.IsDBNull("SocioNombre") ? null : reader.GetString("SocioNombre"),
                SocioApellido = reader.IsDBNull("SocioApellido") ? null : reader.GetString("SocioApellido"),
                InvitadoNombre = reader.IsDBNull("InvitadoNombre") ? null : reader.GetString("InvitadoNombre"),
                InvitadoApellido = reader.IsDBNull("InvitadoApellido") ? null : reader.GetString("InvitadoApellido")
            };
        }
    }
}