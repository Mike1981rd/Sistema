using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using System;
using System.Threading.Tasks;

namespace SistemaContable
{
    /// <summary>
    /// Middleware personalizado para verificar y configurar la empresa en sesión
    /// </summary>
    public class SessionMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ApplicationDbContext dbContext)
        {
            try
            {
                // Verificar si hay empresa en sesión
                if (!context.Session.TryGetValue("EmpresaId", out _))
                {
                    // Si no hay empresa en sesión, buscar la primera disponible
                    var empresa = await dbContext.Empresas.FirstOrDefaultAsync();
                    if (empresa != null)
                    {
                        // Establecer empresa en sesión
                        context.Session.SetInt32("EmpresaId", empresa.Id);
                        Console.WriteLine($"[SessionMiddleware] Empresa {empresa.Id} establecida en sesión");
                    }
                    else
                    {
                        // No hay empresas disponibles, esto es un problema crítico
                        Console.WriteLine("[SessionMiddleware] ADVERTENCIA: No se encontraron empresas en la base de datos");
                    }
                }
            }
            catch (Exception ex)
            {
                // Registrar la excepción pero continuar con la solicitud
                Console.WriteLine($"[SessionMiddleware] ERROR: {ex.Message}");
            }

            // Llamar al siguiente middleware en la pipeline
            await _next(context);
        }
    }

    // Clase de extensión para registrar el middleware
    public static class SessionMiddlewareExtensions
    {
        public static IApplicationBuilder UseSessionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SessionMiddleware>();
        }
    }
}