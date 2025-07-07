using Agenda.Entidades.DTOs;
using Agenda.Interfaces;
using Agenda.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Agenda.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("registro")]
        public async Task<IActionResult> Registro(RegistroDto dto)
        {
            var result = await _authService.Registrar(dto);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.Login(dto);
            if (!result.Success)
                return Unauthorized(result.Message);

            return Ok(result.Data);
        }



        [Authorize(Roles = "Administrador")]
        [HttpPost("crear-odontologo")]
        public async Task<IActionResult> CrearOdontologo([FromBody] RegistroDto dto)
        {
            // ✅ Obtener el ID del admin logueado
            int usuarioIdAdmin = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");

            var resultado = await _authService.CrearOdontologoDesdeAdmin(dto, usuarioIdAdmin);
            if (!resultado.Success)
                return BadRequest(new { mensaje = resultado.Message });

            return Ok(new { mensaje = resultado.Message });
        }


    }
}
