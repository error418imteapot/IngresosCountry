using System.ComponentModel.DataAnnotations;

namespace IngresosCountry.Models
{
    public class Invitado
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es requerido")]
        public string Apellido { get; set; } = string.Empty;

        [Display(Name = "Documento de Identidad")]
        public string? DocumentoIdentidad { get; set; }

        [Display(Name = "Teléfono")]
        public string? Telefono { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string NombreCompleto => $"{Nombre} {Apellido}";
    }

    public class Invitacion
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Socio")]
        public int SocioId { get; set; }

        [Required]
        [Display(Name = "Invitado")]
        public int InvitadoId { get; set; }

        [Required]
        [Display(Name = "Fecha de Invitación")]
        [DataType(DataType.Date)]
        public DateTime FechaInvitacion { get; set; } = DateTime.Now;

        [Display(Name = "Fecha de Expiración")]
        [DataType(DataType.Date)]
        public DateTime? FechaExpiracion { get; set; }

        [Display(Name = "Código QR")]
        public string? CodigoQR { get; set; }

        public string Estado { get; set; } = "Pendiente";
        public string? Notas { get; set; }

        // Datos para mostrar (joins)
        public string? SocioNombre { get; set; }
        public string? SocioApellido { get; set; }
        public string? InvitadoNombre { get; set; }
        public string? InvitadoApellido { get; set; }
        public string? NumeroMembresia { get; set; }
    }
}