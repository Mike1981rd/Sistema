using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Models;
using SistemaContable.Models.ViewModels;
using SistemaContable.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SistemaContable.Controllers
{
    public class MarcaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmpresaService _empresaService;

        public MarcaController(ApplicationDbContext context, IEmpresaService empresaService)
        {
            _context = context;
            _empresaService = empresaService;
        }

        public async Task<IActionResult> Index()
        {
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            
            var marcas = await _context.Marcas
                .Where(m => m.EmpresaId == empresaId)
                .OrderBy(m => m.Nombre)
                .ToListAsync();
                
            return View(marcas);
        }

        public async Task<IActionResult> Create()
        {
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            
            var viewModel = new MarcaViewModel
            {
                EmpresaId = empresaId,
                Estado = true
            };
            
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MarcaViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var marca = new Marca
                    {
                        Nombre = viewModel.Nombre,
                        Descripcion = viewModel.Descripcion,
                        Estado = viewModel.Estado,
                        EmpresaId = viewModel.EmpresaId
                    };
                    
                    _context.Add(marca);
                    await _context.SaveChangesAsync();
                    
                    TempData["SuccessMessage"] = "Marca creada exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ocurrió un error al crear la marca: " + ex.Message);
                }
            }
            
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            
            var marca = await _context.Marcas
                .FirstOrDefaultAsync(m => m.Id == id && m.EmpresaId == empresaId);
                
            if (marca == null)
            {
                return NotFound();
            }
            
            var viewModel = new MarcaViewModel
            {
                Id = marca.Id,
                Nombre = marca.Nombre,
                Descripcion = marca.Descripcion,
                Estado = marca.Estado,
                EmpresaId = marca.EmpresaId
            };
            
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MarcaViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    var marca = await _context.Marcas.FindAsync(id);
                    
                    if (marca == null)
                    {
                        return NotFound();
                    }
                    
                    marca.Nombre = viewModel.Nombre;
                    marca.Descripcion = viewModel.Descripcion;
                    marca.Estado = viewModel.Estado;
                    
                    _context.Update(marca);
                    await _context.SaveChangesAsync();
                    
                    TempData["SuccessMessage"] = "Marca actualizada exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await MarcaExists(viewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ocurrió un error al actualizar la marca: " + ex.Message);
                }
            }
            
            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            
            var marca = await _context.Marcas
                .FirstOrDefaultAsync(m => m.Id == id && m.EmpresaId == empresaId);
                
            if (marca == null)
            {
                return NotFound();
            }
            
            return View(marca);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            
            var marca = await _context.Marcas
                .FirstOrDefaultAsync(m => m.Id == id && m.EmpresaId == empresaId);
                
            if (marca == null)
            {
                return NotFound();
            }
            
            // En lugar de eliminar, marcar como inactiva
            marca.Estado = false;
            
            _context.Update(marca);
            await _context.SaveChangesAsync();
            
            TempData["SuccessMessage"] = "Marca eliminada exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> MarcaExists(int id)
        {
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            
            return await _context.Marcas.AnyAsync(m => m.Id == id && m.EmpresaId == empresaId);
        }

        // Endpoints para select2 y formularios parciales

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

                var marcas = await _context.Marcas
                    .Where(m => m.EmpresaId == empresaId && m.Estado)
                    .OrderBy(m => m.Nombre)
                    .Select(m => new { 
                        id = m.Id, 
                        nombre = m.Nombre,
                        descripcion = m.Descripcion
                    })
                    .ToListAsync();

                return Json(marcas);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ObtenerTodas: {ex.Message}");
                return Json(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreatePartial()
        {
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            
            var viewModel = new MarcaViewModel
            {
                EmpresaId = empresaId,
                Estado = true
            };
            
            return PartialView("_Create", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePartial(MarcaViewModel viewModel)
        {
            // Remove description validation
            if (!ModelState.IsValid)
            {
                // Only check if errors are from fields other than Description
                var errorsExcludingDescription = ModelState
                    .Where(x => x.Key != "Descripcion" && x.Value.Errors.Count > 0)
                    .ToList();
                
                if (errorsExcludingDescription.Any())
                {
                    var errors = errorsExcludingDescription
                        .SelectMany(x => x.Value.Errors)
                        .Select(e => e.ErrorMessage);
                    
                    return Json(new { 
                        success = false, 
                        message = "Datos inválidos", 
                        errors = errors 
                    });
                }
            }
            
            try
            {
                var marca = new Marca
                {
                    Nombre = viewModel.Nombre,
                    Descripcion = viewModel.Descripcion,
                    Estado = true,
                    EmpresaId = viewModel.EmpresaId
                };
                
                _context.Add(marca);
                await _context.SaveChangesAsync();
                
                return Json(new { 
                    success = true, 
                    message = "Marca creada exitosamente", 
                    marcaId = marca.Id, 
                    nombre = marca.Nombre 
                });
            }
            catch (Exception ex)
            {
                return Json(new { 
                    success = false, 
                    message = "Error al crear la marca: " + ex.Message 
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditPartial(int id)
        {
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            
            var marca = await _context.Marcas
                .FirstOrDefaultAsync(m => m.Id == id && m.EmpresaId == empresaId);
                
            if (marca == null)
            {
                return NotFound();
            }
            
            var viewModel = new MarcaViewModel
            {
                Id = marca.Id,
                Nombre = marca.Nombre,
                Descripcion = marca.Descripcion,
                Estado = marca.Estado,
                EmpresaId = marca.EmpresaId
            };
            
            return PartialView("_Edit", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditPartial(MarcaViewModel viewModel)
        {
            // Remove description validation
            if (!ModelState.IsValid)
            {
                // Only check if errors are from fields other than Description
                var errorsExcludingDescription = ModelState
                    .Where(x => x.Key != "Descripcion" && x.Value.Errors.Count > 0)
                    .ToList();
                
                if (errorsExcludingDescription.Any())
                {
                    var errors = errorsExcludingDescription
                        .SelectMany(x => x.Value.Errors)
                        .Select(e => e.ErrorMessage);
                    
                    return Json(new { 
                        success = false, 
                        message = "Datos inválidos", 
                        errors = errors 
                    });
                }
            }
            
            try
            {
                var marca = await _context.Marcas.FindAsync(viewModel.Id);
                
                if (marca == null)
                {
                    return Json(new { success = false, message = "Marca no encontrada" });
                }
                
                marca.Nombre = viewModel.Nombre;
                marca.Descripcion = viewModel.Descripcion;
                marca.Estado = viewModel.Estado;
                
                _context.Update(marca);
                await _context.SaveChangesAsync();
                
                return Json(new { 
                    success = true, 
                    message = "Marca actualizada exitosamente", 
                    marcaId = marca.Id, 
                    nombre = marca.Nombre 
                });
            }
            catch (Exception ex)
            {
                return Json(new { 
                    success = false, 
                    message = "Error al actualizar la marca: " + ex.Message 
                });
            }
        }

        // GET: Marca/Buscar
        [HttpGet]
        public async Task<JsonResult> Buscar(string term)
        {
            try
            {
                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                if (empresaId <= 0)
                {
                    return Json(new { results = new List<object>() });
                }

                var marcas = await _context.Marcas
                    .Where(m => m.EmpresaId == empresaId && m.Estado && 
                          (string.IsNullOrEmpty(term) || m.Nombre.Contains(term)))
                    .OrderBy(m => m.Nombre)
                    .Select(m => new { 
                        id = m.Id, 
                        text = m.Nombre
                    })
                    .ToListAsync();

                return Json(new { results = marcas });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Buscar: {ex.Message}");
                return Json(new { results = new List<object>() });
            }
        }

        [HttpGet]
        public async Task<JsonResult> Obtener(int id)
        {
            var marca = await _context.Marcas.FindAsync(id);
            if (marca == null)
                return Json(new { success = false, message = "No encontrada" });

            return Json(new {
                success = true,
                id = marca.Id,
                nombre = marca.Nombre,
                descripcion = marca.Descripcion,
                estado = marca.Estado
            });
        }
    }
} 