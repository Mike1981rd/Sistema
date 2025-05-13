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
using SistemaContable.Models.Enums;

namespace SistemaContable.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmpresaService _empresaService;

        public CategoriaController(
            ApplicationDbContext context,
            IEmpresaService empresaService)
        {
            _context = context;
            _empresaService = empresaService;
        }

        // GET: Categoria
        public async Task<IActionResult> Index(string tab)
        {
            Console.WriteLine($"======= INICIO INDEX CATEGORIA =======");
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
            
            // Consultar todas las categorías para verificar si existen datos
            var todasCategorias = await _context.Categorias
                .Where(c => c.EmpresaId == empresaId)
                .ToListAsync();
                
            Console.WriteLine($"Total de categorías para esta empresa: {todasCategorias.Count}");
            Console.WriteLine($"- Activas: {todasCategorias.Count(c => c.Estado)}");
            Console.WriteLine($"- Inactivas: {todasCategorias.Count(c => !c.Estado)}");
            
            // Consultar aplicando el filtro
            var categorias = await _context.Categorias
                .Include(c => c.Familia)
                .Include(c => c.CuentaVentas)
                .Include(c => c.CuentaComprasInventarios)
                .Include(c => c.CuentaCostoVentasGastos)
                .Include(c => c.CuentaDescuentos)
                .Include(c => c.CuentaDevoluciones)
                .Include(c => c.CuentaAjustes)
                .Include(c => c.CuentaCostoMateriaPrima)
                .Include(c => c.PropinaImpuesto)
                .Where(c => c.Estado == activos && c.EmpresaId == empresaId)
                .ToListAsync();
                
            Console.WriteLine($"Categorías filtradas obtenidas: {categorias.Count}");
            
            if (categorias.Count == 0 && todasCategorias.Count > 0)
            {
                // Si no hay resultados con el filtro pero sí hay categorías en total,
                // sugerir al usuario cambiar de pestaña
                Console.WriteLine("No hay categorías con el filtro actual, pero sí existen categorías con otro estado");
                ViewBag.SugerirCambiarPestana = true;
                ViewBag.HayCategoriaEnOtraPestana = activos ? "Inactivas" : "Activas";
            }
            
            Console.WriteLine("======= FIN INDEX CATEGORIA =======");
            return View(categorias);
        }

        // GET: Categoria/Create
        public async Task<IActionResult> Create()
        {
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            if (empresaId <= 0)
            {
                return RedirectToAction("Index", "Empresas");
            }

            var viewModel = new CategoriaViewModel
            {
                Estado = true,
                EmpresaId = empresaId
            };

            await CargarDatosFormulario(viewModel);
            return View(viewModel);
        }

        // POST: Categoria/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoriaViewModel viewModel)
        {
            try
            {
                Console.WriteLine("======= INICIO CREATE CATEGORIA =======");
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
                    Console.WriteLine("Verificando valores antes de crear Categoria:");
                    Console.WriteLine($"- Nombre: {viewModel.Nombre ?? "NULL"}, longitud: {viewModel.Nombre?.Length ?? 0}");
                    Console.WriteLine($"- Estado: {viewModel.Estado}");
                    Console.WriteLine($"- EmpresaId: {viewModel.EmpresaId}");
                    Console.WriteLine($"- FamiliaId: {viewModel.FamiliaId}");
                    
                    // Crear una nueva instancia de Categoria con los valores del ViewModel
                    var categoria = new Categoria
                    {
                        Nombre = viewModel.Nombre ?? string.Empty,
                        Nota = viewModel.Nota,
                        Estado = viewModel.Estado,
                        EmpresaId = viewModel.EmpresaId,
                        FamiliaId = viewModel.FamiliaId,
                        CuentaVentasId = viewModel.CuentaVentasId,
                        CuentaComprasInventariosId = viewModel.CuentaComprasInventariosId,
                        CuentaCostoVentasGastosId = viewModel.CuentaCostoVentasGastosId,
                        CuentaDescuentosId = viewModel.CuentaDescuentosId,
                        CuentaDevolucionesId = viewModel.CuentaDevolucionesId,
                        CuentaAjustesId = viewModel.CuentaAjustesId,
                        CuentaCostoMateriaPrimaId = viewModel.CuentaCostoMateriaPrimaId,
                        ImpuestoId = viewModel.ImpuestoId,
                        PropinaImpuestoId = viewModel.PropinaImpuestoId,
                        RutaImpresoraId = viewModel.RutaImpresoraId,
                        FechaCreacion = DateTime.UtcNow
                    };

                    Console.WriteLine($"Creando categoría: {categoria.Nombre}, Estado: {categoria.Estado}, EmpresaId: {categoria.EmpresaId}, FamiliaId: {categoria.FamiliaId}");
                    
                    // Verificar si hay algún problema con EmpresaId o FamiliaId
                    if (categoria.EmpresaId <= 0)
                    {
                        Console.WriteLine("ERROR CRÍTICO: EmpresaId inválido en el objeto Categoria");
                        ModelState.AddModelError("", "La empresa seleccionada no es válida.");
                        await CargarDatosFormulario(viewModel);
                        return View(viewModel);
                    }
                    
                    if (categoria.FamiliaId <= 0)
                    {
                        Console.WriteLine("ERROR CRÍTICO: FamiliaId inválido en el objeto Categoria");
                        ModelState.AddModelError("", "Debe seleccionar una familia (grupo).");
                        await CargarDatosFormulario(viewModel);
                        return View(viewModel);
                    }
                    
                    // Agregar al contexto y guardar
                    _context.Categorias.Add(categoria);
                    var result = await _context.SaveChangesAsync();
                    
                    Console.WriteLine($"SaveChangesAsync completado, cambios realizados: {result}");
                    Console.WriteLine($"Categoria guardada con éxito, Id: {categoria.Id}");
                    
                    // Después de guardar, verificar si se guardó correctamente
                    var categoriaGuardada = await _context.Categorias.FindAsync(categoria.Id);
                    if (categoriaGuardada != null)
                    {
                        Console.WriteLine($"Categoria guardada verificada: {categoriaGuardada.Nombre}");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        Console.WriteLine("ERROR: No se pudo verificar la categoría guardada");
                        ModelState.AddModelError("", "Error al guardar la categoría. Por favor, intente nuevamente.");
                    }
                }
                else
                {
                    Console.WriteLine("ModelState no es válido. Errores:");
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        Console.WriteLine($"- {error.ErrorMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR al crear categoría: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                ModelState.AddModelError("", $"Error al guardar: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("======= FIN CREATE CATEGORIA =======");
            }

            // Si llegamos aquí, hubo algún error, volvemos a la vista
            await CargarDatosFormulario(viewModel);
            return View(viewModel);
        }

        // GET: Categoria/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            if (empresaId <= 0)
            {
                return RedirectToAction("Index", "Empresas");
            }

            var categoria = await _context.Categorias
                .Include(c => c.Familia)
                .Include(c => c.CuentaVentas)
                .Include(c => c.CuentaComprasInventarios)
                .Include(c => c.CuentaCostoVentasGastos)
                .Include(c => c.CuentaDescuentos)
                .Include(c => c.CuentaDevoluciones)
                .Include(c => c.CuentaAjustes)
                .Include(c => c.CuentaCostoMateriaPrima)
                .FirstOrDefaultAsync(c => c.Id == id && c.EmpresaId == empresaId);

            if (categoria == null)
                return NotFound();

            var viewModel = new CategoriaViewModel
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre,
                Nota = categoria.Nota,
                Estado = categoria.Estado,
                EmpresaId = categoria.EmpresaId,
                FamiliaId = categoria.FamiliaId,
                CuentaVentasId = categoria.CuentaVentasId,
                CuentaComprasInventariosId = categoria.CuentaComprasInventariosId,
                CuentaCostoVentasGastosId = categoria.CuentaCostoVentasGastosId,
                CuentaDescuentosId = categoria.CuentaDescuentosId,
                CuentaDevolucionesId = categoria.CuentaDevolucionesId,
                CuentaAjustesId = categoria.CuentaAjustesId,
                CuentaCostoMateriaPrimaId = categoria.CuentaCostoMateriaPrimaId,
                ImpuestoId = categoria.ImpuestoId,
                PropinaImpuestoId = categoria.PropinaImpuestoId,
                RutaImpresoraId = categoria.RutaImpresoraId
            };

            await CargarDatosFormulario(viewModel);
            return View(viewModel);
        }

        // POST: Categoria/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoriaViewModel viewModel)
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
                    var categoria = await _context.Categorias
                        .FirstOrDefaultAsync(c => c.Id == id && c.EmpresaId == empresaId);
                    
                    if (categoria == null)
                        return NotFound();

                    categoria.Nombre = viewModel.Nombre ?? string.Empty;
                    categoria.Nota = viewModel.Nota;
                    categoria.Estado = viewModel.Estado;
                    categoria.FamiliaId = viewModel.FamiliaId;
                    categoria.CuentaVentasId = viewModel.CuentaVentasId;
                    categoria.CuentaComprasInventariosId = viewModel.CuentaComprasInventariosId;
                    categoria.CuentaCostoVentasGastosId = viewModel.CuentaCostoVentasGastosId;
                    categoria.CuentaDescuentosId = viewModel.CuentaDescuentosId;
                    categoria.CuentaDevolucionesId = viewModel.CuentaDevolucionesId;
                    categoria.CuentaAjustesId = viewModel.CuentaAjustesId;
                    categoria.CuentaCostoMateriaPrimaId = viewModel.CuentaCostoMateriaPrimaId;
                    categoria.ImpuestoId = viewModel.ImpuestoId;
                    categoria.PropinaImpuestoId = viewModel.PropinaImpuestoId;
                    categoria.RutaImpresoraId = viewModel.RutaImpresoraId;
                    categoria.FechaModificacion = DateTime.UtcNow;

                    _context.Update(categoria);
                    await _context.SaveChangesAsync();
                    
                    // Agregar mensaje de éxito
                    TempData["SuccessMessage"] = $"La categoría '{categoria.Nombre}' ha sido actualizada correctamente";
                    
                    return RedirectToAction(nameof(Index), new { tab = categoria.Estado ? "Activos" : "Inactivos" });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaExists(viewModel.Id))
                        return NotFound();
                    else
                        throw;
                }
            }

            await CargarDatosFormulario(viewModel);
            return View(viewModel);
        }

        // POST: Categoria/ToggleEstado/5
        [HttpPost]
        public async Task<IActionResult> ToggleEstado(int id)
        {
            Console.WriteLine($"======= INICIO TOGGLE ESTADO CATEGORIA ID:{id} =======");
            try
            {
                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                Console.WriteLine($"EmpresaId obtenido: {empresaId}");

                if (empresaId <= 0)
                {
                    Console.WriteLine("EmpresaId inválido, redirigiendo a selección de empresa");
                    return RedirectToAction("Index", "Empresas");
                }

                var categoria = await _context.Categorias
                    .FirstOrDefaultAsync(c => c.Id == id && c.EmpresaId == empresaId);

                if (categoria == null)
                {
                    Console.WriteLine($"No se encontró ninguna categoría con Id={id} y EmpresaId={empresaId}");
                    return NotFound();
                }

                var estadoAnterior = categoria.Estado;
                categoria.Estado = !categoria.Estado;
                categoria.FechaModificacion = DateTime.UtcNow;
                Console.WriteLine($"Cambiando estado de categoría {categoria.Nombre} de {estadoAnterior} a {categoria.Estado}");

                _context.Update(categoria);
                
                var cambios = await _context.SaveChangesAsync();
                Console.WriteLine($"SaveChangesAsync completado, cambios realizados: {cambios}");
                Console.WriteLine($"Estado actualizado correctamente a {categoria.Estado}");
                
                // Agregar mensaje de éxito
                TempData["SuccessMessage"] = $"La categoría '{categoria.Nombre}' ha sido {(categoria.Estado ? "activada" : "desactivada")} correctamente";
                
                Console.WriteLine($"Redirigiendo a pestaña: {(categoria.Estado ? "Activos" : "Inactivos")}");
                return RedirectToAction(nameof(Index), new { tab = categoria.Estado ? "Activos" : "Inactivos" });
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
                Console.WriteLine("======= FIN TOGGLE ESTADO CATEGORIA =======");
            }
        }

        private bool CategoriaExists(int id)
        {
            return _context.Categorias.Any(e => e.Id == id);
        }

        private async Task CargarDatosFormulario(CategoriaViewModel viewModel)
        {
            var empresaId = viewModel.EmpresaId;
            Console.WriteLine($"CargarDatosFormulario para EmpresaId: {empresaId}");
            
            // Cargar familias disponibles
            var familias = await _context.Familias
                .Where(f => f.Estado && f.EmpresaId == viewModel.EmpresaId)
                .OrderBy(f => f.Nombre)
                .Select(f => new { Id = f.Id, Nombre = f.Nombre })
                .ToListAsync();
                
            viewModel.FamiliasDisponibles = new SelectList(familias, "Id", "Nombre", viewModel.FamiliaId);
            
            // Cargar impuestos disponibles
            var impuestos = await _context.Impuestos
                .Where(i => i.EmpresaId == viewModel.EmpresaId)
                .OrderBy(i => i.Nombre)
                .Select(i => new { Id = i.Id, Nombre = i.Nombre })
                .ToListAsync();
                
            viewModel.ImpuestosDisponibles = new SelectList(impuestos, "Id", "Nombre", viewModel.ImpuestoId);

            // Cargar rutas de impresora disponibles
            var rutasImpresora = await _context.RutasImpresora
                .Where(r => r.EmpresaId == viewModel.EmpresaId)
                .OrderBy(r => r.Nombre)
                .Select(r => new { Id = r.Id, Nombre = r.Nombre })
                .ToListAsync();
                
            viewModel.RutasImpresoraDisponibles = new SelectList(rutasImpresora, "Id", "Nombre", viewModel.RutaImpresoraId);

            // NUEVO: Cargar propinas disponibles (solo impuestos de tipo "Propina")
            var propinasDb = await _context.Impuestos
                .Where(i => i.EmpresaId == viewModel.EmpresaId)
                .ToListAsync();

            // Imprimir todos los tipos para debugging
            Console.WriteLine("=== Tipos de impuestos disponibles ===");
            foreach (var imp in propinasDb)
            {
                Console.WriteLine($"Impuesto: {imp.Nombre}, Tipo: {imp.Tipo}");
            }

            var propinasSelect = propinasDb
                .Where(i => i.Tipo.ToString().Contains("Propina", StringComparison.OrdinalIgnoreCase) || 
                            i.Tipo.ToString() == "Propina legal")
                .OrderBy(i => i.Nombre)
                .Select(i => new { Id = i.Id, Nombre = $"{i.Nombre} {(i.Porcentaje.HasValue ? i.Porcentaje.Value.ToString() + "%" : "")}" })
                .ToList();

            // Imprimir propinas filtradas para debugging
            Console.WriteLine($"=== Propinas filtradas: {propinasSelect.Count} ===");
            foreach (var prop in propinasSelect)
            {
                Console.WriteLine($"ID: {prop.Id}, Nombre: {prop.Nombre}");
            }

            viewModel.PropinasDisponibles = new SelectList(propinasSelect, "Id", "Nombre", viewModel.PropinaImpuestoId);
            
            // Cargar cuentas contables disponibles
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
            
            // Si se ha seleccionado una familia y es edición (no creación), cargar cuentas de la familia
            if (viewModel.FamiliaId > 0 && viewModel.Id == 0)
            {
                var familia = await _context.Familias
                    .FirstOrDefaultAsync(f => f.Id == viewModel.FamiliaId && f.EmpresaId == viewModel.EmpresaId);
                    
                if (familia != null)
                {
                    // Solo asignar si no se ha seleccionado un valor específico
                    if (viewModel.CuentaVentasId == null)
                        viewModel.CuentaVentasId = familia.CuentaVentasId;
                    if (viewModel.CuentaComprasInventariosId == null)
                        viewModel.CuentaComprasInventariosId = familia.CuentaComprasInventariosId;
                    if (viewModel.CuentaCostoVentasGastosId == null)
                        viewModel.CuentaCostoVentasGastosId = familia.CuentaCostoVentasGastosId;
                    if (viewModel.CuentaDescuentosId == null)
                        viewModel.CuentaDescuentosId = familia.CuentaDescuentosId;
                    if (viewModel.CuentaDevolucionesId == null)
                        viewModel.CuentaDevolucionesId = familia.CuentaDevolucionesId;
                    if (viewModel.CuentaAjustesId == null)
                        viewModel.CuentaAjustesId = familia.CuentaAjustesId;
                    if (viewModel.CuentaCostoMateriaPrimaId == null)
                        viewModel.CuentaCostoMateriaPrimaId = familia.CuentaCostoMateriaPrimaId;
                }
            }
        }

        // GET: Categoria/ObtenerTodas
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

                var categorias = await _context.Categorias
                    .Where(c => c.EmpresaId == empresaId && c.Estado)
                    .OrderBy(c => c.Nombre)
                    .Select(c => new { 
                        id = c.Id, 
                        nombre = c.Nombre,
                        familiaId = c.FamiliaId,
                        familiaName = c.Familia.Nombre
                    })
                    .ToListAsync();

                return Json(categorias);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ObtenerTodas: {ex.Message}");
                return Json(new { error = ex.Message });
            }
        }

        // GET: Categoria/ObtenerCuentasContablesFamilia/5
        [HttpGet]
        public async Task<IActionResult> ObtenerCuentasContablesFamilia(int familiaId)
        {
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            if (empresaId <= 0 || familiaId <= 0)
            {
                return Json(new { });
            }

            var familia = await _context.Familias
                .FirstOrDefaultAsync(f => f.Id == familiaId && f.EmpresaId == empresaId);
            
            if (familia == null)
            {
                return Json(new { });
            }
            
            return Json(new {
                cuentaVentasId = familia.CuentaVentasId,
                cuentaComprasInventariosId = familia.CuentaComprasInventariosId,
                cuentaCostoVentasGastosId = familia.CuentaCostoVentasGastosId,
                cuentaDescuentosId = familia.CuentaDescuentosId,
                cuentaDevolucionesId = familia.CuentaDevolucionesId,
                cuentaAjustesId = familia.CuentaAjustesId,
                cuentaCostoMateriaPrimaId = familia.CuentaCostoMateriaPrimaId
            });
        }
        
        // GET: Categoria/CreatePartial
        [HttpGet]
        public async Task<IActionResult> CreatePartial()
        {
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            if (empresaId <= 0)
            {
                return Json(new { success = false, message = "Empresa no seleccionada" });
            }

            var viewModel = new CategoriaViewModel
            {
                Estado = true,
                EmpresaId = empresaId
            };

            await CargarDatosFormulario(viewModel);
            return PartialView("_CreatePartial", viewModel);
        }
        
        // POST: Categoria/CreatePartial
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> CreatePartial(CategoriaViewModel viewModel)
        {
            try
            {
                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                if (empresaId <= 0)
                {
                    return Json(new { success = false, message = "Empresa no seleccionada" });
                }

                viewModel.EmpresaId = empresaId;

                if (ModelState.IsValid)
                {
                    var categoria = new Categoria
                    {
                        Nombre = viewModel.Nombre ?? string.Empty,
                        Nota = viewModel.Nota,
                        Estado = viewModel.Estado,
                        EmpresaId = viewModel.EmpresaId,
                        FamiliaId = viewModel.FamiliaId,
                        CuentaVentasId = viewModel.CuentaVentasId,
                        CuentaComprasInventariosId = viewModel.CuentaComprasInventariosId,
                        CuentaCostoVentasGastosId = viewModel.CuentaCostoVentasGastosId,
                        CuentaDescuentosId = viewModel.CuentaDescuentosId,
                        CuentaDevolucionesId = viewModel.CuentaDevolucionesId,
                        CuentaAjustesId = viewModel.CuentaAjustesId,
                        CuentaCostoMateriaPrimaId = viewModel.CuentaCostoMateriaPrimaId,
                        ImpuestoId = viewModel.ImpuestoId,
                        PropinaImpuestoId = viewModel.PropinaImpuestoId,
                        RutaImpresoraId = viewModel.RutaImpresoraId,
                        FechaCreacion = DateTime.UtcNow
                    };
                    
                    _context.Categorias.Add(categoria);
                    await _context.SaveChangesAsync();
                    
                    return Json(new { 
                        success = true, 
                        message = "Categoría creada con éxito", 
                        categoria = new { 
                            id = categoria.Id, 
                            nombre = categoria.Nombre 
                        } 
                    });
                }
                else
                {
                    return Json(new { 
                        success = false, 
                        message = "Error al crear categoría", 
                        errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() 
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }
        
        // GET: Categoria/EditPartial/5
        [HttpGet]
        public async Task<IActionResult> EditPartial(int id)
        {
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            if (empresaId <= 0)
            {
                return Json(new { success = false, message = "Empresa no seleccionada" });
            }

            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(c => c.Id == id && c.EmpresaId == empresaId);
                
            if (categoria == null)
            {
                return Json(new { success = false, message = "Categoría no encontrada" });
            }

            var viewModel = new CategoriaViewModel
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre,
                Nota = categoria.Nota,
                Estado = categoria.Estado,
                EmpresaId = categoria.EmpresaId,
                FamiliaId = categoria.FamiliaId,
                CuentaVentasId = categoria.CuentaVentasId,
                CuentaComprasInventariosId = categoria.CuentaComprasInventariosId,
                CuentaCostoVentasGastosId = categoria.CuentaCostoVentasGastosId,
                CuentaDescuentosId = categoria.CuentaDescuentosId,
                CuentaDevolucionesId = categoria.CuentaDevolucionesId,
                CuentaAjustesId = categoria.CuentaAjustesId,
                CuentaCostoMateriaPrimaId = categoria.CuentaCostoMateriaPrimaId,
                ImpuestoId = categoria.ImpuestoId,
                PropinaImpuestoId = categoria.PropinaImpuestoId,
                RutaImpresoraId = categoria.RutaImpresoraId
            };

            await CargarDatosFormulario(viewModel);
            return PartialView("_EditPartial", viewModel);
        }
        
        // POST: Categoria/EditPartial/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> EditPartial(int id, CategoriaViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return Json(new { success = false, message = "ID de categoría no válido" });
            }

            try
            {
                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                if (empresaId <= 0)
                {
                    return Json(new { success = false, message = "Empresa no seleccionada" });
                }

                var categoria = await _context.Categorias
                    .FirstOrDefaultAsync(c => c.Id == id && c.EmpresaId == empresaId);
                    
                if (categoria == null)
                {
                    return Json(new { success = false, message = "Categoría no encontrada" });
                }

                if (ModelState.IsValid)
                {
                    categoria.Nombre = viewModel.Nombre ?? string.Empty;
                    categoria.Nota = viewModel.Nota;
                    categoria.Estado = viewModel.Estado;
                    categoria.FamiliaId = viewModel.FamiliaId;
                    categoria.CuentaVentasId = viewModel.CuentaVentasId;
                    categoria.CuentaComprasInventariosId = viewModel.CuentaComprasInventariosId;
                    categoria.CuentaCostoVentasGastosId = viewModel.CuentaCostoVentasGastosId;
                    categoria.CuentaDescuentosId = viewModel.CuentaDescuentosId;
                    categoria.CuentaDevolucionesId = viewModel.CuentaDevolucionesId;
                    categoria.CuentaAjustesId = viewModel.CuentaAjustesId;
                    categoria.CuentaCostoMateriaPrimaId = viewModel.CuentaCostoMateriaPrimaId;
                    categoria.ImpuestoId = viewModel.ImpuestoId;
                    categoria.PropinaImpuestoId = viewModel.PropinaImpuestoId;
                    categoria.RutaImpresoraId = viewModel.RutaImpresoraId;
                    categoria.FechaModificacion = DateTime.UtcNow;

                    _context.Update(categoria);
                    await _context.SaveChangesAsync();
                    
                    return Json(new { 
                        success = true, 
                        message = "Categoría actualizada con éxito", 
                        categoria = new { 
                            id = categoria.Id, 
                            nombre = categoria.Nombre 
                        } 
                    });
                }
                else
                {
                    return Json(new { 
                        success = false, 
                        message = "Error al actualizar categoría", 
                        errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() 
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        /**
         * Obtiene los detalles completos de una categoría incluida la información de impuestos y cuentas contables
         */
        [HttpGet]
        public async Task<JsonResult> ObtenerDetalle(int id)
        {
            try
            {
                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                if (empresaId <= 0)
                {
                    return Json(new { success = false, message = "Empresa no seleccionada" });
                }

                // Obtener la categoría con información completa incluyendo referencias
                var categoria = await _context.Categorias
                    .Include(c => c.CuentaVentas)
                    .Include(c => c.CuentaComprasInventarios)
                    .Include(c => c.CuentaCostoVentasGastos)
                    .Include(c => c.CuentaDescuentos)
                    .Include(c => c.CuentaDevoluciones)
                    .Include(c => c.CuentaAjustes)
                    .Include(c => c.CuentaCostoMateriaPrima)
                    .Where(c => c.Id == id && c.EmpresaId == empresaId)
                    .Select(c => new {
                        id = c.Id,
                        nombre = c.Nombre,
                        nota = c.Nota,
                        familiaId = c.FamiliaId,
                        impuestoId = c.ImpuestoId,
                        propinaImpuestoId = c.PropinaImpuestoId,
                        // IDs de cuentas
                        cuentaVentasId = c.CuentaVentasId,
                        cuentaComprasInventariosId = c.CuentaComprasInventariosId,
                        cuentaCostoVentasGastosId = c.CuentaCostoVentasGastosId,
                        cuentaDescuentosId = c.CuentaDescuentosId,
                        cuentaDevolucionesId = c.CuentaDevolucionesId,
                        cuentaAjustesId = c.CuentaAjustesId,
                        cuentaCostoMateriaPrimaId = c.CuentaCostoMateriaPrimaId,
                        rutaImpresoraId = c.RutaImpresoraId,
                        // Nombres de cuentas para usar en Select2
                        nombreCuentaVentas = c.CuentaVentas != null ? $"{c.CuentaVentas.Codigo} - {c.CuentaVentas.Nombre}" : null,
                        nombreCuentaComprasInventarios = c.CuentaComprasInventarios != null ? $"{c.CuentaComprasInventarios.Codigo} - {c.CuentaComprasInventarios.Nombre}" : null,
                        nombreCuentaCostoVentasGastos = c.CuentaCostoVentasGastos != null ? $"{c.CuentaCostoVentasGastos.Codigo} - {c.CuentaCostoVentasGastos.Nombre}" : null,
                        nombreCuentaDescuentos = c.CuentaDescuentos != null ? $"{c.CuentaDescuentos.Codigo} - {c.CuentaDescuentos.Nombre}" : null,
                        nombreCuentaDevoluciones = c.CuentaDevoluciones != null ? $"{c.CuentaDevoluciones.Codigo} - {c.CuentaDevoluciones.Nombre}" : null,
                        nombreCuentaAjustes = c.CuentaAjustes != null ? $"{c.CuentaAjustes.Codigo} - {c.CuentaAjustes.Nombre}" : null,
                        nombreCuentaCostoMateriaPrima = c.CuentaCostoMateriaPrima != null ? $"{c.CuentaCostoMateriaPrima.Codigo} - {c.CuentaCostoMateriaPrima.Nombre}" : null
                    })
                    .FirstOrDefaultAsync();

                if (categoria == null)
                {
                    return Json(new { success = false, message = "Categoría no encontrada" });
                }

                return Json(new { success = true, categoria });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ObtenerDetalle: {ex.Message}");
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: Categoria/Buscar
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

                var categorias = await _context.Categorias
                    .Where(c => c.EmpresaId == empresaId && c.Estado && 
                          (string.IsNullOrEmpty(term) || c.Nombre.Contains(term)))
                    .OrderBy(c => c.Nombre)
                    .Select(c => new { 
                        id = c.Id, 
                        text = c.Nombre
                    })
                    .ToListAsync();

                return Json(new { results = categorias });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Buscar: {ex.Message}");
                return Json(new { results = new List<object>() });
            }
        }
    }
} 