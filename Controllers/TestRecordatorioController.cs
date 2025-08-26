using Agenda.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Agenda.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestRecordatorioController : ControllerBase
    {
        private readonly RecordatorioTurnoService _recordatorio;

        public TestRecordatorioController(RecordatorioTurnoService recordatorio)
        {
            _recordatorio = recordatorio;
        }

        [HttpGet]
        public async Task<IActionResult> Ejecutar()
        {
            await _recordatorio.EnviarRecordatoriosAsync();
            return Ok("Mensaje de prueba enviado");
        }
    }
}
