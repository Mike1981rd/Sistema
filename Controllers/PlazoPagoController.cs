using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Models;
using SistemaContable.Services;

namespace SistemaContable.Controllers
{
    public class PlazoPagoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPlazoPagoService _plazoPagoService;
        
        public PlazoPagoController(ApplicationDbContext context, IPlazoPagoService plazoPagoService)
        {
            _context = context;
            _plazoPagoService = plazoPagoService;
        }
        
        // GET: PlazoPago
        public async Task<IActionResult> Index()
        {
            var plazosPago = await _context.PlazosPago
                .OrderBy(p => p.Dias ?? 999) // Ordenar por días, con vencimiento manual al final
                .ToListAsync();
            return View(plazosPago);
        }
        
        // GET: PlazoPago/Create
        public IActionResult Create()
        {
            return View();
        }
        
        // POST: PlazoPago/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlazoPago plazoPago)
        {
            // Si es vencimiento manual, el campo días puede ser nulo
            if (plazoPago.EsVencimientoManual)
            {
                plazoPago.Dias = null;
                ModelState.Remove("Dias");
            }
            
            if (ModelState.IsValid)
            {
                _context.Add(plazoPago);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Plazo de pago creado correctamente";
                return RedirectToAction(nameof(Index));
            }
            
            return View(plazoPago);
        }
        
        // GET: PlazoPago/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var plazoPago = await _context.PlazosPago.FindAsync(id);
            if (plazoPago == null)
            {
                return NotFound();
            }
            
            // No permitir editar los plazos predeterminados (IDs 1-6)
            if (plazoPago.Id <= 6)
            {
                TempData["ErrorMessage"] = "Los plazos de pago predeterminados no pueden ser editados";
                return RedirectToAction(nameof(Index));
            }
            
            return View(plazoPago);
        }
        
        // POST: PlazoPago/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PlazoPago plazoPago)
        {
            if (id != plazoPago.Id)
            {
                return NotFound();
            }
            
            // No permitir editar los plazos predeterminados (IDs 1-6)
            if (plazoPago.Id <= 6)
            {
                TempData["ErrorMessage"] = "Los plazos de pago predeterminados no pueden ser editados";
                return RedirectToAction(nameof(Index));
            }
            
            // Si es vencimiento manual, el campo días puede ser nulo
            if (plazoPago.EsVencimientoManual)
            {
                plazoPago.Dias = null;
                ModelState.Remove("Dias");
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    // Obtener el estado actual de uso
                    var plazoActual = await _context.PlazosPago.AsNoTracking()
                        .FirstOrDefaultAsync(p => p.Id == id);
                    
                    plazoPago.EstaEnUso = plazoActual?.EstaEnUso ?? false;
                    plazoPago.FechaCreacion = plazoActual?.FechaCreacion ?? DateTime.UtcNow;
                    plazoPago.FechaModificacion = DateTime.UtcNow;
                    
                    _context.Update(plazoPago);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Plazo de pago actualizado correctamente";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlazoPagoExists(plazoPago.Id))
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
            
            return View(plazoPago);
        }
        
        // GET: PlazoPago/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var plazoPago = await _context.PlazosPago
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (plazoPago == null)
            {
                return NotFound();
            }
            
            // No permitir eliminar los plazos predeterminados (IDs 1-6)
            if (plazoPago.Id <= 6)
            {
                TempData["ErrorMessage"] = "Los plazos de pago predeterminados no pueden ser eliminados";
                return RedirectToAction(nameof(Index));
            }
            
            // Verificar si el plazo puede ser eliminado
            ViewBag.PuedeEliminar = await _plazoPagoService.PuedeEliminarPlazoPago(plazoPago.Id);
            
            return View(plazoPago);
        }
        
        // POST: PlazoPago/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // No permitir eliminar los plazos predeterminados (IDs 1-6)
            if (id <= 6)
            {
                TempData["ErrorMessage"] = "Los plazos de pago predeterminados no pueden ser eliminados";
                return RedirectToAction(nameof(Index));
            }
            
            // Verificar si el plazo puede ser eliminado
            bool puedeEliminar = await _plazoPagoService.PuedeEliminarPlazoPago(id);
            
            if (!puedeEliminar)
            {
                TempData["ErrorMessage"] = "No se puede eliminar el plazo porque está en uso";
                return RedirectToAction(nameof(Index));
            }
            
            var plazoPago = await _context.PlazosPago.FindAsync(id);
            if (plazoPago != null)
            {
                _context.PlazosPago.Remove(plazoPago);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Plazo de pago eliminado correctamente";
            }
            
            return RedirectToAction(nameof(Index));
        }
        
        private bool PlazoPagoExists(int id)
        {
            return _context.PlazosPago.Any(e => e.Id == id);
        }
    }
} 