using Agenda.Base;
using Agenda.Entidades.DTOs.DTO.PacientesDTO;
using Agenda.Entidades;
using Agenda.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Servicios
{
   
        public class PacienteService : IPacienteService
        {
            private readonly AppDbContext _context;
            public PacienteService(AppDbContext context) => _context = context;

            public async Task<List<PacienteDTO>> ObtenerTodosAsync(int usuarioId) =>
                await _context.Pacientes
                    .Include(p => p.ObraSocial)
                    .Where(p => p.UsuarioId == usuarioId)
                    .Select(p => new PacienteDTO
                    {
                        Id = p.Id,
                        Nombre = p.Nombre,
                        Email = p.Email,
                        ObraSocial = p.ObraSocial.Nombre
                    })
                    .ToListAsync();

            public async Task<PacienteDTO> CrearAsync(CrearPacienteDTO dto, int usuarioId)
            {
                var paciente = new Paciente
                {
                    Nombre = dto.Nombre,
                    Email = dto.Email,
                    ObraSocialId = dto.ObraSocial,
                    UsuarioId = usuarioId
                };

                _context.Pacientes.Add(paciente);
                await _context.SaveChangesAsync();

                var obraSocial = await _context.ObrasSociales.FindAsync(dto.ObraSocialId);

                return new PacienteDTO
                {
                    Id = paciente.Id,
                    Nombre = paciente.Nombre,
                    Email = paciente.Email,
                    ObraSocial = obraSocial?.Nombre
                };
            }

            public async Task<bool> EliminarAsync(int id, int usuarioId)
            {
                var paciente = await _context.Pacientes.FirstOrDefaultAsync(x => x.Id == id && x.UsuarioId == usuarioId);
                if (paciente == null) return false;
                _context.Pacientes.Remove(paciente);
                await _context.SaveChangesAsync();
                return true;
            }
        }

    }

