namespace Agenda.Entidades
{
    public class Rol
    {
        public int Id { get; set; } // 1 = Admin, 2 = Odontólogo
        public string Nombre { get; set; }
        public ICollection<Usuario> Usuarios { get; set; }
    }
}
