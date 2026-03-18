using System.ComponentModel.DataAnnotations;

namespace IngresosCountry.Models
{
    public class AccessLog
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tipo de Visitante")]
        public string TipoVisitante { get; set; } = string.Empty;

        public int? SocioId { get; set; }
        public int? InvitadoId { get; set; }
        public int? NoSocioId { get; set; }
        public int? EventoId { get; set; }

        [Display(Name = "Área")]
        public int? AreaId { get; set; }
        public string? AreaNombre { get; set; }

        [Display(Name = "Fecha de Entrada")]
        public DateTime FechaEntrada { get; set; }

        [Display(Name = "Fecha de Salida")]
        public DateTime? FechaSalida { get; set; }

        [Display(Name = "Resultado")]
        public string ResultadoAcceso { get; set; } = "Aprobado";

        [Display(Name = "Motivo de Rechazo")]
        public string? MotivoRechazo { get; set; }

        public int? RegistradoPor { get; set; }
        public string? RegistradoPorNombre { get; set; }

        [Display(Name = "Punto de Acceso")]
        public string? PuntoAcceso { get; set; }

        public string? Notas { get; set; }
        public string? VisitanteNombre { get; set; }
    }

    public class RegistrarAccesoViewModel
    {
        [Required]
        public string TipoVisitante { get; set; } = "Socio";
        public int? SocioId { get; set; }
        public int? InvitadoId { get; set; }
        public int? NoSocioId { get; set; }
        public int? EventoId { get; set; }
        public int? AreaId { get; set; }
        public string ResultadoAcceso { get; set; } = "Aprobado";
        public string? MotivoRechazo { get; set; }
        public string? PuntoAcceso { get; set; }
        public string? Notas { get; set; }
    }
}