using Agenda.Base;
using Agenda.Entidades;
using Agenda.Entidades.DTOs.DTO.PacientesDTO;
using Agenda.Exceptions;
using Agenda.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Servicios
{

	public class PacienteService : IPacienteService
	{
		private readonly AppDbContext _context;

		public PacienteService(AppDbContext context) { 
			_context = context; 
		
		}

		public async Task<List<PacienteDTO>> ObtenerTodosAsync(int usuarioId) =>
			await _context.Pacientes
				.Include(p => p.ObraSocial)
				.Where(p => p.UsuarioId == usuarioId)
				.Select(p => new PacienteDTO
				{
					Id = p.Id,
					Nombre = p.Nombre,
					Apellido = p.Apellido,
					Dni = p.Dni,
					Telefono = p.Telefono,
					Email = p.Email,
					ObraSocial = p.ObraSocial != null ? p.ObraSocial.Nombre : "Sin obra social"
				}).ToListAsync();

		public async Task<PacienteDTO?> ObtenerPorIdAsync(int id, int usuarioId)
		{
			var paciente = await _context.Pacientes
				.Include(p => p.ObraSocial)
				.FirstOrDefaultAsync(p => p.Id == id && p.UsuarioId == usuarioId);
			
            if (paciente == null)
                throw new NotFoundException("Paciente no encontrado.");

            return new PacienteDTO
			{
				Id = paciente.Id,
				Nombre = paciente.Nombre,
				Dni = paciente.Dni,
				Telefono = paciente.Telefono,
				Email = paciente.Email,
				ObraSocial = paciente.ObraSocial != null ? paciente.ObraSocial.Nombre : "Sin obra social"
			};
		}

		public async Task<PacienteDTO> CrearAsync(CrearPacienteDTO dto, int usuarioId)
		{
			var paciente = new Paciente
			{
				Nombre = dto.Nombre,
				Apellido = dto.Apellido,
				Dni = dto.Dni,
				Telefono = dto.Telefono,
				Email = dto.Email,
				ObraSocialId = dto.ObraSocialId,
				UsuarioId = usuarioId
			};

			_context.Pacientes.Add(paciente);
			await _context.SaveChangesAsync();

			return new PacienteDTO
			{
				Id = paciente.Id,
				Nombre = paciente.Nombre,
				Apellido = paciente.Apellido,
				Dni = dto.Dni,
				Telefono = dto.Telefono,
				Email = dto.Email,
				ObraSocial = (await _context.ObrasSociales.FindAsync(dto.ObraSocialId))?.Nombre ?? "Sin obra social"
			};
		}

        public async Task<bool> EditarAsync(int id, EditarPacientesDTO dto, int usuarioId)
        {
            var paciente = await _context.Pacientes
                .FirstOrDefaultAsync(p => p.Id == id && p.UsuarioId == usuarioId);
            
			if (paciente == null)
				throw new BadRequestException("El paciente no existe");
			

            paciente.Nombre = dto.Nombre;
            paciente.Apellido = dto.Apellido;
            paciente.Dni = dto.Dni;
            paciente.Telefono = dto.Telefono;
            paciente.Email = dto.Email;
            
            var obraSocialExiste = await _context.ObrasSociales
                .AnyAsync(o => o.Id == dto.ObraSocialId && o.UsuarioId == usuarioId);

            paciente.ObraSocialId = obraSocialExiste ? dto.ObraSocialId : null;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarAsync(int id, int usuarioId)
		{
			var paciente = await _context.Pacientes.FirstOrDefaultAsync(p => p.Id == id && p.UsuarioId == usuarioId);
			if (paciente == null) return false;

			_context.Pacientes.Remove(paciente);
			await _context.SaveChangesAsync();
			return true;
		}

	}

}