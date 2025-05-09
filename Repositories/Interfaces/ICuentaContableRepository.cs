using SistemaContable.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaContable.Repositories.Interfaces
{
    public interface ICuentaContableRepository
    {
        Task<IEnumerable<CuentaContable>> GetAllAsync();
        Task<CuentaContable> GetByIdAsync(int id);
        Task<CuentaContable> CreateAsync(CuentaContable cuentaContable);
        Task UpdateAsync(CuentaContable cuentaContable);
        Task DeleteAsync(int id);
        Task<bool> ExisteCodigoAsync(string codigo, int empresaId);
        Task<IEnumerable<CuentaContable>> BuscarPorNombreOCodigoAsync(string termino);
        Task<IEnumerable<CuentaContable>> GetCuentasMovimientoAsync(int empresaId);
    }
} 