using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Models;
using SistemaContable.Models.Enums;
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
            Console.WriteLine($"=== Create Action - EmpresaId: {empresaId} ===");
            
            if (empresaId <= 0)
            {
                Console.WriteLine("=== EmpresaId inválido, redirigiendo ===");
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
        /// Test de impuestos
        /// </summary>
        public async Task<IActionResult> TestImpuestos()
        {
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            var totalImpuestos = await _context.Impuestos.CountAsync();
            var impuestosEmpresa = await _context.Impuestos.Where(i => i.EmpresaId == empresaId).CountAsync();
            
            var impuestos = await _context.Impuestos
                .Select(i => new 
                { 
                    i.Id, 
                    i.Nombre, 
                    i.EmpresaId, 
                    i.Activo, 
                    i.Estado 
                })
                .ToListAsync();
            
            // Query sin filtros para verificar
            var todosSinFiltro = await _context.Impuestos.ToListAsync();
            
            ViewBag.EmpresaId = empresaId;
            ViewBag.TotalImpuestos = totalImpuestos;
            ViewBag.ImpuestosEmpresa = impuestosEmpresa;
            ViewBag.TodosImpuestos = impuestos;
            ViewBag.TodosSinFiltro = todosSinFiltro;
            
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
            Console.WriteLine($"=== CargarDatosViewBag - EmpresaId: {empresaId} ===");
            
            // Verificar cuántos impuestos existen en total
            var totalImpuestos = await _context.Impuestos.CountAsync();
            Console.WriteLine($"=== Total impuestos en la base de datos: {totalImpuestos} ===");
            
            // Verificar cuántos impuestos hay para esta empresa
            var impuestosEmpresa = await _context.Impuestos
                .Where(i => i.EmpresaId == empresaId)
                .CountAsync();
            Console.WriteLine($"=== Impuestos para empresa {empresaId}: {impuestosEmpresa} ===");
            
            // Cargar impuestos con porcentajes (sin filtrar por Activo para diagnosticar)
            var impuestos = await _context.Impuestos
                .Where(i => i.EmpresaId == empresaId)
                .OrderBy(i => i.Nombre)
                .Select(i => new 
                { 
                    Id = i.Id,
                    Nombre = i.Nombre,
                    Porcentaje = i.Porcentaje ?? 0
                })
                .ToListAsync();
                
            Console.WriteLine($"=== Impuestos encontrados: {impuestos.Count} ===");
            
            ViewBag.Impuestos = impuestos.Select(i => new SelectListItem 
            { 
                Value = i.Id.ToString(), 
                Text = $"{i.Nombre} ({i.Porcentaje}%)" 
            }).ToList();
            
            ViewBag.ImpuestosPorcentajes = impuestos.ToDictionary(i => i.Id.ToString(), i => i.Porcentaje);
            
            Console.WriteLine($"=== ViewBag.Impuestos count: {((List<SelectListItem>)ViewBag.Impuestos).Count} ===");
            Console.WriteLine($"=== ViewBag.ImpuestosPorcentajes count: {((Dictionary<string, decimal>)ViewBag.ImpuestosPorcentajes).Count} ===");
            
            // Agregar EmpresaId al ViewBag para debugging
            ViewBag.EmpresaId = empresaId;
            
            // Cargar impuestos de propina (filtrar por tipo Propina_legal)
            var impuestosPropina = await _context.Impuestos
                .Where(i => i.EmpresaId == empresaId && 
                       i.Tipo == TipoImpuesto.Propina_legal)
                .OrderBy(i => i.Nombre)
                .Select(i => new SelectListItem 
                { 
                    Value = i.Id.ToString(), 
                    Text = i.Nombre 
                })
                .ToListAsync();
            ViewBag.ImpuestosPropina = impuestosPropina;
            
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
        
        // Acción para buscar cuentas contables (para Select2)
        [HttpGet]
        public async Task<IActionResult> BuscarCuentasContables(string term)
        {
            try
            {
                // Asegurarse de que term no sea null
                term = term ?? string.Empty;
                
                Console.WriteLine($"[ProductosController] Buscando cuentas contables con término: '{term}'");
                
                if (string.IsNullOrEmpty(term))
                {
                    return Json(new { results = new List<object>() });
                }
                
                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                if (empresaId <= 0)
                {
                    return Json(new { results = new List<object>() });
                }
                
                // Buscar cuentas contables que coincidan con el término (case-insensitive)
                var cuentas = await _context.CuentasContables
                    .Where(c => c.EmpresaId == empresaId && c.Activo &&
                           (EF.Functions.ILike(c.Codigo, $"%{term}%") || 
                            EF.Functions.ILike(c.Nombre, $"%{term}%") || 
                           (c.Descripcion != null && EF.Functions.ILike(c.Descripcion, $"%{term}%"))))
                    .OrderBy(c => c.Codigo)
                    .Take(20) // Limitar resultados
                    .ToListAsync();
                
                Console.WriteLine($"[ProductosController] Encontradas {cuentas.Count()} cuentas contables para devolver al cliente");
                
                // Asegurarnos de que los datos estén en el formato esperado por Select2
                var results = cuentas.Select(c => new
                {
                    id = c.Id,
                    text = $"{c.Codigo} - {c.Nombre}", // Este es el texto que se muestra en el select
                    codigo = c.Codigo,
                    nombre = c.Nombre
                }).ToList();
                
                return Json(new { results = results });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al buscar cuentas contables: {ex.Message}");
                return Json(new { results = new List<object>() });
            }
        }
    }
}