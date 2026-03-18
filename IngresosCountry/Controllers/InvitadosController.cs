using IngresosCountry.Models;
using IngresosCountry.Services;
using Microsoft.AspNetCore.Mvc;

namespace IngresosCountry.Controllers
{
    public class InvitadosController : Controller
    {
        private readonly IInvitadoService _invitadoService;
        private readonly ISocioService _socioService;

        public InvitadosController(IInvitadoService invitadoService, ISocioService socioService)
        {
            _invitadoService = invitadoService;
            _socioService = socioService;
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

        public async Task<IActionResult> Create()
        {
            ViewBag.Socios = await _socioService.GetAllAsync(estado: "Activo");
            ViewBag.Invitados = await _invitadoService.GetAllAsync();
            return View(new Invitacion());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Invitacion invitacion)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Socios = await _socioService.GetAllAsync(estado: "Activo");
                ViewBag.Invitados = await _invitadoService.GetAllAsync();
                return View(invitacion);
            }

            invitacion.CodigoQR = $"INV-{Guid.NewGuid():N}".Substring(0, 20).ToUpper();

            await _invitadoService.CreateInvitacionAsync(invitacion);

            TempData["Success"] = "Invitación registrada exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult CreateInvitado()
        {
            return View(new Invitado());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateInvitado(Invitado invitado)
        {
            if (!ModelState.IsValid)
                return View(invitado);

            await _invitadoService.CreateAsync(invitado);

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