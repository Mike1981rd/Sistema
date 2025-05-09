using SistemaContable.Models;
using SistemaContable.Repositories.Interfaces;
using SistemaContable.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaContable.Services
{
    public class EntradaDiarioService : IEntradaDiarioService
    {
        private readonly IEntradaDiarioRepository _entradaDiarioRepository;

        public EntradaDiarioService(IEntradaDiarioRepository entradaDiarioRepository)
        {
            _entradaDiarioRepository = entradaDiarioRepository;
        }

        public async Task<IEnumerable<EntradaDiario>> GetAllEntradasDiarioAsync()
        {
            return await _entradaDiarioRepository.GetAllAsync();
        }

        public async Task<EntradaDiario> GetEntradaDiarioByIdAsync(int id)
        {
            return await _entradaDiarioRepository.GetByIdAsync(id);
        }

        public async Task<EntradaDiario> CreateEntradaDiarioAsync(EntradaDiario entradaDiario)
        {
            // Validar que el código sea único
            if (await _entradaDiarioRepository.ExisteCodigoAsync(entradaDiario.Codigo))
            {
                throw new InvalidOperationException($"Ya existe una entrada de diario con el código {entradaDiario.Codigo}");
            }

            // Establecer fechas de creación y modificación
            entradaDiario.FechaCreacion = DateTime.UtcNow;
            
            // Crear la entrada de diario
            return await _entradaDiarioRepository.CreateAsync(entradaDiario);
        }

        public async Task UpdateEntradaDiarioAsync(EntradaDiario entradaDiario)
        {
            var existingEntrada = await _entradaDiarioRepository.GetByIdAsync(entradaDiario.Id);
            if (existingEntrada == null)
            {
                throw new InvalidOperationException($"No se encontró la entrada de diario con ID {entradaDiario.Id}");
            }

            // Validar que el código sea único si ha cambiado
            if (existingEntrada.Codigo != entradaDiario.Codigo && 
                await _entradaDiarioRepository.ExisteCodigoAsync(entradaDiario.Codigo))
            {
                throw new InvalidOperationException($"Ya existe una entrada de diario con el código {entradaDiario.Codigo}");
            }

            // Actualizar la fecha de modificación
            entradaDiario.FechaModificacion = DateTime.UtcNow;
            
            // Mantener la fecha de creación original
            entradaDiario.FechaCreacion = existingEntrada.FechaCreacion;
            
            await _entradaDiarioRepository.UpdateAsync(entradaDiario);
        }

        public async Task DeleteEntradaDiarioAsync(int id)
        {
            var existingEntrada = await _entradaDiarioRepository.GetByIdAsync(id);
            if (existingEntrada == null)
            {
                throw new InvalidOperationException($"No se encontró la entrada de diario con ID {id}");
            }

            await _entradaDiarioRepository.DeleteAsync(id);
        }

        public async Task<bool> ValidarCodigoUnicoAsync(string codigo)
        {
            return !(await _entradaDiarioRepository.ExisteCodigoAsync(codigo));
        }

        public async Task<IEnumerable<EntradaDiario>> BuscarPorPeriodoAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _entradaDiarioRepository.BuscarPorFechasAsync(fechaInicio, fechaFin);
        }
    }
} 