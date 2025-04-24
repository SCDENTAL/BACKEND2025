using Agenda.Entidades.DTOs;
using Microsoft.AspNetCore.Identity.Data;

namespace Agenda.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string Message, AuthResponse Data)> Registrar(RegistroDto dto);
        Task<(bool Success, string Message, AuthResponse Data)> Login(LoginDto dto);
    }
}
