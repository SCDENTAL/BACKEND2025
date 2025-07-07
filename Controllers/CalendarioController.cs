using Agenda.Entidades.DTOs.DTO.CalendarioDTO;
using Agenda.Interfaces;
using Agenda.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Agenda.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador")]
    public class CalendarioController : ControllerBase
    {
        private readonly ICalendarioService _service;

        public CalendarioController(ICalendarioService service) => _service = service;
        
        private int ObtenerUsuarioId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        [HttpGet]
        public async Task<ActionResult<List<CalendarioDTO>>> ObtenerTodos()
        {
            return Ok(await _service.ObtenerTodosAsync(ObtenerUsuarioId()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CalendarioDTO>> ObtenerPorId(int id)
        {
            var calendario = await _service.ObtenerPorIdAsync(id, ObtenerUsuarioId());
            if (calendario == null) return NotFound();
            return Ok(calendario);
        }
      

        [HttpPost]
        public async Task<ActionResult<CalendarioDTO>> Crear([FromBody] CrearCalendarioDTO dto)
        {
            try
            {
                var creado = await _service.CrearAsync(dto, ObtenerUsuarioId());
                return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Id }, creado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message }); 
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error interno del servidor", detalle = ex.Message });
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Editar(int id, [FromBody] CrearCalendarioDTO dto)
        {
            var actualizado = await _service.EditarAsync(id, dto, ObtenerUsuarioId());
            if (!actualizado) return NotFound();
            return NoContent();
        }

        [HttpPut("{id}/extender")]
        public async Task<IActionResult> Extender(int id, [FromBody] ExtenderCalendarioDTO dto)
        {
            var extendido = await _service.ExtenderCalendarioAsync(id, dto, ObtenerUsuarioId());
            if (!extendido) return BadRequest("No se pudo extender el calendario.");
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar()
        {
            var usuarioId = ObtenerUsuarioId(); 
            var resultado = await _service.EliminarAsync(usuarioId);

            if (!resultado)
                return NotFound("No se encontró un calendario para este usuario.");

            return NoContent();
        }
    }
}