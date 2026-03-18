using IngresosCountry.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IngresosCountry.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class ApiController : ControllerBase
    {
        private readonly ISocioService _socioService;
        private readonly IInvitadoService _invitadoService;

        public ApiController(ISocioService socioService, IInvitadoService invitadoService)
        {
            _socioService = socioService;
            _invitadoService = invitadoService;
        }

        [HttpGet("socios/buscar")]
        public async Task<IActionResult> BuscarSocios(string? q)
        {
            var socios = await _socioService.GetAllAsync(busqueda: q);
            return Ok(socios.Select(s => new { s.Id, s.NumeroMembresia, s.Nombre, s.Apellido, s.Estado }));
        }

        [HttpGet("socios/validar/{numero}")]
        public async Task<IActionResult> ValidarSocio(string numero)
        {
            var socio = await _socioService.GetByMembresiaAsync(numero);
            if (socio == null)
                return Ok(new { success = false, message = "Membresía no encontrada." });

            if (socio.TieneRestricciones)
                return Ok(new { success = false, message = $"Acceso denegado. Estado: {socio.Estado}", data = socio });

            return Ok(new { success = true, message = "Acceso aprobado.", data = socio });
        }

        [HttpGet("invitados/validar-qr/{codigo}")]
        public async Task<IActionResult> ValidarQR(string codigo)
        {
            var invitacion = await _invitadoService.ValidateQRAsync(codigo);
            if (invitacion == null)
                return Ok(new { success = false, message = "Código QR inválido o expirado." });

            return Ok(new { success = true, message = "Invitación válida.", data = invitacion });
        }
    }
}