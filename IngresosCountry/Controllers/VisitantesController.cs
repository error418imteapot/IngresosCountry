using IngresosCountry.Models;
using IngresosCountry.Services;
using Microsoft.AspNetCore.Mvc;

namespace IngresosCountry.Controllers
{
    public class VisitantesController : Controller
    {
        private readonly IVisitanteService _visitanteService;
        private readonly ICatalogService _catalogService;

        public VisitantesController(IVisitanteService visitanteService, ICatalogService catalogService)
        {
            _visitanteService = visitanteService;
            _catalogService = catalogService;
        }

        public async Task<IActionResult> Index()
        {
            var visitantes = await _visitanteService.GetAllAsync();
            return View(visitantes);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.TiposVisita = await _catalogService.GetTiposVisitaAsync();
            return View(new VisitanteNoSocio());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VisitanteNoSocio visitante)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TiposVisita = await _catalogService.GetTiposVisitaAsync();
                return View(visitante);
            }

            await _visitanteService.CreateAsync(visitante);

            TempData["Success"] = "Visitante registrado exitosamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}