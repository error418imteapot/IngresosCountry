namespace IngresosCountry.Models
{
    public class DashboardViewModel
    {
        public int EntradasHoy { get; set; }
        public int DenegadosHoy { get; set; }
        public int DentroDelClub { get; set; }
        public int SociosActivos { get; set; }
        public int SociosMorosos { get; set; }
        public int EventosProximos { get; set; }
        public List<AccessLog> UltimosAccesos { get; set; } = new();
    }

    public class ReportFilterViewModel
    {
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public string? TipoVisitante { get; set; }
        public string? ResultadoAcceso { get; set; }
        public int? AreaId { get; set; }
    }

    public class AccessReportItem
    {
        public DateTime Fecha { get; set; }
        public string TipoVisitante { get; set; } = string.Empty;
        public string ResultadoAcceso { get; set; } = string.Empty;
        public int Total { get; set; }
    }

    public class Area
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string? Ubicacion { get; set; }
        public bool Activo { get; set; }
    }

    public class Pais
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Codigo { get; set; }
    }
}