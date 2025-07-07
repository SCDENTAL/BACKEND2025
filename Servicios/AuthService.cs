using Agenda.Base;
using Agenda.Entidades;
using Agenda.Entidades.DTOs;
using Agenda.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Servicios
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly TokenService _token;
        private readonly PasswordHasher<Usuario> _hasher = new();

        public AuthService(AppDbContext context, IConfiguration config, TokenService token)
        {
            _context = context;
            _config = config;
            _token = token;
        }

        public async Task<(bool Success, string Message, AuthResponse Data)> Registrar(RegistroDto dto)
        {
            var existe = await _context.Usuarios.AnyAsync(u => u.Email == dto.Email);
            if (existe)
                return (false, "El email ya está registrado", null);

            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                RolId = 1
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();


            var usuarioConRol = await _context.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.Id == usuario.Id);

            return await _token.GenerarToken(usuario);
        }

        public async Task<(bool Success, string Message)> CrearOdontologoDesdeAdmin(RegistroDto dto, int usuarioIdAdmin)
        {
            // Validar duplicado
            var existe = await _context.Usuarios.AnyAsync(u => u.Email == dto.Email);
            if (existe)
                return (false, "El email ya está registrado.");

            // Crear el nuevo usuario odontólogo
            var usuarioNuevo = new Usuario
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                RolId = 2
            };

            _context.Usuarios.Add(usuarioNuevo);
            await _context.SaveChangesAsync();

            // ✅ Crear el odontólogo vinculado al ADMIN que lo creó
            var odontologo = new Odontologo
            {
                Nombre = dto.Nombre,
                UsuarioId = usuarioIdAdmin // ✅ correcto: es el ID del admin logueado
            };

            _context.Odontologos.Add(odontologo);
            await _context.SaveChangesAsync();

            return (true, "Odontólogo creado correctamente.");
        }





        public async Task<(bool Success, string Message, AuthResponse Data)> Login(LoginDto dto)
        {         
            var usuario = await _context.Usuarios.Include(u => u.Rol).FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Password, usuario.Password))
                return (false, "Credenciales inválidas", null);

            return await _token.GenerarToken(usuario);
        }
    }
}
