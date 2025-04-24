using Agenda.Entidades.DTOs.DTO.TurnosDTO;
using Agenda.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TurnoController : ControllerBase
    {
        private readonly ITurnoService _turnoService;

        public TurnoController(ITurnoService turnoService)
        {
            _turnoService = turnoService;
        }

        [HttpPost]
        public async Task<IActionResult> CrearTurno([FromBody] CrearTurnosDTO dto)
        {
            var turno = await _turnoService.CrearTurnoAsync(dto);
            return Ok(turno);
        }

        [HttpGet("{usuarioId}")]
        public async Task<IActionResult> ObtenerTurnos(int usuarioId)
        {
            var turnos = await _turnoService.ObtenerTurnosAsync(usuarioId);
            return Ok(turnos);
        }
    }

}
