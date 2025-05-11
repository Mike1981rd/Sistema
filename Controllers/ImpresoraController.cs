using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaContable.Data;
using SistemaContable.Models;
using SistemaContable.ViewModels;
using SistemaContable.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SistemaContable.Controllers
{
    public class ImpresoraController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmpresaService _empresaService;

        public ImpresoraController(
            ApplicationDbContext context,
            IEmpresaService empresaService)
        {
            _context = context;
            _empresaService = empresaService;
        }

        // GET: Impresora
        public async Task<IActionResult> Index(string tab)
        {
            Console.WriteLine($"======= INICIO INDEX IMPRESORA =======");
            Console.WriteLine($"Tab recibido: '{tab}'");
            
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            Console.WriteLine($"EmpresaId obtenido: {empresaId}");
            
            if (empresaId <= 0)
            {
                Console.WriteLine("EmpresaId inválido, redirigiendo a selección de empresa");
                return RedirectToAction("Index", "Empresas");
            }

            bool activos = string.IsNullOrEmpty(tab) || tab == "Activos";
            Console.WriteLine($"Filtro establecido: mostrar {(activos ? "Activos" : "Inactivos")}");
            
            ViewBag.Tab = activos ? "Activos" : "Inactivos";
            
            // Consultar todas las impresoras para verificar si existen datos
            var todasImpresoras = await _context.Impresoras
                .Where(i => i.EmpresaId == empresaId)
                .ToListAsync();
                
            Console.WriteLine($"Total de impresoras para esta empresa: {todasImpresoras.Count}");
            Console.WriteLine($"- Activas: {todasImpresoras.Count(i => i.Estado)}");
            Console.WriteLine($"- Inactivas: {todasImpresoras.Count(i => !i.Estado)}");
            
            // Consultar aplicando el filtro
            var impresoras = await _context.Impresoras
                .Where(i => i.Estado == activos && i.EmpresaId == empresaId)
                .ToListAsync();
                
            Console.WriteLine($"Impresoras filtradas obtenidas: {impresoras.Count}");
            
            if (impresoras.Count == 0 && todasImpresoras.Count > 0)
            {
                // Si no hay resultados con el filtro pero sí hay impresoras en total,
                // sugerir al usuario cambiar de pestaña
                Console.WriteLine("No hay impresoras con el filtro actual, pero sí existen impresoras con otro estado");
                ViewBag.SugerirCambiarPestana = true;
                ViewBag.HayImpresorasEnOtraPestana = activos ? "Inactivas" : "Activas";
            }
            
            Console.WriteLine("======= FIN INDEX IMPRESORA =======");
            return View(impresoras);
        }

        // GET: Impresora/Create
        public async Task<IActionResult> Create()
        {
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            if (empresaId <= 0)
            {
                return RedirectToAction("Index", "Empresas");
            }

            var viewModel = new ImpresoraViewModel
            {
                Estado = true,
                EmpresaId = empresaId
            };

            await CargarRutasFisicas(viewModel);
            return View(viewModel);
        }

        // POST: Impresora/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ImpresoraViewModel viewModel)
        {
            try
            {
                Console.WriteLine("======= INICIO CREATE IMPRESORA =======");
                Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");

                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                Console.WriteLine($"EmpresaId obtenido: {empresaId}, es cero o negativo: {empresaId <= 0}");

                if (empresaId <= 0)
                {
                    Console.WriteLine("ADVERTENCIA: EmpresaId inválido, redirigiendo a selección de empresa");
                    return RedirectToAction("Index", "Empresas");
                }

                viewModel.EmpresaId = empresaId;
                Console.WriteLine($"EmpresaId asignado al viewModel: {viewModel.EmpresaId}");

                if (ModelState.IsValid)
                {
                    // Verificar explícitamente todos los valores para diagnosticar problemas de NULL
                    Console.WriteLine("Verificando valores antes de crear Impresora:");
                    Console.WriteLine($"- Nombre: {viewModel.Nombre ?? "NULL"}, longitud: {viewModel.Nombre?.Length ?? 0}");
                    Console.WriteLine($"- Modelo: {viewModel.Modelo ?? "NULL"}, longitud: {viewModel.Modelo?.Length ?? 0}");
                    Console.WriteLine($"- RutasFisicas: {viewModel.RutasFisicas ?? "NULL"}, longitud: {viewModel.RutasFisicas?.Length ?? 0}");
                    Console.WriteLine($"- Estado: {viewModel.Estado}");
                    Console.WriteLine($"- EmpresaId: {viewModel.EmpresaId}");
                    
                    // Crear una nueva instancia de Impresora con los valores del ViewModel
                    var impresora = new Impresora
                    {
                        Nombre = viewModel.Nombre ?? string.Empty,
                        Modelo = viewModel.Modelo ?? string.Empty,
                        RutasFisicas = viewModel.RutasFisicas ?? string.Empty,
                        Estado = viewModel.Estado,
                        EmpresaId = viewModel.EmpresaId,
                        FechaCreacion = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc)
                    };

                    Console.WriteLine($"Creando impresora: {impresora.Nombre}, Estado: {impresora.Estado}, EmpresaId: {impresora.EmpresaId}");
                    
                    // Verificar si hay algún problema con EmpresaId
                    if (impresora.EmpresaId <= 0)
                    {
                        Console.WriteLine("ERROR CRÍTICO: EmpresaId inválido en el objeto Impresora");
                        ModelState.AddModelError("", "La empresa seleccionada no es válida.");
                        await CargarRutasFisicas(viewModel);
                        return View(viewModel);
                    }
                    
                    // Agregar al contexto y guardar
                    _context.Impresoras.Add(impresora);
                    var result = await _context.SaveChangesAsync();
                    
                    Console.WriteLine($"SaveChangesAsync completado, cambios realizados: {result}");
                    Console.WriteLine($"Impresora guardada con éxito, Id: {impresora.Id}");
                    
                    // Después de guardar, verificar si se guardó correctamente
                    var impresoraGuardada = await _context.Impresoras.FindAsync(impresora.Id);
                    if (impresoraGuardada != null)
                    {
                        Console.WriteLine($"Verificación: Impresora encontrada en DB con Id: {impresoraGuardada.Id}, Nombre: {impresoraGuardada.Nombre}, Estado: {impresoraGuardada.Estado}");
                        
                        // Agregar mensaje de éxito
                        TempData["SuccessMessage"] = $"La impresora '{impresoraGuardada.Nombre}' ha sido creada correctamente";
                        
                        // Redireccionar a la pestaña correspondiente según el estado
                        return RedirectToAction(nameof(Index), new { tab = impresoraGuardada.Estado ? "Activos" : "Inactivos" });
                    }
                    else
                    {
                        Console.WriteLine("ERROR: No se pudo encontrar la impresora recién creada en la base de datos");
                        ModelState.AddModelError("", "No se pudo guardar la impresora. Intente nuevamente.");
                    }
                }
                else
                {
                    Console.WriteLine("ModelState inválido. Errores:");
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            Console.WriteLine($"- {error.ErrorMessage}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR al crear impresora: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                ModelState.AddModelError("", $"Error al guardar: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("======= FIN CREATE IMPRESORA =======");
            }

            // Si llegamos aquí, hubo algún error, volvemos a la vista
            await CargarRutasFisicas(viewModel);
            return View(viewModel);
        }

        // GET: Impresora/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            if (empresaId <= 0)
            {
                return RedirectToAction("Index", "Empresas");
            }

            var impresora = await _context.Impresoras
                .FirstOrDefaultAsync(i => i.Id == id && i.EmpresaId == empresaId);

            if (impresora == null)
                return NotFound();

            var viewModel = new ImpresoraViewModel
            {
                Id = impresora.Id,
                Nombre = impresora.Nombre,
                Modelo = impresora.Modelo,
                RutasFisicas = impresora.RutasFisicas,
                Estado = impresora.Estado,
                EmpresaId = impresora.EmpresaId
            };

            await CargarRutasFisicas(viewModel);
            return View(viewModel);
        }

        // POST: Impresora/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ImpresoraViewModel viewModel)
        {
            if (id != viewModel.Id)
                return NotFound();

            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            if (empresaId <= 0)
            {
                return RedirectToAction("Index", "Empresas");
            }

            viewModel.EmpresaId = empresaId;

            if (ModelState.IsValid)
            {
                try
                {
                    var impresora = await _context.Impresoras
                        .FirstOrDefaultAsync(i => i.Id == id && i.EmpresaId == empresaId);
                    
                    if (impresora == null)
                        return NotFound();

                    impresora.Nombre = viewModel.Nombre ?? string.Empty;
                    impresora.Modelo = viewModel.Modelo ?? string.Empty;
                    impresora.RutasFisicas = viewModel.RutasFisicas ?? string.Empty;
                    impresora.Estado = viewModel.Estado;
                    impresora.FechaModificacion = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

                    _context.Update(impresora);
                    await _context.SaveChangesAsync();
                    
                    // Agregar mensaje de éxito
                    TempData["SuccessMessage"] = $"La impresora '{impresora.Nombre}' ha sido actualizada correctamente";
                    
                    return RedirectToAction(nameof(Index), new { tab = impresora.Estado ? "Activos" : "Inactivos" });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImpresoraExists(viewModel.Id))
                        return NotFound();
                    else
                        throw;
                }
            }

            await CargarRutasFisicas(viewModel);
            return View(viewModel);
        }

        // POST: Impresora/ToggleEstado/5
        [HttpPost]
        public async Task<IActionResult> ToggleEstado(int id)
        {
            Console.WriteLine($"======= INICIO TOGGLE ESTADO IMPRESORA ID:{id} =======");
            try
            {
                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                Console.WriteLine($"EmpresaId obtenido: {empresaId}");

                if (empresaId <= 0)
                {
                    Console.WriteLine("EmpresaId inválido, redirigiendo a selección de empresa");
                    return RedirectToAction("Index", "Empresas");
                }

                var impresora = await _context.Impresoras
                    .FirstOrDefaultAsync(i => i.Id == id && i.EmpresaId == empresaId);

                if (impresora == null)
                {
                    Console.WriteLine($"No se encontró ninguna impresora con Id={id} y EmpresaId={empresaId}");
                    return NotFound();
                }

                var estadoAnterior = impresora.Estado;
                impresora.Estado = !impresora.Estado;
                impresora.FechaModificacion = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
                Console.WriteLine($"Cambiando estado de impresora {impresora.Nombre} de {estadoAnterior} a {impresora.Estado}");

                _context.Update(impresora);
                
                var cambios = await _context.SaveChangesAsync();
                Console.WriteLine($"SaveChangesAsync completado, cambios realizados: {cambios}");
                Console.WriteLine($"Estado actualizado correctamente a {impresora.Estado}");
                
                // Agregar mensaje de éxito
                TempData["SuccessMessage"] = $"La impresora '{impresora.Nombre}' ha sido {(impresora.Estado ? "activada" : "desactivada")} correctamente";
                
                Console.WriteLine($"Redirigiendo a pestaña: {(impresora.Estado ? "Activos" : "Inactivos")}");
                return RedirectToAction(nameof(Index), new { tab = impresora.Estado ? "Activos" : "Inactivos" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR en ToggleEstado: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                
                // En caso de error, redireccionar a la vista principal
                return RedirectToAction(nameof(Index));
            }
            finally
            {
                Console.WriteLine("======= FIN TOGGLE ESTADO IMPRESORA =======");
            }
        }

        private bool ImpresoraExists(int id)
        {
            return _context.Impresoras.Any(e => e.Id == id);
        }

        private async Task CargarRutasFisicas(ImpresoraViewModel viewModel)
        {
            // Aquí cargamos las rutas físicas disponibles como un SelectList
            // En un caso real, podrías obtener estas opciones de la base de datos
            var rutasFisicas = new List<SelectListItem>
            {
                new SelectListItem { Value = "CAJA", Text = "CAJA" },
                new SelectListItem { Value = "COCINA", Text = "COCINA" },
                new SelectListItem { Value = "BAR", Text = "BAR" },
                new SelectListItem { Value = "TERRAZA", Text = "TERRAZA" }
            };
            
            viewModel.RutasFisicasDisponibles = new SelectList(rutasFisicas, "Value", "Text", viewModel.RutasFisicas);
        }

        // GET: Impresora/Test/5
        public IActionResult Test(int id)
        {
            var empresaId = _empresaService.ObtenerEmpresaActualId().GetAwaiter().GetResult();
            if (empresaId <= 0)
            {
                return RedirectToAction("Index", "Empresas");
            }

            var impresora = _context.Impresoras
                .FirstOrDefault(i => i.Id == id && i.EmpresaId == empresaId);

            if (impresora == null)
                return NotFound();

            // Aquí iría la lógica para probar la impresora
            // En un entorno real, esto podría enviar una página de prueba a la impresora
            TempData["InfoMessage"] = $"Se ha enviado una página de prueba a la impresora '{impresora.Nombre}'";
            
            return RedirectToAction(nameof(Index));
        }
    }
} 