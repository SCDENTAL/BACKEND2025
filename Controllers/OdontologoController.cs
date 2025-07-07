using Agenda.Entidades.DTOs;
using Agenda.Entidades.DTOs.DTO.OdontologosDTO;
using Agenda.Interfaces;
using Agenda.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Agenda.Controllers
{
    [ApiController]
    [Route("api/odontologos")]
    [Authorize]
    public class OdontologosController : ControllerBase
    {
        private readonly IOdontologoService _service;

        public OdontologosController(IOdontologoService service)
        {
            _service = service;
        }

        private int ObtenerUsuarioId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");


        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var usuarioId = ObtenerUsuarioId();
            return Ok(await _service.ObtenerOdontologosAsync(usuarioId));
        }


        [Authorize(Roles = "Administrador")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var usuarioId = ObtenerUsuarioId();
            var odontologo = await _service.ObtenerPorIdAsync(id, usuarioId);
            return odontologo == null ? NotFound() : Ok(odontologo);
        }


        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegistroDto dto)
        {
            try
            {
                var usuarioId = ObtenerUsuarioId();
                var creado = await _service.CrearAync(dto, usuarioId);
                return CreatedAtAction(nameof(Get), new { id = creado }, creado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }


        [Authorize(Roles = "Administrador")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] EditarOdontologoDTO dto)
        {
            var usuarioId = ObtenerUsuarioId();
            var actualizado = await _service.EditarAsync(id, dto, usuarioId);
            return actualizado ? NoContent() : NotFound();
        }

        [Authorize(Roles = "Administrador")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuarioId = ObtenerUsuarioId();
            var eliminado = await _service.EliminarAsync(id, usuarioId);
            return eliminado ? NoContent() : NotFound();
        }
    }
}
