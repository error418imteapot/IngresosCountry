using IngresosCountry.Models;
using IngresosCountry.Services;
using Microsoft.AspNetCore.Mvc;

namespace IngresosCountry.Controllers
{
    public class EventosController : Controller
    {
        private readonly IEventoService _eventoService;
        private readonly ISocioService _socioService;

        public EventosController(IEventoService eventoService, ISocioService socioService)
        {
            _eventoService = eventoService;
            _socioService = socioService;
        }

        public async Task<IActionResult> Index(string? estado)
        {
            var eventos = await _eventoService.GetAllAsync(estado);
            ViewBag.Estado = estado;
            return View(eventos);
        }

        public async Task<IActionResult> Details(int id)
        {
            var evento = await _eventoService.GetByIdAsync(id);
            if (evento == null) return NotFound();

            ViewBag.Participantes = await _eventoService.GetParticipantesAsync(id);
            return View(evento);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Socios = await _socioService.GetAllAsync(estado: "Activo");
            return View(new Evento());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Evento evento)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Socios = await _socioService.GetAllAsync(estado: "Activo");
                return View(evento);
            }

            await _eventoService.CreateAsync(evento);

            TempData["Success"] = "Evento creado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var evento = await _eventoService.GetByIdAsync(id);
            if (evento == null) return NotFound();

            ViewBag.Socios = await _socioService.GetAllAsync(estado: "Activo");
            return View(evento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Evento evento)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Socios = await _socioService.GetAllAsync(estado: "Activo");
                return View(evento);
            }

            await _eventoService.UpdateAsync(evento);

            TempData["Success"] = "Evento actualizado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddParticipante(EventoParticipante participante)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Details), new { id = participante.EventoId });

            await _eventoService.AddParticipanteAsync(participante);

            TempData["Success"] = "Participante agregado exitosamente.";
            return RedirectToAction(nameof(Details), new { id = participante.EventoId });
        }
    }
}