using Agenda.Base;
using Agenda.Entidades;
using Agenda.Interfaces;
using Microsoft.EntityFrameworkCore;
using Agenda.Entidades.DTOs.DTO.TurnosDTO;

namespace Agenda.Servicios
{
    public class TurnoService : ITurnoService
    {
        private readonly AppDbContext _context;

        public TurnoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Turno> CrearTurnoAsync(CrearTurnosDTO dto)
        {            
            var paciente = await _context.Pacientes
                .FirstOrDefaultAsync(p => p.Dni == dto.DniPaciente && p.UsuarioId == dto.UsuarioId);

            if (paciente == null)
            {
                paciente = new Paciente
                {
                    Nombre = dto.NombrePaciente,
                    Dni = dto.DniPaciente,
                    UsuarioId = dto.UsuarioId,
                    ObraSocial = await _context.ObrasSociales
                        .FirstOrDefaultAsync(o => o.Nombre == dto.ObraSocial && o.UsuarioId == dto.UsuarioId)
                };

                _context.Pacientes.Add(paciente);
                await _context.SaveChangesAsync();
            }

            var turno = new Turno
            {
                FechaHora = dto.FechaHora,
                Estado = "Pendiente",
                PacienteId = paciente.Id,
                OdontologoId = dto.OdontologoId,
                UsuarioId = dto.UsuarioId
            };

            _context.Turnos.Add(turno);
            await _context.SaveChangesAsync();

            return turno;
        }

        public async Task<IEnumerable<Turno>> ObtenerTurnosAsync(int usuarioId)
        {
            return await _context.Turnos
                .Include(t => t.Paciente)
                .Include(t => t.Odontologo)
                .Where(t => t.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<Turno> ObtenerTurnoPorIdAsync(int id)
        {
            return await _context.Turnos
                .Include(t => t.Paciente)
                .Include(t => t.Odontologo)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<bool> EliminarTurnoAsync(int id)
        {
            var turno = await _context.Turnos.FindAsync(id);
            if (turno == null) return false;

            _context.Turnos.Remove(turno);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
