using IngresosCountry.Models;
using IngresosCountry.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IngresosCountry.Controllers
{
    [Authorize]
    public class SociosController : Controller
    {
        private readonly ISocioService _socioService;
        private readonly ICatalogService _catalogService;
        private readonly IAuditService _auditService;

        public SociosController(ISocioService socioService, ICatalogService catalogService, IAuditService auditService)
        {
            _socioService = socioService;
            _catalogService = catalogService;
            _auditService = auditService;
        }

        public async Task<IActionResult> Index(string? estado, string? busqueda)
        {
            var socios = await _socioService.GetAllAsync(estado, busqueda);
            ViewBag.Estado = estado;
            ViewBag.Busqueda = busqueda;
            return View(socios);
        }

        public async Task<IActionResult> Details(int id)
        {
            var socio = await _socioService.GetByIdAsync(id);
            if (socio == null) return NotFound();
            return View(socio);
        }

        [Authorize(Policy = "ServiceDesk")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Paises = await _catalogService.GetPaisesAsync();
            return View(new Socio());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "ServiceDesk")]
        public async Task<IActionResult> Create(Socio socio)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Paises = await _catalogService.GetPaisesAsync();
                return View(socio);
            }

            var id = await _socioService.CreateAsync(socio);
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _auditService.LogAsync(userId, "Crear Socio", "tbl_socios", id,
                $"Socio creado: {socio.NombreCompleto}");

            TempData["Success"] = "Socio registrado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "ServiceDesk")]
        public async Task<IActionResult> Edit(int id)
        {
            var socio = await _socioService.GetByIdAsync(id);
            if (socio == null) return NotFound();
            ViewBag.Paises = await _catalogService.GetPaisesAsync();
            return View(socio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "ServiceDesk")]
        public async Task<IActionResult> Edit(Socio socio)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Paises = await _catalogService.GetPaisesAsync();
                return View(socio);
            }

            await _socioService.UpdateAsync(socio);
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _auditService.LogAsync(userId, "Editar Socio", "tbl_socios", socio.Id,
                $"Socio editado: {socio.NombreCompleto}");

            TempData["Success"] = "Socio actualizado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            await _socioService.DeleteAsync(id);
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _auditService.LogAsync(userId, "Eliminar Socio", "tbl_socios", id);

            TempData["Success"] = "Socio eliminado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        // API endpoint for member validation
        [HttpGet]
        public async Task<IActionResult> ValidarMembresia(string numero)
        {
            var socio = await _socioService.GetByMembresiaAsync(numero);
            if (socio == null)
                return Json(new { success = false, message = "Membresía no encontrada." });

            if (socio.TieneRestricciones)
                return Json(new { success = false, message = $"Acceso denegado. Estado: {socio.Estado}", socio });

            return Json(new { success = true, message = "Acceso aprobado.", socio });
        }
    }
}