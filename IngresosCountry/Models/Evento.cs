using System.ComponentModel.DataAnnotations;

namespace IngresosCountry.Models
{
    public class Evento
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del evento es requerido")]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es requerida")]
        [Display(Name = "Fecha Inicio")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es requerida")]
        [Display(Name = "Fecha Fin")]
        public DateTime FechaFin { get; set; }

        [Display(Name = "Ubicación")]
        public string? Ubicacion { get; set; }

        public int? Capacidad { get; set; }

        [Display(Name = "Organizador (Socio)")]
        public int? OrganizadorSocioId { get; set; }
        public string? OrganizadorNombre { get; set; }

        public string Estado { get; set; } = "Programado";
        public bool Activo { get; set; } = true;
        public int TotalParticipantes { get; set; }
    }

    public class EventoParticipante
    {
        public int Id { get; set; }

        [Required]
        public int EventoId { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "Documento de Identidad")]
        public string? DocumentoIdentidad { get; set; }

        [Required]
        [Display(Name = "Tipo de Participante")]
        public string TipoParticipante { get; set; } = "Externo";

        public int? SocioId { get; set; }
        public int? InvitadoId { get; set; }
        public bool Confirmado { get; set; }
        public string? NumeroMembresia { get; set; }
    }
}