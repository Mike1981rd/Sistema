using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using SistemaContable.Data;
using Microsoft.Extensions.Logging;

namespace SistemaContable.Services
{
    public class EmpresaService : IEmpresaService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmpresaService> _logger;

        public EmpresaService(
            IHttpContextAccessor httpContextAccessor, 
            ApplicationDbContext context,
            ILogger<EmpresaService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _logger = logger;
        }

        public int ObtenerEmpresaActualId()
        {
            try
            {
                // Intenta obtener el ID de empresa de la sesión
                if (_httpContextAccessor.HttpContext?.Session != null)
                {
                    if (_httpContextAccessor.HttpContext.Session.TryGetValue("EmpresaId", out byte[]? empresaIdBytes) && 
                        empresaIdBytes != null)
                    {
                        return BitConverter.ToInt32(empresaIdBytes, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error al obtener empresa de sesión. Usando valor por defecto.");
            }
            
            // Si no se pudo obtener de la sesión, busca en la base de datos
            try
            {
                // Verifica si existe al menos una empresa
                var empresaId = _context.Empresas.Select(e => e.Id).FirstOrDefault();
                
                // Si hay al menos una empresa, devuelve su ID
                if (empresaId != 0)
                {
                    return empresaId;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error al obtener empresa de base de datos. Usando valor por defecto.");
            }
            
            // Si no hay empresas o hubo un error, devuelve 1 (valor predeterminado)
            return 1;
        }
    }
} 