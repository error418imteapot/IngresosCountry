using IngresosCountry.Models;
using IngresosCountry.Services;
using Microsoft.AspNetCore.Mvc;

namespace IngresosCountry.Controllers
{
    public class AccessLogsController : Controller
    {
        private readonly IAccessLogService _accessLogService;
        private readonly ICatalogService _catalogService;
        private readonly ISocioService _socioService;

        public AccessLogsController(IAccessLogService accessLogService, ICatalogService catalogService,
            ISocioService socioService)
        {
            _accessLogService = accessLogService;
            _catalogService = catalogService;
            _socioService = socioService;
        }

        public async Task<IActionResult> Index(DateTime? fechaDesde, DateTime? fechaHasta,
            string? tipoVisitante, string? resultadoAcceso, int? areaId)
        {
            var logs = await _accessLogService.GetAllAsync(fechaDesde, fechaHasta, tipoVisitante, resultadoAcceso, areaId);
            ViewBag.Areas = await _catalogService.GetAreasAsync();
            ViewBag.FechaDesde = fechaDesde;
            ViewBag.FechaHasta = fechaHasta;
            ViewBag.TipoVisitante = tipoVisitante;
            ViewBag.ResultadoAcceso = resultadoAcceso;
            ViewBag.AreaId = areaId;
            return View(logs);
        }

        public async Task<IActionResult> RegistrarAcceso()
        {
            ViewBag.Areas = await _catalogService.GetAreasAsync();
            return View(new RegistrarAccesoViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarAcceso(RegistrarAccesoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Areas = await _catalogService.GetAreasAsync();
                return View(model);
            }

            if (model.TipoVisitante == "Socio" && model.SocioId.HasValue)
            {
                var socio = await _socioService.GetByIdAsync(model.SocioId.Value);
                if (socio != null && socio.TieneRestricciones)
                {
                    model.ResultadoAcceso = "Denegado";
                    model.MotivoRechazo = $"Socio con estado: {socio.Estado}";
                }
            }

            var id = await _accessLogService.RegistrarEntradaAsync(model);

            TempData["Success"] = model.ResultadoAcceso == "Aprobado"
                ? "Acceso registrado exitosamente."
                : $"Acceso DENEGADO. Motivo: {model.MotivoRechazo}";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarSalida(int id)
        {
            await _accessLogService.RegistrarSalidaAsync(id);

            TempData["Success"] = "Salida registrada exitosamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}