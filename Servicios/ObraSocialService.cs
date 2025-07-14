using Agenda.Base;
using Agenda.Entidades;
using Agenda.Entidades.DTOs;
using Agenda.Entidades.DTOs.DTO.ObrasSocialesDTO;
using Agenda.Exceptions;
using Agenda.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public async Task<ObraSocialDTO?> ObtenerPorIdAsync(int id, int usuarioId)
        {
            return await _context.ObrasSociales
                .Where(x => x.Id == id && x.UsuarioId == usuarioId)
                .Select(x => new ObraSocialDTO { Id = x.Id, Nombre = x.Nombre })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> EditarAsync(int id, EditarObraSocialDTO dto, int usuarioId)
        {
            var obra = await _context.ObrasSociales
                .FirstOrDefaultAsync(x => x.Id == id && x.UsuarioId == usuarioId);

            if (obra == null)
                throw new BadRequestException("No se encontró la obra social.");

            var nombreNuevo = dto.Nombre.Trim();

            var nombreDuplicado = await _context.ObrasSociales
                .AnyAsync(x => x.UsuarioId == usuarioId && x.Id != id && x.Nombre.ToLower() == nombreNuevo.ToLower());

            if (nombreDuplicado)
                throw new BadRequestException("Ya existe otra obra social con ese nombre.");

            obra.Nombre = nombreNuevo;
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<ObraSocialDTO> CrearAsync(CrearObraSocialDTO dto, int usuarioId)
        {
            var nombre = dto.Nombre.Trim();

            var existe = await _context.ObrasSociales
                .AnyAsync(x => x.UsuarioId == usuarioId && x.Nombre.ToLower() == nombre.ToLower());

            if (existe)
                throw new BadRequestException("Ya existe una obra social con ese nombre.");

            var obra = new ObraSocial { Nombre = nombre, UsuarioId = usuarioId };

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
