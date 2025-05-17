using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System;

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
            // Primero intentar obtener de la sesión
            if (_httpContextAccessor.HttpContext?.Session != null)
            {
                var empresaId = _httpContextAccessor.HttpContext.Session.GetInt32("EmpresaId");
                if (empresaId.HasValue)
                {
                    return empresaId.Value;
                }
            }
            
            // Si no hay sesión, devolver valor por defecto
            return 4; // Valor por defecto para empresa existente
        }

        public string GetUserName()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Usuario"; // Valor por defecto
        }
    }
}
