using System.ComponentModel.DataAnnotations;

namespace IngresosCountry.Models
{
    public class Socio
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El número de membresía es requerido")]
        [Display(Name = "No. Membresía")]
        public string NumeroMembresia { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es requerido")]
        public string Apellido { get; set; } = string.Empty;

        [Display(Name = "Documento de Identidad")]
        public string? DocumentoIdentidad { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Display(Name = "Teléfono")]
        public string? Telefono { get; set; }

        [Display(Name = "Dirección")]
        public string? Direccion { get; set; }

        [Display(Name = "País")]
        public int? PaisId { get; set; }
        public string? PaisNombre { get; set; }

        [Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date)]
        public DateTime? FechaNacimiento { get; set; }

        [Display(Name = "Fecha de Ingreso")]
        [DataType(DataType.Date)]
        public DateTime FechaIngreso { get; set; } = DateTime.Now;

        [Display(Name = "Fecha de Vencimiento")]
        [DataType(DataType.Date)]
        public DateTime? FechaVencimiento { get; set; }

        [Display(Name = "Tipo de Membresía")]
        public string? TipoMembresia { get; set; }

        [Required]
        public string Estado { get; set; } = "Activo";

        public string? FotoUrl { get; set; }
        public string? Notas { get; set; }
        public bool Activo { get; set; } = true;

        public string NombreCompleto => $"{Nombre} {Apellido}";
        public bool TieneRestricciones => Estado == "Moroso" || Estado == "Suspendido";
    }
}