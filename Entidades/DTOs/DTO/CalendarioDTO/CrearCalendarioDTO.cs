using System.ComponentModel.DataAnnotations;

namespace Agenda.Entidades.DTOs.DTO.CalendarioDTO
{
    public class CrearCalendarioDTO
    {
        [Required]
        public string Nombre { get; set; }
        [Required]
        public DateTime FechaInicio { get; set; }
        [Required]
        public DateTime FechaFin { get; set; }
        [Required]
        public int HoraInicioTurnos { get; set; }
        [Required]
        public int HoraFinTurnos { get; set; }
        [Required]
        public int IntervaloTurnos { get; set; }
        public bool ExcluirDomingo { get; set; } = true;
    }
}
