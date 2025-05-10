using SistemaContable.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaContable.Repositories.Interfaces
{
    public interface IEntradaDiarioRepository
    {
        Task<IEnumerable<EntradaDiario>> GetAllAsync();
        Task<EntradaDiario> GetByIdAsync(int id);
        Task<EntradaDiario> CreateAsync(EntradaDiario entradaDiario);
        Task UpdateAsync(EntradaDiario entradaDiario);
        Task DeleteAsync(int id);
        Task<bool> ExisteCodigoAsync(string codigo);
        Task<IEnumerable<EntradaDiario>> BuscarPorFechasAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<EntradaDiario> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<EntradaDiario>> GetAllWithDetailsAsync();
    }
} 