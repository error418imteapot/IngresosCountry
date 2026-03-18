using IngresosCountry.Models;
using IngresosCountry.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IngresosCountry.Controllers
{
    [Authorize]
    public class EventosController : Controller
    {
        private readonly IEventoService _eventoService;
        private readonly ISocioService _socioService;
        private readonly IAuditService _auditService;

        public EventosController(IEventoService eventoService, ISocioService socioService, IAuditService auditService)
        {
            _eventoService = eventoService;
            _socioService = socioService;
            _auditService = auditService;
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

        [Authorize(Policy = "ServiceDesk")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Socios = await _socioService.GetAllAsync(estado: "Activo");
            return View(new Evento());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "ServiceDesk")]
        public async Task<IActionResult> Create(Evento evento)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Socios = await _socioService.GetAllAsync(estado: "Activo");
                return View(evento);
            }

            var id = await _eventoService.CreateAsync(evento);
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _auditService.LogAsync(userId, "Crear Evento", "tbl_eventos", id,
                $"Evento: {evento.Nombre}");

            TempData["Success"] = "Evento creado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "ServiceDesk")]
        public async Task<IActionResult> Edit(int id)
        {
            var evento = await _eventoService.GetByIdAsync(id);
            if (evento == null) return NotFound();

            ViewBag.Socios = await _socioService.GetAllAsync(estado: "Activo");
            return View(evento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "ServiceDesk")]
        public async Task<IActionResult> Edit(Evento evento)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Socios = await _socioService.GetAllAsync(estado: "Activo");
                return View(evento);
            }

            await _eventoService.UpdateAsync(evento);
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _auditService.LogAsync(userId, "Editar Evento", "tbl_eventos", evento.Id);

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