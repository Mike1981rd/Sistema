using SistemaContable.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaContable.Services.Interfaces
{
    public interface IEntradaDiarioService
    {
        Task<IEnumerable<EntradaDiario>> GetAllEntradasDiarioAsync();
        Task<EntradaDiario> GetEntradaDiarioByIdAsync(int id);
        Task<EntradaDiario> CreateEntradaDiarioAsync(EntradaDiario entradaDiario);
        Task UpdateEntradaDiarioAsync(EntradaDiario entradaDiario);
        Task DeleteEntradaDiarioAsync(int id);
        Task<bool> ValidarCodigoUnicoAsync(string codigo);
        Task<IEnumerable<EntradaDiario>> BuscarPorPeriodoAsync(DateTime fechaInicio, DateTime fechaFin);
    }
} 