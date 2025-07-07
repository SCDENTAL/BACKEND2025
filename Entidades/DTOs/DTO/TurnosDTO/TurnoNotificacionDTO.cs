namespace Agenda.Entidades.DTOs.DTO.TurnosDTO
{
	public class TurnoNotificacionDTO
	{
		
			public int TurnoId { get; set; }
			public string NombrePaciente { get; set; }
			public string TelefonoPaciente { get; set; } // Para WhatsApp 
 			public string EmailPaciente { get; set; }    // Para email
			public string NombreOdontologo { get; set; }
			public DateTime FechaHoraInicio { get; set; }
			public string Estado { get; set; }
			public string Consultorio { get; set; } // Opcional, si aplica
		

	}
}
