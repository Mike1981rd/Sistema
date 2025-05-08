using SistemaContable.Models;
using SistemaContable.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace SistemaContable.Services
{
    public interface IPlazoPagoService
    {
        Task<bool> PuedeEliminarPlazoPago(int plazoId);
        Task ActualizarEstadoUsoPlazoPago(int plazoId, bool estaEnUso);
    }

    public class PlazoPagoService : IPlazoPagoService
    {
        private readonly ApplicationDbContext _context;
        
        public PlazoPagoService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Verifica si un plazo de pago puede ser eliminado
        /// </summary>
        public async Task<bool> PuedeEliminarPlazoPago(int plazoId)
        {
            var plazo = await _context.PlazosPago.FindAsync(plazoId);
            if (plazo == null)
                return false;
                
            // No se puede eliminar si está en uso
            if (plazo.EstaEnUso)
                return false;
                
            // No se pueden eliminar los plazos predeterminados (IDs 1-6)
            if (plazo.Id <= 6)
                return false;
                
            return true;
        }
        
        /// <summary>
        /// Actualiza el estado "EstaEnUso" del plazo
        /// </summary>
        public async Task ActualizarEstadoUsoPlazoPago(int plazoId, bool estaEnUso)
        {
            var plazo = await _context.PlazosPago.FindAsync(plazoId);
            if (plazo != null)
            {
                plazo.EstaEnUso = estaEnUso;
                plazo.FechaModificacion = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
} 