using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Models;
using SistemaContable.Services;
using System.Threading.Tasks;
using System.Linq;

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
        public async Task<IActionResult> Index()
        {
            // Obtener la empresa actual
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            if (empresaId <= 0)
            {
                return RedirectToAction("Index", "Empresas");
            }

            return View(await _context.Impuestos
                .Include(i => i.CuentaContableVentas)
                .Include(i => i.CuentaContableCompras)
                .Where(i => i.EmpresaId == empresaId)
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
        public async Task<IActionResult> Create([Bind("Id,Nombre,Tipo,Porcentaje,Descripcion,EsAcreditable,CuentaContableVentasId,CuentaContableComprasId")] Impuesto impuesto)
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Tipo,Porcentaje,Descripcion,EsAcreditable,CuentaContableVentasId,CuentaContableComprasId,FechaCreacion,EstaEnUso,EmpresaId")] Impuesto impuesto)
        {
            if (id != impuesto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
        
        private async Task CargarCuentasContables(int empresaId)
        {
            // Cargar solo cuentas de tipo Movimiento para los selectores
            ViewBag.CuentasContables = new SelectList(
                await _context.CuentasContables
                    .Where(c => c.EmpresaId == empresaId && c.TipoCuenta == "Movimiento" && c.Activo)
                    .OrderBy(c => c.Codigo)
                    .ToListAsync(),
                "Id",
                "CodigoNombre",
                null
            );
        }
    }
} 