using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Models;
using SistemaContable.Utilities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaContable.Controllers
{
    public class ComprobantesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComprobantesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /configuracion/comprobantes-fiscales
        [Route("/configuracion/comprobantes-fiscales")]
        public async Task<IActionResult> Index(string tab = "Activos")
        {
            ViewBag.Tab = tab;
            
            var comprobantes = await _context.ComprobantesFiscales.ToListAsync();
            
            if (tab == "Activos")
            {
                comprobantes = comprobantes.Where(a => a.Activo).ToList();
                
                // Si no hay activos pero hay inactivos, sugerir cambiar pestaña
                if (!comprobantes.Any() && await _context.ComprobantesFiscales.AnyAsync(a => !a.Activo))
                {
                    ViewBag.SugerirCambiarPestana = true;
                    ViewBag.HayComprobantesEnOtraPestana = "Inactivos";
                }
            }
            else
            {
                comprobantes = comprobantes.Where(a => !a.Activo).ToList();
                
                // Si no hay inactivos pero hay activos, sugerir cambiar pestaña
                if (!comprobantes.Any() && await _context.ComprobantesFiscales.AnyAsync(a => a.Activo))
                {
                    ViewBag.SugerirCambiarPestana = true;
                    ViewBag.HayComprobantesEnOtraPestana = "Activos";
                }
            }
            
            // Add type information for filter modal
            ViewBag.TiposDocumento = Constantes.TiposDocumento.ObtenerTiposDocumento();
            ViewBag.TiposComprobante = Constantes.TiposComprobanteFiscal.ObtenerTipos();
            
            return View(comprobantes);
        }

        // GET: /configuracion/comprobantes-fiscales/crear
        [Route("/configuracion/comprobantes-fiscales/crear")]
        public IActionResult Crear()
        {
            ViewBag.TiposDocumento = Constantes.TiposDocumento.ObtenerTiposDocumento();
            ViewBag.TiposComprobante = Constantes.TiposComprobanteFiscal.ObtenerTipos();
            
            var comprobante = new ComprobanteFiscal
            {
                NumeroInicial = 1,
                SiguienteNumero = 1,
                FechaVencimiento = DateTime.Now.AddYears(1),
                Activo = true
            };
            
            return View(comprobante);
        }

        // POST: /configuracion/comprobantes-fiscales/crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/configuracion/comprobantes-fiscales/crear")]
        public async Task<IActionResult> Crear([Bind("Nombre,TipoDocumento,Tipo,Preferida,Electronica,Prefijo,NumeroInicial,NumeroFinal,FechaVencimiento,Sucursal")] ComprobanteFiscal comprobante)
        {
            if (ModelState.IsValid)
            {
                // Asignar siguiente número igual al número inicial
                comprobante.SiguienteNumero = comprobante.NumeroInicial;
                comprobante.Activo = true;
                
                // Si es preferida, desmarcar otras del mismo tipo
                if (comprobante.Preferida)
                {
                    var numeracionesPreferidas = await _context.ComprobantesFiscales
                        .Where(c => c.TipoDocumento == comprobante.TipoDocumento && c.Preferida)
                        .ToListAsync();
                    
                    foreach (var num in numeracionesPreferidas)
                    {
                        num.Preferida = false;
                        _context.Update(num);
                    }
                }
                
                _context.Add(comprobante);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Comprobante fiscal creado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.TiposDocumento = Constantes.TiposDocumento.ObtenerTiposDocumento();
            ViewBag.TiposComprobante = Constantes.TiposComprobanteFiscal.ObtenerTipos();
            return View(comprobante);
        }

        // GET: /configuracion/comprobantes-fiscales/editar/5
        [Route("/configuracion/comprobantes-fiscales/editar/{id}")]
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comprobante = await _context.ComprobantesFiscales.FindAsync(id);
            if (comprobante == null)
            {
                return NotFound();
            }
            
            ViewBag.TiposDocumento = Constantes.TiposDocumento.ObtenerTiposDocumento();
            ViewBag.TiposComprobante = Constantes.TiposComprobanteFiscal.ObtenerTipos();
            return View(comprobante);
        }

        // POST: /configuracion/comprobantes-fiscales/editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/configuracion/comprobantes-fiscales/editar/{id}")]
        public async Task<IActionResult> Editar(int id, [Bind("Id,Nombre,TipoDocumento,Tipo,Preferida,Electronica,Prefijo,NumeroInicial,NumeroFinal,SiguienteNumero,FechaVencimiento,Sucursal,Activo")] ComprobanteFiscal comprobante)
        {
            if (id != comprobante.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Si es preferida, desmarcar otras del mismo tipo
                    if (comprobante.Preferida)
                    {
                        var numeracionesPreferidas = await _context.ComprobantesFiscales
                            .Where(c => c.Id != comprobante.Id && c.TipoDocumento == comprobante.TipoDocumento && c.Preferida)
                            .ToListAsync();
                        
                        foreach (var num in numeracionesPreferidas)
                        {
                            num.Preferida = false;
                            _context.Update(num);
                        }
                    }
                    
                    // Actualizar fecha de modificación
                    comprobante.UltimaModificacion = DateTime.Now;
                    
                    _context.Update(comprobante);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Comprobante fiscal actualizado correctamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComprobanteFiscalExists(comprobante.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { tab = comprobante.Activo ? "Activos" : "Inactivos" });
            }
            
            ViewBag.TiposDocumento = Constantes.TiposDocumento.ObtenerTiposDocumento();
            ViewBag.TiposComprobante = Constantes.TiposComprobanteFiscal.ObtenerTipos();
            return View(comprobante);
        }

        // POST: /configuracion/comprobantes-fiscales/toggle-estado/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/configuracion/comprobantes-fiscales/toggle-estado/{id}")]
        public async Task<IActionResult> ToggleEstado(int id)
        {
            var comprobante = await _context.ComprobantesFiscales.FindAsync(id);
            
            if (comprobante == null)
            {
                return NotFound();
            }
            
            // Invertir el estado actual
            comprobante.Activo = !comprobante.Activo;
            
            try
            {
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Comprobante fiscal {(comprobante.Activo ? "activado" : "desactivado")} correctamente.";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "No se pudo cambiar el estado del comprobante fiscal.";
            }
            
            return RedirectToAction(nameof(Index), new { tab = comprobante.Activo ? "Activos" : "Inactivos" });
        }

        // GET: /configuracion/comprobantes-fiscales/eliminar/5
        [Route("/configuracion/comprobantes-fiscales/eliminar/{id}")]
        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comprobante = await _context.ComprobantesFiscales
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comprobante == null)
            {
                return NotFound();
            }

            return View(comprobante);
        }

        // POST: /configuracion/comprobantes-fiscales/eliminar/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        [Route("/configuracion/comprobantes-fiscales/eliminar/{id}")]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var comprobante = await _context.ComprobantesFiscales.FindAsync(id);
            if (comprobante != null)
            {
                _context.ComprobantesFiscales.Remove(comprobante);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Comprobante fiscal eliminado correctamente.";
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool ComprobanteFiscalExists(int id)
        {
            return _context.ComprobantesFiscales.Any(e => e.Id == id);
        }
    }
} 