using IngresosCountry.Models;
using IngresosCountry.Services;
using Microsoft.AspNetCore.Mvc;

namespace IngresosCountry.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IReportService _reportService;
        private readonly ICatalogService _catalogService;

        public ReportsController(IReportService reportService, ICatalogService catalogService)
        {
            _reportService = reportService;
            _catalogService = catalogService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AccessByDate(DateTime? fechaDesde, DateTime? fechaHasta)
        {
            var desde = fechaDesde ?? DateTime.Today.AddDays(-30);
            var hasta = fechaHasta ?? DateTime.Today;

            var data = await _reportService.GetAccessByDateAsync(desde, hasta);
            ViewBag.FechaDesde = desde;
            ViewBag.FechaHasta = hasta;
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> DeniedAccess(DateTime? fechaDesde, DateTime? fechaHasta)
        {
            var data = await _reportService.GetDeniedAccessAsync(fechaDesde, fechaHasta);
            ViewBag.FechaDesde = fechaDesde;
            ViewBag.FechaHasta = fechaHasta;
            return View(data);
        }
    }
}