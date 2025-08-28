using Agenda.Base;
using Agenda.Entidades;
using Agenda.Entidades.DTOs.DTO.TurnosDTO;
using Agenda.Exceptions;
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

    public async Task<IEnumerable<TurnoOdontologoDTO>> ObtenerTurnosDelDiaAsync(int usuarioIdOdontologo)
    {
        var hoy = DateTime.Today;

        var odontologo = await _context.Odontologos
            .FirstOrDefaultAsync(o => o.UsuarioId == usuarioIdOdontologo);

        if (odontologo == null)
            throw new Exception("Odontólogo no encontrado.");

        return await _context.Turnos
            .Include(t => t.Paciente)
            .Include(t => t.ObraSocial)
            .Where(t => t.OdontologoId == odontologo.Id && t.Fecha == hoy && !t.Disponible)
            .Select(t => new TurnoOdontologoDTO
            {
                Id = t.Id,
                Fecha = t.Fecha,
                Horario = t.Horario,
                NombrePaciente = t.Paciente != null ? t.Paciente.Nombre : "No asignado",
                NumeroPaciente = t.Paciente != null ? t.Paciente.Telefono : "0",
                NombreObraSocial = t.ObraSocial != null ? t.ObraSocial.Nombre : "No asignada",
                Asistio = t.Asistio ?? false // ✅ Asegurate de incluir esto
            })
            .ToListAsync();
    }
    public async Task<List<TurnoDTO>> ObtenerTurnosDelOdontologoAsync(int usuarioIdOdontologo)
    {        
        var odontologo = await _context.Odontologos
            .FirstOrDefaultAsync(o => o.UsuarioId == usuarioIdOdontologo);

        if (odontologo == null)
            return new List<TurnoDTO>();

        var turnos = await _context.Turnos
            .Include(t => t.Paciente)
            .Include(t => t.ObraSocial)
            .Where(t => t.OdontologoId == odontologo.Id)
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
            NombreOdontologo = odontologo.Nombre, 
            IdObraSocial = t.ObraSocialId,
            NombreObraSocial = t.ObraSocial?.Nombre
        }).ToList();
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
    public async Task<ResultadoOperacion> ReservarTurnoAsync(int turnoId, ReservarTurnoDTO dto, int usuarioId)
    {
        var turno = await _context.Turnos.FirstOrDefaultAsync(t => t.Id == turnoId && t.UsuarioId == usuarioId);
        if (turno == null) 
            return new ResultadoOperacion(false, "Turno no encontrado o no pertenece al usuario");
        if (!turno.Disponible) 
            return new ResultadoOperacion(false, "El turno no está disponible");

        if (!await _context.Pacientes.AnyAsync(p => p.Id == dto.IdPaciente && p.UsuarioId == usuarioId))
            return new ResultadoOperacion(false, "Paciente no encontrado o no pertenece al usuario");

        turno.Disponible = false;
        turno.IdPaciente = dto.IdPaciente;
        turno.OdontologoId = dto.IdOdontologo;
        turno.ObraSocialId = dto.IdObraSocial;

        await _context.SaveChangesAsync();
        return new ResultadoOperacion(true, string.Empty);
    }
    public async Task<ResultadoOperacion> EditarTurnoAsync(int turnoId, EditarTurnosDTO dto, int usuarioId)
    {
        var turno = await _context.Turnos
            .Include(t => t.Calendario)
            .FirstOrDefaultAsync(t => t.Id == turnoId && t.Calendario.IdUsuario == usuarioId);

        if (turno == null)
            return ResultadoOperacion.Fallo("Turno no encontrado o no te pertenece.");

        if (turno.Disponible)
            return ResultadoOperacion.Fallo("No se puede editar un turno disponible.");

        turno.IdPaciente = dto.IdPaciente;
        turno.OdontologoId = dto.IdOdontologo;
        turno.ObraSocialId = dto.IdObraSocial;

        await _context.SaveChangesAsync();

        return ResultadoOperacion.Exito();
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
    public async Task<bool> MarcarAsistenciaAsync(int turnoId, bool asistio, int usuarioId, string rol)
    {
        Turno turno = null;

        if (rol == "Odontologo")
        {
            turno = await _context.Turnos
                .Include(t => t.Odontologo)
                .FirstOrDefaultAsync(t => t.Id == turnoId && t.Odontologo != null && t.Odontologo.UsuarioId == usuarioId);
        }
        else // administrador
        {
            turno = await _context.Turnos
                .Include(t => t.Calendario)
                 .FirstOrDefaultAsync(t => t.Id == turnoId && t.Calendario.IdUsuario == usuarioId);
        }

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
    public async Task<int> AsignarTurnosMasivosAsync(int usuarioId)
    {
        var turnosDisponibles = await _context.Turnos
            .Where(t => t.UsuarioId == usuarioId && t.Disponible == true && t.OdontologoId == null)
            .ToListAsync();

        var pacientes = await _context.Pacientes.Where(p => p.UsuarioId == usuarioId).ToListAsync();
        var odontologos = await _context.Odontologos.Where(o => o.AdministradorId == usuarioId).ToListAsync();
        var obras = await _context.ObrasSociales.Where(o => o.UsuarioId == usuarioId).ToListAsync();

        if (!pacientes.Any() || !odontologos.Any() || !obras.Any())
        {
            throw new Exception($"Pacientes: {pacientes.Count}, Odontólogos: {odontologos.Count}, Obras: {obras.Count}");
        }

        var random = new Random();
        int asignados = 0;

        foreach (var turno in turnosDisponibles)
        {
            turno.IdPaciente = pacientes[random.Next(pacientes.Count)].Id;
            turno.OdontologoId = odontologos[random.Next(odontologos.Count)].Id;
            turno.ObraSocialId = obras[random.Next(obras.Count)].Id;
            turno.Disponible = false;
            turno.Asistio = null;
            asignados++;
        }

        await _context.SaveChangesAsync();
        return asignados;
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


