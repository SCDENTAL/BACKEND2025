namespace Agenda.Entidades.DTOs.DTO.TurnosDTO
{
    public class TurnoDetalleDTO
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public TimeSpan Horario { get; set; }

        public bool Disponible { get; set; }

        public bool? Asistio { get; set; }

        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; }

        public int IdCalendario { get; set; }
        public string NombreCalendario { get; set; }

        public int IdPaciente { get; set; }
        public string NombrePaciente { get; set; }

        public int? OdontologoId { get; set; }
        public string NombreOdontologo { get; set; }

        public int? IdObraSocial { get; set; }
        public string NombreObraSocial { get; set; }
    }
}
