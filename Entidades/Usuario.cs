namespace Agenda.Entidades
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }


        public int RolId { get; set; }
        public Rol Rol { get; set; }

        // Cada usuario administra su propio centro
        public ICollection<Odontologo> Odontologos { get; set; }
        public ICollection<Paciente> Pacientes { get; set; }
        public ICollection<ObraSocial> ObrasSociales { get; set; }
        public ICollection<Turno> Turnos { get; set; }

    }
}
