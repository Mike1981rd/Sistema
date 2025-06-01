using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaContable.Services
{
    public class PermisosService : IPermisosService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmpresaService _empresaService;

        public PermisosService(ApplicationDbContext context, IEmpresaService empresaService)
        {
            _context = context;
            _empresaService = empresaService;
        }

        public async Task<bool> TienePermisoAsync(int usuarioId, string permiso)
        {
            try
            {
                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                
                if (empresaId == 0 || usuarioId == 0 || string.IsNullOrWhiteSpace(permiso))
                    return false;

                var usuario = await _context.Usuarios
                    .Include(u => u.Rol)
                    .FirstOrDefaultAsync(u => u.Id == usuarioId 
                                         && u.EmpresaId == empresaId 
                                         && u.Activo
                                         && u.Rol != null 
                                         && u.Rol.Activo);

                if (usuario?.Rol?.Permisos == null)
                    return false;

                return usuario.Rol.Permisos.Contains(permiso);
            }
            catch (Exception)
            {
                // Log error si es necesario
                return false;
            }
        }

        public async Task<List<string>> ObtenerPermisosUsuarioAsync(int usuarioId)
        {
            try
            {
                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                
                if (empresaId == 0 || usuarioId == 0)
                    return new List<string>();

                var usuario = await _context.Usuarios
                    .Include(u => u.Rol)
                    .FirstOrDefaultAsync(u => u.Id == usuarioId 
                                         && u.EmpresaId == empresaId 
                                         && u.Activo
                                         && u.Rol != null 
                                         && u.Rol.Activo);

                return usuario?.Rol?.Permisos ?? new List<string>();
            }
            catch (Exception)
            {
                // Log error si es necesario
                return new List<string>();
            }
        }

        public async Task<bool> TieneAccesoModuloAsync(int usuarioId, string modulo)
        {
            try
            {
                var permisos = await ObtenerPermisosUsuarioAsync(usuarioId);
                
                if (!permisos.Any())
                    return false;

                // Verificar si el usuario tiene algún permiso que comience con el nombre del módulo
                var moduloLower = modulo.ToLower();
                return permisos.Any(p => p.ToLower().StartsWith(moduloLower + "."));
            }
            catch (Exception)
            {
                // Log error si es necesario
                return false;
            }
        }

        public async Task<Dictionary<string, Dictionary<string, string>>> ObtenerPermisosDisponiblesAsync(int usuarioId)
        {
            try
            {
                var permisos = await ObtenerPermisosUsuarioAsync(usuarioId);
                var permisosDisponibles = new Dictionary<string, Dictionary<string, string>>();

                if (!permisos.Any())
                    return permisosDisponibles;

                // Filtrar la estructura completa de permisos según los permisos del usuario
                foreach (var categoria in PermisosSistema.EstructuraPermisos)
                {
                    var permisosCategoria = new Dictionary<string, string>();

                    foreach (var permiso in categoria.Value)
                    {
                        if (permisos.Contains(permiso.Key))
                        {
                            permisosCategoria[permiso.Key] = permiso.Value;
                        }
                    }

                    // Solo agregar la categoría si tiene al menos un permiso
                    if (permisosCategoria.Any())
                    {
                        permisosDisponibles[categoria.Key] = permisosCategoria;
                    }
                }

                return permisosDisponibles;
            }
            catch (Exception)
            {
                // Log error si es necesario
                return new Dictionary<string, Dictionary<string, string>>();
            }
        }

        /// <summary>
        /// Método helper para verificar permisos específicos del menú
        /// </summary>
        public async Task<bool> PuedeVerMenuAsync(int usuarioId, string seccionMenu)
        {
            switch (seccionMenu.ToLower())
            {
                case "dashboard":
                    return await TienePermisoAsync(usuarioId, "dashboard.ver");
                
                case "configuracion":
                    return await TieneAccesoModuloAsync(usuarioId, "configuracion");
                
                case "ventas":
                    return await TieneAccesoModuloAsync(usuarioId, "ventas");
                
                case "compras":
                    return await TieneAccesoModuloAsync(usuarioId, "compras");
                
                case "inventario":
                    return await TieneAccesoModuloAsync(usuarioId, "inventario");
                
                case "pos":
                case "punto_venta":
                    return await TieneAccesoModuloAsync(usuarioId, "pos");
                
                case "bancos":
                    return await TieneAccesoModuloAsync(usuarioId, "bancos");
                
                case "contabilidad":
                    return await TieneAccesoModuloAsync(usuarioId, "contabilidad");
                
                case "reportes":
                    return await TieneAccesoModuloAsync(usuarioId, "reportes");
                
                // Permisos específicos
                case "impuestos":
                    return await TienePermisoAsync(usuarioId, "configuracion.impuestos");
                
                case "categorias":
                    return await TienePermisoAsync(usuarioId, "configuracion.categorias");
                
                case "familias":
                    return await TienePermisoAsync(usuarioId, "configuracion.familias");
                
                case "roles":
                    return await TienePermisoAsync(usuarioId, "configuracion.roles");
                
                case "usuarios":
                    return await TienePermisoAsync(usuarioId, "configuracion.usuarios");
                
                case "clientes":
                    return await TienePermisoAsync(usuarioId, "ventas.clientes");
                
                case "proveedores":
                    return await TienePermisoAsync(usuarioId, "compras.proveedores");
                
                case "almacenes":
                    return await TienePermisoAsync(usuarioId, "inventario.almacenes");
                
                case "items":
                    return await TienePermisoAsync(usuarioId, "inventario.items");
                
                case "productos":
                    return await TienePermisoAsync(usuarioId, "pos.productos");
                
                case "impresoras":
                    return await TienePermisoAsync(usuarioId, "pos.impresoras");
                
                default:
                    return false;
            }
        }
    }
}