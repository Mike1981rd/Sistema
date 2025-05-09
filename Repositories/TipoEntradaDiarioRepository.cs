using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Models;
using SistemaContable.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaContable.Repositories
{
    public class TipoEntradaDiarioRepository : ITipoEntradaDiarioRepository
    {
        private readonly ApplicationDbContext _context;

        public TipoEntradaDiarioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TipoEntradaDiario>> GetAllAsync()
        {
            return await _context.TiposEntradaDiario
                .Include(t => t.Numeraciones)
                .ToListAsync();
        }

        public async Task<TipoEntradaDiario> GetByIdAsync(int id)
        {
            return await _context.TiposEntradaDiario
                .Include(t => t.Numeraciones)
                .Include(t => t.EntradasDiario)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<TipoEntradaDiario> CreateAsync(TipoEntradaDiario tipoEntrada)
        {
            _context.TiposEntradaDiario.Add(tipoEntrada);
            await _context.SaveChangesAsync();
            return tipoEntrada;
        }

        public async Task UpdateAsync(TipoEntradaDiario tipoEntrada)
        {
            _context.Entry(tipoEntrada).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var tipoEntrada = await _context.TiposEntradaDiario.FindAsync(id);
            if (tipoEntrada != null)
            {
                _context.TiposEntradaDiario.Remove(tipoEntrada);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExisteCodigoAsync(string codigo)
        {
            return await _context.TiposEntradaDiario.AnyAsync(t => t.Codigo == codigo);
        }
    }
} 