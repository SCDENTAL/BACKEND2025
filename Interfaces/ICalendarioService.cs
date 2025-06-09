using Agenda.Entidades;
using Agenda.Entidades.DTOs.DTO.CalendarioDTO;

namespace Agenda.Interfaces
{
    public interface ICalendarioService
    {
        Task<List<CalendarioDTO>> ObtenerTodosAsync(int usuarioId);
        Task<CalendarioDTO?> ObtenerPorIdAsync(int id, int usuarioId);
        Task<CalendarioDTO> CrearAsync(CrearCalendarioDTO dto, int usuarioId);
        Task<bool> EditarAsync(int id, CrearCalendarioDTO dto, int usuarioId);
        Task<bool> EliminarAsync(int usuarioId);
        Task<bool> ExtenderCalendarioAsync(int id, ExtenderCalendarioDTO dto, int usuarioId);
    }
}
