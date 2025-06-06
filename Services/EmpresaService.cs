using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using SistemaContable.Data;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace SistemaContable.Services
{
    public class EmpresaService : IEmpresaService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmpresaService> _logger;
        private readonly int _empresaActualId;

        public EmpresaService(
            IHttpContextAccessor httpContextAccessor, 
            ApplicationDbContext context,
            ILogger<EmpresaService> logger,
            IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _logger = logger;

            // Leer el EmpresaUnicaId desde la configuración
            _empresaActualId = configuration.GetValue<int>("AppSettings:EmpresaUnicaId");

            // Validar el valor leído
            if (_empresaActualId == 0)
            {
                _logger.LogError("La configuración 'AppSettings:EmpresaUnicaId' es requerida y debe ser un valor válido mayor que 0.");
                throw new InvalidOperationException("La configuración 'AppSettings:EmpresaUnicaId' es requerida y no se encontró o es inválida. Por favor, configure un ID de empresa válido en appsettings.json.");
            }

            _logger.LogInformation("EmpresaService inicializado correctamente. EmpresaUnicaId configurado: {EmpresaId}", _empresaActualId);
        }

        public async Task<int> ObtenerEmpresaActualId()
        {
            // Devolver el valor configurado en appsettings.json
            return await Task.FromResult(_empresaActualId);
            
            /* CÓDIGO ORIGINAL COMENTADO
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
                var empresaId = await _context.Empresas.Select(e => e.Id).FirstOrDefaultAsync();
                
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
            */
        }

        public int ObtenerEmpresaActualIdSincrono()
        {
            return _empresaActualId;
        }
    }
} 