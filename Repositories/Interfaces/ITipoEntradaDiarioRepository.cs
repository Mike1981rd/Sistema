using SistemaContable.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaContable.Repositories.Interfaces
{
    public interface ITipoEntradaDiarioRepository
    {
        Task<IEnumerable<TipoEntradaDiario>> GetAllAsync();
        Task<TipoEntradaDiario> GetByIdAsync(int id);
        Task<TipoEntradaDiario> CreateAsync(TipoEntradaDiario tipoEntrada);
        Task UpdateAsync(TipoEntradaDiario tipoEntrada);
        Task DeleteAsync(int id);
        Task<bool> ExisteCodigoAsync(string codigo);
    }
} 