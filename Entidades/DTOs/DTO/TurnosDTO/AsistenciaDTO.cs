using System.ComponentModel.DataAnnotations;

namespace Agenda.Entidades.DTOs.DTO.TurnosDTO
{
    public class AsistenciaDTO
    {
        [Required]
        public bool Asistio { get; set; }
    }
}
