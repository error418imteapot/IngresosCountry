using IngresosCountry.Models;
using IngresosCountry.Services;
using Microsoft.AspNetCore.Mvc;

namespace IngresosCountry.Controllers
{
    public class SociosController : Controller
    {
        private readonly ISocioService _socioService;
        private readonly ICatalogService _catalogService;

        public SociosController(ISocioService socioService, ICatalogService catalogService)
        {
            _socioService = socioService;
            _catalogService = catalogService;
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

        public async Task<IActionResult> Create()
        {
            ViewBag.Paises = await _catalogService.GetPaisesAsync();
            return View(new Socio());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Socio socio)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Paises = await _catalogService.GetPaisesAsync();
                return View(socio);
            }

            await _socioService.CreateAsync(socio);

            TempData["Success"] = "Socio registrado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var socio = await _socioService.GetByIdAsync(id);
            if (socio == null) return NotFound();
            ViewBag.Paises = await _catalogService.GetPaisesAsync();
            return View(socio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Socio socio)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Paises = await _catalogService.GetPaisesAsync();
                return View(socio);
            }

            await _socioService.UpdateAsync(socio);

            TempData["Success"] = "Socio actualizado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _socioService.DeleteAsync(id);

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