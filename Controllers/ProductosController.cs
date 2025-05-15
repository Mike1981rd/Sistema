using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Models;
using SistemaContable.Services;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaContable.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmpresaService _empresaService;

        public ProductosController(ApplicationDbContext context, IEmpresaService empresaService)
        {
            _context = context;
            _empresaService = empresaService;
        }

        /// <summary>
        /// Vista principal - Lista de productos
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var productos = await _context.ProductosVenta
                .Include(p => p.Categoria)
                .Include(p => p.Impuesto)
                .OrderBy(p => p.Nombre)
                .ToListAsync();

            return View(productos);
        }

        /// <summary>
        /// Vista de creación de producto
        /// </summary>
        public async Task<IActionResult> Create()
        {
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            if (empresaId <= 0)
            {
                return RedirectToAction("Index", "Empresas");
            }
            
            await CargarDatosViewBag(empresaId);
            return View("GestionarProducto", null);
        }
        
        /// <summary>
        /// Vista de prueba
        /// </summary>
        public IActionResult Test()
        {
            return View();
        }
        
        /// <summary>
        /// Vista de prueba de Select2
        /// </summary>
        public IActionResult TestSelect2()
        {
            return View();
        }
        
        /// <summary>
        /// Vista de prueba simple
        /// </summary>
        public IActionResult TestSimple()
        {
            return View();
        }

        /// <summary>
        /// Vista de edición de producto
        /// </summary>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.ProductosVenta.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            if (empresaId <= 0)
            {
                return RedirectToAction("Index", "Empresas");
            }
            
            await CargarDatosViewBag(empresaId);
            return View("GestionarProducto", id);
        }

        /// <summary>
        /// Vista de detalles del producto
        /// </summary>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.ProductosVenta
                .Include(p => p.Categoria)
                .Include(p => p.Impuesto)
                .Include(p => p.RutaImpresora)
                .Include(p => p.Item)
                .Include(p => p.ItemContenedor)
                .Include(p => p.Variantes)
                .Include(p => p.ProductoModificadorGrupos)
                    .ThenInclude(pmg => pmg.GrupoModificadores)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        /// <summary>
        /// Confirmación de eliminación
        /// </summary>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.ProductosVenta
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        /// <summary>
        /// Eliminación confirmada
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.ProductosVenta.FindAsync(id);
            if (producto != null)
            {
                _context.ProductosVenta.Remove(producto);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProductoVentaExists(int id)
        {
            return _context.ProductosVenta.Any(e => e.Id == id);
        }
        
        private async Task CargarDatosViewBag(int empresaId)
        {
            // Cargar impuestos
            var impuestos = await _context.Impuestos
                .Where(i => i.EmpresaId == empresaId && i.Activo)
                .OrderBy(i => i.Nombre)
                .Select(i => new SelectListItem 
                { 
                    Value = i.Id.ToString(), 
                    Text = i.Nombre 
                })
                .ToListAsync();
            ViewBag.Impuestos = impuestos;
            
            // Cargar rutas de impresión
            var rutasImpresora = await _context.RutasImpresora
                .Where(r => r.EmpresaId == empresaId && r.Estado)
                .OrderBy(r => r.Nombre)
                .Select(r => new SelectListItem 
                { 
                    Value = r.Id.ToString(), 
                    Text = r.Nombre 
                })
                .ToListAsync();
            ViewBag.RutasImpresora = rutasImpresora;
        }
    }
}