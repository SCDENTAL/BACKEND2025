using Agenda.Entidades;
using Agenda.Entidades.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Turno
{

    
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "date")]
    public DateTime Fecha { get; set; }

    [Required]
    [Column(TypeName = "TIME(0)")]
    public TimeSpan Horario { get; set; }

    [Required]
    public bool Disponible { get; set; }

    public bool? Asistio { get; set; }
  

    [Required]
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }

    [Required]
    public int IdCalendario { get; set; }
    public Calendario Calendario { get; set; }
          
    public int? IdPaciente { get; set; }
    public Paciente? Paciente { get; set; }

    public int? OdontologoId { get; set; }
    public Odontologo? Odontologo { get; set; }

    public int? ObraSocialId { get; set; }
    public ObraSocial? ObraSocial { get; set; }

    public bool MensajeConfirmacionEnviado { get; set; } = false;


}
