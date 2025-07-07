namespace Agenda.Entidades.DTOs.DTO.TurnosDTO
{
    public class TurnoOdontologoDTO
    {
       
            public int Id { get; set; }
            public DateTime Fecha { get; set; } 
            public TimeSpan Horario { get; set; }

            public string NombrePaciente { get; set; }
            public string NumeroPaciente { get; set; }  

            public string NombreObraSocial { get; set; }

            public bool Asistio { get; set; }
        
    }
}
