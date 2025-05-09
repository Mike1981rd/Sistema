using SistemaContable.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaContable.Repositories.Interfaces
{
    public interface INumeracionEntradaDiarioRepository
    {
        Task<IEnumerable<NumeracionEntradaDiario>> GetAllAsync();
        Task<NumeracionEntradaDiario> GetByIdAsync(int id);
        Task<IEnumerable<NumeracionEntradaDiario>> GetByTipoEntradaDiarioIdAsync(int tipoEntradaDiarioId);
        Task<NumeracionEntradaDiario> GetPreferidaByTipoEntradaDiarioIdAsync(int tipoEntradaDiarioId);
        Task<NumeracionEntradaDiario> CreateAsync(NumeracionEntradaDiario numeracion);
        Task UpdateAsync(NumeracionEntradaDiario numeracion);
        Task DeleteAsync(int id);
        Task<bool> ExisteNombreAsync(string nombre, int tipoEntradaDiarioId);
        Task<bool> ExistePrefijoAsync(string prefijo);
        Task IncrementarNumeroActualAsync(int id);
        Task DesmarcarPreferidas(int tipoEntradaDiarioId);
    }
} 