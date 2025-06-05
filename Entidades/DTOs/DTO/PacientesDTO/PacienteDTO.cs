namespace Agenda.Entidades.DTOs.DTO.PacientesDTO
{
    public class PacienteDTO
    {
        public int Id { get; set; } 
        public string Nombre { get; set; }

        public string Apellido { get; set; }
        public int Dni { get; set; }
        public string ObraSocial { get; set; }

        public string Telefono { get; set; }

        public string Email { get; set; }
    }
}
