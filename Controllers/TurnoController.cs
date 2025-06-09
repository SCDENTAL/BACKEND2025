using Agenda.Base;
using Agenda.Entidades.DTOs.DTO.TurnosDTO;
using Agenda.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Agenda.Controllers
{
     [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador")]
    public class TurnoController : ControllerBase
    {
        private readonly ITurnoService _turnoService;
        private readonly AppDbContext _context;
        public TurnoController(ITurnoService turnoService, AppDbContext AppDbContext)
        {
            _context = AppDbContext;
            _turnoService = turnoService;
        }
               

        private int ObtenerUsuarioId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet("{calendarioId}")]
        public async Task<ActionResult<List<TurnoDTO>>> ObtenerTurnos(int calendarioId)
        {
            var turnos = await _turnoService.ObtenerTurnosAsync(calendarioId, ObtenerUsuarioId());
            return Ok(turnos);
        }

        [HttpGet("filtrar/{calendarioId}")]
        public async Task<ActionResult<ResTurnosFiltrados>> Filtrar(int calendarioId, [FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            var result = await _turnoService.FiltrarPorFechasAsync(calendarioId, fechaInicio, fechaFin, ObtenerUsuarioId());
            if (result == null) return BadRequest("Calendario no encontrado o parámetros inválidos.");
            return Ok(result);
        }

        [HttpPost("reservar/{turnoId}")]
        public async Task<IActionResult> Reservar(int turnoId, [FromBody] ReservarTurnoDTO dto)
        {
            var success = await _turnoService.ReservarTurnoAsync(turnoId, dto, ObtenerUsuarioId());
            return success ? Ok("Turno reservado.") : BadRequest("No se pudo reservar el turno.");
        }

        [HttpPost("cancelar/{turnoId}")]
        public async Task<IActionResult> Cancelar(int turnoId)
        {
            var success = await _turnoService.CancelarTurnoAsync(turnoId, ObtenerUsuarioId());
            return success ? Ok("Turno cancelado.") : BadRequest("No se pudo cancelar el turno.");
        }

        [HttpGet("mi-calendario")]
        [Authorize]
        public async Task<IActionResult> GetCalendarioActivo()
        {            
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            var ultimo = await _context.Calendarios
                .Where(c => c.IdUsuario == userId)
                .OrderByDescending(c => c.FechaFin)
                .FirstOrDefaultAsync();

            if (ultimo == null)
                return NotFound(); 
            
            return Ok(new
            {
                calendarioId = ultimo.Id,
                fechaInicio = ultimo.FechaInicio,
                fechaFin = ultimo.FechaFin
            });
        }


        [Authorize]
        [HttpPut("{id}/asistencia")]
        public async Task<IActionResult> MarcarAsistencia(int id, [FromBody] MarcarAsistenciaDTO dto)
        {
            var usuarioId = ObtenerUsuarioId(); 

            var result = await _turnoService.MarcarAsistenciaAsync(id, dto.Asistio, usuarioId);
            if (!result) return BadRequest("No se pudo marcar la asistencia.");

            return Ok("Asistencia marcada correctamente.");
        }

        [Authorize]
        [HttpPut("{id}/cancelar")]
        public async Task<IActionResult> CancelarTurno(int id)
        {
            var usuarioId = ObtenerUsuarioId(); 

            var result = await _turnoService.CancelarTurnoAsync(id, usuarioId);
            if (!result) return BadRequest("No se pudo cancelar el turno.");

            return Ok("Turno cancelado correctamente.");
        }


        [HttpPost("crear/{calendarioId}")]
        public async Task<IActionResult> CrearTurnos(int calendarioId)
        {            
            var calendario = await _turnoService.ObtenerCalendarioEntidadAsync(calendarioId, ObtenerUsuarioId());
            if (calendario == null) return BadRequest("Calendario no encontrado.");
            await _turnoService.CrearTurnosAsync(
                Enumerable.Range(0, (int)(calendario.FechaFin - calendario.FechaInicio).TotalDays + 1)
                          .Select(i => calendario.FechaInicio.AddDays(i)).ToList(),
                Enumerable.Range(0, (int)((calendario.HoraFinTurnos - calendario.HoraInicioTurnos).TotalMinutes / calendario.IntervaloTurnos.TotalMinutes))
                          .Select(i => calendario.HoraInicioTurnos.Add(TimeSpan.FromMinutes(calendario.IntervaloTurnos.TotalMinutes * i))).ToList(),
                calendario);
            return Ok("Turnos generados.");
        }
    }
 
    
}
