using Agenda.Entidades;
using Agenda.Entidades.DTOs.DTO.TurnosDTO;
using System.Security.Claims;

namespace Agenda.Interfaces
{
    public interface ITurnoService
    {		
		Task<List<TurnoDTO>> ObtenerTurnos(ClaimsPrincipal user);
		Task<string> CrearTurno(CrearTurnosDTO dto, ClaimsPrincipal user);
		Task<string> ActualizarTurno(int id, CrearTurnosDTO dto, ClaimsPrincipal user);
		Task<string> EliminarTurno(int id, ClaimsPrincipal user);
		Task<string> CambiarEstado(int id, EstadoTurno nuevoEstado, ClaimsPrincipal user);
	}
}
