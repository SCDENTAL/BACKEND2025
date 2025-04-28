namespace Agenda.Entidades.DTOs.DTO.PacientesDTO
{
    public class EditarPacientesDTO
    {        
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Dni { get; set; }
        public string ObraSocial { get; set; }

        public int Telefono { get; set; }

        public string Email { get; set; }
    }
}
