using Agenda.Entidades;
using Agenda.Entidades.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Base
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }
                
        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Odontologo> Odontologos { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<ObraSocial> ObrasSociales { get; set; }
        public DbSet<Turno> Turnos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Usuario -> Odontologos
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Odontologos)
                .WithOne(o => o.Usuario)
                .HasForeignKey(o => o.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Usuario -> Pacientes
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Pacientes)
                .WithOne(p => p.Usuario)
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Usuario -> ObrasSociales
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.ObrasSociales)
                .WithOne(o => o.Usuario)
                .HasForeignKey(o => o.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Usuario -> Turnos
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Turnos)
                .WithOne(t => t.Usuario)
                .HasForeignKey(t => t.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Odontologo -> Turnos
            modelBuilder.Entity<Odontologo>()
                .HasMany(o => o.Turnos)
                .WithOne(t => t.Odontologo)
                .HasForeignKey(t => t.OdontologoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Paciente -> Turnos
            modelBuilder.Entity<Paciente>()
                .HasMany(p => p.Turnos)
                .WithOne(t => t.Paciente)
                .HasForeignKey(t => t.PacienteId)
                .OnDelete(DeleteBehavior.Restrict);

            // ObraSocial -> Pacientes
            modelBuilder.Entity<ObraSocial>()
                .HasMany(o => o.Pacientes)
                .WithOne(p => p.ObraSocial)
                .HasForeignKey(p => p.ObraSocialId)
                .OnDelete(DeleteBehavior.SetNull);


            modelBuilder.Entity<Rol>().HasData(
                new Rol { Id = 1, Nombre = "Administrador" },
                new Rol { Id = 2, Nombre = "Odontologo" }
   );
        }






    }
}
