using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Models;
using SistemaContable.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaContable.Repositories
{
    public class NumeracionEntradaDiarioRepository : INumeracionEntradaDiarioRepository
    {
        private readonly ApplicationDbContext _context;

        public NumeracionEntradaDiarioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NumeracionEntradaDiario>> GetAllAsync()
        {
            return await _context.NumeracionesEntradaDiario
                .Include(n => n.TipoEntradaDiario)
                .ToListAsync();
        }

        public async Task<NumeracionEntradaDiario> GetByIdAsync(int id)
        {
            return await _context.NumeracionesEntradaDiario
                .Include(n => n.TipoEntradaDiario)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<IEnumerable<NumeracionEntradaDiario>> GetByTipoEntradaDiarioIdAsync(int tipoEntradaDiarioId)
        {
            return await _context.NumeracionesEntradaDiario
                .Where(n => n.TipoEntradaDiarioId == tipoEntradaDiarioId)
                .ToListAsync();
        }

        public async Task<NumeracionEntradaDiario> GetPreferidaByTipoEntradaDiarioIdAsync(int tipoEntradaDiarioId)
        {
            return await _context.NumeracionesEntradaDiario
                .FirstOrDefaultAsync(n => n.TipoEntradaDiarioId == tipoEntradaDiarioId && n.EsPreferida);
        }

        public async Task<NumeracionEntradaDiario> CreateAsync(NumeracionEntradaDiario numeracion)
        {
            _context.NumeracionesEntradaDiario.Add(numeracion);
            await _context.SaveChangesAsync();
            return numeracion;
        }

        public async Task UpdateAsync(NumeracionEntradaDiario numeracion)
        {
            _context.Entry(numeracion).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var numeracion = await _context.NumeracionesEntradaDiario.FindAsync(id);
            if (numeracion != null)
            {
                _context.NumeracionesEntradaDiario.Remove(numeracion);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExisteNombreAsync(string nombre, int tipoEntradaDiarioId)
        {
            return await _context.NumeracionesEntradaDiario
                .AnyAsync(n => n.Nombre == nombre && n.TipoEntradaDiarioId == tipoEntradaDiarioId);
        }

        public async Task<bool> ExistePrefijoAsync(string prefijo)
        {
            return await _context.NumeracionesEntradaDiario.AnyAsync(n => n.Prefijo == prefijo);
        }

        public async Task IncrementarNumeroActualAsync(int id)
        {
            var numeracion = await _context.NumeracionesEntradaDiario.FindAsync(id);
            if (numeracion != null)
            {
                numeracion.NumeroActual += 1;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DesmarcarPreferidas(int tipoEntradaDiarioId)
        {
            var numeracionesPreferidas = await _context.NumeracionesEntradaDiario
                .Where(n => n.TipoEntradaDiarioId == tipoEntradaDiarioId && n.EsPreferida)
                .ToListAsync();
                
            foreach (var numeracion in numeracionesPreferidas)
            {
                numeracion.EsPreferida = false;
            }
            
            await _context.SaveChangesAsync();
        }
    }
} 