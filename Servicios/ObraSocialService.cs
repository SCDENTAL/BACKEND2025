using Agenda.Entidades.DTOs.DTO.ObrasSocialesDTO;
using Agenda.Entidades;
using Agenda.Base;
using Microsoft.EntityFrameworkCore;
using Agenda.Interfaces;

namespace Agenda.Servicios
{
    public class ObraSocialService : IObrasSocialesService
    {
        private readonly AppDbContext _context;
        public ObraSocialService(AppDbContext context) => _context = context;

        public async Task<List<ObraSocialDTO>> ObtenerTodasAsync(int usuarioId) =>
            await _context.ObrasSociales
                .Where(x => x.UsuarioId == usuarioId)
                .Select(x => new ObraSocialDTO { Id = x.Id, Nombre = x.Nombre })
                .ToListAsync();

        public async Task<ObraSocialDTO> CrearAsync(CrearObraSocialDTO dto, int usuarioId)
        {
            var obra = new ObraSocial { Nombre = dto.Nombre, UsuarioId = usuarioId };
            _context.ObrasSociales.Add(obra);
            await _context.SaveChangesAsync();
            return new ObraSocialDTO { Id = obra.Id, Nombre = obra.Nombre };
        }

        public async Task<bool> EliminarAsync(int id, int usuarioId)
        {
            var obra = await _context.ObrasSociales.FirstOrDefaultAsync(x => x.Id == id && x.UsuarioId == usuarioId);
            if (obra == null) return false;
            _context.ObrasSociales.Remove(obra);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
