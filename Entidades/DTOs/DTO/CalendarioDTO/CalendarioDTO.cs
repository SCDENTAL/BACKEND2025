using System.ComponentModel.DataAnnotations;

namespace Agenda.Entidades.DTOs.DTO.CalendarioDTO
{
    public class CalendarioDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaFin { get; set; }

        [Required]
        public TimeSpan HoraInicioTurnos { get; set; }

        [Required]
        public TimeSpan HoraFinTurnos { get; set; }

        [Required]
        public TimeSpan IntervaloTurnos { get; set; }
    }
}
