using Agenda.Base;
using Agenda.Entidades;
using Agenda.Entidades.DTOs.DTO.TurnosDTO;
using Agenda.Interfaces;
using Microsoft.EntityFrameworkCore;

public class TurnoService : ITurnoService
{
    private readonly AppDbContext _context;

    public TurnoService(AppDbContext context) => _context = context;

    public async Task<Calendario?> ObtenerCalendarioEntidadAsync(int calendarioId, int usuarioId)
    {
        return await _context.Calendarios
            .FirstOrDefaultAsync(c => c.Id == calendarioId && c.IdUsuario == usuarioId);
    }

    public async Task<List<TurnoDTO>> ObtenerTurnosAsync(int calendarioId, int usuarioId)
    {
        var calendario = await ObtenerCalendarioEntidadAsync(calendarioId, usuarioId);
        if (calendario == null) return new List<TurnoDTO>();

        var turnos = await _context.Turnos
            .Include(t => t.Paciente)
            .Include(t => t.Odontologo)
            .Include(t => t.ObraSocial)
            .Where(t => t.IdCalendario == calendarioId)
            .ToListAsync();

        return turnos.Select(t => new TurnoDTO
        {
            Id = t.Id,
            Fecha = t.Fecha,
            Horario = t.Horario,
            Disponible = t.Disponible,
            Asistio = t.Asistio,
            IdPaciente = t.IdPaciente,
            NombrePaciente = t.Paciente?.Nombre,
            OdontologoId = t.OdontologoId,
            NombreOdontologo = t.Odontologo?.Nombre,
            IdObraSocial = t.ObraSocialId,
            NombreObraSocial = t.ObraSocial?.Nombre
        }).ToList();
    }

    public async Task<ResTurnosFiltrados> FiltrarPorFechasAsync(int calendarioId, DateTime fechaInicio, DateTime fechaFin, int usuarioId)
    {
        var turnos = await _context.Turnos
            .Include(t => t.Paciente)
            .Include(t => t.Odontologo)
            .Include(t => t.ObraSocial)
            .Where(t => t.IdCalendario == calendarioId && t.Fecha.Date >= fechaInicio.Date && t.Fecha.Date <= fechaFin.Date)
            .ToListAsync();

        return new ResTurnosFiltrados
        {
            Lunes = MapDia(turnos, DayOfWeek.Monday),
            Martes = MapDia(turnos, DayOfWeek.Tuesday),
            Miercoles = MapDia(turnos, DayOfWeek.Wednesday),
            Jueves = MapDia(turnos, DayOfWeek.Thursday),
            Viernes = MapDia(turnos, DayOfWeek.Friday),
            Sabado = MapDia(turnos, DayOfWeek.Saturday),
            Domingo = MapDia(turnos, DayOfWeek.Sunday),
            CantidadHorarios = (await ObtenerCalendarioEntidadAsync(calendarioId, usuarioId))?.CantidadHorarios ?? 0
        };
    }

    public async Task<bool> ReservarTurnoAsync(int turnoId, ReservarTurnoDTO dto, int usuarioId)
    {
        var turno = await _context.Turnos.FirstOrDefaultAsync(t => t.Id == turnoId && t.UsuarioId == usuarioId);
        if (turno == null || !turno.Disponible) return false;

        if (!await _context.Pacientes.AnyAsync(p => p.Id == dto.IdPaciente && p.UsuarioId == usuarioId))
            return false;

        turno.Disponible = false;
        turno.IdPaciente = dto.IdPaciente;
        turno.OdontologoId = dto.IdOdontologo;
        turno.ObraSocialId = dto.IdObraSocial;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CancelarTurnoAsync(int turnoId, int usuarioId)
    {
        var turno = await _context.Turnos.FirstOrDefaultAsync(t => t.Id == turnoId && t.UsuarioId == usuarioId);
        if (turno == null || turno.Disponible) return false;

        turno.Disponible = true;
        turno.Asistio = null;
        turno.IdPaciente = null;
        turno.OdontologoId = null;
        turno.ObraSocialId = null;

        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<bool> MarcarAsistenciaAsync(int turnoId, bool asistio, int usuarioId)
    {
        var turno = await _context.Turnos.FirstOrDefaultAsync(t => t.Id == turnoId && t.UsuarioId == usuarioId);
        if (turno == null || turno.Disponible) 
            
            return false; 

        turno.Asistio = asistio;
        await _context.SaveChangesAsync();
        return true;
    }


    public async Task CrearTurnosAsync(List<DateTime> fechas, List<TimeSpan> horarios, Calendario calendario)
    {
        foreach (var fecha in fechas)
            foreach (var horario in horarios)
                _context.Turnos.Add(new Turno
                {
                    Fecha = fecha,
                    Horario = horario,
                    Disponible = true,
                    IdCalendario = calendario.Id,
                    UsuarioId = calendario.IdUsuario
                });

        await _context.SaveChangesAsync();
    }

    private List<TurnoDTO> MapDia(List<Turno> turnos, DayOfWeek dia) =>
        turnos.Where(t => t.Fecha.DayOfWeek == dia)
              .Select(t => new TurnoDTO
              {
                  Id = t.Id,
                  Fecha = t.Fecha,
                  Horario = t.Horario,
                  Disponible = t.Disponible,
                  Asistio = t.Asistio,
                  IdPaciente = t.IdPaciente,
                  NombrePaciente = t.Paciente?.Nombre,
                  OdontologoId = t.OdontologoId,
                  NombreOdontologo = t.Odontologo?.Nombre,
                  IdObraSocial = t.ObraSocialId,
                  NombreObraSocial = t.ObraSocial?.Nombre
              })
              .ToList();
}


