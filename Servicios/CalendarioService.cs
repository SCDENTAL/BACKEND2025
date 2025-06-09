using Agenda.Base;
using Agenda.Entidades;
using Agenda.Entidades.DTOs.DTO.CalendarioDTO;
using Agenda.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Servicios
{
    public class CalendarioService : ICalendarioService
    {
        private readonly AppDbContext _context;

        public CalendarioService(AppDbContext context) => _context = context;

        public async Task<CalendarioDTO?> ObtenerPorIdAsync(int id, int usuarioId)
        {
            return await _context.Calendarios
                .Where(c => c.Id == id && c.IdUsuario == usuarioId)
                .Select(c => new CalendarioDTO
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    FechaInicio = c.FechaInicio,
                    FechaFin = c.FechaFin,
                    HoraInicioTurnos = c.HoraInicioTurnos,
                    HoraFinTurnos = c.HoraFinTurnos,
                    IntervaloTurnos = c.IntervaloTurnos
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<CalendarioDTO>> ObtenerTodosAsync(int usuarioId)
        {
            return await _context.Calendarios
                .Where(c => c.IdUsuario == usuarioId)
                .Select(c => new CalendarioDTO
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    FechaInicio = c.FechaInicio,
                    FechaFin = c.FechaFin,
                    HoraInicioTurnos = c.HoraInicioTurnos,
                    HoraFinTurnos = c.HoraFinTurnos,
                    IntervaloTurnos = c.IntervaloTurnos
                })
                .ToListAsync();
        }


        public async Task<CalendarioDTO> CrearAsync(CrearCalendarioDTO dto, int usuarioId)
        {
            // Validaciones
            if (dto.HoraInicioTurnos < 0)
                throw new ArgumentException("La hora de inicio no puede ser negativa.");

            if (dto.HoraFinTurnos <= dto.HoraInicioTurnos)
                throw new ArgumentException("La hora de fin debe ser mayor a la hora de inicio.");

            if (dto.HoraFinTurnos > 24)
                throw new ArgumentException("La hora de fin no puede superar las 24hs.");

            if (dto.IntervaloTurnos < 10)
                throw new ArgumentException("El intervalo mínimo debe ser de 10 minutos.");

            if (dto.FechaFin <= dto.FechaInicio)
                throw new ArgumentException("La fecha de fin debe ser posterior a la fecha de inicio.");

            if (dto.HoraInicioTurnos < 0 || dto.HoraInicioTurnos > 23)
                throw new ArgumentException("La hora de inicio debe estar entre 0 y 23.");

            if (dto.HoraFinTurnos < 1 || dto.HoraFinTurnos > 24)
                throw new ArgumentException("La hora de fin debe estar entre 1 y 23.");

            var yaExiste = await _context.Calendarios.AnyAsync(c => c.IdUsuario == usuarioId);
            if (yaExiste)
                throw new InvalidOperationException("El usuario ya tiene un calendario.");

            // Crear el calendario
            var calendario = new Calendario
            {
                Nombre = dto.Nombre,
                FechaInicio = dto.FechaInicio.Date,
                FechaFin = dto.FechaFin.Date,
                HoraInicioTurnos = TimeSpan.FromHours(dto.HoraInicioTurnos),
                HoraFinTurnos = TimeSpan.FromHours(dto.HoraFinTurnos),
                IntervaloTurnos = TimeSpan.FromMinutes(dto.IntervaloTurnos),
                IdUsuario = usuarioId
            };

            _context.Calendarios.Add(calendario);
            await _context.SaveChangesAsync();

            // Cargar fechas
            var fechas = new List<DateTime>();
            var fechaActual = calendario.FechaInicio;

            while (fechaActual <= calendario.FechaFin)
            {
                if (!dto.ExcluirDomingo || fechaActual.DayOfWeek != DayOfWeek.Sunday)
                    fechas.Add(fechaActual);

                fechaActual = fechaActual.AddDays(1);
            }

            // Cargar horarios
            var horarios = new List<TimeSpan>();
            var horaActual = calendario.HoraInicioTurnos;
            while (horaActual < calendario.HoraFinTurnos)
            {
                horarios.Add(horaActual);
                horaActual += calendario.IntervaloTurnos;
            }

            calendario.CantidadHorarios = horarios.Count;

            // Crear los turnos (puede ir en un método aparte si preferís separar lógica)
            foreach (var fecha in fechas)
            {
                foreach (var hora in horarios)
                {
                    var turno = new Turno
                    {
                        Fecha = fecha,
                        Horario = hora,
                        Disponible = true,
                        IdCalendario = calendario.Id,
                        UsuarioId = usuarioId
                        
                    };

                    _context.Turnos.Add(turno);
                }
            }

            await _context.SaveChangesAsync();

            // Devolver el DTO manualmente
            return new CalendarioDTO
            {
                Id = calendario.Id,
                Nombre = calendario.Nombre,
                FechaInicio = calendario.FechaInicio,
                FechaFin = calendario.FechaFin,
                HoraInicioTurnos = calendario.HoraInicioTurnos,
                HoraFinTurnos = calendario.HoraFinTurnos,
                IntervaloTurnos = calendario.IntervaloTurnos
            };
        }


        public async Task<bool> EditarAsync(int id, CrearCalendarioDTO dto, int usuarioId)
        {
            var calendario = await _context.Calendarios.FirstOrDefaultAsync(c => c.Id == id && c.IdUsuario == usuarioId);
            if (calendario == null) return false;

            calendario.Nombre = dto.Nombre;
            calendario.FechaInicio = dto.FechaInicio;
            calendario.FechaFin = dto.FechaFin;
            calendario.HoraInicioTurnos = TimeSpan.FromHours(dto.HoraInicioTurnos);
            calendario.HoraFinTurnos = TimeSpan.FromHours(dto.HoraFinTurnos);
            calendario.IntervaloTurnos = TimeSpan.FromMinutes(dto.IntervaloTurnos);
            calendario.CantidadHorarios = CalcularCantidadHorarios(TimeSpan.FromHours(dto.HoraInicioTurnos), TimeSpan.FromHours(dto.HoraFinTurnos), TimeSpan.FromMinutes(dto.IntervaloTurnos));

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarAsync(int usuarioId)
        {
            var calendario = await _context.Calendarios
                .Include(c => c.Turnos)
                .FirstOrDefaultAsync(c => c.IdUsuario == usuarioId);

            if (calendario == null)
                return false;

            _context.Turnos.RemoveRange(calendario.Turnos);
            _context.Calendarios.Remove(calendario);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExtenderCalendarioAsync(int id, ExtenderCalendarioDTO dto, int usuarioId)
        {
            var calendario = await _context.Calendarios.FirstOrDefaultAsync(c => c.Id == id && c.IdUsuario == usuarioId);
            if (calendario == null) return false;

            if (dto.FechaFin <= calendario.FechaFin) return false;

            calendario.FechaFin = dto.FechaFin;
            await _context.SaveChangesAsync();
            return true;
        }

        private int CalcularCantidadHorarios(TimeSpan horaInicio, TimeSpan horaFin, TimeSpan intervalo)
        {
            return (int)((horaFin - horaInicio).TotalMinutes / intervalo.TotalMinutes);
        }

       
    }
}