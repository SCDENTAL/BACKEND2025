using Agenda.Base;
using Agenda.Entidades;
using Agenda.Entidades.DTOs;
using Agenda.Entidades.DTOs.DTO.TurnosDTO;
using Agenda.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Agenda.Servicios
{
	

	public class TurnoService : ITurnoService
	{
		private readonly AppDbContext _context;

		public TurnoService(AppDbContext context)
		{
			_context = context;
		}

		public async Task<List<TurnoDTO>> ObtenerTurnos(ClaimsPrincipal user)
		{
			var usuarioId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
			var rol = user.FindFirst(ClaimTypes.Role).Value;

			if (rol == "Administrador")
			{
				return await _context.Turnos
					.Where(t => t.UsuarioId == usuarioId)
					.Include(t => t.Odontologo)
					.Include(t => t.Paciente)
					.Select(t => new TurnoDTO
					{
						Id = t.Id,
						FechaHora = t.FechaHora,
						OdontologoNombre = t.Odontologo.Nombre,
						PacienteNombre = t.Paciente.Nombre
					})
					.ToListAsync();
			}
			else if (rol == "Odontologo")
			{
				var odontologo = await _context.Odontologos.FirstOrDefaultAsync(o => o.UsuarioId == usuarioId);
				if (odontologo == null) return new List<TurnoDTO>();

				return await _context.Turnos
					.Where(t => t.OdontologoId == odontologo.Id)
					.Include(t => t.Paciente)
					.Select(t => new TurnoDTO
					{
						Id = t.Id,
						FechaHora = t.FechaHora,
						OdontologoNombre = odontologo.Nombre,
						PacienteNombre = t.Paciente.Nombre
					})
					.ToListAsync();
			}

			return new List<TurnoDTO>();
		}

		public async Task<string> CrearTurno(CrearTurnosDTO dto, ClaimsPrincipal user)
		{
			var usuarioId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);

			// Validaciones
			var pacienteExiste = await _context.Pacientes.AnyAsync(p => p.Id == dto.PacienteId && p.UsuarioId == usuarioId);
			var odontologoExiste = await _context.Odontologos.AnyAsync(o => o.Id == dto.OdontologoId && o.UsuarioId == usuarioId);

			if (!pacienteExiste || !odontologoExiste)
				return "Paciente u odontólogo no válidos.";

			var solapado = await _context.Turnos.AnyAsync(t =>
				t.OdontologoId == dto.OdontologoId &&
				t.FechaHora == dto.FechaHora
			);

			if (solapado)
				return "Ya hay un turno asignado para esa fecha y hora.";

			var turno = new Turno
			{
				UsuarioId = usuarioId,
				OdontologoId = dto.OdontologoId,
				PacienteId = dto.PacienteId,
				FechaHora = dto.FechaHora,
				Estado = EstadoTurno.Pendiente
			};

			_context.Turnos.Add(turno);
			await _context.SaveChangesAsync();

			return "Turno creado correctamente.";
		}

		public async Task<string> ActualizarTurno(int id, CrearTurnosDTO dto, ClaimsPrincipal user)
		{
			var usuarioId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);

			var turno = await _context.Turnos.FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == usuarioId);
			if (turno == null) return "Turno no encontrado.";

			turno.FechaHora = dto.FechaHora;
			turno.PacienteId = dto.PacienteId;
			turno.OdontologoId = dto.OdontologoId;

			await _context.SaveChangesAsync();
			return "Turno actualizado correctamente.";
		}

		public async Task<string> EliminarTurno(int id, ClaimsPrincipal user)
		{
			var usuarioId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);

			var turno = await _context.Turnos.FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == usuarioId);
			if (turno == null) return "Turno no encontrado.";

			_context.Turnos.Remove(turno);
			await _context.SaveChangesAsync();

			return "Turno eliminado correctamente.";
		}

		public async Task<string> CambiarEstado(int id, EstadoTurno nuevoEstado, ClaimsPrincipal user)
		{
			var usuarioId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);

			var odontologo = await _context.Odontologos.FirstOrDefaultAsync(o => o.UsuarioId == usuarioId);
			if (odontologo == null) return "Odontólogo no válido.";

			var turno = await _context.Turnos.FirstOrDefaultAsync(t => t.Id == id && t.OdontologoId == odontologo.Id);
			if (turno == null) return "Turno no encontrado o no asignado.";

			turno.Estado = nuevoEstado;
			await _context.SaveChangesAsync();

			return "Estado del turno actualizado.";
		}
	}
}
