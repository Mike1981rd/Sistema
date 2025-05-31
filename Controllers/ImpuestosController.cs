using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Models;
using SistemaContable.Services;
using System.Threading.Tasks;
using System.Linq;
using System.Text.Json;
using System;
using System.Collections.Generic;

namespace SistemaContable.Controllers
{
    public class ImpuestosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmpresaService _empresaService;
        private readonly IImpuestoService _impuestoService;

        public ImpuestosController(
            ApplicationDbContext context,
            IEmpresaService empresaService,
            IImpuestoService impuestoService)
        {
            _context = context;
            _empresaService = empresaService;
            _impuestoService = impuestoService;
        }

        // GET: Impuestos
        public async Task<IActionResult> Index(string tab)
        {
            // Obtener la empresa actual
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            if (empresaId <= 0)
            {
                return RedirectToAction("Index", "Empresas");
            }
            
            // Determinar si mostrar activos o inactivos
            bool activos = string.IsNullOrEmpty(tab) || tab == "Activos";
            
            // Pasar el estado de filtro a la vista
            ViewBag.Tab = activos ? "Activos" : "Inactivos";

            return View(await _context.Impuestos
                .Include(i => i.CuentaContableVentas)
                .Include(i => i.CuentaContableCompras)
                .Where(i => i.EmpresaId == empresaId && i.Estado == activos)
                .OrderBy(i => i.Nombre)
                .ToListAsync());
        }

        // GET: Impuestos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var impuesto = await _context.Impuestos
                .Include(i => i.CuentaContableVentas)
                .Include(i => i.CuentaContableCompras)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (impuesto == null)
            {
                return NotFound();
            }

            return View(impuesto);
        }

        // GET: Impuestos/Create
        public async Task<IActionResult> Create()
        {
            // Obtener la empresa actual
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            if (empresaId <= 0)
            {
                return RedirectToAction("Index", "Empresas");
            }
            
            await CargarCuentasContables(empresaId);
            return View();
        }

        // POST: Impuestos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Tipo,Porcentaje,Descripcion,EsAcreditable,CuentaContableVentasId,CuentaContableComprasId,Estado")] Impuesto impuesto)
        {
            // Obtener la empresa actual
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            if (empresaId <= 0)
            {
                return RedirectToAction("Index", "Empresas");
            }
            
            impuesto.EmpresaId = empresaId;
            
            if (ModelState.IsValid)
            {
                _context.Add(impuesto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            await CargarCuentasContables(empresaId);
            return View(impuesto);
        }

        // GET: Impuestos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var impuesto = await _context.Impuestos.FindAsync(id);
            if (impuesto == null)
            {
                return NotFound();
            }
            
            await CargarCuentasContables(impuesto.EmpresaId);
            return View(impuesto);
        }

        // POST: Impuestos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Tipo,Porcentaje,Descripcion,EsAcreditable,CuentaContableVentasId,CuentaContableComprasId,FechaCreacion,EstaEnUso,EmpresaId,Estado")] Impuesto impuesto)
        {
            if (id != impuesto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    impuesto.FechaModificacion = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
                    _context.Update(impuesto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImpuestoExists(impuesto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            
            await CargarCuentasContables(impuesto.EmpresaId);
            return View(impuesto);
        }

        // GET: Impuestos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var impuesto = await _context.Impuestos
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (impuesto == null)
            {
                return NotFound();
            }

            return View(impuesto);
        }

        // POST: Impuestos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Verificar si el impuesto puede ser eliminado
            bool puedeEliminar = await _impuestoService.PuedeEliminarImpuesto(id);
            
            if (puedeEliminar)
            {
                var impuesto = await _context.Impuestos.FindAsync(id);
                if (impuesto != null)
                {
                    _context.Impuestos.Remove(impuesto);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                TempData["Error"] = "El impuesto no puede ser eliminado porque está en uso o tiene documentos relacionados.";
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool ImpuestoExists(int id)
        {
            return _context.Impuestos.Any(e => e.Id == id);
        }
        
        // GET: Impuestos/ObtenerTodos
        [HttpGet]
        public async Task<JsonResult> ObtenerTodos()
        {
            try
            {
                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                if (empresaId <= 0)
                {
                    return Json(new { error = "Empresa no seleccionada" });
                }

                var impuestos = await _context.Impuestos
                    .Where(i => i.EmpresaId == empresaId && i.Estado)
                    .OrderBy(i => i.Nombre)
                    .Select(i => new {
                        id = i.Id,
                        nombre = i.Nombre,
                        porcentaje = i.Porcentaje,
                        tipo = i.Tipo
                    })
                    .ToListAsync();

                return Json(impuestos);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ObtenerTodos: {ex.Message}");
                return Json(new { error = ex.Message });
            }
        }
        
        // GET: Impuestos/CreatePartial
        [HttpGet]
        public async Task<IActionResult> CreatePartial()
        {
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            if (empresaId <= 0)
            {
                return Json(new { success = false, message = "Empresa no seleccionada" });
            }

            await CargarCuentasContables(empresaId);
            return PartialView("_CreatePartial");
        }
        
        // POST: Impuestos/CreatePartial
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> CreatePartial([Bind("Id,Nombre,Tipo,Porcentaje,Descripcion,EsAcreditable,CuentaContableVentasId,CuentaContableComprasId,Estado")] Impuesto impuesto)
        {
            try
            {
                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                if (empresaId <= 0)
                {
                    return Json(new { success = false, message = "Empresa no seleccionada" });
                }

                impuesto.EmpresaId = empresaId;

                if (ModelState.IsValid)
                {
                    _context.Add(impuesto);
                    await _context.SaveChangesAsync();
                    
                    return Json(new { 
                        success = true, 
                        message = "Impuesto creado con éxito", 
                        impuesto = new { 
                            id = impuesto.Id, 
                            nombre = impuesto.Nombre,
                            porcentaje = impuesto.Porcentaje
                        } 
                    });
                }
                else
                {
                    return Json(new { 
                        success = false, 
                        message = "Error al crear impuesto", 
                        errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() 
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }
        
        // GET: Impuestos/EditPartial/5
        [HttpGet]
        public async Task<IActionResult> EditPartial(int id)
        {
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            if (empresaId <= 0)
            {
                return Json(new { success = false, message = "Empresa no seleccionada" });
            }

            var impuesto = await _context.Impuestos
                .FirstOrDefaultAsync(i => i.Id == id && i.EmpresaId == empresaId);
                
            if (impuesto == null)
            {
                return Json(new { success = false, message = "Impuesto no encontrado" });
            }

            await CargarCuentasContables(impuesto.EmpresaId);
            return PartialView("_EditPartial", impuesto);
        }
        
        // POST: Impuestos/EditPartial/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> EditPartial(int id, [Bind("Id,Nombre,Tipo,Porcentaje,Descripcion,EsAcreditable,CuentaContableVentasId,CuentaContableComprasId,FechaCreacion,EstaEnUso,EmpresaId,Estado")] Impuesto impuesto)
        {
            if (id != impuesto.Id)
            {
                return Json(new { success = false, message = "ID de impuesto no válido" });
            }

            try
            {
                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                if (empresaId <= 0)
                {
                    return Json(new { success = false, message = "Empresa no seleccionada" });
                }

                var existingImpuesto = await _context.Impuestos
                    .FirstOrDefaultAsync(i => i.Id == id && i.EmpresaId == empresaId);
                    
                if (existingImpuesto == null)
                {
                    return Json(new { success = false, message = "Impuesto no encontrado" });
                }

                if (ModelState.IsValid)
                {
                    existingImpuesto.Nombre = impuesto.Nombre;
                    existingImpuesto.Tipo = impuesto.Tipo;
                    existingImpuesto.Porcentaje = impuesto.Porcentaje;
                    existingImpuesto.Descripcion = impuesto.Descripcion;
                    existingImpuesto.EsAcreditable = impuesto.EsAcreditable;
                    existingImpuesto.CuentaContableVentasId = impuesto.CuentaContableVentasId;
                    existingImpuesto.CuentaContableComprasId = impuesto.CuentaContableComprasId;
                    existingImpuesto.Estado = impuesto.Estado;
                    existingImpuesto.FechaModificacion = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
                    
                    _context.Update(existingImpuesto);
                    await _context.SaveChangesAsync();
                    
                    return Json(new { 
                        success = true, 
                        message = "Impuesto actualizado con éxito", 
                        impuesto = new { 
                            id = existingImpuesto.Id, 
                            nombre = existingImpuesto.Nombre,
                            porcentaje = existingImpuesto.Porcentaje
                        } 
                    });
                }
                else
                {
                    return Json(new { 
                        success = false, 
                        message = "Error al actualizar impuesto", 
                        errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() 
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }
        
        private async Task CargarCuentasContables(int empresaId)
        {
            // Obtener todas las cuentas contables para la empresa actual
            var cuentasContables = await _context.CuentasContables
                .Where(c => c.EmpresaId == empresaId && c.Estado)
                .OrderBy(c => c.Codigo)
                .ToListAsync();
            
            // Guardamos la lista completa para su uso en el select2
            ViewBag.CuentasContablesJson = JsonSerializer.Serialize(
                cuentasContables.Select(c => new { 
                    id = c.Id, 
                    text = $"{c.Codigo} - {c.Nombre}",
                    codigo = c.Codigo,
                    nombre = c.Nombre
                }));
            
            // Mantenemos el SelectList para compatibilidad con la vista actual
            ViewBag.CuentasContables = new SelectList(
                cuentasContables,
                "Id",
                "CodigoNombre",
                null
            );
        }

        // GET: Impuestos/Buscar
        [HttpGet]
        public async Task<JsonResult> Buscar(string term, bool exactId = false)
        {
            try
            {
                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                if (empresaId <= 0)
                {
                    return Json(new { results = new List<object>() });
                }

                IQueryable<Impuesto> query = _context.Impuestos
                    .Where(i => i.EmpresaId == empresaId && i.Estado);

                // Si exactId es true, buscar por ID exacto
                if (exactId && !string.IsNullOrEmpty(term) && int.TryParse(term, out int impuestoId))
                {
                    query = query.Where(i => i.Id == impuestoId);
                }
                else if (!string.IsNullOrEmpty(term))
                {
                    query = query.Where(i => i.Nombre.Contains(term));
                }

                var impuestos = await query
                    .OrderBy(i => i.Nombre)
                    .Select(i => new { 
                        id = i.Id, 
                        text = $"{i.Nombre} {(i.Porcentaje.HasValue ? i.Porcentaje.Value.ToString() + "%" : "")}"
                    })
                    .ToListAsync();

                return Json(new { results = impuestos });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Buscar: {ex.Message}");
                return Json(new { results = new List<object>() });
            }
        }

        // POST: Impuestos/ToggleEstado
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleEstado(int id)
        {
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            if (empresaId <= 0)
            {
                return RedirectToAction("Index", "Empresas");
            }

            var impuesto = await _context.Impuestos
                .FirstOrDefaultAsync(i => i.Id == id && i.EmpresaId == empresaId);
                
            if (impuesto != null)
            {
                impuesto.Estado = !impuesto.Estado;
                impuesto.FechaModificacion = DateTime.UtcNow;
                
                _context.Update(impuesto);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = $"El impuesto '{impuesto.Nombre}' ha sido {(impuesto.Estado ? "activado" : "desactivado")} correctamente";
            }
            else
            {
                TempData["ErrorMessage"] = "No se pudo encontrar el impuesto";
            }
            
            return RedirectToAction(nameof(Index), new { tab = impuesto?.Estado == true ? "Activos" : "Inactivos" });
        }
    }
} 