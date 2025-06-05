using Agenda.Entidades;
using Agenda.Entidades.DTOs;
using Agenda.Entidades.DTOs.DTO.TurnosDTO;
using Agenda.Interfaces;
using Agenda.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agenda.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class TurnoController : ControllerBase
	{
		private readonly ITurnoService _turnoService;

		public TurnoController(ITurnoService turnoService)
		{
			_turnoService = turnoService;
		}

		[HttpGet]
		public async Task<IActionResult> GetTurnos()
		{
			var turnos = await _turnoService.ObtenerTurnos(User);
			return Ok(turnos);
		}

		[Authorize(Roles = "Administrador")]
		[HttpPost]
		public async Task<IActionResult> Crear([FromBody] CrearTurnosDTO dto)
		{
			var result = await _turnoService.CrearTurno(dto, User);
			return Ok(result);
		}

		[Authorize(Roles = "Administrador")]
		[HttpPut("{id}")]
		public async Task<IActionResult> Actualizar(int id, [FromBody] CrearTurnosDTO dto)
		{
			var result = await _turnoService.ActualizarTurno(id, dto, User);
			return Ok(result);
		}

		[Authorize(Roles = "Administrador")]
		[HttpDelete("{id}")]
		public async Task<IActionResult> Eliminar(int id)
		{
			var result = await _turnoService.EliminarTurno(id, User);
			return Ok(result);
		}

		[Authorize(Roles = "Odontologo")]
		[HttpPut("{id}/estado")]
		public async Task<IActionResult> CambiarEstado(int id, [FromBody] EstadoTurno estado)
		{
			var result = await _turnoService.CambiarEstado(id, estado, User);
			return Ok(result);
		}
	}
}
