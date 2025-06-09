using System.ComponentModel.DataAnnotations;

namespace Agenda.Entidades.DTOs.DTO.TurnosDTO
{
    public class CrearTurnosDTO
    {

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public TimeSpan Horario { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Required]
        public int IdCalendario { get; set; }

        [Required]
        public int IdPaciente { get; set; }

        public int? OdontologoId { get; set; }

        public int? IdObraSocial { get; set; }

    }
}
