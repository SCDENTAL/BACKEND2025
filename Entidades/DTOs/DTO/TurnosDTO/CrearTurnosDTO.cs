namespace Agenda.Entidades.DTOs.DTO.TurnosDTO
{
    public class CrearTurnosDTO
    {
      
            public string NombrePaciente { get; set; }
            public int DniPaciente { get; set; }
            public string ObraSocial { get; set; }
            public DateTime FechaHora { get; set; }
            public int OdontologoId { get; set; }
            public int UsuarioId { get; set; } // para asociarlo al dueño del sistema
        
    }
}
