using System.ComponentModel.DataAnnotations;

namespace Agenda.Entidades.DTOs.DTO.TurnosDTO
{
    public class EditarTurnosDTO
    {
        [Required]
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public TimeSpan Horario { get; set; }

        public bool Disponible { get; set; }

        public bool? Asistio { get; set; }
        [Required]
        public int IdPaciente { get; set; }

        public int? OdontologoId { get; set; }

        public int? IdObraSocial { get; set; }
    }
}
