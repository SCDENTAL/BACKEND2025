using Agenda.Entidades;
using Agenda.Entidades.DTOs;

public class Turno
{

	public int Id { get; set; }
	public DateTime FechaHora { get; set; }

	public EstadoTurno Estado { get; set; } = EstadoTurno.Pendiente;

	public int UsuarioId { get; set; }
	public Usuario Usuario { get; set; }

	public int OdontologoId { get; set; }
	public Odontologo Odontologo { get; set; }

	public int PacienteId { get; set; }
	public Paciente Paciente { get; set; }

	public bool Confirmado { get; set; } = false;


}
