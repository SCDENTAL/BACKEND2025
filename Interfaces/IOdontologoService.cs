using Agenda.Entidades;
using Agenda.Entidades.DTOs;
using Agenda.Entidades.DTOs.DTO.OdontologosDTO;

namespace Agenda.Interfaces
{
    public interface IOdontologoService
    {
        Task<IEnumerable<VerOdontologoDTO>> ObtenerOdontologosAsync(int usuarioIdAdmin);
        Task<OdontologosDTO> ObtenerPorIdAsync(int id, int usuarioId);
        Task<int> CrearAync(RegistroDto dto, int usuarioIdAdmin);
        Task<bool> EditarAsync(int id, EditarOdontologoDTO dto, int usuarioId);
        Task<bool> EliminarAsync(int id, int usuarioId);
    }
}
