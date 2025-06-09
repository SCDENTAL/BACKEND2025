namespace Agenda.Entidades.DTOs.DTO.TurnosDTO
{
    public class ResTurnosFiltrados
    {
        public List<TurnoDTO> Lunes { get; set; }

        public List<TurnoDTO> Martes { get; set; }

        public List<TurnoDTO> Miercoles { get; set; }

        public List<TurnoDTO> Jueves { get; set; }

        public List<TurnoDTO> Viernes { get; set; }

        public List<TurnoDTO> Sabado { get; set; }

        public List<TurnoDTO> Domingo { get; set; }

        public int CantidadHorarios { get; set; }
    }
}
