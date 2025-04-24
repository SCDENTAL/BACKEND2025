using Agenda.Entidades.DTOs.DTO.PacientesDTO;

namespace Agenda.Interfaces
{
    public interface IPacienteService
    {
        Task<List<PacienteDTO>> ObtenerTodosAsync(int usuarioId);
        Task<PacienteDTO> CrearAsync(CrearPacienteDTO dto, int usuarioId);
        Task<bool> EliminarAsync(int id, int usuarioId);
    }
}

