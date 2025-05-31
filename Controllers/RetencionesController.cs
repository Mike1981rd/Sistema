using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Models;

namespace SistemaContable.Controllers
{
    public class RetencionesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RetencionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /configuracion/retenciones y Retenciones/Index
        [Route("/configuracion/retenciones")]
        public async Task<IActionResult> Index(string tab)
        {
            bool activos = string.IsNullOrEmpty(tab) || tab == "Activos";
            ViewBag.Tab = activos ? "Activos" : "Inactivos";
            
            var retenciones = await _context.Retenciones
                .Where(r => r.Activo == activos)
                .OrderBy(r => r.Nombre)
                .ToListAsync();
            return View(retenciones);
        }

        // GET: Retenciones/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Retenciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Retencion retencion)
        {
            if (ModelState.IsValid)
            {
                retencion.Activo = true; // Por defecto activo
                retencion.FechaCreacion = DateTime.UtcNow;
                retencion.FechaModificacion = DateTime.UtcNow;
                _context.Add(retencion);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Retención creada correctamente.";
                return RedirectToAction(nameof(Index), new { tab = "Activos" });
            }
            return View(retencion);
        }

        // GET: Retenciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var retencion = await _context.Retenciones.FindAsync(id);
            if (retencion == null)
            {
                return NotFound();
            }
            return View(retencion);
        }

        // POST: Retenciones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Retencion retencion)
        {
            if (id != retencion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    retencion.FechaModificacion = DateTime.UtcNow;
                    _context.Update(retencion);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Retención actualizada correctamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RetencionExists(retencion.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { tab = retencion.Activo ? "Activos" : "Inactivos" });
            }
            return View(retencion);
        }

        // GET: Retenciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var retencion = await _context.Retenciones
                .FirstOrDefaultAsync(m => m.Id == id);
            if (retencion == null)
            {
                return NotFound();
            }

            return View(retencion);
        }

        // POST: Retenciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var retencion = await _context.Retenciones.FindAsync(id);
            if (retencion != null)
            {
                _context.Retenciones.Remove(retencion);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Retención eliminada correctamente.";
            }
            
            return RedirectToAction(nameof(Index));
        }

        // POST: Retenciones/ToggleEstado/5
        [HttpPost]
        public async Task<IActionResult> ToggleEstado(int id)
        {
            try
            {
                var retencion = await _context.Retenciones.FirstOrDefaultAsync(r => r.Id == id);

                if (retencion == null)
                {
                    TempData["ErrorMessage"] = "No se encontró la retención especificada";
                    return RedirectToAction(nameof(Index));
                }

                var estadoAnterior = retencion.Activo;
                retencion.Activo = !retencion.Activo;
                retencion.FechaModificacion = DateTime.UtcNow;

                _context.Update(retencion);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = $"La retención '{retencion.Nombre}' ha sido {(retencion.Activo ? "activada" : "desactivada")} correctamente";
                
                return RedirectToAction(nameof(Index), new { tab = retencion.Activo ? "Activos" : "Inactivos" });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cambiar el estado: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool RetencionExists(int id)
        {
            return _context.Retenciones.Any(e => e.Id == id);
        }
        
        // GET: /api/obtener-cuentas-contables
        [HttpGet]
        [Route("/api/obtener-cuentas-contables")]
        public async Task<IActionResult> ObtenerCuentasContables(string termino)
        {
            if (string.IsNullOrWhiteSpace(termino))
            {
                return Json(new { results = new object[] { } });
            }
            
            var cuentas = await _context.CuentasContables
                .Where(c => c.Nombre.ToLower().Contains(termino.ToLower()) || c.Codigo.ToLower().Contains(termino.ToLower()))
                .OrderBy(c => c.Codigo)
                .Take(10)
                .Select(c => new
                {
                    id = c.Id,
                    text = c.Codigo + " - " + c.Nombre
                })
                .ToListAsync();

            return Json(new { results = cuentas });
        }
        
        // GET: /api/obtener-detalles-cuentas-contables
        [HttpGet]
        [Route("/api/obtener-detalles-cuentas-contables")]
        public IActionResult ObtenerDetallesCuentasContables(string? cuentaVentas, string? cuentaCompras, string? cuentaRetencionesAsumidas)
        {
            var result = new
            {
                cuentaVentas = GetCuentaDetails(cuentaVentas),
                cuentaCompras = GetCuentaDetails(cuentaCompras),
                cuentaRetencionesAsumidas = GetCuentaDetails(cuentaRetencionesAsumidas)
            };
            
            return Json(result);
        }
        
        private object? GetCuentaDetails(string? cuentaId)
        {
            if (string.IsNullOrEmpty(cuentaId))
                return null;
                
            if (int.TryParse(cuentaId, out int id))
            {
                var cuenta = _context.CuentasContables
                    .Where(c => c.Id == id)
                    .Select(c => new { id = c.Id, text = c.Codigo + " - " + c.Nombre })
                    .FirstOrDefault();
                return cuenta;
            }
            else
            {
                // Si no es un ID numérico, podría ser un código de cuenta
                var cuenta = _context.CuentasContables
                    .Where(c => c.Codigo.ToLower() == cuentaId.ToLower())
                    .Select(c => new { id = c.Id, text = c.Codigo + " - " + c.Nombre })
                    .FirstOrDefault();
                return cuenta;
            }
        }
    }
} 