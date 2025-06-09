using Agenda.Entidades;
using Agenda.Entidades.DTOs.DTO.TurnosDTO;
using System.Security.Claims;

namespace Agenda.Interfaces
{
    public interface ITurnoService
    {

        Task<List<TurnoDTO>> ObtenerTurnosAsync(int calendarioId, int usuarioId);
        Task<ResTurnosFiltrados> FiltrarPorFechasAsync(int calendarioId, DateTime fechaInicio, DateTime fechaFin, int usuarioId);
        Task<bool> ReservarTurnoAsync(int turnoId, ReservarTurnoDTO dto, int usuarioId);
        Task<bool> CancelarTurnoAsync(int turnoId, int usuarioId);
        Task CrearTurnosAsync(List<DateTime> fechas, List<TimeSpan> horarios, Calendario calendario);
        Task<Calendario?> ObtenerCalendarioEntidadAsync(int calendarioId, int usuarioId);
        Task<bool> MarcarAsistenciaAsync(int turnoId, bool asistio, int usuarioId);

    }
}
