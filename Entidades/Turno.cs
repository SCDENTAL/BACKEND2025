using Agenda.Entidades;

public class Turno
{
    public int Id { get; set; }
    public DateTime FechaHora { get; set; }
    public string Estado { get; set; }
    public string Consultorio { get; set; }

    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }

    public int OdontologoId { get; set; }
    public Odontologo Odontologo { get; set; }

    public int PacienteId { get; set; }
    public Paciente Paciente { get; set; }
}
