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
    public class UnidadMedidaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmpresaService _empresaService;

        public UnidadMedidaController(
            ApplicationDbContext context,
            IEmpresaService empresaService)
        {
            _context = context;
            _empresaService = empresaService;
        }

        // GET: UnidadMedida
        public async Task<IActionResult> Index()
        {
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            var unidades = await _context.UnidadesMedida
                .Where(u => u.EmpresaId == empresaId)
                .OrderBy(u => u.Nombre)
                .ToListAsync();
                
            return View(unidades);
        }

        // GET: UnidadMedida/ObtenerTodas
        [HttpGet]
        public async Task<JsonResult> ObtenerTodas()
        {
            try
            {
                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                if (empresaId <= 0)
                {
                    return Json(new { error = "Empresa no seleccionada" });
                }

                var unidades = await _context.UnidadesMedida
                    .Where(u => u.Estado)
                    .OrderBy(u => u.Nombre)
                    .Select(u => new { 
                        id = u.Id, 
                        nombre = u.Nombre,
                        abreviatura = u.Abreviatura
                    })
                    .ToListAsync();

                return Json(unidades);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ObtenerTodas: {ex.Message}");
                return Json(new { error = ex.Message });
            }
        }

        // GET: UnidadMedida/Buscar
        [HttpGet]
        public async Task<IActionResult> Buscar(string term = "")
        {
            try
            {
                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                var query = _context.UnidadesMedida.Where(u => u.EmpresaId == empresaId && u.Estado);
                
                if (!string.IsNullOrEmpty(term))
                {
                    term = term.ToLower();
                    query = query.Where(u => 
                        u.Nombre.ToLower().Contains(term) || 
                        u.Abreviatura.ToLower().Contains(term)
                    );
                }
                
                var unidades = await query
                    .OrderBy(u => u.Nombre)
                    .Select(u => new { id = u.Id, text = u.Nombre })
                    .Take(10)
                    .ToListAsync();

                return Json(new { results = unidades });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Buscar: {ex.Message}");
                return Json(new { error = ex.Message });
            }
        }

        // POST: UnidadMedida/Create
        [HttpPost]
        public async Task<IActionResult> Create(string nombre, string abreviatura, string descripcion)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombre))
                    return Json(new { success = false, message = "El nombre es requerido" });

                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                
                var unidad = new UnidadMedida 
                { 
                    Nombre = nombre,
                    Abreviatura = abreviatura ?? (nombre.Length > 5 ? nombre.Substring(0, 5) : nombre),
                    Descripcion = descripcion,
                    EmpresaId = empresaId,
                    Estado = true,
                    FechaCreacion = DateTime.Now
                };
                
                _context.UnidadesMedida.Add(unidad);
                await _context.SaveChangesAsync();

                return Json(new { success = true, id = unidad.Id, nombre = unidad.Nombre });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Create: {ex.Message}");
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: UnidadMedida/Edit/{id}
        [HttpPost]
        [Route("UnidadMedida/Edit/{id}")]
        public async Task<IActionResult> Edit(int id, string nombre, string abreviatura, string descripcion)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombre))
                    return Json(new { success = false, message = "El nombre es requerido" });

                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                var unidad = await _context.UnidadesMedida
                    .FirstOrDefaultAsync(u => u.Id == id && u.EmpresaId == empresaId);
                
                if (unidad == null)
                    return Json(new { success = false, message = "Unidad de medida no encontrada" });

                unidad.Nombre = nombre;
                unidad.Abreviatura = abreviatura ?? unidad.Abreviatura;
                unidad.Descripcion = descripcion;
                unidad.FechaModificacion = DateTime.Now;
                
                await _context.SaveChangesAsync();

                return Json(new { success = true, id = unidad.Id, nombre = unidad.Nombre });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Edit: {ex.Message}");
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: UnidadMedida/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                var unidad = await _context.UnidadesMedida
                    .FirstOrDefaultAsync(u => u.Id == id && u.EmpresaId == empresaId);
                
                if (unidad == null)
                    return RedirectToAction(nameof(Index));
                
                // Verificar si está siendo usada
                var enUso = await _context.ItemContenedores
                    .AnyAsync(ic => ic.UnidadMedidaId == id);
                    
                if (enUso)
                {
                    TempData["Error"] = "No se puede eliminar esta unidad de medida porque está siendo utilizada.";
                    return RedirectToAction(nameof(Index));
                }
                
                _context.UnidadesMedida.Remove(unidad);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Unidad de medida eliminada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al eliminar: {ex.Message}";
            }
            
            return RedirectToAction(nameof(Index));
        }

        // GET: UnidadMedida/CreateBasicUnits
        public async Task<IActionResult> CreateBasicUnits()
        {
            try
            {
                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                var fecha = DateTime.Now;
                
                // Lista de unidades básicas
                var unidadesBasicas = new[]
                {
                    new UnidadMedida { Nombre = "Unidad", Abreviatura = "UN", Descripcion = "Unidad individual", EmpresaId = empresaId, FechaCreacion = fecha, Estado = true },
                    new UnidadMedida { Nombre = "Caja", Abreviatura = "CJ", Descripcion = "Caja", EmpresaId = empresaId, FechaCreacion = fecha, Estado = true },
                    new UnidadMedida { Nombre = "Botella", Abreviatura = "BOT", Descripcion = "Botella", EmpresaId = empresaId, FechaCreacion = fecha, Estado = true },
                    new UnidadMedida { Nombre = "Onza", Abreviatura = "OZ", Descripcion = "Onza", EmpresaId = empresaId, FechaCreacion = fecha, Estado = true },
                    new UnidadMedida { Nombre = "Libra", Abreviatura = "LB", Descripcion = "Libra", EmpresaId = empresaId, FechaCreacion = fecha, Estado = true },
                    new UnidadMedida { Nombre = "Kilogramo", Abreviatura = "KG", Descripcion = "Kilogramo", EmpresaId = empresaId, FechaCreacion = fecha, Estado = true },
                    new UnidadMedida { Nombre = "Litro", Abreviatura = "L", Descripcion = "Litro", EmpresaId = empresaId, FechaCreacion = fecha, Estado = true },
                    new UnidadMedida { Nombre = "Galón", Abreviatura = "GAL", Descripcion = "Galón", EmpresaId = empresaId, FechaCreacion = fecha, Estado = true },
                    new UnidadMedida { Nombre = "Metro", Abreviatura = "M", Descripcion = "Metro", EmpresaId = empresaId, FechaCreacion = fecha, Estado = true },
                    new UnidadMedida { Nombre = "Paquete", Abreviatura = "PQ", Descripcion = "Paquete", EmpresaId = empresaId, FechaCreacion = fecha, Estado = true },
                    new UnidadMedida { Nombre = "Docena", Abreviatura = "DOC", Descripcion = "Docena", EmpresaId = empresaId, FechaCreacion = fecha, Estado = true },
                    new UnidadMedida { Nombre = "Fardo", Abreviatura = "FDO", Descripcion = "Fardo", EmpresaId = empresaId, FechaCreacion = fecha, Estado = true }
                };
                
                // Verificar cuáles no existen
                foreach (var unidad in unidadesBasicas)
                {
                    var existe = await _context.UnidadesMedida
                        .AnyAsync(u => u.Nombre == unidad.Nombre && u.EmpresaId == empresaId);
                        
                    if (!existe)
                    {
                        _context.UnidadesMedida.Add(unidad);
                    }
                }
                
                await _context.SaveChangesAsync();
                TempData["Success"] = "Unidades de medida básicas creadas correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al crear unidades básicas: {ex.Message}";
            }
            
            return RedirectToAction(nameof(Index));
        }
    }
} 