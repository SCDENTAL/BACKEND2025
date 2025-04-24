using Agenda.Entidades.DTOs.DTO.ObrasSocialesDTO;

namespace Agenda.Interfaces
{
    public interface IObrasSocialesService
    {
        Task<List<ObraSocialDTO>> ObtenerTodasAsync(int usuarioId);
        Task<ObraSocialDTO> CrearAsync(CrearObraSocialDTO dto, int usuarioId);
        Task<bool> EliminarAsync(int id, int usuarioId);
    }
}
