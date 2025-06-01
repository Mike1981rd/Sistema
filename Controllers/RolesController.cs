using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Models;
using SistemaContable.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaContable.Controllers
{
    [Route("[controller]")]
    [Route("configuracion/roles")]
    public class RolesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmpresaService _empresaService;

        public RolesController(ApplicationDbContext context, IEmpresaService empresaService)
        {
            _context = context;
            _empresaService = empresaService;
        }

        // GET: configuracion/roles
        public async Task<IActionResult> Index()
        {
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            
            if (empresaId == 0)
            {
                TempData["ErrorMessage"] = "No se ha configurado una empresa activa.";
                return RedirectToAction("Index", "Home");
            }

            var roles = await _context.Roles
                .Include(r => r.Usuarios)
                .Where(r => r.EmpresaId == empresaId && r.Activo)
                .OrderBy(r => r.Prioridad)
                .ThenBy(r => r.Nombre)
                .ToListAsync();

            ViewBag.RolesDestacados = roles.Take(4).ToList();
            
            // Obtener todos los usuarios activos de la empresa
            var todosLosUsuarios = await _context.Usuarios
                .Include(u => u.Rol)
                .Where(u => u.EmpresaId == empresaId && u.Activo)
                .OrderBy(u => u.NombreCompleto)
                .ToListAsync();
                
            ViewBag.TodosLosUsuarios = todosLosUsuarios;
            
            return View(roles);
        }

        // GET: configuracion/roles/crear
        [HttpGet("crear")]
        public async Task<IActionResult> Create()
        {
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            
            if (empresaId == 0)
            {
                TempData["ErrorMessage"] = "No se ha configurado una empresa activa.";
                return RedirectToAction("Index");
            }

            ViewBag.PermisosEstructura = PermisosSistema.EstructuraPermisos;
            return View();
        }

        // POST: configuracion/roles/crear
        [HttpPost("crear")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Rol rol, string[] permisosSeleccionados)
        {
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            
            if (empresaId == 0)
            {
                TempData["ErrorMessage"] = "No se ha configurado una empresa activa.";
                return RedirectToAction("Index");
            }

            // Verificar si ya existe un rol con el mismo nombre
            var existeRol = await _context.Roles
                .AnyAsync(r => r.EmpresaId == empresaId && 
                              r.Nombre.ToLower() == rol.Nombre.ToLower() && 
                              r.Activo);

            if (existeRol)
            {
                ModelState.AddModelError("Nombre", "Ya existe un rol con este nombre.");
                ViewBag.PermisosEstructura = PermisosSistema.EstructuraPermisos;
                return View(rol);
            }

            if (ModelState.IsValid)
            {
                rol.EmpresaId = empresaId;
                rol.Permisos = permisosSeleccionados?.ToList() ?? new List<string>();
                rol.FechaCreacion = DateTime.UtcNow;
                rol.Activo = true;

                _context.Add(rol);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Rol creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.PermisosEstructura = PermisosSistema.EstructuraPermisos;
            return View(rol);
        }

        // GET: configuracion/roles/editar/5
        [HttpGet("editar/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            
            if (empresaId == 0)
            {
                TempData["ErrorMessage"] = "No se ha configurado una empresa activa.";
                return RedirectToAction("Index");
            }

            var rol = await _context.Roles
                .FirstOrDefaultAsync(r => r.Id == id && r.EmpresaId == empresaId && r.Activo);

            if (rol == null)
            {
                return NotFound();
            }

            ViewBag.PermisosEstructura = PermisosSistema.EstructuraPermisos;
            ViewBag.PermisosSeleccionados = rol.Permisos ?? new List<string>();
            
            return View(rol);
        }

        // POST: configuracion/roles/editar/5
        [HttpPost("editar/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Rol rol, string[] permisosSeleccionados)
        {
            if (id != rol.Id)
            {
                return NotFound();
            }

            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            
            if (empresaId == 0)
            {
                TempData["ErrorMessage"] = "No se ha configurado una empresa activa.";
                return RedirectToAction("Index");
            }

            // Verificar si existe otro rol con el mismo nombre
            var existeRol = await _context.Roles
                .AnyAsync(r => r.EmpresaId == empresaId && 
                              r.Nombre.ToLower() == rol.Nombre.ToLower() && 
                              r.Id != id && 
                              r.Activo);

            if (existeRol)
            {
                ModelState.AddModelError("Nombre", "Ya existe otro rol con este nombre.");
                ViewBag.PermisosEstructura = PermisosSistema.EstructuraPermisos;
                ViewBag.PermisosSeleccionados = permisosSeleccionados ?? new string[0];
                return View(rol);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var rolActual = await _context.Roles
                        .FirstOrDefaultAsync(r => r.Id == id && r.EmpresaId == empresaId);

                    if (rolActual == null)
                    {
                        return NotFound();
                    }

                    // Actualizar propiedades
                    rolActual.Nombre = rol.Nombre;
                    rolActual.Descripcion = rol.Descripcion;
                    rolActual.Prioridad = rol.Prioridad;
                    rolActual.Permisos = permisosSeleccionados?.ToList() ?? new List<string>();
                    rolActual.FechaModificacion = DateTime.UtcNow;

                    _context.Update(rolActual);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Rol actualizado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RolExists(rol.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewBag.PermisosEstructura = PermisosSistema.EstructuraPermisos;
            ViewBag.PermisosSeleccionados = permisosSeleccionados ?? new string[0];
            return View(rol);
        }

        // POST: configuracion/roles/eliminar/5
        [HttpPost("eliminar/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            
            if (empresaId == 0)
            {
                TempData["ErrorMessage"] = "No se ha configurado una empresa activa.";
                return RedirectToAction("Index");
            }

            var rol = await _context.Roles
                .FirstOrDefaultAsync(r => r.Id == id && r.EmpresaId == empresaId);

            if (rol == null)
            {
                return NotFound();
            }

            // Soft delete
            rol.Activo = false;
            rol.FechaModificacion = DateTime.UtcNow;

            _context.Update(rol);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Rol eliminado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        private bool RolExists(int id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
    }
}