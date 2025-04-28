using Agenda.Entidades.DTOs.DTO.ObrasSocialesDTO;
using Agenda.Interfaces;
using Agenda.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Agenda.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObrasSocialesController : ControllerBase
    {
        private readonly IObrasSocialesService _service;

        public ObrasSocialesController(IObrasSocialesService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            int usuarioId = GetUsuarioId();
            return Ok(await _service.ObtenerTodasAsync(usuarioId));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CrearObraSocialDTO dto)
        {
            int usuarioId = GetUsuarioId();
            var creada = await _service.CrearAsync(dto, usuarioId);
            return CreatedAtAction(nameof(Get), new { id = creada.Id }, creada);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            int usuarioId = GetUsuarioId();
            var obra = await _service.ObtenerPorIdAsync(id, usuarioId);
            return obra == null ? NotFound() : Ok(obra);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] EditarObraSocialDTO dto)
        {
            int usuarioId = GetUsuarioId();
            var editada = await _service.EditarAsync(id, dto, usuarioId);
            return editada ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            int usuarioId = GetUsuarioId();
            var eliminada = await _service.EliminarAsync(id, usuarioId);
            return eliminada ? NoContent() : NotFound();
        }

        private int GetUsuarioId()
        {
            var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (claim == null) throw new UnauthorizedAccessException("Claim 'NameIdentifier' no encontrado.");
            return int.Parse(claim.Value);
        }
    }
}

