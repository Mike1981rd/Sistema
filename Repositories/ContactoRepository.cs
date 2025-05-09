using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Models;
using SistemaContable.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaContable.Repositories
{
    public class ContactoRepository : IContactoRepository
    {
        private readonly ApplicationDbContext _context;

        public ContactoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Contacto>> GetAllAsync()
        {
            return await _context.Contactos
                .OrderBy(c => c.Nombre)
                .ToListAsync();
        }

        public async Task<Contacto> GetByIdAsync(int id)
        {
            return await _context.Contactos.FindAsync(id);
        }

        public async Task<Contacto> CreateAsync(Contacto contacto)
        {
            _context.Contactos.Add(contacto);
            await _context.SaveChangesAsync();
            return contacto;
        }

        public async Task UpdateAsync(Contacto contacto)
        {
            _context.Entry(contacto).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var contacto = await _context.Contactos.FindAsync(id);
            if (contacto != null)
            {
                _context.Contactos.Remove(contacto);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExisteIdentificacionAsync(string identificacion, int empresaId)
        {
            if (string.IsNullOrEmpty(identificacion))
            {
                return false;
            }
            
            return await _context.Contactos
                .AnyAsync(c => c.Identificacion == identificacion && c.EmpresaId == empresaId);
        }

        public async Task<IEnumerable<Contacto>> BuscarPorNombreAsync(string termino)
        {
            if (string.IsNullOrEmpty(termino))
            {
                return new List<Contacto>();
            }

            termino = termino.ToLower();
            return await _context.Contactos
                .Where(c => c.Nombre.ToLower().Contains(termino) || 
                           (c.Identificacion != null && c.Identificacion.ToLower().Contains(termino)))
                .Where(c => c.Activo)
                .OrderBy(c => c.Nombre)
                .Take(20)
                .ToListAsync();
        }

        public async Task<IEnumerable<Contacto>> GetClientesAsync(int empresaId)
        {
            return await _context.Contactos
                .Where(c => c.EmpresaId == empresaId)
                .Where(c => c.EsCliente)
                .Where(c => c.Activo)
                .OrderBy(c => c.Nombre)
                .ToListAsync();
        }

        public async Task<IEnumerable<Contacto>> GetProveedoresAsync(int empresaId)
        {
            return await _context.Contactos
                .Where(c => c.EmpresaId == empresaId)
                .Where(c => c.EsProveedor)
                .Where(c => c.Activo)
                .OrderBy(c => c.Nombre)
                .ToListAsync();
        }
    }
} 