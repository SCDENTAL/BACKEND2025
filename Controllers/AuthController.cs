using Agenda.Entidades.DTOs;
using Agenda.Interfaces;
using Agenda.Servicios;
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

		[HttpGet("ping")]
		public IActionResult Ping()
		{
			return Ok("pong");
		}
	}
}
