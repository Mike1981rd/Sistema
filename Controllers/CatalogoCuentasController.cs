using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using SistemaContable.Data;
using SistemaContable.Models;
using SistemaContable.ViewModels;
using SistemaContable.Services;
using OfficeOpenXml;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace SistemaContable.Controllers
{
    public class CatalogoCuentasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmpresaService _empresaService;
        private readonly ILogger<CatalogoCuentasController> _logger;

        public CatalogoCuentasController(
            ApplicationDbContext context, 
            IEmpresaService empresaService, 
            ILogger<CatalogoCuentasController> logger)
        {
            _context = context;
            _empresaService = empresaService;
            _logger = logger;
        }

        // GET: CatalogoCuentas
        public async Task<IActionResult> Index()
        {
            var empresaId = _empresaService.ObtenerEmpresaActualId();
            
            // Obtener todas las cuentas de la empresa actual
            var cuentas = await _context.CuentasContables
                .Where(c => c.EmpresaId == empresaId)
                .Include(c => c.SubCuentas)
                .ToListAsync();
            
            // Construir jerarquía de cuentas comenzando por las cuentas principales (sin padre)
            var cuentasPrincipales = cuentas.Where(c => c.CuentaPadreId == null)
                .OrderBy(c => c.Codigo)
                .ToList();
                
            return View(cuentasPrincipales);
        }

        // GET: CatalogoCuentas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresaId = _empresaService.ObtenerEmpresaActualId();
            var cuenta = await _context.CuentasContables
                .Include(c => c.SubCuentas)
                .FirstOrDefaultAsync(c => c.Id == id && c.EmpresaId == empresaId);
                
            if (cuenta == null)
            {
                return NotFound();
            }

            return View(cuenta);
        }

        // GET: CatalogoCuentas/Create
        public IActionResult Create(int? padreId)
        {
            var empresaId = _empresaService.ObtenerEmpresaActualId();
            
            // Si se proporciona un ID de cuenta padre, preconfiguramos algunos valores
            if (padreId.HasValue)
            {
                var cuentaPadre = _context.CuentasContables.FirstOrDefault(c => c.Id == padreId && c.EmpresaId == empresaId);
                if (cuentaPadre != null)
                {
                    var viewModel = new CuentaContableViewModel
                    {
                        CuentaPadreId = cuentaPadre.Id,
                        CuentaPadreNombre = cuentaPadre.Nombre,
                        Categoria = cuentaPadre.Categoria,
                        Naturaleza = cuentaPadre.Naturaleza,
                        Nivel = cuentaPadre.Nivel + 1
                    };
                    
                    return View(viewModel);
                }
            }
            
            // Si no hay padre o no se encontró, mostramos formulario vacío
            var categorias = new List<SelectListItem>
            {
                new SelectListItem { Value = "Activo", Text = "Activo" },
                new SelectListItem { Value = "Pasivo", Text = "Pasivo" },
                new SelectListItem { Value = "Patrimonio", Text = "Patrimonio" },
                new SelectListItem { Value = "Ingreso", Text = "Ingreso" },
                new SelectListItem { Value = "Gasto", Text = "Gasto" },
                new SelectListItem { Value = "Costo", Text = "Costo" },
                new SelectListItem { Value = "CuentaOrden", Text = "Cuenta de Orden" }
            };
            
            ViewBag.Categorias = categorias;
            
            var usosCuenta = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Ninguno" },
                new SelectListItem { Value = "CuentasPorCobrar", Text = "Cuentas por cobrar" },
                new SelectListItem { Value = "CuentasPorPagar", Text = "Cuentas por pagar" },
                new SelectListItem { Value = "Bancos", Text = "Bancos" },
                new SelectListItem { Value = "Inventario", Text = "Inventario" },
                new SelectListItem { Value = "Ventas", Text = "Ventas" },
                new SelectListItem { Value = "CostosInventario", Text = "Costos de inventario" },
                new SelectListItem { Value = "DevolucionesClientes", Text = "Devoluciones de clientes" },
                new SelectListItem { Value = "DevolucionesProveedores", Text = "Devoluciones de proveedores" },
                new SelectListItem { Value = "AnticiposEntregados", Text = "Anticipos entregados" },
                new SelectListItem { Value = "AnticiposRecibidos", Text = "Anticipos recibidos" }
            };
            
            ViewBag.UsosCuenta = usosCuenta;
            
            return View(new CuentaContableViewModel { Nivel = 1 });
        }

        // POST: CatalogoCuentas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CuentaContableViewModel viewModel)
        {
            var empresaId = _empresaService.ObtenerEmpresaActualId();
            
            if (ModelState.IsValid)
            {
                // Verificar que no exista otra cuenta con el mismo nombre
                bool existeNombre = await _context.CuentasContables
                    .AnyAsync(c => c.Nombre == viewModel.Nombre && c.EmpresaId == empresaId);
                    
                if (existeNombre)
                {
                    ModelState.AddModelError("Nombre", "Ya existe una cuenta con este nombre.");
                    return View(viewModel);
                }
                
                // Si se proporciona un código, verificar que sea único
                if (!string.IsNullOrEmpty(viewModel.Codigo))
                {
                    bool existeCodigo = await _context.CuentasContables
                        .AnyAsync(c => c.Codigo == viewModel.Codigo && c.EmpresaId == empresaId);
                        
                    if (existeCodigo)
                    {
                        ModelState.AddModelError("Codigo", "Ya existe una cuenta con este código.");
                        return View(viewModel);
                    }
                }
                
                // Crear la nueva cuenta
                var cuentaContable = new CuentaContable
                {
                    Nombre = viewModel.Nombre,
                    Codigo = viewModel.Codigo,
                    Categoria = viewModel.Categoria,
                    TipoCuenta = viewModel.TipoCuenta,
                    UsoCuenta = viewModel.UsoCuenta,
                    Naturaleza = viewModel.Naturaleza,
                    Descripcion = viewModel.Descripcion,
                    VerSaldoPorTercero = viewModel.VerSaldoPorTercero,
                    CuentaPadreId = viewModel.CuentaPadreId,
                    Nivel = viewModel.Nivel,
                    Orden = await ObtenerSiguienteOrden(viewModel.CuentaPadreId, empresaId),
                    EmpresaId = empresaId,
                    FechaCreacion = DateTime.UtcNow
                };
                
                _context.Add(cuentaContable);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = "Cuenta creada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            
            // Si llegamos aquí, hubo un error de validación
            // Volvemos a cargar las listas desplegables
            var categorias = new List<SelectListItem>
            {
                new SelectListItem { Value = "Activo", Text = "Activo" },
                new SelectListItem { Value = "Pasivo", Text = "Pasivo" },
                new SelectListItem { Value = "Patrimonio", Text = "Patrimonio" },
                new SelectListItem { Value = "Ingreso", Text = "Ingreso" },
                new SelectListItem { Value = "Gasto", Text = "Gasto" },
                new SelectListItem { Value = "Costo", Text = "Costo" },
                new SelectListItem { Value = "CuentaOrden", Text = "Cuenta de Orden" }
            };
            
            ViewBag.Categorias = categorias;
            
            var usosCuenta = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Ninguno" },
                new SelectListItem { Value = "CuentasPorCobrar", Text = "Cuentas por cobrar" },
                new SelectListItem { Value = "CuentasPorPagar", Text = "Cuentas por pagar" },
                new SelectListItem { Value = "Bancos", Text = "Bancos" },
                new SelectListItem { Value = "Inventario", Text = "Inventario" },
                new SelectListItem { Value = "Ventas", Text = "Ventas" },
                new SelectListItem { Value = "CostosInventario", Text = "Costos de inventario" },
                new SelectListItem { Value = "DevolucionesClientes", Text = "Devoluciones de clientes" },
                new SelectListItem { Value = "DevolucionesProveedores", Text = "Devoluciones de proveedores" },
                new SelectListItem { Value = "AnticiposEntregados", Text = "Anticipos entregados" },
                new SelectListItem { Value = "AnticiposRecibidos", Text = "Anticipos recibidos" }
            };
            
            ViewBag.UsosCuenta = usosCuenta;
            
            return View(viewModel);
        }

        // GET: CatalogoCuentas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresaId = _empresaService.ObtenerEmpresaActualId();
            var cuentaContable = await _context.CuentasContables
                .FirstOrDefaultAsync(c => c.Id == id && c.EmpresaId == empresaId);
                
            if (cuentaContable == null)
            {
                return NotFound();
            }
            
            // Verificar si es una cuenta del sistema
            if (cuentaContable.EsCuentaSistema)
            {
                TempData["WarningMessage"] = "Esta es una cuenta del sistema. Algunos campos no pueden ser modificados.";
            }
            
            var viewModel = new CuentaContableViewModel
            {
                Id = cuentaContable.Id,
                Nombre = cuentaContable.Nombre,
                Codigo = cuentaContable.Codigo,
                Categoria = cuentaContable.Categoria,
                TipoCuenta = cuentaContable.TipoCuenta,
                UsoCuenta = cuentaContable.UsoCuenta,
                Naturaleza = cuentaContable.Naturaleza,
                Descripcion = cuentaContable.Descripcion,
                VerSaldoPorTercero = cuentaContable.VerSaldoPorTercero,
                CuentaPadreId = cuentaContable.CuentaPadreId,
                Nivel = cuentaContable.Nivel,
                EsCuentaSistema = cuentaContable.EsCuentaSistema
            };
            
            // Si tiene padre, obtener su nombre
            if (cuentaContable.CuentaPadreId.HasValue)
            {
                var cuentaPadre = await _context.CuentasContables
                    .FirstOrDefaultAsync(c => c.Id == cuentaContable.CuentaPadreId);
                    
                viewModel.CuentaPadreNombre = cuentaPadre?.Nombre ?? "";
            }
            
            // Cargar listas desplegables
            var categorias = new List<SelectListItem>
            {
                new SelectListItem { Value = "Activo", Text = "Activo" },
                new SelectListItem { Value = "Pasivo", Text = "Pasivo" },
                new SelectListItem { Value = "Patrimonio", Text = "Patrimonio" },
                new SelectListItem { Value = "Ingreso", Text = "Ingreso" },
                new SelectListItem { Value = "Gasto", Text = "Gasto" },
                new SelectListItem { Value = "Costo", Text = "Costo" },
                new SelectListItem { Value = "CuentaOrden", Text = "Cuenta de Orden" }
            };
            
            ViewBag.Categorias = categorias;
            
            var usosCuenta = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Ninguno" },
                new SelectListItem { Value = "CuentasPorCobrar", Text = "Cuentas por cobrar" },
                new SelectListItem { Value = "CuentasPorPagar", Text = "Cuentas por pagar" },
                new SelectListItem { Value = "Bancos", Text = "Bancos" },
                new SelectListItem { Value = "Inventario", Text = "Inventario" },
                new SelectListItem { Value = "Ventas", Text = "Ventas" },
                new SelectListItem { Value = "CostosInventario", Text = "Costos de inventario" },
                new SelectListItem { Value = "DevolucionesClientes", Text = "Devoluciones de clientes" },
                new SelectListItem { Value = "DevolucionesProveedores", Text = "Devoluciones de proveedores" },
                new SelectListItem { Value = "AnticiposEntregados", Text = "Anticipos entregados" },
                new SelectListItem { Value = "AnticiposRecibidos", Text = "Anticipos recibidos" }
            };
            
            ViewBag.UsosCuenta = usosCuenta;
            
            return View(viewModel);
        }

        // POST: CatalogoCuentas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CuentaContableViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            var empresaId = _empresaService.ObtenerEmpresaActualId();
            
            if (ModelState.IsValid)
            {
                try
                {
                    var cuentaContable = await _context.CuentasContables
                        .FirstOrDefaultAsync(c => c.Id == id && c.EmpresaId == empresaId);
                        
                    if (cuentaContable == null)
                    {
                        return NotFound();
                    }
                    
                    // Verificar unicidad de nombre (excluyendo la cuenta actual)
                    bool existeNombre = await _context.CuentasContables
                        .AnyAsync(c => c.Nombre == viewModel.Nombre && c.Id != id && c.EmpresaId == empresaId);
                        
                    if (existeNombre)
                    {
                        ModelState.AddModelError("Nombre", "Ya existe otra cuenta con este nombre.");
                        return View(viewModel);
                    }
                    
                    // Verificar unicidad de código si se proporciona (excluyendo la cuenta actual)
                    if (!string.IsNullOrEmpty(viewModel.Codigo))
                    {
                        bool existeCodigo = await _context.CuentasContables
                            .AnyAsync(c => c.Codigo == viewModel.Codigo && c.Id != id && c.EmpresaId == empresaId);
                            
                        if (existeCodigo)
                        {
                            ModelState.AddModelError("Codigo", "Ya existe otra cuenta con este código.");
                            return View(viewModel);
                        }
                    }
                    
                    // Si es una cuenta del sistema, solo permitir modificar ciertos campos
                    if (cuentaContable.EsCuentaSistema)
                    {
                        cuentaContable.Descripcion = viewModel.Descripcion;
                        cuentaContable.VerSaldoPorTercero = viewModel.VerSaldoPorTercero;
                        
                        // Si se permite cambiar el código en cuentas del sistema
                        cuentaContable.Codigo = viewModel.Codigo;
                    }
                    else
                    {
                        // Si no es cuenta del sistema, permitir modificar todos los campos
                        cuentaContable.Nombre = viewModel.Nombre;
                        cuentaContable.Codigo = viewModel.Codigo;
                        cuentaContable.TipoCuenta = viewModel.TipoCuenta;
                        cuentaContable.UsoCuenta = viewModel.UsoCuenta;
                        cuentaContable.Descripcion = viewModel.Descripcion;
                        cuentaContable.VerSaldoPorTercero = viewModel.VerSaldoPorTercero;
                        
                        // Naturaleza solo se puede cambiar en cuentas de Activo (excepto Efectivo) y Patrimonio
                        if (cuentaContable.Categoria == "Activo" || cuentaContable.Categoria == "Patrimonio")
                        {
                            if (cuentaContable.Categoria == "Activo" && !cuentaContable.Nombre.Contains("Efectivo"))
                            {
                                cuentaContable.Naturaleza = viewModel.Naturaleza;
                            }
                            else if (cuentaContable.Categoria == "Patrimonio")
                            {
                                cuentaContable.Naturaleza = viewModel.Naturaleza;
                            }
                        }
                    }
                    
                    cuentaContable.FechaModificacion = DateTime.UtcNow;
                    
                    _context.Update(cuentaContable);
                    await _context.SaveChangesAsync();
                    
                    TempData["SuccessMessage"] = "Cuenta actualizada exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CuentaContableExists(viewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            
            // Cargar listas desplegables en caso de error
            var categorias = new List<SelectListItem>
            {
                new SelectListItem { Value = "Activo", Text = "Activo" },
                new SelectListItem { Value = "Pasivo", Text = "Pasivo" },
                new SelectListItem { Value = "Patrimonio", Text = "Patrimonio" },
                new SelectListItem { Value = "Ingreso", Text = "Ingreso" },
                new SelectListItem { Value = "Gasto", Text = "Gasto" },
                new SelectListItem { Value = "Costo", Text = "Costo" },
                new SelectListItem { Value = "CuentaOrden", Text = "Cuenta de Orden" }
            };
            
            ViewBag.Categorias = categorias;
            
            var usosCuenta = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Ninguno" },
                new SelectListItem { Value = "CuentasPorCobrar", Text = "Cuentas por cobrar" },
                new SelectListItem { Value = "CuentasPorPagar", Text = "Cuentas por pagar" },
                new SelectListItem { Value = "Bancos", Text = "Bancos" },
                new SelectListItem { Value = "Inventario", Text = "Inventario" },
                new SelectListItem { Value = "Ventas", Text = "Ventas" },
                new SelectListItem { Value = "CostosInventario", Text = "Costos de inventario" },
                new SelectListItem { Value = "DevolucionesClientes", Text = "Devoluciones de clientes" },
                new SelectListItem { Value = "DevolucionesProveedores", Text = "Devoluciones de proveedores" },
                new SelectListItem { Value = "AnticiposEntregados", Text = "Anticipos entregados" },
                new SelectListItem { Value = "AnticiposRecibidos", Text = "Anticipos recibidos" }
            };
            
            ViewBag.UsosCuenta = usosCuenta;
            
            return View(viewModel);
        }

        // GET: CatalogoCuentas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresaId = _empresaService.ObtenerEmpresaActualId();
            var cuentaContable = await _context.CuentasContables
                .FirstOrDefaultAsync(c => c.Id == id && c.EmpresaId == empresaId);
                
            if (cuentaContable == null)
            {
                return NotFound();
            }
            
            // Verificar si es una cuenta del sistema
            if (cuentaContable.EsCuentaSistema)
            {
                TempData["ErrorMessage"] = "No se puede eliminar una cuenta del sistema.";
                return RedirectToAction(nameof(Index));
            }
            
            // Verificar si tiene subcuentas
            var tieneSubcuentas = await _context.CuentasContables
                .AnyAsync(c => c.CuentaPadreId == id);
                
            if (tieneSubcuentas)
            {
                TempData["WarningMessage"] = "Esta cuenta tiene subcuentas. Al eliminarla, todas sus subcuentas también serán eliminadas.";
            }
            
            // Verificar si tiene saldos o movimientos
            var tieneSaldos = await _context.SaldosIniciales
                .AnyAsync(s => s.CuentaContableId == id);
                
            if (tieneSaldos)
            {
                TempData["WarningMessage"] = "Esta cuenta tiene saldos iniciales. Al eliminarla, tendrás que transferir estos saldos a otra cuenta.";
            }
            
            return View(cuentaContable);
        }

        // POST: CatalogoCuentas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int? cuentaDestinoId)
        {
            var empresaId = _empresaService.ObtenerEmpresaActualId();
            var cuentaContable = await _context.CuentasContables
                .FirstOrDefaultAsync(c => c.Id == id && c.EmpresaId == empresaId);
                
            if (cuentaContable == null)
            {
                return NotFound();
            }
            
            // Verificar si es una cuenta del sistema
            if (cuentaContable.EsCuentaSistema)
            {
                TempData["ErrorMessage"] = "No se puede eliminar una cuenta del sistema.";
                return RedirectToAction(nameof(Index));
            }
            
            // Verificar si tiene saldos o movimientos
            var tieneSaldos = await _context.SaldosIniciales
                .AnyAsync(s => s.CuentaContableId == id);
                
            if (tieneSaldos && cuentaDestinoId.HasValue)
            {
                // Transferir saldos a la cuenta destino
                var saldos = await _context.SaldosIniciales
                    .Where(s => s.CuentaContableId == id)
                    .ToListAsync();
                    
                foreach (var saldo in saldos)
                {
                    saldo.CuentaContableId = cuentaDestinoId.Value;
                }
                
                await _context.SaveChangesAsync();
            }
            else if (tieneSaldos && !cuentaDestinoId.HasValue)
            {
                TempData["ErrorMessage"] = "No se puede eliminar esta cuenta porque tiene saldos. Debes transferir estos saldos a otra cuenta.";
                return RedirectToAction(nameof(Delete), new { id });
            }
            
            // Eliminar subcuentas recursivamente
            await EliminarSubcuentasRecursivamente(id);
            
            // Eliminar la cuenta
            _context.CuentasContables.Remove(cuentaContable);
            await _context.SaveChangesAsync();
            
            TempData["SuccessMessage"] = "Cuenta eliminada exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: CatalogoCuentas/Movimientos/5
        public async Task<IActionResult> Movimientos(int? id, DateTime? fechaInicio, DateTime? fechaFin)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresaId = _empresaService.ObtenerEmpresaActualId();
            var cuenta = await _context.CuentasContables
                .FirstOrDefaultAsync(c => c.Id == id && c.EmpresaId == empresaId);
                
            if (cuenta == null)
            {
                return NotFound();
            }
            
            // Si no se proporcionan fechas, usar el año actual
            if (!fechaInicio.HasValue)
            {
                fechaInicio = new DateTime(DateTime.Now.Year, 1, 1);
            }
            
            if (!fechaFin.HasValue)
            {
                fechaFin = DateTime.Now;
            }
            
            // Aquí iría la lógica para obtener los movimientos de la cuenta
            // Este es un ejemplo simplificado, adaptar según el modelo de datos real
            var viewModel = new MovimientosCuentaViewModel
            {
                Cuenta = cuenta,
                FechaInicio = fechaInicio.Value,
                FechaFin = fechaFin.Value,
                // MovimientosList = await ObtenerMovimientosCuenta(id.Value, fechaInicio.Value, fechaFin.Value)
            };
            
            return View(viewModel);
        }

        // GET: CatalogoCuentas/SaldosIniciales
        public async Task<IActionResult> SaldosIniciales()
        {
            var empresaId = _empresaService.ObtenerEmpresaActualId();
            
            // Obtener todas las cuentas de la empresa actual
            var cuentas = await _context.CuentasContables
                .Where(c => c.EmpresaId == empresaId && 
                       (c.TipoCuenta == "Movimiento" || string.IsNullOrEmpty(c.TipoCuenta)) &&
                       (c.Categoria == "Activo" || c.Categoria == "Pasivo" || c.Categoria == "Patrimonio"))
                .OrderBy(c => c.Categoria)
                .ThenBy(c => c.Codigo)
                .ToListAsync();
                
            // Obtener saldos iniciales existentes
            var saldosIniciales = await _context.SaldosIniciales
                .Where(s => s.EmpresaId == empresaId)
                .ToDictionaryAsync(s => s.CuentaContableId, s => s);
                
            var viewModel = new SaldosInicialesViewModel
            {
                CuentasActivo = cuentas.Where(c => c.Categoria == "Activo").ToList(),
                CuentasPasivo = cuentas.Where(c => c.Categoria == "Pasivo").ToList(),
                CuentasPatrimonio = cuentas.Where(c => c.Categoria == "Patrimonio").ToList(),
                SaldosIniciales = saldosIniciales,
                FechaInicial = DateTime.Today
            };
            
            return View(viewModel);
        }

        // POST: CatalogoCuentas/SaldosIniciales
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaldosIniciales(SaldosInicialesViewModel viewModel)
        {
            var empresaId = _empresaService.ObtenerEmpresaActualId();
            
            if (ModelState.IsValid)
            {
                // Procesar los saldos iniciales
                foreach (var saldo in viewModel.SaldosIngresados)
                {
                    if (saldo.Valor != 0)
                    {
                        var saldoExistente = await _context.SaldosIniciales
                            .FirstOrDefaultAsync(s => s.CuentaContableId == saldo.CuentaContableId && 
                                                s.EmpresaId == empresaId &&
                                                s.ContactoId == saldo.ContactoId);
                                                
                        if (saldoExistente != null)
                        {
                            // Actualizar saldo existente
                            saldoExistente.Valor = saldo.Valor;
                            saldoExistente.FechaInicial = viewModel.FechaInicial;
                            _context.Update(saldoExistente);
                        }
                        else
                        {
                            // Crear nuevo saldo
                            var nuevoSaldo = new SaldoInicial
                            {
                                CuentaContableId = saldo.CuentaContableId,
                                Valor = saldo.Valor,
                                FechaInicial = viewModel.FechaInicial,
                                EmpresaId = empresaId,
                                ContactoId = saldo.ContactoId
                            };
                            
                            _context.Add(nuevoSaldo);
                        }
                    }
                }
                
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = "Saldos iniciales guardados exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            
            // Si hay errores, volver a cargar las cuentas
            var cuentas = await _context.CuentasContables
                .Where(c => c.EmpresaId == empresaId)
                .OrderBy(c => c.Categoria)
                .ThenBy(c => c.Codigo)
                .ToListAsync();
                
            viewModel.CuentasActivo = cuentas.Where(c => c.Categoria == "Activo").ToList();
            viewModel.CuentasPasivo = cuentas.Where(c => c.Categoria == "Pasivo").ToList();
            viewModel.CuentasPatrimonio = cuentas.Where(c => c.Categoria == "Patrimonio").ToList();
            
            return View(viewModel);
        }

        // GET: CatalogoCuentas/ExportarExcel
        public async Task<IActionResult> ExportarExcel()
        {
            var empresaId = _empresaService.ObtenerEmpresaActualId();
            
            // Obtener todas las cuentas de la empresa actual
            var cuentas = await _context.CuentasContables
                .Where(c => c.EmpresaId == empresaId)
                .OrderBy(c => c.Categoria)
                .ThenBy(c => c.Codigo)
                .ToListAsync();
                
            // Crear lista de cuentas para exportar
            var cuentasExportar = cuentas.Select(c => new CuentaContableExport
            {
                Nivel = c.Nivel,
                Codigo = c.Codigo,
                Nombre = c.Nombre,
                Categoria = c.Categoria,
                Naturaleza = c.Naturaleza,
                UsoCuenta = c.UsoCuenta,
                TipoCuenta = c.TipoCuenta,
                Descripcion = c.Descripcion
            }).ToList();
            
            // Generar CSV
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                HasHeaderRecord = true
            };
            
            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
            using (var csv = new CsvWriter(writer, config))
            {
                await csv.WriteRecordsAsync(cuentasExportar);
                await writer.FlushAsync();
                
                memoryStream.Position = 0;
                return File(memoryStream.ToArray(), "text/csv", "catalogo_cuentas.csv");
            }
        }

        // GET: CatalogoCuentas/ImportarExcel
        public IActionResult ImportarExcel()
        {
            return View();
        }

        // POST: CatalogoCuentas/ImportarExcel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportarExcel(IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
            {
                ModelState.AddModelError("", "Por favor seleccione un archivo.");
                return View();
            }
            
            // Verificar extensión
            var extension = Path.GetExtension(archivo.FileName).ToLowerInvariant();
            if (extension != ".xlsx")
            {
                ModelState.AddModelError("", "El archivo debe ser Excel (.xlsx).");
                return View();
            }
            
            try
            {
                var cuentasImportadas = new List<CuentaContableImport>();
                
                // Configuración EPPlus 8.0+ para uso no comercial
                ExcelPackage.License.SetNonCommercialOrganization("SistemaContable");
                
                using (var stream = new MemoryStream())
                {
                    await archivo.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0]; // Primera hoja
                        var rowCount = worksheet.Dimension.Rows;
                        var colCount = worksheet.Dimension.Columns;
                        
                        // Empezamos desde la fila 2 para omitir los encabezados
                        for (int row = 2; row <= rowCount; row++)
                        {
                            // Verificamos que la fila tenga datos
                            if (string.IsNullOrWhiteSpace(worksheet.Cells[row, 3].Text))
                                continue; // Salta filas vacías
                                
                            var cuenta = new CuentaContableImport
                            {
                                Nivel = int.TryParse(worksheet.Cells[row, 1].Text, out int nivel) ? nivel : 1,
                                Codigo = worksheet.Cells[row, 2].Text,
                                Nombre = worksheet.Cells[row, 3].Text,
                                Categoria = worksheet.Cells[row, 4].Text,
                                Naturaleza = worksheet.Cells[row, 5].Text,
                                TipoCuenta = worksheet.Cells[row, 6].Text,
                                UsoCuenta = worksheet.Cells[row, 7].Text
                            };
                            
                            // Verificación básica de datos
                            if (string.IsNullOrEmpty(cuenta.Nombre) || string.IsNullOrEmpty(cuenta.Categoria))
                                continue;
                                
                            cuentasImportadas.Add(cuenta);
                        }
                    }
                }
                
                if (cuentasImportadas.Count == 0)
                {
                    ModelState.AddModelError("", "No se pudo extraer ninguna cuenta del archivo. Verifique el formato.");
                    return View();
                }
                
                // Guardar lista de cuentas para el siguiente paso
                TempData["CuentasImportadas"] = System.Text.Json.JsonSerializer.Serialize(cuentasImportadas);
                
                return RedirectToAction(nameof(ValidarImportacion));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al procesar el archivo: {ex.Message}");
                return View();
            }
        }
        
        // Este método debe usarse en la descarga de la plantilla de ejemplo
        public IActionResult DescargarPlantilla()
        {
            // Configuración EPPlus 8.0+ para uso no comercial
            ExcelPackage.License.SetNonCommercialOrganization("SistemaContable");
            
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("CatalogoCuentas");
                
                // Agregar encabezados
                string[] headers = new string[] { "Nivel", "Código", "Nombre", "Categoría", "Naturaleza", "Tipo de cuenta", "Uso de cuenta" };
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                }
                
                // Agregar algunas cuentas de ejemplo
                var ejemplos = new List<object[]>
                {
                    new object[] { 1, "1", "ACTIVOS", "Activo", "Deudora", "Cuenta mayor", "" },
                    new object[] { 2, "10", "ACTIVOS CORRIENTES", "Activo", "Deudora", "Cuenta mayor", "" },
                    new object[] { 3, "101", "EFECTIVO Y EQUIVALENTES", "Activo", "Deudora", "Cuenta mayor", "" },
                    new object[] { 4, "10000", "Efectivo en Caja y Banco", "Activo", "Deudora", "Cuenta mayor", "" }
                };
                
                for (int i = 0; i < ejemplos.Count; i++)
                {
                    for (int j = 0; j < ejemplos[i].Length; j++)
                    {
                        worksheet.Cells[i + 2, j + 1].Value = ejemplos[i][j];
                    }
                }
                
                // Ajustar ancho de columnas
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                
                // Generar el archivo
                var stream = new MemoryStream(package.GetAsByteArray());
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PlantillaCatalogoCuentas.xlsx");
            }
        }

        // GET: CatalogoCuentas/ValidarImportacion
        public IActionResult ValidarImportacion()
        {
            // Recuperar cuentas importadas del TempData
            if (TempData["CuentasImportadas"] is not string cuentasImportadasJson)
            {
                TempData["ErrorMessage"] = "Se perdieron los datos de importación. Por favor, intente nuevamente.";
                return RedirectToAction(nameof(ImportarExcel));
            }
            
            var cuentasImportadas = System.Text.Json.JsonSerializer.Deserialize<List<CuentaContableImport>>(cuentasImportadasJson);
            
            if (cuentasImportadas == null)
            {
                TempData["ErrorMessage"] = "Error al deserializar los datos de importación. Por favor, intente nuevamente.";
                return RedirectToAction(nameof(ImportarExcel));
            }
            
            // Realizar validaciones
            var errores = new List<string>();
            var advertencias = new List<string>();
            
            // Ejemplos de validaciones
            if (cuentasImportadas.Count > 2000)
            {
                errores.Add("La plantilla excede el límite de 2000 cuentas contables.");
            }
            
            // Verificar nombres duplicados
            var nombresDuplicados = cuentasImportadas
                .GroupBy(c => c.Nombre ?? "")
                .Where(g => g.Count() > 1 && !string.IsNullOrEmpty(g.Key))
                .Select(g => g.Key)
                .ToList();
                
            if (nombresDuplicados.Any())
            {
                errores.Add($"Se encontraron nombres duplicados: {string.Join(", ", nombresDuplicados)}");
            }
            
            // Guardar nuevamente para el siguiente paso
            TempData["CuentasImportadas"] = cuentasImportadasJson;
            
            var viewModel = new ValidarImportacionViewModel
            {
                CuentasImportadas = cuentasImportadas,
                Errores = errores,
                Advertencias = advertencias
            };
            
            return View(viewModel);
        }

        // POST: CatalogoCuentas/ConfirmarImportacion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarImportacion(bool transferirSaldos = true)
        {
            try
            {
                // 1. Recuperar cuentas importadas del TempData con manejo de errores mejorado
                if (TempData["CuentasImportadas"] is not string cuentasImportadasJson || string.IsNullOrEmpty(cuentasImportadasJson))
                {
                    TempData["ErrorMessage"] = "Se perdieron los datos de importación. Por favor, intente nuevamente.";
                    return RedirectToAction(nameof(ImportarExcel));
                }
                
                _logger.LogInformation("Deserializando cuentas importadas");
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var cuentasImportadas = JsonSerializer.Deserialize<List<CuentaContableImport>>(cuentasImportadasJson, options);
                
                if (cuentasImportadas == null || !cuentasImportadas.Any())
                {
                    TempData["ErrorMessage"] = "No se encontraron cuentas para importar.";
                    return RedirectToAction(nameof(ImportarExcel));
                }
                
                _logger.LogInformation($"Se encontraron {cuentasImportadas.Count} cuentas para importar");
                
                // Obtener ID de empresa y VERIFICAR QUE EXISTA
                var empresaId = _empresaService.ObtenerEmpresaActualId();
                var empresaExiste = await _context.Empresas.AnyAsync(e => e.Id == empresaId);

                // SOLUCIÓN AL PROBLEMA: Si la empresa no existe, crearla
                if (!empresaExiste)
                {
                    _logger.LogWarning($"No se encontró la empresa con ID {empresaId}. Creando empresa predeterminada.");
                    
                    // Crea una empresa por defecto si no existe
                    var empresa = new Empresa
                    {
                        Id = empresaId, // Usar el mismo ID que devuelve el servicio
                        Nombre = "Empresa Default",
                        NumeroIdentificacion = "00000000001",
                        TipoIdentificacion = "RNC",
                        Direccion = "Dirección por defecto",
                        Ciudad = "Ciudad",
                        Provincia = "Provincia",
                        CodigoPostal = "00000",
                        Pais = "República Dominicana",
                        Telefono = "000-000-0000",
                        Email = "contacto@ejemplo.com",
                        SitioWeb = "www.ejemplo.com",
                        NombreComercial = "Empresa Default",
                        MonedaPrincipal = "DOP",
                        PrecisionDecimal = 2,
                        SeparadorDecimal = ".",
                        LogoUrl = "/img/default-logo.png",
                        ResponsabilidadTributaria = "Normal",
                        Activo = true,
                        FechaCreacion = DateTime.UtcNow
                    };

                    _context.Empresas.Add(empresa);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"Empresa creada con ID: {empresaId}");
                }
                
                // 2. Iniciar transacción con manejo de excepciones mejorado
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Guardar saldos existentes para transferirlos después si es necesario
                        var saldosExistentes = new Dictionary<string, List<SaldoInicial>>();
                        
                        if (transferirSaldos)
                        {
                            _logger.LogInformation("Guardando saldos existentes para transferencia");
                            var cuentasExistentes = await _context.CuentasContables
                                .Where(c => c.EmpresaId == empresaId)
                                .ToListAsync();
                                
                            foreach (var cuenta in cuentasExistentes)
                            {
                                var saldos = await _context.SaldosIniciales
                                    .Where(s => s.CuentaContableId == cuenta.Id && s.EmpresaId == empresaId)
                                    .ToListAsync();
                                    
                                if (saldos.Any())
                                {
                                    saldosExistentes[cuenta.Nombre] = saldos;
                                }
                            }
                            
                            _logger.LogInformation($"Se guardaron saldos para {saldosExistentes.Count} cuentas existentes");
                        }
                        
                        // 3. Eliminar catálogo existente con manejo de errores
                        _logger.LogInformation("Eliminando catálogo existente");
                        var cuentasParaEliminar = await _context.CuentasContables
                            .Where(c => c.EmpresaId == empresaId)
                            .ToListAsync();
                        
                        if (cuentasParaEliminar.Any())
                        {
                            _context.CuentasContables.RemoveRange(cuentasParaEliminar);
                            await _context.SaveChangesAsync();
                            _logger.LogInformation($"Se eliminaron {cuentasParaEliminar.Count} cuentas existentes");
                        }
                        
                        // 4. Crear nuevas cuentas con validación mejorada
                        _logger.LogInformation("Creando nuevas cuentas");
                        var cuentasNuevas = new Dictionary<string, CuentaContable>();
                        var cuentasPadres = new Dictionary<string, List<string>>();
                        
                        // Primer paso: crear todas las cuentas
                        foreach (var cuentaImport in cuentasImportadas)
                        {
                            if (string.IsNullOrEmpty(cuentaImport.Nombre) || string.IsNullOrEmpty(cuentaImport.Categoria))
                            {
                                continue; // Saltamos cuentas con datos básicos incompletos
                            }
                            
                            var cuenta = new CuentaContable
                            {
                                Nombre = cuentaImport.Nombre.Trim(),
                                Codigo = cuentaImport.Codigo?.Trim() ?? string.Empty,
                                Categoria = cuentaImport.Categoria.Trim(),
                                Naturaleza = cuentaImport.Naturaleza?.Trim() ?? 
                                    (cuentaImport.Categoria.Trim().Equals("Activo", StringComparison.OrdinalIgnoreCase) || 
                                     cuentaImport.Categoria.Trim().Equals("Gasto", StringComparison.OrdinalIgnoreCase) || 
                                     cuentaImport.Categoria.Trim().Equals("Costo", StringComparison.OrdinalIgnoreCase) 
                                        ? "Deudora" : "Acreedora"),
                                UsoCuenta = cuentaImport.UsoCuenta?.Trim(),
                                TipoCuenta = cuentaImport.TipoCuenta?.Trim() ?? "Movimiento",
                                Nivel = cuentaImport.Nivel,
                                Descripcion = cuentaImport.Descripcion,
                                VerSaldoPorTercero = false,
                                EmpresaId = empresaId,
                                Orden = 0,
                                FechaCreacion = DateTime.UtcNow,
                                Activo = true
                            };
                            
                            _context.CuentasContables.Add(cuenta);
                            
                            // 5. Guardar cada cuenta individualmente para obtener su ID generado
                            await _context.SaveChangesAsync();
                            _logger.LogDebug($"Cuenta creada: {cuenta.Nombre} (ID: {cuenta.Id})");
                            
                            cuentasNuevas[cuenta.Nombre] = cuenta;
                            
                            // Guardar relación padre-hijo para establecer después
                            if (cuentaImport.Nivel > 1 && !string.IsNullOrEmpty(cuentaImport.Codigo))
                            {
                                // Buscar padre por código
                                string? codigoPadre = null;
                                
                                // Si el código es numérico, intentamos determinar el padre por prefijo
                                if (int.TryParse(cuentaImport.Codigo, out _))
                                {
                                    foreach (var posiblePadre in cuentasImportadas)
                                    {
                                        if (posiblePadre.Nivel < cuentaImport.Nivel && 
                                            !string.IsNullOrEmpty(posiblePadre.Codigo) &&
                                            !string.IsNullOrEmpty(cuentaImport.Codigo) &&
                                            cuentaImport.Codigo.StartsWith(posiblePadre.Codigo) && 
                                            (codigoPadre == null || posiblePadre.Codigo.Length > codigoPadre.Length))
                                        {
                                            codigoPadre = posiblePadre.Codigo;
                                        }
                                    }
                                }
                                
                                if (!string.IsNullOrEmpty(codigoPadre))
                                {
                                    var padre = cuentasImportadas.FirstOrDefault(c => c.Codigo == codigoPadre);
                                    if (padre != null && !string.IsNullOrEmpty(padre.Nombre))
                                    {
                                        if (!cuentasPadres.ContainsKey(padre.Nombre))
                                        {
                                            cuentasPadres[padre.Nombre] = new List<string>();
                                        }
                                        
                                        cuentasPadres[padre.Nombre].Add(cuentaImport.Nombre);
                                    }
                                }
                            }
                            else if (cuentaImport.Nivel > 1 && !string.IsNullOrEmpty(cuentaImport.CuentaPadre))
                            {
                                if (!cuentasPadres.ContainsKey(cuentaImport.CuentaPadre))
                                {
                                    cuentasPadres[cuentaImport.CuentaPadre] = new List<string>();
                                }
                                
                                cuentasPadres[cuentaImport.CuentaPadre].Add(cuentaImport.Nombre);
                            }
                        }
                        
                        // 6. Segundo paso: establecer relaciones padre-hijo
                        _logger.LogInformation("Estableciendo relaciones jerárquicas");
                        foreach (var padreCuentas in cuentasPadres)
                        {
                            if (cuentasNuevas.TryGetValue(padreCuentas.Key, out var cuentaPadre))
                            {
                                foreach (var nombreHijo in padreCuentas.Value)
                                {
                                    if (cuentasNuevas.TryGetValue(nombreHijo, out var cuentaHijo))
                                    {
                                        cuentaHijo.CuentaPadreId = cuentaPadre.Id;
                                        _context.Update(cuentaHijo);
                                    }
                                }
                            }
                        }
                        
                        await _context.SaveChangesAsync();
                        
                        // 7. Tercer paso: transferir saldos si es necesario
                        if (transferirSaldos && saldosExistentes.Any())
                        {
                            _logger.LogInformation("Transfiriendo saldos");
                            foreach (var kvp in saldosExistentes)
                            {
                                var nombreCuenta = kvp.Key;
                                var saldos = kvp.Value;
                                
                                // Buscar cuenta equivalente en el nuevo catálogo
                                var cuentaNueva = cuentasNuevas.Values.FirstOrDefault(c => c.Nombre == nombreCuenta);
                                
                                if (cuentaNueva != null)
                                {
                                    foreach (var saldo in saldos)
                                    {
                                        var nuevoSaldo = new SaldoInicial
                                        {
                                            CuentaContableId = cuentaNueva.Id,
                                            ContactoId = saldo.ContactoId,
                                            Valor = saldo.Valor,
                                            FechaInicial = saldo.FechaInicial,
                                            EmpresaId = empresaId
                                        };
                                        
                                        _context.SaldosIniciales.Add(nuevoSaldo);
                                    }
                                }
                            }
                            
                            await _context.SaveChangesAsync();
                        }
                        
                        // 8. Confirmar transacción con manejo de errores
                        await transaction.CommitAsync();
                        _logger.LogInformation("Transacción completada exitosamente");
                        
                        TempData["SuccessMessage"] = "Catálogo de cuentas importado exitosamente.";
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        // 9. En caso de error, rollback de la transacción
                        await transaction.RollbackAsync();
                        _logger.LogError(ex, "Error durante la importación del catálogo");
                        
                        // Guardar detalles del error para depuración
                        TempData["ErrorMessage"] = $"Error al importar el catálogo: {ex.Message}";
                        if (ex.InnerException != null)
                        {
                            _logger.LogError(ex.InnerException, "Error interno durante importación");
                            TempData["ErrorDetails"] = ex.InnerException.Message;
                        }
                        
                        return RedirectToAction(nameof(ImportarExcel));
                    }
                }
            }
            catch (Exception ex)
            {
                // 10. Capturar cualquier excepción no controlada
                _logger.LogError(ex, "Error no controlado durante la importación");
                TempData["ErrorMessage"] = $"Error general al importar el catálogo: {ex.Message}";
                return RedirectToAction(nameof(ImportarExcel));
            }
        }

        private bool CuentaContableExists(int id)
        {
            return _context.CuentasContables.Any(e => e.Id == id);
        }

        private async Task<int> ObtenerSiguienteOrden(int? padreId, int empresaId)
        {
            if (padreId.HasValue)
            {
                var maxOrden = await _context.CuentasContables
                    .Where(c => c.CuentaPadreId == padreId && c.EmpresaId == empresaId)
                    .Select(c => (int?)c.Orden)
                    .MaxAsync() ?? 0;
                    
                return maxOrden + 1;
            }
            else
            {
                var maxOrden = await _context.CuentasContables
                    .Where(c => c.CuentaPadreId == null && c.EmpresaId == empresaId)
                    .Select(c => (int?)c.Orden)
                    .MaxAsync() ?? 0;
                    
                return maxOrden + 1;
            }
        }

        private async Task EliminarSubcuentasRecursivamente(int cuentaId)
        {
            var subcuentas = await _context.CuentasContables
                .Where(c => c.CuentaPadreId == cuentaId)
                .ToListAsync();
                
            foreach (var subcuenta in subcuentas)
            {
                await EliminarSubcuentasRecursivamente(subcuenta.Id);
                _context.CuentasContables.Remove(subcuenta);
            }
            
            await _context.SaveChangesAsync();
        }

        private async Task<List<CuentaContableImport>> ValidarCuentasImportadas(List<CuentaContableImport> cuentasImportadas)
        {
            var empresaId = _empresaService.ObtenerEmpresaActualId();
            var cuentasExistentesImport = await _context.CuentasContables
                .Where(c => c.EmpresaId == empresaId)
                .ToListAsync();

            foreach (var cuenta in cuentasImportadas)
            {
                // Validar que el código no exista
                if (cuentasExistentesImport.Any(c => c.Codigo == cuenta.Codigo))
                {
                    cuenta.Errores.Add($"Ya existe una cuenta con el código {cuenta.Codigo}");
                    continue;
                }

                // Validar que el código de la cuenta padre exista si se especificó
                if (!string.IsNullOrEmpty(cuenta.CodigoPadre))
                {
                    var cuentaPadre = cuentasExistentesImport.FirstOrDefault(c => c.Codigo == cuenta.CodigoPadre);
                    if (cuentaPadre == null)
                    {
                        cuenta.Errores.Add($"No se encontró la cuenta padre con código {cuenta.CodigoPadre}");
                    }
                }

                // Validar la naturaleza de la cuenta
                if (!string.IsNullOrEmpty(cuenta.Naturaleza) && 
                    cuenta.Naturaleza != "Deudora" && 
                    cuenta.Naturaleza != "Acreedora")
                {
                    cuenta.Errores.Add("La naturaleza de la cuenta debe ser 'Deudora' o 'Acreedora'");
                }

                // Validar el tipo de cuenta
                if (!string.IsNullOrEmpty(cuenta.TipoCuenta) && 
                    cuenta.TipoCuenta != "Mayor" && 
                    cuenta.TipoCuenta != "Movimiento")
                {
                    cuenta.Errores.Add("El tipo de cuenta debe ser 'Mayor' o 'Movimiento'");
                }

                // Validar la categoría
                if (!string.IsNullOrEmpty(cuenta.Categoria) && 
                    !new[] { "Activo", "Pasivo", "Patrimonio", "Ingreso", "Gasto", "Costo", "CuentaOrden" }
                        .Contains(cuenta.Categoria))
                {
                    cuenta.Errores.Add("La categoría no es válida");
                }
            }

            return cuentasImportadas;
        }

        private async Task<bool> ValidarImportacion(List<CuentaContableImport> cuentasImportadas)
        {
            var empresaId = _empresaService.ObtenerEmpresaActualId();
            var cuentasExistentesDb = await _context.CuentasContables
                .Where(c => c.EmpresaId == empresaId)
                .ToListAsync();

            // Validar que no haya códigos duplicados
            var codigosImportados = cuentasImportadas.Select(c => c.Codigo).ToList();
            if (codigosImportados.Count != codigosImportados.Distinct().Count())
            {
                return false;
            }

            // Validar que no haya códigos que ya existan en la base de datos
            var codigosExistentes = cuentasExistentesDb.Select(c => c.Codigo).ToList();
            if (codigosImportados.Any(c => codigosExistentes.Contains(c)))
            {
                return false;
            }

            return true;
        }
    }
} 