using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Models;
using SistemaContable.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            if (string.IsNullOrEmpty(termino))
            {
                return new List<CuentaContable>();
            }

            termino = termino.ToLower();
            return await _context.CuentasContables
                .Where(c => c.Nombre.ToLower().Contains(termino) || c.Codigo.ToLower().Contains(termino))
                .Where(c => c.TipoCuenta == "Movimiento") // Solo cuentas de movimiento
                .Where(c => c.Activo)
                .OrderBy(c => c.Codigo)
                .Take(20)
                .ToListAsync();
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