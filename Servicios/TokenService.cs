using Agenda.Entidades.DTOs;
using Agenda.Entidades;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Agenda.Servicios
{
    public class TokenService

    {
        private readonly IConfiguration _config;
        public TokenService(IConfiguration config)
        {
            _config = config;

        }

        public async Task<(bool Success, string Message, AuthResponse Data)> GenerarToken(Usuario usuario)
        {
            var claims = new List<Claim>
                 {
                       new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                       new Claim(ClaimTypes.Name, usuario.Email),
                       new Claim(ClaimTypes.Role, usuario.Rol.Nombre)
                 };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(4),
                signingCredentials: creds
            );

            return (true, "OK", new AuthResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiracion = token.ValidTo
            });
        }
    }
}