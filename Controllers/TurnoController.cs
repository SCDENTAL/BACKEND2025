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


        [Authorize(Roles = "Odontologo")]
        [HttpGet("mis-turnos-hoy")]
        public async Task<IActionResult> ObtenerTurnosDelDia()
        {
            var usuarioId = ObtenerUsuarioId();
            var turnos = await _turnoService.ObtenerTurnosDelDiaAsync(usuarioId);
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
            var resultado = await _turnoService.ReservarTurnoAsync(turnoId, dto, ObtenerUsuarioId());
            if (resultado.Success)
                return Ok(new { message = "Turno reservado." });
            else
                return BadRequest(new { message = resultado.ErrorMessage });
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



        [Authorize(Roles = "Odontologo")]
        [HttpGet("mis-turnos")]
        public async Task<IActionResult> ObtenerMisTurnos()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId))
                return Unauthorized();

            try
            {
                var turnos = await _turnoService.ObtenerTurnosDelOdontologoAsync(userId);
                return Ok(turnos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize]
        [HttpPut("{id}/asistencia")]
        public async Task<IActionResult> MarcarAsistencia(int id, [FromBody] MarcarAsistenciaDTO dto)
        {
            var usuarioId = ObtenerUsuarioId();
            var rol = User.FindFirstValue(ClaimTypes.Role);  

            var result = await _turnoService.MarcarAsistenciaAsync(id, dto.Asistio, usuarioId, rol);

            if (!result)
                return BadRequest("No se pudo marcar la asistencia.");

            return Ok(new { mensaje = "Asistencia registrada correctamente" });


        }

        [Authorize]
        [HttpPut("{id}/cancelar")]
        public async Task<IActionResult> CancelarTurno(int id)
        {
            var usuarioId = ObtenerUsuarioId(); 

            var result = await _turnoService.CancelarTurnoAsync(id, usuarioId);
            if (!result) return BadRequest("No se pudo cancelar el turno.");

            return Ok(new { mensaje = "Turno cancelado correctamente." });
        }

        [HttpGet("por-semana/{calendarioId}")]
        public async Task<IActionResult> ObtenerTurnosPorSemana(int calendarioId, [FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            var turnos = await _context.Turnos
                .Where(t => t.IdCalendario == calendarioId && t.Fecha >= fechaInicio && t.Fecha <= fechaFin)
                .ToListAsync();

            return Ok(turnos);
        }

        [HttpPut("editar/{turnoId}")]
        public async Task<IActionResult> EditarTurno(int turnoId, [FromBody] EditarTurnosDTO dto)
        {
            var resultado = await _turnoService.EditarTurnoAsync(turnoId, dto, ObtenerUsuarioId());

            if (!resultado.Success)
                return BadRequest(resultado.ErrorMessage);

            return Ok(new { mensaje = "Turno editado correctamente." });

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
