namespace Agenda.Entidades.DTOs.DTO.CalendarioDTO
{
    public class ExtenderCalendarioDTO
    {
        public DateTime FechaFin { get; set; }

        public bool ExcluirDomingo { get; set; } = false;
    }
}
