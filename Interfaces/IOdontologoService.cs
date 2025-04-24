using Agenda.Entidades.DTOs.DTO.OdontologosDTO;
using Agenda.Entidades;

namespace Agenda.Interfaces
{
    public interface IOdontologoService
    {
        Task<IEnumerable<VerOdontologoDTO>> ObtenerOdontologosAsync(int usuarioId);
        Task<OdontologosDTO> ObtenerPorIdAsync(int id, int usuarioId);
        Task<OdontologosDTO> CrearAsync(CrearOdontologoDTO dto, int usuarioId);
        Task<bool> EditarAsync(int id, EditarOdontologoDTO dto, int usuarioId);
        Task<bool> EliminarAsync(int id, int usuarioId);
    }
}
