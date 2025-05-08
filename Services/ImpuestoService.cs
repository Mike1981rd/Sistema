using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SistemaContable.Data;
using SistemaContable.Models;

namespace SistemaContable.Services
{
    public class ImpuestoService : IImpuestoService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ImpuestoService> _logger;
        
        public ImpuestoService(
            ApplicationDbContext context,
            ILogger<ImpuestoService> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        /// <summary>
        /// Verifica si un impuesto puede ser eliminado según las reglas contables
        /// </summary>
        public async Task<bool> PuedeEliminarImpuesto(int impuestoId)
        {
            try
            {
                var impuesto = await _context.Impuestos.FindAsync(impuestoId);
                if (impuesto == null)
                {
                    _logger.LogWarning("Intento de verificar eliminación de impuesto inexistente. ID: {ImpuestoId}", impuestoId);
                    return false;
                }
                    
                // Regla 1: No se puede eliminar si está marcado como en uso
                if (impuesto.EstaEnUso)
                {
                    _logger.LogInformation("El impuesto {ImpuestoId} ({Nombre}) no puede ser eliminado porque está en uso", 
                        impuestoId, impuesto.Nombre);
                    return false;
                }
                    
                // Regla 2: No se puede eliminar si tiene documentos relacionados
                // Esta lógica se ampliará cuando existan los módulos de facturas y compras
                
                // Por ahora, solo verificamos el flag de uso
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar si el impuesto {ImpuestoId} puede ser eliminado", impuestoId);
                return false;
            }
        }
        
        /// <summary>
        /// Actualiza el estado "EstaEnUso" del impuesto
        /// </summary>
        public async Task ActualizarEstadoUsoImpuesto(int impuestoId, bool estaEnUso)
        {
            try
            {
                var impuesto = await _context.Impuestos.FindAsync(impuestoId);
                if (impuesto == null)
                {
                    _logger.LogWarning("Intento de actualizar estado de uso de impuesto inexistente. ID: {ImpuestoId}", impuestoId);
                    return;
                }
                
                impuesto.EstaEnUso = estaEnUso;
                impuesto.FechaModificacion = DateTime.UtcNow;
                
                await _context.SaveChangesAsync();
                _logger.LogInformation("Estado de uso del impuesto {ImpuestoId} ({Nombre}) actualizado a: {EstaEnUso}", 
                    impuestoId, impuesto.Nombre, estaEnUso);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el estado de uso del impuesto {ImpuestoId}", impuestoId);
            }
        }
    }
} 