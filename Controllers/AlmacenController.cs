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
    public class AlmacenController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmpresaService _empresaService;

        public AlmacenController(
            ApplicationDbContext context,
            IEmpresaService empresaService)
        {
            _context = context;
            _empresaService = empresaService;
        }

        // GET: Almacen
        public async Task<IActionResult> Index(string tab)
        {
            Console.WriteLine($"======= INICIO INDEX ALMACEN =======");
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
            
            // Consultar todos los almacenes para verificar si existen datos
            var todosAlmacenes = await _context.Almacenes
                .Where(a => a.EmpresaId == empresaId)
                .ToListAsync();
                
            Console.WriteLine($"Total de almacenes para esta empresa: {todosAlmacenes.Count}");
            Console.WriteLine($"- Activos: {todosAlmacenes.Count(a => a.Estado)}");
            Console.WriteLine($"- Inactivos: {todosAlmacenes.Count(a => !a.Estado)}");
            
            // Consultar aplicando el filtro
            var almacenes = await _context.Almacenes
                .Where(a => a.Estado == activos && a.EmpresaId == empresaId)
                .ToListAsync();
                
            Console.WriteLine($"Almacenes filtrados obtenidos: {almacenes.Count}");
            
            if (almacenes.Count == 0 && todosAlmacenes.Count > 0)
            {
                // Si no hay resultados con el filtro pero sí hay almacenes en total,
                // sugerir al usuario cambiar de pestaña
                Console.WriteLine("No hay almacenes con el filtro actual, pero sí existen almacenes con otro estado");
                ViewBag.SugerirCambiarPestana = true;
                ViewBag.HayAlmacenesEnOtraPestana = activos ? "Inactivos" : "Activos";
            }
            
            Console.WriteLine("======= FIN INDEX ALMACEN =======");
            return View(almacenes);
        }

        // GET: Almacen/Create
        public async Task<IActionResult> Create()
        {
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            if (empresaId <= 0)
            {
                return RedirectToAction("Index", "Empresas");
            }

            var viewModel = new AlmacenViewModel
            {
                Estado = true,
                EmpresaId = empresaId
            };

            return View(viewModel);
        }

        // POST: Almacen/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AlmacenViewModel viewModel)
        {
            try
            {
                Console.WriteLine("======= INICIO CREATE ALMACEN =======");
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
                    Console.WriteLine("Verificando valores antes de crear Almacen:");
                    Console.WriteLine($"- Nombre: {viewModel.Nombre ?? "NULL"}, longitud: {viewModel.Nombre?.Length ?? 0}");
                    Console.WriteLine($"- CorreoElectronico: {viewModel.CorreoElectronico ?? "NULL"}, longitud: {viewModel.CorreoElectronico?.Length ?? 0}");
                    Console.WriteLine($"- Telefono: {viewModel.Telefono ?? "NULL"}, longitud: {viewModel.Telefono?.Length ?? 0}");
                    Console.WriteLine($"- Estado: {viewModel.Estado}");
                    Console.WriteLine($"- EmpresaId: {viewModel.EmpresaId}");
                    
                    // Crear una nueva instancia de Almacen con los valores del ViewModel
                    var almacen = new Almacen
                    {
                        Nombre = viewModel.Nombre ?? string.Empty,
                        CorreoElectronico = viewModel.CorreoElectronico ?? string.Empty,
                        Telefono = viewModel.Telefono ?? string.Empty,
                        Estado = viewModel.Estado,
                        EmpresaId = viewModel.EmpresaId,
                        FechaCreacion = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc)
                    };

                    Console.WriteLine($"Creando almacén: {almacen.Nombre}, Estado: {almacen.Estado}, EmpresaId: {almacen.EmpresaId}");
                    
                    // Verificar si hay algún problema con EmpresaId
                    if (almacen.EmpresaId <= 0)
                    {
                        Console.WriteLine("ERROR CRÍTICO: EmpresaId inválido en el objeto Almacen");
                        ModelState.AddModelError("", "La empresa seleccionada no es válida.");
                        return View(viewModel);
                    }
                    
                    // Agregar al contexto y guardar
                    _context.Almacenes.Add(almacen);
                    var result = await _context.SaveChangesAsync();
                    
                    Console.WriteLine($"SaveChangesAsync completado, cambios realizados: {result}");
                    Console.WriteLine($"Almacén guardado con éxito, Id: {almacen.Id}");
                    
                    // Después de guardar, verificar si se guardó correctamente
                    var almacenGuardado = await _context.Almacenes.FindAsync(almacen.Id);
                    if (almacenGuardado != null)
                    {
                        Console.WriteLine($"Verificación: Almacén encontrado en DB con Id: {almacenGuardado.Id}, Nombre: {almacenGuardado.Nombre}, Estado: {almacenGuardado.Estado}");
                        
                        // Agregar mensaje de éxito
                        TempData["SuccessMessage"] = $"El almacén '{almacenGuardado.Nombre}' ha sido creado correctamente";
                        
                        // Redireccionar a la pestaña correspondiente según el estado
                        return RedirectToAction(nameof(Index), new { tab = almacenGuardado.Estado ? "Activos" : "Inactivos" });
                    }
                    else
                    {
                        Console.WriteLine("ERROR: No se pudo encontrar el almacén recién creado en la base de datos");
                        ModelState.AddModelError("", "No se pudo guardar el almacén. Intente nuevamente.");
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
                Console.WriteLine($"ERROR al crear almacén: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                ModelState.AddModelError("", $"Error al guardar: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("======= FIN CREATE ALMACEN =======");
            }

            // Si llegamos aquí, hubo algún error, volvemos a la vista
            return View(viewModel);
        }

        // GET: Almacen/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            if (empresaId <= 0)
            {
                return RedirectToAction("Index", "Empresas");
            }

            var almacen = await _context.Almacenes
                .FirstOrDefaultAsync(a => a.Id == id && a.EmpresaId == empresaId);

            if (almacen == null)
                return NotFound();

            var viewModel = new AlmacenViewModel
            {
                Id = almacen.Id,
                Nombre = almacen.Nombre,
                CorreoElectronico = almacen.CorreoElectronico,
                Telefono = almacen.Telefono,
                Estado = almacen.Estado,
                EmpresaId = almacen.EmpresaId
            };

            return View(viewModel);
        }

        // POST: Almacen/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AlmacenViewModel viewModel)
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
                    var almacen = await _context.Almacenes
                        .FirstOrDefaultAsync(a => a.Id == id && a.EmpresaId == empresaId);
                    
                    if (almacen == null)
                        return NotFound();

                    almacen.Nombre = viewModel.Nombre ?? string.Empty;
                    almacen.CorreoElectronico = viewModel.CorreoElectronico ?? string.Empty;
                    almacen.Telefono = viewModel.Telefono ?? string.Empty;
                    almacen.Estado = viewModel.Estado;
                    almacen.FechaModificacion = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

                    _context.Update(almacen);
                    await _context.SaveChangesAsync();
                    
                    // Agregar mensaje de éxito
                    TempData["SuccessMessage"] = $"El almacén '{almacen.Nombre}' ha sido actualizado correctamente";
                    
                    return RedirectToAction(nameof(Index), new { tab = almacen.Estado ? "Activos" : "Inactivos" });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlmacenExists(viewModel.Id))
                        return NotFound();
                    else
                        throw;
                }
            }

            return View(viewModel);
        }

        // POST: Almacen/ToggleEstado/5
        [HttpPost]
        public async Task<IActionResult> ToggleEstado(int id)
        {
            Console.WriteLine($"======= INICIO TOGGLE ESTADO ALMACEN ID:{id} =======");
            try
            {
                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                Console.WriteLine($"EmpresaId obtenido: {empresaId}");

                if (empresaId <= 0)
                {
                    Console.WriteLine("EmpresaId inválido, redirigiendo a selección de empresa");
                    return RedirectToAction("Index", "Empresas");
                }

                var almacen = await _context.Almacenes
                    .FirstOrDefaultAsync(a => a.Id == id && a.EmpresaId == empresaId);

                if (almacen == null)
                {
                    Console.WriteLine($"No se encontró ningún almacén con Id={id} y EmpresaId={empresaId}");
                    return NotFound();
                }

                var estadoAnterior = almacen.Estado;
                almacen.Estado = !almacen.Estado;
                almacen.FechaModificacion = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
                Console.WriteLine($"Cambiando estado de almacén {almacen.Nombre} de {estadoAnterior} a {almacen.Estado}");

                _context.Update(almacen);
                
                var cambios = await _context.SaveChangesAsync();
                Console.WriteLine($"SaveChangesAsync completado, cambios realizados: {cambios}");
                Console.WriteLine($"Estado actualizado correctamente a {almacen.Estado}");
                
                // Agregar mensaje de éxito
                TempData["SuccessMessage"] = $"El almacén '{almacen.Nombre}' ha sido {(almacen.Estado ? "activado" : "desactivado")} correctamente";
                
                Console.WriteLine($"Redirigiendo a pestaña: {(almacen.Estado ? "Activos" : "Inactivos")}");
                return RedirectToAction(nameof(Index), new { tab = almacen.Estado ? "Activos" : "Inactivos" });
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
                Console.WriteLine("======= FIN TOGGLE ESTADO ALMACEN =======");
            }
        }

        private bool AlmacenExists(int id)
        {
            return _context.Almacenes.Any(e => e.Id == id);
        }
    }
} 