using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace SistemaContable.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userId != null ? int.Parse(userId) : 1; // Valor por defecto para desarrollo
        }

        public int GetEmpresaId()
        {
            // Esto debería implementarse según tu lógica de negocio específica
            // Podría ser de una claim, de la sesión, o de la base de datos
            return 1; // Valor por defecto para desarrollo
        }

        public string GetUserName()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Usuario"; // Valor por defecto
        }
    }
}
