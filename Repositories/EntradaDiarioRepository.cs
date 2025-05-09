using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Models;
using SistemaContable.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaContable.Repositories
{
    public class EntradaDiarioRepository : IEntradaDiarioRepository
    {
        private readonly ApplicationDbContext _context;

        public EntradaDiarioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EntradaDiario>> GetAllAsync()
        {
            return await _context.EntradasDiario
                .Include(e => e.TipoEntrada)
                .Include(e => e.Numeracion)
                .Include(e => e.Movimientos)
                    .ThenInclude(m => m.CuentaContable)
                .OrderByDescending(e => e.Fecha)
                .ToListAsync();
        }

        public async Task<EntradaDiario> GetByIdAsync(int id)
        {
            return await _context.EntradasDiario
                .Include(e => e.TipoEntrada)
                .Include(e => e.Numeracion)
                .Include(e => e.Movimientos)
                    .ThenInclude(m => m.CuentaContable)
                .Include(e => e.Movimientos)
                    .ThenInclude(m => m.Contacto)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<EntradaDiario> CreateAsync(EntradaDiario entradaDiario)
        {
            // Asegurarnos que cada movimiento tenga referencia a esta entrada de diario
            if (entradaDiario.Movimientos != null)
            {
                foreach (var movimiento in entradaDiario.Movimientos)
                {
                    movimiento.EntradaDiarioId = entradaDiario.Id;
                }
            }

            _context.EntradasDiario.Add(entradaDiario);
            await _context.SaveChangesAsync();
            return entradaDiario;
        }

        public async Task UpdateAsync(EntradaDiario entradaDiario)
        {
            // Primero, obtenemos los movimientos actuales en la base de datos
            var movimientosActuales = await _context.MovimientosContables
                .Where(m => m.EntradaDiarioId == entradaDiario.Id)
                .ToListAsync();

            // Eliminamos los movimientos que ya no existen en la entrada de diario actualizada
            if (entradaDiario.Movimientos != null)
            {
                var movimientosIds = entradaDiario.Movimientos
                    .Where(m => m.Id > 0)
                    .Select(m => m.Id);

                var movimientosAEliminar = movimientosActuales
                    .Where(m => !movimientosIds.Contains(m.Id))
                    .ToList();

                foreach (var movimiento in movimientosAEliminar)
                {
                    _context.MovimientosContables.Remove(movimiento);
                }

                // Actualizamos los movimientos existentes y agregamos los nuevos
                foreach (var movimiento in entradaDiario.Movimientos)
                {
                    if (movimiento.Id > 0)
                    {
                        // Actualizar movimiento existente
                        _context.Entry(movimiento).State = EntityState.Modified;
                    }
                    else
                    {
                        // Agregar nuevo movimiento
                        movimiento.EntradaDiarioId = entradaDiario.Id;
                        _context.MovimientosContables.Add(movimiento);
                    }
                }
            }
            else
            {
                // Si no hay movimientos, eliminamos todos los existentes
                foreach (var movimiento in movimientosActuales)
                {
                    _context.MovimientosContables.Remove(movimiento);
                }
            }

            // Actualizar la entrada principal
            _context.Entry(entradaDiario).State = EntityState.Modified;
            
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            // Primero, eliminamos todos los movimientos asociados
            var movimientos = await _context.MovimientosContables
                .Where(m => m.EntradaDiarioId == id)
                .ToListAsync();

            foreach (var movimiento in movimientos)
            {
                _context.MovimientosContables.Remove(movimiento);
            }

            // Luego, eliminamos la entrada de diario
            var entradaDiario = await _context.EntradasDiario.FindAsync(id);
            if (entradaDiario != null)
            {
                _context.EntradasDiario.Remove(entradaDiario);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExisteCodigoAsync(string codigo)
        {
            return await _context.EntradasDiario.AnyAsync(e => e.Codigo == codigo);
        }

        public async Task<IEnumerable<EntradaDiario>> BuscarPorFechasAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _context.EntradasDiario
                .Include(e => e.TipoEntrada)
                .Include(e => e.Numeracion)
                .Include(e => e.Movimientos)
                    .ThenInclude(m => m.CuentaContable)
                .Where(e => e.Fecha >= fechaInicio && e.Fecha <= fechaFin)
                .OrderByDescending(e => e.Fecha)
                .ToListAsync();
        }
    }
} 