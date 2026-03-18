using IngresosCountry.Data;
using IngresosCountry.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace IngresosCountry.Services
{
    public class AuthService : IAuthService
    {
        private readonly DatabaseConnection _db;

        public AuthService(DatabaseConnection db)
        {
            _db = db;
        }

        public async Task<Usuario?> ValidateUserAsync(string username, string password)
        {
            using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("sp_Usuario_Login", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Username", username);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var user = new Usuario
                {
                    Id = reader.GetInt32("Id"),
                    Username = reader.GetString("Username"),
                    PasswordHash = reader.GetString("PasswordHash"),
                    NombreCompleto = reader.GetString("NombreCompleto"),
                    Email = reader.IsDBNull("Email") ? null : reader.GetString("Email"),
                    RolId = reader.GetInt32("RolId"),
                    RolNombre = reader.GetString("RolNombre"),
                    Activo = reader.GetBoolean("Activo")
                };

                // Verify password using BCrypt
                if (BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                {
                    return user;
                }
            }

            return null;
        }
    }
}