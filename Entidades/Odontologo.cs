namespace Agenda.Entidades
{
    public class Odontologo
    {
        public int Id { get; set; }
        public string Nombre { get; set; } 
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public int? AdministradorId { get; set; }     // Usuario que lo creó
        public Usuario Administrador { get; set; }
        public ICollection<Turno> Turnos { get; set; }



    }
}
