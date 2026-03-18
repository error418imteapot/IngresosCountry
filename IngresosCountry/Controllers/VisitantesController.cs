using IngresosCountry.Models;
using IngresosCountry.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IngresosCountry.Controllers
{
    [Authorize(Policy = "Security")]
    public class VisitantesController : Controller
    {
        private readonly IVisitanteService _visitanteService;
        private readonly ICatalogService _catalogService;
        private readonly IAuditService _auditService;

        public VisitantesController(IVisitanteService visitanteService, ICatalogService catalogService, IAuditService auditService)
        {
            _visitanteService = visitanteService;
            _catalogService = catalogService;
            _auditService = auditService;
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

            var id = await _visitanteService.CreateAsync(visitante);
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _auditService.LogAsync(userId, "Registrar Visitante", "tbl_visitas_nosocios", id,
                $"Visitante: {visitante.NombreCompleto}");

            TempData["Success"] = "Visitante registrado exitosamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}