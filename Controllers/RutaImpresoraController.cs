using Microsoft.AspNetCore.Mvc;
using SistemaContable.Data;
using SistemaContable.Models;
using SistemaContable.ViewModels;
using SistemaContable.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaContable.Controllers
{
    public class RutaImpresoraController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmpresaService _empresaService;

        public RutaImpresoraController(
            ApplicationDbContext context,
            IEmpresaService empresaService)
        {
            _context = context;
            _empresaService = empresaService;
        }

        // GET: RutaImpresora
        public async Task<IActionResult> Index(string tab)
        {
            Console.WriteLine($"======= INICIO INDEX RUTA IMPRESORA =======");
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
            
            // Consultar todas las rutas para verificar si existen datos
            var todasRutas = await _context.RutasImpresora
                .Where(r => r.EmpresaId == empresaId)
                .ToListAsync();
                
            Console.WriteLine($"Total de rutas para esta empresa: {todasRutas.Count}");
            Console.WriteLine($"- Activas: {todasRutas.Count(r => r.Estado)}");
            Console.WriteLine($"- Inactivas: {todasRutas.Count(r => !r.Estado)}");
            
            // Consultar aplicando el filtro
            var rutas = await _context.RutasImpresora
                .Where(r => r.Estado == activos && r.EmpresaId == empresaId)
                .ToListAsync();
                
            Console.WriteLine($"Rutas filtradas obtenidas: {rutas.Count}");
            
            if (rutas.Count == 0 && todasRutas.Count > 0)
            {
                // Si no hay resultados con el filtro pero sí hay rutas en total,
                // sugerir al usuario cambiar de pestaña
                Console.WriteLine("No hay rutas con el filtro actual, pero sí existen rutas con otro estado");
                ViewBag.SugerirCambiarPestana = true;
                ViewBag.HayRutasEnOtraPestana = activos ? "Inactivas" : "Activas";
            }
            
            Console.WriteLine("======= FIN INDEX RUTA IMPRESORA =======");
            return View(rutas);
        }

        // GET: RutaImpresora/Create
        public async Task<IActionResult> Create()
        {
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            if (empresaId <= 0)
            {
                return RedirectToAction("Index", "Empresas");
            }

            var viewModel = new RutaImpresoraViewModel
            {
                Estado = true,
                EmpresaId = empresaId
            };

            return View(viewModel);
        }

        // POST: RutaImpresora/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RutaImpresoraViewModel viewModel)
        {
            try
            {
                Console.WriteLine("======= INICIO CREATE RUTA IMPRESORA =======");
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
                    Console.WriteLine("Verificando valores antes de crear Ruta:");
                    Console.WriteLine($"- Nombre: {viewModel.Nombre ?? "NULL"}, longitud: {viewModel.Nombre?.Length ?? 0}");
                    Console.WriteLine($"- Default: {viewModel.Default}");
                    Console.WriteLine($"- Estado: {viewModel.Estado}");
                    Console.WriteLine($"- EmpresaId: {viewModel.EmpresaId}");
                    
                    // Si esta ruta se marca como predeterminada, hay que quitar ese estado de las demás
                    if (viewModel.Default)
                    {
                        await QuitarPredeterminadoDeOtrasRutas(empresaId);
                    }
                    
                    // Crear una nueva instancia de RutaImpresora con los valores del ViewModel
                    var ruta = new RutaImpresora
                    {
                        Nombre = viewModel.Nombre ?? string.Empty,
                        Default = viewModel.Default,
                        Estado = viewModel.Estado,
                        EmpresaId = viewModel.EmpresaId,
                        FechaCreacion = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc)
                    };

                    Console.WriteLine($"Creando ruta: {ruta.Nombre}, Default: {ruta.Default}, Estado: {ruta.Estado}, EmpresaId: {ruta.EmpresaId}");
                    
                    // Verificar si hay algún problema con EmpresaId
                    if (ruta.EmpresaId <= 0)
                    {
                        Console.WriteLine("ERROR CRÍTICO: EmpresaId inválido en el objeto RutaImpresora");
                        ModelState.AddModelError("", "La empresa seleccionada no es válida.");
                        return View(viewModel);
                    }
                    
                    // Agregar al contexto y guardar
                    _context.RutasImpresora.Add(ruta);
                    var result = await _context.SaveChangesAsync();
                    
                    Console.WriteLine($"SaveChangesAsync completado, cambios realizados: {result}");
                    Console.WriteLine($"Ruta guardada con éxito, Id: {ruta.Id}");
                    
                    // Después de guardar, verificar si se guardó correctamente
                    var rutaGuardada = await _context.RutasImpresora.FindAsync(ruta.Id);
                    if (rutaGuardada != null)
                    {
                        Console.WriteLine($"Verificación: Ruta encontrada en DB con Id: {rutaGuardada.Id}, Nombre: {rutaGuardada.Nombre}, Estado: {rutaGuardada.Estado}");
                        
                        // Agregar mensaje de éxito
                        TempData["SuccessMessage"] = $"La ruta '{rutaGuardada.Nombre}' ha sido creada correctamente";
                        
                        // Redireccionar a la pestaña correspondiente según el estado
                        return RedirectToAction(nameof(Index), new { tab = rutaGuardada.Estado ? "Activos" : "Inactivos" });
                    }
                    else
                    {
                        Console.WriteLine("ERROR: No se pudo encontrar la ruta recién creada en la base de datos");
                        ModelState.AddModelError("", "No se pudo guardar la ruta. Intente nuevamente.");
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
                Console.WriteLine($"ERROR al crear ruta: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                ModelState.AddModelError("", $"Error al guardar: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("======= FIN CREATE RUTA IMPRESORA =======");
            }

            // Si llegamos aquí, hubo algún error, volvemos a la vista
            return View(viewModel);
        }

        // GET: RutaImpresora/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            if (empresaId <= 0)
            {
                return RedirectToAction("Index", "Empresas");
            }

            var ruta = await _context.RutasImpresora
                .FirstOrDefaultAsync(r => r.Id == id && r.EmpresaId == empresaId);

            if (ruta == null)
                return NotFound();

            var viewModel = new RutaImpresoraViewModel
            {
                Id = ruta.Id,
                Nombre = ruta.Nombre,
                Default = ruta.Default,
                Estado = ruta.Estado,
                EmpresaId = ruta.EmpresaId
            };

            return View(viewModel);
        }

        // POST: RutaImpresora/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RutaImpresoraViewModel viewModel)
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
                    var ruta = await _context.RutasImpresora
                        .FirstOrDefaultAsync(r => r.Id == id && r.EmpresaId == empresaId);
                    
                    if (ruta == null)
                        return NotFound();

                    // Si esta ruta se marca como predeterminada y antes no lo era, quitar ese estado de las demás
                    if (viewModel.Default && !ruta.Default)
                    {
                        await QuitarPredeterminadoDeOtrasRutas(empresaId);
                    }

                    ruta.Nombre = viewModel.Nombre ?? string.Empty;
                    ruta.Default = viewModel.Default;
                    ruta.Estado = viewModel.Estado;
                    ruta.FechaModificacion = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

                    _context.Update(ruta);
                    await _context.SaveChangesAsync();
                    
                    // Agregar mensaje de éxito
                    TempData["SuccessMessage"] = $"La ruta '{ruta.Nombre}' ha sido actualizada correctamente";
                    
                    return RedirectToAction(nameof(Index), new { tab = ruta.Estado ? "Activos" : "Inactivos" });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RutaImpresoraExists(viewModel.Id))
                        return NotFound();
                    else
                        throw;
                }
            }

            return View(viewModel);
        }

        // GET: RutaImpresora/Buscar
        public async Task<IActionResult> Buscar(string term)
        {
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            if (empresaId <= 0)
            {
                return Json(new { results = new object[0] });
            }

            if (string.IsNullOrWhiteSpace(term))
            {
                return Json(new { results = new object[0] });
            }

            var rutas = await _context.RutasImpresora
                .Where(r => r.EmpresaId == empresaId &&
                           r.Estado &&
                           EF.Functions.ILike(r.Nombre, $"%{term}%"))
                .Select(r => new
                {
                    id = r.Id,
                    text = r.Nombre
                })
                .Take(10)
                .ToListAsync();

            return Json(new { results = rutas });
        }

        // POST: RutaImpresora/ToggleEstado/5
        [HttpPost]
        public async Task<IActionResult> ToggleEstado(int id)
        {
            Console.WriteLine($"======= INICIO TOGGLE ESTADO RUTA IMPRESORA ID:{id} =======");
            try
            {
                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                Console.WriteLine($"EmpresaId obtenido: {empresaId}");

                if (empresaId <= 0)
                {
                    Console.WriteLine("EmpresaId inválido, redirigiendo a selección de empresa");
                    return RedirectToAction("Index", "Empresas");
                }

                var ruta = await _context.RutasImpresora
                    .FirstOrDefaultAsync(r => r.Id == id && r.EmpresaId == empresaId);

                if (ruta == null)
                {
                    Console.WriteLine($"No se encontró ninguna ruta con Id={id} y EmpresaId={empresaId}");
                    return NotFound();
                }

                var estadoAnterior = ruta.Estado;
                ruta.Estado = !ruta.Estado;
                ruta.FechaModificacion = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
                Console.WriteLine($"Cambiando estado de ruta {ruta.Nombre} de {estadoAnterior} a {ruta.Estado}");

                _context.Update(ruta);
                
                var cambios = await _context.SaveChangesAsync();
                Console.WriteLine($"SaveChangesAsync completado, cambios realizados: {cambios}");
                Console.WriteLine($"Estado actualizado correctamente a {ruta.Estado}");
                
                // Agregar mensaje de éxito
                TempData["SuccessMessage"] = $"La ruta '{ruta.Nombre}' ha sido {(ruta.Estado ? "activada" : "desactivada")} correctamente";
                
                Console.WriteLine($"Redirigiendo a pestaña: {(ruta.Estado ? "Activos" : "Inactivos")}");
                return RedirectToAction(nameof(Index), new { tab = ruta.Estado ? "Activos" : "Inactivos" });
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
                Console.WriteLine("======= FIN TOGGLE ESTADO RUTA IMPRESORA =======");
            }
        }

        // Método auxiliar para quitar el estado predeterminado de otras rutas
        private async Task QuitarPredeterminadoDeOtrasRutas(int empresaId)
        {
            var rutasPredeterminadas = await _context.RutasImpresora
                .Where(r => r.EmpresaId == empresaId && r.Default)
                .ToListAsync();
                
            foreach (var ruta in rutasPredeterminadas)
            {
                ruta.Default = false;
                ruta.FechaModificacion = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
                _context.Update(ruta);
            }
            
            await _context.SaveChangesAsync();
            Console.WriteLine($"Se quitó el estado predeterminado de {rutasPredeterminadas.Count} rutas");
        }

        private bool RutaImpresoraExists(int id)
        {
            return _context.RutasImpresora.Any(e => e.Id == id);
        }
    }
} 