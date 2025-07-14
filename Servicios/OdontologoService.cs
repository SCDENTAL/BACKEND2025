using Agenda.Base;
using Agenda.Entidades;
using Agenda.Entidades.DTOs;
using Agenda.Entidades.DTOs.DTO.OdontologosDTO;
using Agenda.Exceptions;
using Agenda.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Agenda.Servicios
{
    public class OdontologoService : IOdontologoService
    {
        private readonly AppDbContext _context;

        public OdontologoService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<VerOdontologoDTO>> ObtenerOdontologosAsync(int usuarioIdAdmin)
        {
            return await _context.Odontologos
                .Include(o => o.Usuario)
                .Where(o => o.AdministradorId == usuarioIdAdmin) 
                .Select(o => new VerOdontologoDTO
                {
                    Id = o.Id,
                    Nombre = o.Nombre,
                    Email = o.Usuario.Email,                    
                }).ToListAsync();
        }



        public async Task<OdontologosDTO> ObtenerPorIdAsync(int id, int usuarioId)
        {
            var odontologo = await _context.Odontologos
                .Include(o => o.Usuario)
                .FirstOrDefaultAsync(o => o.Id == id && o.UsuarioId == usuarioId);

            if (odontologo == null) return null;

            return new OdontologosDTO
            {
                Id = odontologo.Id,
                Nombre = odontologo.Nombre,
                Email = odontologo.Usuario.Email,                
            };
        }


        public async Task<int> CrearAync(RegistroDto dto, int usuarioIdAdmin)
        {
            var existe = await _context.Usuarios.AnyAsync(u => u.Email == dto.Email);
            if (existe)
                throw new BadRequestException("El email ya está registrado.");

            var usuarioNuevo = new Usuario
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                RolId = 2
            };

            _context.Usuarios.Add(usuarioNuevo);
            await _context.SaveChangesAsync();

            var odontologo = new Odontologo
            {
                Nombre = dto.Nombre,
                UsuarioId = usuarioNuevo.Id,
                AdministradorId = usuarioIdAdmin
            };

            _context.Odontologos.Add(odontologo);
            await _context.SaveChangesAsync();

            return odontologo.Id;
        }

        public async Task<bool> EditarAsync(int id, EditarOdontologoDTO dto, int usuarioIdAdmin)
        {
            var odontologo = await _context.Odontologos
                .Include(o => o.Usuario)
                .FirstOrDefaultAsync(o => o.Id == id && o.AdministradorId == usuarioIdAdmin);

            if (odontologo == null)
                throw new BadRequestException("El odontólogo no existe o no pertenece al administrador.");

            odontologo.Nombre = dto.Nombre;

            if (!string.IsNullOrWhiteSpace(dto.Email))
            {
                var emailEnUso = await _context.Usuarios
                    .AnyAsync(u => u.Email == dto.Email && u.Id != odontologo.UsuarioId);

                if (emailEnUso)
                    throw new BadRequestException("Ese email ya está en uso por otro usuario.");

                odontologo.Usuario.Email = dto.Email;
            }

            if (!string.IsNullOrWhiteSpace(dto.Password))
                odontologo.Usuario.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> EliminarAsync(int id, int usuarioIdAdmin)
        {
            var odontologo = await _context.Odontologos
                .FirstOrDefaultAsync(o => o.Id == id && o.AdministradorId == usuarioIdAdmin);

            if (odontologo == null)
                throw new BadRequestException("El odontólogo no existe o no pertenece al administrador.");

            _context.Odontologos.Remove(odontologo);
            await _context.SaveChangesAsync();
            return true;
        }




    }
}
