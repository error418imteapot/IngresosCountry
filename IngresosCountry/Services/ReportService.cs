using IngresosCountry.Data;
using IngresosCountry.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace IngresosCountry.Services
{
    public class ReportService : IReportService
    {
        private readonly DatabaseConnection _db;
        private readonly IAccessLogService _accessLogService;

        public ReportService(DatabaseConnection db, IAccessLogService accessLogService)
        {
            _db = db;
            _accessLogService = accessLogService;
        }

        public async Task<DashboardViewModel> GetDashboardAsync()
        {
            var dashboard = new DashboardViewModel();
            using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("sp_Reports_Dashboard", connection);
            command.CommandType = CommandType.StoredProcedure;

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                dashboard.EntradasHoy = reader.GetInt32("EntradasHoy");
                dashboard.DenegadosHoy = reader.GetInt32("DenegadosHoy");
                dashboard.DentroDelClub = reader.GetInt32("DentroDelClub");
                dashboard.SociosActivos = reader.GetInt32("SociosActivos");
                dashboard.SociosMorosos = reader.GetInt32("SociosMorosos");
                dashboard.EventosProximos = reader.GetInt32("EventosProximos");
            }

            // Get latest access logs
            dashboard.UltimosAccesos = await _accessLogService.GetAllAsync(
                fechaDesde: DateTime.Today, fechaHasta: DateTime.Today);

            return dashboard;
        }

        public async Task<List<AccessReportItem>> GetAccessByDateAsync(DateTime fechaDesde, DateTime fechaHasta)
        {
            var list = new List<AccessReportItem>();
            using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("sp_Reports_AccessByDate", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@FechaDesde", fechaDesde);
            command.Parameters.AddWithValue("@FechaHasta", fechaHasta);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new AccessReportItem
                {
                    Fecha = reader.GetDateTime("Fecha"),
                    TipoVisitante = reader.GetString("TipoVisitante"),
                    ResultadoAcceso = reader.GetString("ResultadoAcceso"),
                    Total = reader.GetInt32("Total")
                });
            }
            return list;
        }

        public async Task<List<AccessLog>> GetDeniedAccessAsync(DateTime? fechaDesde = null, DateTime? fechaHasta = null)
        {
            var list = new List<AccessLog>();
            using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("sp_Reports_DeniedAccess", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@FechaDesde", (object?)fechaDesde ?? DBNull.Value);
            command.Parameters.AddWithValue("@FechaHasta", (object?)fechaHasta ?? DBNull.Value);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new AccessLog
                {
                    Id = reader.GetInt32("Id"),
                    TipoVisitante = reader.GetString("TipoVisitante"),
                    FechaEntrada = reader.GetDateTime("FechaEntrada"),
                    ResultadoAcceso = reader.GetString("ResultadoAcceso"),
                    MotivoRechazo = reader.IsDBNull("MotivoRechazo") ? null : reader.GetString("MotivoRechazo"),
                    VisitanteNombre = reader.IsDBNull("VisitanteNombre") ? null : reader.GetString("VisitanteNombre")
                });
            }
            return list;
        }
    }
}