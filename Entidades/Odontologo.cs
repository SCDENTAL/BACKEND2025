namespace Agenda.Entidades
{
    public class Odontologo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public ICollection<Turno> Turnos { get; set; }



    }
}
