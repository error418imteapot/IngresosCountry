using IngresosCountry.Services;
using Microsoft.AspNetCore.Mvc;

namespace IngresosCountry.Controllers
{
    public class HomeController : Controller
    {
        private readonly IReportService _reportService;

        public HomeController(IReportService reportService)
        {
            _reportService = reportService;
        }

        public async Task<IActionResult> Index()
        {
            var dashboard = await _reportService.GetDashboardAsync();
            return View(dashboard);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}