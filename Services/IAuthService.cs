using SistemaContable.Models;

namespace SistemaContable.Services
{
    public interface IAuthService
    {
        Task<Usuario?> AuthenticateAsync(string loginIdentifier, string password);
        Task<bool> ValidatePasswordAsync(string password, string hashedPassword);
        string HashPassword(string password);
        Task LoginUserAsync(Usuario usuario);
        Task LogoutUserAsync();
        Usuario? GetCurrentUser();
        bool IsAuthenticated();
    }
}