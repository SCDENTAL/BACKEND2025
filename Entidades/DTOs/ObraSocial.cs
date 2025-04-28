namespace Agenda.Entidades.DTOs
{
    public class ObraSocial
    {

        public int Id { get; set; }
        public string Nombre { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public ICollection<Paciente> Pacientes { get; set; }

    }
}
