﻿using Agenda.Entidades.DTOs;

namespace Agenda.Entidades
{
    public class Paciente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Dni { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }

        public int? ObraSocialId { get; set; }
        public ObraSocial? ObraSocial { get; set; } 
        
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public ICollection<Turno> Turnos { get; set; }
    }
}
