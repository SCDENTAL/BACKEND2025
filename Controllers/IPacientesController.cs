using Agenda.Entidades.DTOs.DTO.PacientesDTO;
using Agenda.Interfaces;
using Agenda.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agenda.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PacientesController : ControllerBase
    {
        private readonly IPacienteService _service;

        public PacientesController(IPacienteService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            int usuarioId = GetUsuarioId();
            return Ok(await _service.ObtenerTodosAsync(usuarioId));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            int usuarioId = GetUsuarioId();
            var paciente = await _service.ObtenerPorIdAsync(id, usuarioId);
            return paciente is null ? NotFound() : Ok(paciente);
        }

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] CrearPacienteDTO dto)
		{
			try
			{
				int usuarioId = GetUsuarioId();
				var creado = await _service.CrearAsync(dto, usuarioId);
				return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { mensaje = "Error interno", detalle = ex.Message });
			}
		}


		[HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] EditarPacientesDTO dto)
        {
            int usuarioId = GetUsuarioId();
            var actualizado = await _service.EditarAsync(id, dto, usuarioId);
            return actualizado ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            int usuarioId = GetUsuarioId();
            var eliminado = await _service.EliminarAsync(id, usuarioId);
            return eliminado ? NoContent() : NotFound();
        }

        private int GetUsuarioId()
        {
            var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (claim == null) throw new UnauthorizedAccessException("Claim 'NameIdentifier' no encontrado.");
            return int.Parse(claim.Value);
        }
    }
}
