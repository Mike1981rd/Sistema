using System.Threading.Tasks;

namespace SistemaContable.Services
{
    public interface IImpuestoService
    {
        /// <summary>
        /// Verifica si un impuesto puede ser eliminado según las reglas contables
        /// </summary>
        Task<bool> PuedeEliminarImpuesto(int impuestoId);
        
        /// <summary>
        /// Actualiza el estado "EstaEnUso" del impuesto
        /// </summary>
        Task ActualizarEstadoUsoImpuesto(int impuestoId, bool estaEnUso);
    }
} 