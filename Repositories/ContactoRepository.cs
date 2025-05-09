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
            // Log para depuración
            Console.WriteLine($"Búsqueda de contactos (clientes/proveedores): '{termino}'");
            
            try {
                // Normalizar el término de búsqueda
                termino = termino?.Trim().ToLower() ?? "";
                
                // Consultar la tabla de Clientes en lugar de Contactos
                var clientesQuery = await _context.Clientes
                    .Where(c => EF.Functions.Like(c.NombreRazonSocial.ToLower(), $"%{termino}%") || 
                               (c.NumeroIdentificacion != null && 
                                EF.Functions.Like(c.NumeroIdentificacion.ToLower(), $"%{termino}%")))
                    .OrderBy(c => c.NombreRazonSocial)
                    .Take(20)
                    .ToListAsync();
                
                Console.WriteLine($"Búsqueda en Clientes con término '{termino}' devolvió {clientesQuery.Count} resultados");
                
                // Convertir los Clientes a Contactos para mantener la interfaz compatible
                var resultados = clientesQuery.Select(c => new Contacto { 
                    Id = c.Id,
                    Nombre = c.NombreRazonSocial,
                    Identificacion = c.NumeroIdentificacion,
                    EsCliente = c.EsCliente,
                    EsProveedor = c.EsProveedor,
                    Activo = true // Asumimos que todos los clientes devueltos están activos
                }).ToList();
                
                foreach (var contacto in resultados.Take(5))
                {
                    string tipo = contacto.EsCliente ? (contacto.EsProveedor ? "Cliente/Proveedor" : "Cliente") : "Proveedor";
                    Console.WriteLine($"- {contacto.Id}: {contacto.Nombre} ({tipo}) - Identificación: {contacto.Identificacion}");
                }
                
                return resultados;
            }
            catch (Exception ex) {
                Console.WriteLine($"Error en la consulta de contactos/clientes: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                throw;
            }
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