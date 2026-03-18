using IngresosCountry.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace IngresosCountry.Services
{
    public class AuditService : IAuditService
    {
        private readonly DatabaseConnection _db;

        public AuditService(DatabaseConnection db)
        {
            _db = db;
        }

        public async Task LogAsync(int? usuarioId, string accion, string? tabla = null, int? registroId = null, string? detalle = null, string? ip = null)
        {
            using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("sp_AuditLog_Insert", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@UsuarioId", (object?)usuarioId ?? DBNull.Value);
            command.Parameters.AddWithValue("@Accion", accion);
            command.Parameters.AddWithValue("@Tabla", (object?)tabla ?? DBNull.Value);
            command.Parameters.AddWithValue("@RegistroId", (object?)registroId ?? DBNull.Value);
            command.Parameters.AddWithValue("@Detalle", (object?)detalle ?? DBNull.Value);
            command.Parameters.AddWithValue("@DireccionIP", (object?)ip ?? DBNull.Value);

            await command.ExecuteNonQueryAsync();
        }
    }
}