namespace IngresosCountry.Services
{
    public interface IAuditService
    {
        Task LogAsync(int? usuarioId, string accion, string? tabla = null, int? registroId = null, string? detalle = null, string? ip = null);
    }
}