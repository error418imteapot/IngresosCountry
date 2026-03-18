using IngresosCountry.Models;
using IngresosCountry.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IngresosCountry.Controllers
{
    [Authorize]
    public class InvitadosController : Controller
    {
        private readonly IInvitadoService _invitadoService;
        private readonly ISocioService _socioService;
        private readonly IAuditService _auditService;

        public InvitadosController(IInvitadoService invitadoService, ISocioService socioService, IAuditService auditService)
        {
            _invitadoService = invitadoService;
            _socioService = socioService;
            _auditService = auditService;
        }

        public async Task<IActionResult> Index()
        {
            var invitaciones = await _invitadoService.GetInvitacionesAsync();
            return View(invitaciones);
        }

        public async Task<IActionResult> Invitados()
        {
            var invitados = await _invitadoService.GetAllAsync();
            return View(invitados);
        }

        [Authorize(Policy = "ServiceDesk")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Socios = await _socioService.GetAllAsync(estado: "Activo");
            ViewBag.Invitados = await _invitadoService.GetAllAsync();
            return View(new Invitacion());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "ServiceDesk")]
        public async Task<IActionResult> Create(Invitacion invitacion)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Socios = await _socioService.GetAllAsync(estado: "Activo");
                ViewBag.Invitados = await _invitadoService.GetAllAsync();
                return View(invitacion);
            }

            // Generate QR code string
            invitacion.CodigoQR = $"INV-{Guid.NewGuid():N}".Substring(0, 20).ToUpper();

            var id = await _invitadoService.CreateInvitacionAsync(invitacion);
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _auditService.LogAsync(userId, "Crear Invitación", "tbl_invita", id);

            TempData["Success"] = "Invitación registrada exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "ServiceDesk")]
        public IActionResult CreateInvitado()
        {
            return View(new Invitado());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "ServiceDesk")]
        public async Task<IActionResult> CreateInvitado(Invitado invitado)
        {
            if (!ModelState.IsValid)
                return View(invitado);

            var id = await _invitadoService.CreateAsync(invitado);
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _auditService.LogAsync(userId, "Crear Invitado", "tbl_invitados", id);

            TempData["Success"] = "Invitado registrado exitosamente.";
            return RedirectToAction(nameof(Invitados));
        }

        [HttpGet]
        public async Task<IActionResult> ValidarQR(string codigo)
        {
            var invitacion = await _invitadoService.ValidateQRAsync(codigo);
            if (invitacion == null)
                return Json(new { success = false, message = "Código QR inválido o expirado." });

            return Json(new
            {
                success = true,
                message = "Invitación válida.",
                invitacion = new
                {
                    invitacion.InvitadoNombre,
                    invitacion.InvitadoApellido,
                    invitacion.SocioNombre,
                    invitacion.SocioApellido,
                    invitacion.FechaExpiracion
                }
            });
        }
    }
}