using Agenda.Base;
using Agenda.Entidades.DTOs.DTO.OdontologosDTO;
using Agenda.Entidades;
using Microsoft.EntityFrameworkCore;
using Agenda.Interfaces;


namespace Agenda.Servicios
{
    public class OdontologoService : IOdontologoService
    {
        private readonly AppDbContext _context;

        public OdontologoService(AppDbContext context)
        {
            _context = context;
        }

       
        public async Task<IEnumerable<VerOdontologoDTO>> ObtenerOdontologosAsync(int usuarioId)
        {
            return await _context.Odontologos
                .Where(o => o.UsuarioId == usuarioId)
                .Select(o => new VerOdontologoDTO
                {
                    Id = o.Id,
                    Nombre = o.Nombre,
                    Email = o.Email,
                    Password = o.Password,
                }).ToListAsync();
        }
        public async Task<OdontologosDTO> ObtenerPorIdAsync(int id, int usuarioId)
        {
            var odontologo = await _context.Odontologos.FirstOrDefaultAsync(o => o.Id == id && o.UsuarioId == usuarioId);
            if (odontologo == null) return null;

            return new OdontologosDTO
            {
                Id = odontologo.Id,
                Nombre = odontologo.Nombre,
                Email = odontologo.Email
            };
        }
        public async Task<OdontologosDTO> CrearAsync(CrearOdontologoDTO dto, int usuarioIdAdmin)
        {
            var existe = await _context.Usuarios.AnyAsync(u => u.Email == dto.Email);
            if (existe) throw new Exception("Ya existe un usuario con ese email");
            
            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                RolId = 2 
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            
            var odontologo = new Odontologo
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                Password = usuario.Password,
                UsuarioId = usuarioIdAdmin
            };

            _context.Odontologos.Add(odontologo);
            await _context.SaveChangesAsync();

            return new OdontologosDTO
            {
                Id = odontologo.Id,
                Nombre = odontologo.Nombre,
                Email = odontologo.Email
            };
        }

        public async Task<bool> EditarAsync(int id, EditarOdontologoDTO dto, int usuarioId)
        {
            var odontologo = await _context.Odontologos.FirstOrDefaultAsync(o => o.Id == id && o.UsuarioId == usuarioId);
            if (odontologo == null) return false;

            odontologo.Nombre = dto.Nombre;
            odontologo.Email = dto.Email;
            odontologo.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarAsync(int id, int usuarioId)
        {
            var odontologo = await _context.Odontologos.FirstOrDefaultAsync(o => o.Id == id && o.UsuarioId == usuarioId);
            if (odontologo == null) return false;

            _context.Odontologos.Remove(odontologo);
            await _context.SaveChangesAsync();
            return true;
        }

     

       
    }
}
