using Agenda.Entidades.DTOs.DTO.TurnosDTO;

namespace Agenda.Interfaces
{
    public interface ITurnoService
    {
        Task<Turno> CrearTurnoAsync(CrearTurnosDTO turnoDto);
        Task<IEnumerable<Turno>> ObtenerTurnosAsync(int usuarioId);
        Task<Turno> ObtenerTurnoPorIdAsync(int id);
        Task<bool> EliminarTurnoAsync(int id);
    }
}
