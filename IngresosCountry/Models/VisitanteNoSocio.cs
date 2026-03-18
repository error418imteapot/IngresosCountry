using System.ComponentModel.DataAnnotations;

namespace IngresosCountry.Models
{
    public class VisitanteNoSocio
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es requerido")]
        public string Apellido { get; set; } = string.Empty;

        [Display(Name = "Documento de Identidad")]
        public string? DocumentoIdentidad { get; set; }

        public string? Empresa { get; set; }

        [Display(Name = "Tipo de Visita")]
        public int? TipoVisitaId { get; set; }
        public string? TipoVisitaNombre { get; set; }

        [Display(Name = "Otro Tipo de Visita")]
        public int? OtroTipoVisitaId { get; set; }

        [Display(Name = "Placa del Vehículo")]
        public string? PlacaVehiculo { get; set; }

        [Display(Name = "Persona Destino")]
        public string? PersonaDestino { get; set; }

        [Display(Name = "Área Destino")]
        public string? AreaDestino { get; set; }

        [Display(Name = "Teléfono")]
        public string? Telefono { get; set; }

        public string? Notas { get; set; }

        public string NombreCompleto => $"{Nombre} {Apellido}";
    }

    public class TipoVisita
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public bool Activo { get; set; }
    }
}