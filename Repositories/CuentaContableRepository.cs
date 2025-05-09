using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Models;
using SistemaContable.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Query;

namespace SistemaContable.Repositories
{
    public class CuentaContableRepository : ICuentaContableRepository
    {
        private readonly ApplicationDbContext _context;

        public CuentaContableRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CuentaContable>> GetAllAsync()
        {
            return await _context.CuentasContables
                .OrderBy(c => c.Codigo)
                .ToListAsync();
        }

        public async Task<CuentaContable> GetByIdAsync(int id)
        {
            return await _context.CuentasContables.FindAsync(id);
        }

        public async Task<CuentaContable> CreateAsync(CuentaContable cuentaContable)
        {
            _context.CuentasContables.Add(cuentaContable);
            await _context.SaveChangesAsync();
            return cuentaContable;
        }

        public async Task UpdateAsync(CuentaContable cuentaContable)
        {
            _context.Entry(cuentaContable).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var cuentaContable = await _context.CuentasContables.FindAsync(id);
            if (cuentaContable != null)
            {
                _context.CuentasContables.Remove(cuentaContable);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExisteCodigoAsync(string codigo, int empresaId)
        {
            return await _context.CuentasContables.AnyAsync(c => c.Codigo == codigo && c.EmpresaId == empresaId);
        }

        public async Task<IEnumerable<CuentaContable>> BuscarPorNombreOCodigoAsync(string termino)
        {
            // Log para depuración
            Console.WriteLine($"Búsqueda de cuentas contables: '{termino}'");
            
            try {
                // Simplificar la consulta eliminando filtros restrictivos
                // Normalizar el término de búsqueda
                termino = termino?.Trim().ToLower() ?? "";
                
                // Imprimir la cantidad total de cuentas en la base de datos para diagnóstico
                var totalCuentas = await _context.CuentasContables.CountAsync();
                Console.WriteLine($"Total cuentas en BD: {totalCuentas}");
                
                // Consulta simplificada sin filtros restrictivos
                var resultados = await _context.CuentasContables
                    .Where(c => EF.Functions.Like(c.Codigo.ToLower(), $"%{termino}%") || 
                                EF.Functions.Like(c.Nombre.ToLower(), $"%{termino}%"))
                    .OrderBy(c => c.Codigo)
                    .Take(20)
                    .ToListAsync();
                
                Console.WriteLine($"Búsqueda simplificada con término '{termino}' devolvió {resultados.Count()} cuentas contables");
                foreach (var cuenta in resultados.Take(5))
                {
                    Console.WriteLine($"- {cuenta.Id}: {cuenta.Codigo} - {cuenta.Nombre} (Tipo: {cuenta.TipoCuenta}, Uso: {cuenta.UsoCuenta})");
                }
                
                return resultados;
            }
            catch (Exception ex) {
                Console.WriteLine($"Error en la consulta SQL: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<CuentaContable>> GetCuentasMovimientoAsync(int empresaId)
        {
            return await _context.CuentasContables
                .Where(c => c.EmpresaId == empresaId)
                .Where(c => c.TipoCuenta == "Movimiento")
                .Where(c => c.Activo)
                .OrderBy(c => c.Codigo)
                .ToListAsync();
        }
    }
} 