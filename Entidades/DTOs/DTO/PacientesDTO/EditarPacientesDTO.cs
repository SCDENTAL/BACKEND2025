namespace Agenda.Entidades.DTOs.DTO.PacientesDTO
{
    public class EditarPacientesDTO
    {        
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Dni { get; set; }        

        public string Telefono { get; set; }

        public string Email { get; set; }

		public int ObraSocialId { get; set; }

        public string ObraSocial { get; set;}
	}
}
