using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaContable.Data;
using SistemaContable.Models;
using SistemaContable.ViewModels;
using SistemaContable.Services;
using Microsoft.EntityFrameworkCore;

namespace SistemaContable.Controllers
{
    public class FamiliaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmpresaService _empresaService;

        public FamiliaController(
            ApplicationDbContext context,
            IEmpresaService empresaService)
        {
            _context = context;
            _empresaService = empresaService;
        }

        // GET: Familia
        public async Task<IActionResult> Index(string tab)
        {
            Console.WriteLine($"======= INICIO INDEX FAMILIA =======");
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
            
            // Consultar todas las familias para verificar si existen datos
            var todasFamilias = await _context.Familias
                .Where(f => f.EmpresaId == empresaId)
                .ToListAsync();
                
            Console.WriteLine($"Total de familias para esta empresa: {todasFamilias.Count}");
            Console.WriteLine($"- Activas: {todasFamilias.Count(f => f.Estado)}");
            Console.WriteLine($"- Inactivas: {todasFamilias.Count(f => !f.Estado)}");
            
            // Consultar aplicando el filtro
            var familias = await _context.Familias
                .Include(f => f.CuentaVentas)
                .Include(f => f.CuentaComprasInventarios)
                .Include(f => f.CuentaCostoVentasGastos)
                .Include(f => f.CuentaDescuentos)
                .Include(f => f.CuentaDevoluciones)
                .Include(f => f.CuentaAjustes)
                .Include(f => f.CuentaCostoMateriaPrima)
                .Where(f => f.Estado == activos && f.EmpresaId == empresaId)
                .ToListAsync();
                
            Console.WriteLine($"Familias filtradas obtenidas: {familias.Count}");
            
            if (familias.Count == 0 && todasFamilias.Count > 0)
            {
                // Si no hay resultados con el filtro pero sí hay familias en total,
                // sugerir al usuario cambiar de pestaña
                Console.WriteLine("No hay familias con el filtro actual, pero sí existen familias con otro estado");
                ViewBag.SugerirCambiarPestana = true;
                ViewBag.HayFamiliasEnOtraPestana = activos ? "Inactivas" : "Activas";
            }
            
            Console.WriteLine("======= FIN INDEX FAMILIA =======");
            return View(familias);
        }

        // GET: Familia/Create
        public async Task<IActionResult> Create()
        {
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            if (empresaId <= 0)
            {
                return RedirectToAction("Index", "Empresas");
            }

            var viewModel = new FamiliaViewModel
            {
                Estado = true,
                EmpresaId = empresaId
            };

            await CargarCuentasContablesDisponibles(viewModel);
            return View(viewModel);
        }

        // POST: Familia/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FamiliaViewModel viewModel)
        {
            try
            {
                Console.WriteLine("======= INICIO CREATE FAMILIA =======");
                Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");

                // Verificar qué datos llegaron del formulario
                Console.WriteLine("DATOS RECIBIDOS DEL FORMULARIO:");
                Console.WriteLine($"Nombre: '{viewModel.Nombre ?? "NULL"}'");
                Console.WriteLine($"Nota: '{viewModel.Nota ?? "NULL"}'");
                Console.WriteLine($"Estado: {viewModel.Estado}");
                Console.WriteLine($"EmpresaId: {viewModel.EmpresaId}");
                Console.WriteLine($"CuentaVentasId: {viewModel.CuentaVentasId}");
                Console.WriteLine($"CuentaComprasInventariosId: {viewModel.CuentaComprasInventariosId}");
                Console.WriteLine($"CuentaCostoVentasGastosId: {viewModel.CuentaCostoVentasGastosId}");
                Console.WriteLine($"CuentaDescuentosId: {viewModel.CuentaDescuentosId}");
                Console.WriteLine($"CuentaDevolucionesId: {viewModel.CuentaDevolucionesId}");
                Console.WriteLine($"CuentaAjustesId: {viewModel.CuentaAjustesId}");
                Console.WriteLine($"CuentaCostoMateriaPrimaId: {viewModel.CuentaCostoMateriaPrimaId}");

                // Listar todos los errores de ModelState si existen
                if (!ModelState.IsValid)
                {
                    Console.WriteLine("ERRORES DE MODEL STATE:");
                    foreach (var state in ModelState)
                    {
                        Console.WriteLine($"- Campo: {state.Key}");
                        foreach (var error in state.Value.Errors)
                        {
                            Console.WriteLine($"  - Error: {error.ErrorMessage}");
                            if (error.Exception != null)
                            {
                                Console.WriteLine($"  - Excepción: {error.Exception.Message}");
                            }
                        }
                    }
                }

                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                Console.WriteLine($"EmpresaId obtenido del servicio: {empresaId}, es cero o negativo: {empresaId <= 0}");

                if (empresaId <= 0)
                {
                    Console.WriteLine("ADVERTENCIA: EmpresaId inválido, redirigiendo a selección de empresa");
                    TempData["ErrorMessage"] = "No hay empresa seleccionada. Por favor, seleccione una empresa primero.";
                    return RedirectToAction("Index", "Empresas");
                }

                viewModel.EmpresaId = empresaId;
                Console.WriteLine($"EmpresaId asignado al viewModel: {viewModel.EmpresaId}");

                if (ModelState.IsValid)
                {
                    // Verificar explícitamente todos los valores para diagnosticar problemas de NULL
                    Console.WriteLine("Verificando valores antes de crear Familia:");
                    Console.WriteLine($"- Nombre: {viewModel.Nombre ?? "NULL"}, longitud: {viewModel.Nombre?.Length ?? 0}");
                    Console.WriteLine($"- Estado: {viewModel.Estado}");
                    Console.WriteLine($"- EmpresaId: {viewModel.EmpresaId}");
                    
                    // Crear una nueva instancia de Familia con los valores del ViewModel
                    var familia = new Familia
                    {
                        Nombre = viewModel.Nombre,
                        Nota = viewModel.Nota,
                        Estado = viewModel.Estado,
                        EmpresaId = viewModel.EmpresaId,
                        CuentaVentasId = viewModel.CuentaVentasId,
                        CuentaComprasInventariosId = viewModel.CuentaComprasInventariosId,
                        CuentaCostoVentasGastosId = viewModel.CuentaCostoVentasGastosId,
                        CuentaDescuentosId = viewModel.CuentaDescuentosId,
                        CuentaDevolucionesId = viewModel.CuentaDevolucionesId,
                        CuentaAjustesId = viewModel.CuentaAjustesId,
                        CuentaCostoMateriaPrimaId = viewModel.CuentaCostoMateriaPrimaId,
                        FechaCreacion = DateTime.UtcNow,
                        FechaModificacion = null  // Inicialmente nulo hasta que se edite
                    };

                    Console.WriteLine($"Objeto familia creado: {familia.Nombre}, Estado: {familia.Estado}, EmpresaId: {familia.EmpresaId}");
                    
                    // Verificar si hay algún problema con EmpresaId
                    if (familia.EmpresaId <= 0)
                    {
                        Console.WriteLine("ERROR CRÍTICO: EmpresaId inválido en el objeto Familia");
                        ModelState.AddModelError("", "La empresa seleccionada no es válida.");
                        await CargarCuentasContablesDisponibles(viewModel);
                        return View(viewModel);
                    }
                    
                    try {
                        // Marcar como modo detallado para mejor seguimiento
                        Console.WriteLine("INICIO PROCESO DE GUARDADO EN DB ----------------");
                        
                        // Agregar al contexto
                        _context.Familias.Add(familia);
                        Console.WriteLine("Familia agregada al contexto");
                        
                        // Guardar y capturar cambios
                        Console.WriteLine("Llamando a SaveChangesAsync()...");
                        var result = await _context.SaveChangesAsync();
                        
                        Console.WriteLine($"SaveChangesAsync completado, cambios realizados: {result}");
                        Console.WriteLine($"Familia guardada con éxito, Id: {familia.Id}");
                        Console.WriteLine("FIN PROCESO DE GUARDADO EN DB ----------------");
                        
                        // Después de guardar, verificar si se guardó correctamente
                        var familiaGuardada = await _context.Familias.FindAsync(familia.Id);
                        if (familiaGuardada != null)
                        {
                            Console.WriteLine($"Verificación: Familia encontrada en DB con Id: {familiaGuardada.Id}, Nombre: {familiaGuardada.Nombre}, Estado: {familiaGuardada.Estado}");
                            
                            // Agregar mensaje de éxito
                            TempData["SuccessMessage"] = $"La familia '{familiaGuardada.Nombre}' ha sido creada correctamente";
                            
                            // Redireccionar a la pestaña correspondiente según el estado
                            Console.WriteLine($"Redirigiendo a Index con tab={familiaGuardada.Estado}");
                            return RedirectToAction(nameof(Index), new { tab = familiaGuardada.Estado ? "Activos" : "Inactivos" });
                        }
                        else
                        {
                            Console.WriteLine("ERROR: No se pudo encontrar la familia recién creada en la base de datos");
                            ModelState.AddModelError("", "No se pudo guardar la familia. Intente nuevamente.");
                            TempData["ErrorMessage"] = "No se pudo encontrar la familia después de guardar.";
                        }
                    } 
                    catch (Exception dbEx) 
                    {
                        Console.WriteLine($"ERROR de base de datos al guardar: {dbEx.Message}");
                        if (dbEx.InnerException != null) {
                            Console.WriteLine($"Inner Exception: {dbEx.InnerException.Message}");
                            Console.WriteLine($"StackTrace: {dbEx.InnerException.StackTrace}");
                        }
                        Console.WriteLine($"StackTrace: {dbEx.StackTrace}");
                        ModelState.AddModelError("", $"Error al guardar en la base de datos: {dbEx.Message}");
                        TempData["ErrorMessage"] = $"Error de base de datos: {dbEx.Message}";
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
                    TempData["ErrorMessage"] = "El formulario contiene errores. Por favor, revise los campos marcados.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR GENERAL al crear familia: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                ModelState.AddModelError("", $"Error al guardar: {ex.Message}");
                TempData["ErrorMessage"] = $"Error inesperado: {ex.Message}";
            }
            finally
            {
                Console.WriteLine("======= FIN CREATE FAMILIA =======");
            }

            // Si llegamos aquí, hubo algún error, volvemos a la vista
            await CargarCuentasContablesDisponibles(viewModel);
            return View(viewModel);
        }

        // Método auxiliar para crear parámetros de comando
        private System.Data.Common.DbParameter CreateParameter(System.Data.Common.DbCommand command, string name, object value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value ?? DBNull.Value;
            return parameter;
        }

        // GET: Familia/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            if (empresaId <= 0)
            {
                return RedirectToAction("Index", "Empresas");
            }

            var familia = await _context.Familias
                .Include(f => f.CuentaVentas)
                .Include(f => f.CuentaComprasInventarios)
                .Include(f => f.CuentaCostoVentasGastos)
                .Include(f => f.CuentaDescuentos)
                .Include(f => f.CuentaDevoluciones)
                .Include(f => f.CuentaAjustes)
                .Include(f => f.CuentaCostoMateriaPrima)
                .FirstOrDefaultAsync(f => f.Id == id && f.EmpresaId == empresaId);

            if (familia == null)
                return NotFound();

            var viewModel = new FamiliaViewModel
            {
                Id = familia.Id,
                Nombre = familia.Nombre,
                Nota = familia.Nota,
                Estado = familia.Estado,
                EmpresaId = familia.EmpresaId,
                CuentaVentasId = familia.CuentaVentasId,
                CuentaComprasInventariosId = familia.CuentaComprasInventariosId,
                CuentaCostoVentasGastosId = familia.CuentaCostoVentasGastosId,
                CuentaDescuentosId = familia.CuentaDescuentosId,
                CuentaDevolucionesId = familia.CuentaDevolucionesId,
                CuentaAjustesId = familia.CuentaAjustesId,
                CuentaCostoMateriaPrimaId = familia.CuentaCostoMateriaPrimaId
            };

            await CargarCuentasContablesDisponibles(viewModel);
            return View(viewModel);
        }

        // POST: Familia/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FamiliaViewModel viewModel)
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
                    var familia = await _context.Familias
                        .FirstOrDefaultAsync(f => f.Id == id && f.EmpresaId == empresaId);
                    
                    if (familia == null)
                        return NotFound();

                    familia.Nombre = viewModel.Nombre;
                    familia.Nota = viewModel.Nota;
                    familia.Estado = viewModel.Estado;
                    familia.CuentaVentasId = viewModel.CuentaVentasId;
                    familia.CuentaComprasInventariosId = viewModel.CuentaComprasInventariosId;
                    familia.CuentaCostoVentasGastosId = viewModel.CuentaCostoVentasGastosId;
                    familia.CuentaDescuentosId = viewModel.CuentaDescuentosId;
                    familia.CuentaDevolucionesId = viewModel.CuentaDevolucionesId;
                    familia.CuentaAjustesId = viewModel.CuentaAjustesId;
                    familia.CuentaCostoMateriaPrimaId = viewModel.CuentaCostoMateriaPrimaId;

                    _context.Update(familia);
                    await _context.SaveChangesAsync();
                    
                    // Agregar mensaje de éxito
                    TempData["SuccessMessage"] = $"La familia '{familia.Nombre}' ha sido actualizada correctamente";
                    
                    return RedirectToAction(nameof(Index), new { tab = familia.Estado ? "Activos" : "Inactivos" });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FamiliaExists(viewModel.Id))
                        return NotFound();
                    else
                        throw;
                }
            }

            await CargarCuentasContablesDisponibles(viewModel);
            return View(viewModel);
        }

        // POST: Familia/ToggleEstado/5
        [HttpPost]
        public async Task<IActionResult> ToggleEstado(int id)
        {
            Console.WriteLine($"======= INICIO TOGGLE ESTADO FAMILIA ID:{id} =======");
            try
            {
                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                Console.WriteLine($"EmpresaId obtenido: {empresaId}");

                if (empresaId <= 0)
                {
                    Console.WriteLine("EmpresaId inválido, redirigiendo a selección de empresa");
                    return RedirectToAction("Index", "Empresas");
                }

                var familia = await _context.Familias
                    .FirstOrDefaultAsync(f => f.Id == id && f.EmpresaId == empresaId);

                if (familia == null)
                {
                    Console.WriteLine($"No se encontró ninguna familia con Id={id} y EmpresaId={empresaId}");
                    return NotFound();
                }

                var estadoAnterior = familia.Estado;
                familia.Estado = !familia.Estado;
                Console.WriteLine($"Cambiando estado de familia {familia.Nombre} de {estadoAnterior} a {familia.Estado}");

                _context.Update(familia);
                
                var cambios = await _context.SaveChangesAsync();
                Console.WriteLine($"SaveChangesAsync completado, cambios realizados: {cambios}");
                Console.WriteLine($"Estado actualizado correctamente a {familia.Estado}");
                
                // Agregar mensaje de éxito
                TempData["SuccessMessage"] = $"La familia '{familia.Nombre}' ha sido {(familia.Estado ? "activada" : "desactivada")} correctamente";
                
                Console.WriteLine($"Redirigiendo a pestaña: {(familia.Estado ? "Activos" : "Inactivos")}");
                return RedirectToAction(nameof(Index), new { tab = familia.Estado ? "Activos" : "Inactivos" });
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
                Console.WriteLine("======= FIN TOGGLE ESTADO FAMILIA =======");
            }
        }

        private bool FamiliaExists(int id)
        {
            return _context.Familias.Any(e => e.Id == id);
        }

        private async Task CargarCuentasContablesDisponibles(FamiliaViewModel viewModel)
        {
            var cuentasContables = await _context.CuentasContables
                .Where(c => c.Activo && c.EmpresaId == viewModel.EmpresaId)
                .OrderBy(c => c.Codigo)
                .Select(c => new { Id = c.Id, Descripcion = $"{c.Codigo} - {c.Descripcion ?? c.Nombre}" })
                .ToListAsync();

            viewModel.CuentasVentasDisponibles = new SelectList(cuentasContables, "Id", "Descripcion", viewModel.CuentaVentasId);
            viewModel.CuentasComprasInventariosDisponibles = new SelectList(cuentasContables, "Id", "Descripcion", viewModel.CuentaComprasInventariosId);
            viewModel.CuentasCostoVentasGastosDisponibles = new SelectList(cuentasContables, "Id", "Descripcion", viewModel.CuentaCostoVentasGastosId);
            viewModel.CuentasDescuentosDisponibles = new SelectList(cuentasContables, "Id", "Descripcion", viewModel.CuentaDescuentosId);
            viewModel.CuentasDevolucionesDisponibles = new SelectList(cuentasContables, "Id", "Descripcion", viewModel.CuentaDevolucionesId);
            viewModel.CuentasAjustesDisponibles = new SelectList(cuentasContables, "Id", "Descripcion", viewModel.CuentaAjustesId);
            viewModel.CuentasCostoMateriaPrimaDisponibles = new SelectList(cuentasContables, "Id", "Descripcion", viewModel.CuentaCostoMateriaPrimaId);
        }

        // Test action for debugging
        [HttpGet]
        public async Task<IActionResult> TestEstado(bool estado)
        {
            try
            {
                Console.WriteLine($"======= INICIO TEST ESTADO: {estado} =======");
                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                Console.WriteLine($"EmpresaId obtenido: {empresaId}, es cero o negativo: {empresaId <= 0}");
                
                if (empresaId <= 0)
                {
                    Console.WriteLine("ERROR: No hay empresa seleccionada");
                    return Content("Error: No hay empresa seleccionada");
                }
                
                // Crear una familia de prueba con el estado especificado
                var familia = new Familia
                {
                    Nombre = $"Test Estado {(estado ? "Activo" : "Inactivo")} {DateTime.Now.Ticks}",
                    Estado = estado,
                    EmpresaId = empresaId,
                    FechaCreacion = DateTime.UtcNow,
                    FechaModificacion = DateTime.UtcNow
                };
                
                Console.WriteLine($"Creando familia de prueba: {familia.Nombre}, Estado: {familia.Estado}, EmpresaId: {familia.EmpresaId}");
                
                // Guardar en la base de datos
                _context.Familias.Add(familia);
                var result = await _context.SaveChangesAsync();
                Console.WriteLine($"SaveChangesAsync completado, cambios realizados: {result}");
                
                // Verificar si se guardó correctamente
                var familiaGuardada = await _context.Familias
                    .FirstOrDefaultAsync(f => f.Id == familia.Id && f.EmpresaId == empresaId);
                
                if (familiaGuardada != null)
                {
                    string resultado = $"Familia de prueba creada con éxito: Id={familiaGuardada.Id}, " +
                                      $"Nombre={familiaGuardada.Nombre}, " +
                                      $"Estado={familiaGuardada.Estado}, " +
                                      $"EmpresaId={familiaGuardada.EmpresaId}";
                    
                    Console.WriteLine(resultado);
                    Console.WriteLine($"Realizando redirección a Index con tab={familiaGuardada.Estado}");
                    
                    // Opción para redireccionar directamente a la pestaña correspondiente
                    if (Request.Query.ContainsKey("redirect") && Request.Query["redirect"] == "true")
                    {
                        return RedirectToAction(nameof(Index), new { tab = familiaGuardada.Estado ? "Activos" : "Inactivos" });
                    }
                    
                    return Content(resultado);
                }
                else
                {
                    string error = $"No se pudo encontrar la familia después de guardarla. Id={familia.Id}, EmpresaId={empresaId}";
                    Console.WriteLine(error);
                    
                    // Buscar sin filtro de empresa para ver si se guardó con otro empresaId
                    var familiaAnyEmpresa = await _context.Familias.FindAsync(familia.Id);
                    if (familiaAnyEmpresa != null)
                    {
                        Console.WriteLine($"ENCONTRADA con otro EmpresaId: Id={familiaAnyEmpresa.Id}, EmpresaId={familiaAnyEmpresa.EmpresaId}");
                    }
                    
                    return Content(error);
                }
            }
            catch (Exception ex)
            {
                string error = $"Error al crear familia de prueba: {ex.Message}";
                Console.WriteLine(error);
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                return Content(error);
            }
            finally
            {
                Console.WriteLine("======= FIN TEST ESTADO =======");
            }
        }
    }
}
