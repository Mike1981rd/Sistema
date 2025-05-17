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
        public async Task<IActionResult> Create(string nombre)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombre))
                    return Json(new { success = false, message = "El nombre es requerido" });

                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                
                var unidad = new UnidadMedida 
                { 
                    Nombre = nombre,
                    Abreviatura = nombre.Length > 5 ? nombre.Substring(0, 5) : nombre,
                    EmpresaId = empresaId,
                    Estado = true
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
        public async Task<IActionResult> Edit(int id, string nombre)
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
                await _context.SaveChangesAsync();

                return Json(new { success = true, id = unidad.Id, nombre = unidad.Nombre });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Edit: {ex.Message}");
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
} 