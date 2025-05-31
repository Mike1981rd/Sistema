using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaContable.Models;
using SistemaContable.Repositories.Interfaces;
using SistemaContable.ViewModels;
using SistemaContable.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using System.IO;
using OfficeOpenXml;

namespace SistemaContable.Controllers
{
    public class EntradaDiarioController : Controller
    {
        private readonly IEntradaDiarioRepository _entradaDiarioRepository;
        private readonly ITipoEntradaDiarioRepository _tipoEntradaDiarioRepository;
        private readonly INumeracionEntradaDiarioRepository _numeracionRepository;
        private readonly ICuentaContableRepository _cuentaContableRepository;
        private readonly IContactoRepository _contactoRepository;
        private readonly IEmpresaService _empresaService;
        private readonly SistemaContable.Data.ApplicationDbContext _dbContext;

        public EntradaDiarioController(
            IEntradaDiarioRepository entradaDiarioRepository,
            ITipoEntradaDiarioRepository tipoEntradaDiarioRepository,
            INumeracionEntradaDiarioRepository numeracionRepository,
            ICuentaContableRepository cuentaContableRepository,
            IContactoRepository contactoRepository,
            IEmpresaService empresaService,
            SistemaContable.Data.ApplicationDbContext dbContext)
        {
            _entradaDiarioRepository = entradaDiarioRepository;
            _tipoEntradaDiarioRepository = tipoEntradaDiarioRepository;
            _numeracionRepository = numeracionRepository;
            _cuentaContableRepository = cuentaContableRepository;
            _contactoRepository = contactoRepository;
            _empresaService = empresaService;
            _dbContext = dbContext;
        }

        // Acción Index para listar todas las entradas de diario
        public async Task<IActionResult> Index()
        {
            var entradas = await _entradaDiarioRepository.GetAllAsync();
            return View(entradas);
        }

        // Acción Create GET para mostrar el formulario de creación
        public async Task<IActionResult> Create()
        {
            var viewModel = new EntradaDiarioViewModel
            {
                Fecha = DateTime.UtcNow.Date,
                TiposEntrada = new SelectList(await _tipoEntradaDiarioRepository.GetAllAsync(), "Id", "Nombre"),
                Estado = "Abierto",
                Movimientos = new List<MovimientoContableViewModel>
                {
                    new MovimientoContableViewModel { Debito = 0, Credito = 0 },
                    new MovimientoContableViewModel { Debito = 0, Credito = 0 }
                }
            };
            
            // Cargar datos para Select2
            CargarCuentasContables();
            CargarContactos();
            
            // Obtener la configuración de formato decimal de la empresa actual
            await ConfigurarFormatoDecimal();
            
            // Precargar información de la numeración (cuando el usuario seleccione tipo)
            ViewBag.MostrarCodigoEjemplo = true;
            
            return View(viewModel);
        }

        // Acción Create POST para guardar la nueva entrada de diario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EntradaDiarioViewModel viewModel)
        {
            try 
            {
                // Ignorar las validaciones para campos no obligatorios
                ModelState.Clear(); // Limpiar todas las validaciones del ModelState
                
                // Procesar valores de DebitoStr y CreditoStr si están presentes
                if (viewModel.Movimientos != null)
                {
                    foreach (var movimiento in viewModel.Movimientos)
                    {
                        // Convertir string a decimal si hay valores en DebitoStr o CreditoStr
                        if (!string.IsNullOrWhiteSpace(movimiento.DebitoStr))
                        {
                            // Convertir el valor formateado (1,234.56) a decimal
                            string valorLimpio = movimiento.DebitoStr.Replace(",", "");
                            if (decimal.TryParse(valorLimpio, out decimal valorDecimal))
                            {
                                movimiento.Debito = valorDecimal;
                                Console.WriteLine($"Convertido DebitoStr: '{movimiento.DebitoStr}' a Debito: {movimiento.Debito}");
                            }
                        }
                        
                        if (!string.IsNullOrWhiteSpace(movimiento.CreditoStr))
                        {
                            // Convertir el valor formateado (1,234.56) a decimal
                            string valorLimpio = movimiento.CreditoStr.Replace(",", "");
                            if (decimal.TryParse(valorLimpio, out decimal valorDecimal))
                            {
                                movimiento.Credito = valorDecimal;
                                Console.WriteLine($"Convertido CreditoStr: '{movimiento.CreditoStr}' a Credito: {movimiento.Credito}");
                            }
                        }
                    }
                }
                
                // Verificar que débitos = créditos
                decimal totalDebito = viewModel.Movimientos?.Sum(m => m.Debito) ?? 0;
                decimal totalCredito = viewModel.Movimientos?.Sum(m => m.Credito) ?? 0;
                
                if (Math.Abs(totalDebito - totalCredito) > 0.001m)
                {
                    ModelState.AddModelError("", "El total de débitos debe ser igual al total de créditos");
                    viewModel.TiposEntrada = new SelectList(await _tipoEntradaDiarioRepository.GetAllAsync(), "Id", "Nombre");
                    await CargarDatosAuxiliares(viewModel);
                    return View(viewModel);
                }
                
                // Validar que cada movimiento tenga al menos un valor en débito o crédito (no ambos cero)
                Console.WriteLine("Validando movimientos...");
                bool hayMovimientosValidos = false;

                foreach (var movimiento in viewModel.Movimientos.Where(m => m.CuentaContableId > 0))
                {
                    Console.WriteLine($"Movimiento - Cuenta: {movimiento.CuentaContableId}, Débito: {movimiento.Debito}, Crédito: {movimiento.Credito}");
                    
                    // Si al menos uno de los dos es mayor que cero, es un movimiento válido
                    if (movimiento.Debito > 0 || movimiento.Credito > 0)
                    {
                        hayMovimientosValidos = true;
                    }
                    else
                    {
                        Console.WriteLine("Movimiento con débito y crédito en cero");
                    }
                }

                if (!hayMovimientosValidos)
                {
                    ModelState.AddModelError("", "Debe haber al menos un movimiento con valor en Débito o Crédito");
                    viewModel.TiposEntrada = new SelectList(await _tipoEntradaDiarioRepository.GetAllAsync(), "Id", "Nombre");
                    await CargarDatosAuxiliares(viewModel);
                    return View(viewModel);
                }
                
                // Obtener la numeración
                var numeracion = await _numeracionRepository.GetByIdAsync(viewModel.NumeracionId);
                if (numeracion == null)
                {
                    ModelState.AddModelError("NumeracionId", "Numeración no encontrada");
                    viewModel.TiposEntrada = new SelectList(await _tipoEntradaDiarioRepository.GetAllAsync(), "Id", "Nombre");
                    await CargarDatosAuxiliares(viewModel);
                    return View(viewModel);
                }
                
                // Crear la entrada de diario
                var entradaDiario = new EntradaDiario
                {
                    Fecha = viewModel.Fecha.ToUniversalTime(), // Convertir a UTC
                    TipoEntradaId = viewModel.TipoEntradaId,
                    NumeracionId = viewModel.NumeracionId,
                    Codigo = $"{numeracion.Prefijo}-{numeracion.NumeroActual}",
                    Observaciones = viewModel.Observaciones,
                    Estado = EstadoEntradaDiario.Abierto,
                    FechaCreacion = DateTime.UtcNow
                };
                
                // Agregar los movimientos
                var movimientos = new List<MovimientoContable>();
                foreach (var movimiento in viewModel.Movimientos.Where(m => m.CuentaContableId > 0))
                {
                    movimientos.Add(new MovimientoContable
                    {
                        CuentaContableId = movimiento.CuentaContableId,
                        ContactoId = movimiento.ContactoId,
                        TipoContacto = movimiento.TipoContacto,
                        NumeroDocumento = movimiento.NumeroDocumento,
                        Descripcion = movimiento.Descripcion,
                        Debito = movimiento.Debito,
                        Credito = movimiento.Credito
                    });
                }
                
                // Guardar la entrada con sus movimientos
                numeracion.NumeroActual++; // Incrementar el número actual
                await _numeracionRepository.UpdateAsync(numeracion);
                
                // Crear la entrada de diario con sus movimientos
                entradaDiario.Movimientos = movimientos;
                await _entradaDiarioRepository.CreateAsync(entradaDiario);
                
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log del error
                Console.WriteLine($"Error al guardar entrada de diario: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                
                // Agregar error al ModelState
                ModelState.AddModelError("", $"Error al guardar: {ex.Message}");
                
                // Volver a cargar los datos auxiliares
                viewModel.TiposEntrada = new SelectList(await _tipoEntradaDiarioRepository.GetAllAsync(), "Id", "Nombre");
                await CargarDatosAuxiliares(viewModel);
                
                return View(viewModel);
            }
        }

        // Método auxiliar para cargar datos en el viewModel
        private async Task CargarDatosAuxiliares(EntradaDiarioViewModel viewModel)
        {
            CargarCuentasContables();
            CargarContactos();
            await ConfigurarFormatoDecimal();
            
            // Previsualizar el código si hay numeración seleccionada
            if (viewModel.NumeracionId > 0)
            {
                var numeracion = await _numeracionRepository.GetByIdAsync(viewModel.NumeracionId);
                if (numeracion != null)
                {
                    viewModel.Codigo = $"{numeracion.Prefijo}-{numeracion.NumeroActual}";
                }
            }
            
            ViewBag.MostrarCodigoEjemplo = true;
        }

        // Acción para cargar las numeraciones según el tipo seleccionado
        [HttpGet]
        public async Task<IActionResult> GetNumeraciones(int tipoEntradaId)
        {
            var numeraciones = await _numeracionRepository.GetByTipoEntradaDiarioIdAsync(tipoEntradaId);
            return Json(numeraciones.Select(n => new { id = n.Id, nombre = n.Nombre }));
        }

        // Acción para buscar cuentas contables (para Select2)
        [HttpGet]
        public async Task<IActionResult> BuscarCuentasContables(string term)
        {
            try
            {
                // Asegurarse de que term no sea null
                term = term ?? string.Empty;
                
                Console.WriteLine($"Buscando cuentas contables con término: '{term}'");
                
                if (string.IsNullOrEmpty(term))
                {
                    return Json(new { results = new List<object>() });
                }
                
                var cuentas = await _cuentaContableRepository.BuscarPorNombreOCodigoAsync(term);
                
                Console.WriteLine($"Encontradas {cuentas.Count()} cuentas contables para devolver al cliente");
                
                // Asegurarnos de que los datos estén en el formato esperado por Select2
                var results = cuentas.Select(c => new
                {
                    id = c.Id,
                    text = $"{c.Codigo} - {c.Nombre}", // Este es el texto que se muestra en el select
                    codigo = c.Codigo,
                    nombre = c.Nombre
                }).ToList();
                
                // Imprimir los primeros resultados para depuración
                foreach (var result in results.Take(5))
                {
                    Console.WriteLine($"- Select2 resultado: id={result.id}, text='{result.text}'");
                }

                // Envolver los resultados en un objeto con la propiedad 'results' que es lo que espera Select2
                return Json(new { results = results });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al buscar cuentas contables: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                // Devolver un array vacío en caso de error en lugar de error 500
                return Json(new { results = new List<object>() });
            }
        }

        // GET: EntradaDiario/ObtenerCuentaContable
        [HttpGet]
        public async Task<IActionResult> ObtenerCuentaContable(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return Json(new { success = false, message = "ID inválido" });
                }

                var cuenta = await _dbContext.CuentasContables
                    .Where(c => c.Id == id)
                    .Select(c => new
                    {
                        id = c.Id,
                        text = $"{c.Codigo} - {c.Nombre}",
                        codigo = c.Codigo,
                        nombre = c.Nombre
                    })
                    .FirstOrDefaultAsync();

                if (cuenta == null)
                {
                    return Json(new { success = false, message = "Cuenta no encontrada" });
                }

                return Json(new { success = true, cuenta = cuenta });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener cuenta contable: {ex.Message}");
                return Json(new { success = false, message = "Error interno del servidor" });
            }
        }

        // Acción para buscar contactos (para Select2)
        [HttpGet]
        public async Task<IActionResult> BuscarContactos(string term)
        {
            try
            {
                // Asegurarse de que term no sea null
                term = term ?? string.Empty;
                
                Console.WriteLine($"Buscando contactos con término: '{term}'");
                
                if (string.IsNullOrEmpty(term))
                {
                    return Json(new { results = new List<object>() });
                }
                
                var contactos = await _contactoRepository.BuscarPorNombreAsync(term);
                
                Console.WriteLine($"Encontrados {contactos.Count()} contactos para devolver al cliente");
                
                // Asegurarnos de que los datos estén en el formato esperado por Select2
                var results = contactos.Select(c => new
                {
                    id = c.Id,
                    text = $"{c.Nombre} ({(c.EsCliente ? "Cliente" : "Proveedor")})",
                    tipo = c.EsCliente ? "C" : "P", // C para clientes, P para proveedores
                    identificacion = c.Identificacion ?? "",
                    nombre = c.Nombre
                }).ToList();
                
                // Imprimir los primeros resultados para depuración
                foreach (var result in results.Take(5))
                {
                    Console.WriteLine($"- Select2 resultado (contacto): id={result.id}, text='{result.text}', tipo={result.tipo}");
                }

                // Envolver los resultados en un objeto con la propiedad 'results' que es lo que espera Select2
                return Json(new { results = results });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al buscar contactos: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                // Devolver un array vacío en caso de error en lugar de error 500
                return Json(new { results = new List<object>() });
            }
        }
        
        // Método privado para cargar cuentas contables para Select2
        private void CargarCuentasContables()
        {
            try
            {
                // No longer need to serialize everything to ViewBag
                // Just set an empty array for initial render
                ViewBag.CuentasContablesJson = "[]";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Excepción al cargar cuentas contables: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                ViewBag.CuentasContablesJson = "[]";
            }
        }
        
        // Método privado para cargar contactos para Select2
        private void CargarContactos()
        {
            try
            {
                // No longer need to serialize everything to ViewBag
                // Just set an empty array for initial render
                ViewBag.ContactosJson = "[]";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Excepción al cargar contactos: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                ViewBag.ContactosJson = "[]";
            }
        }

        // Método temporal para verificar directamente la base de datos
        [HttpGet]
        public async Task<IActionResult> DepurarCuentasContables(string query = "IT")
        {
            try
            {
                var _context = HttpContext.RequestServices.GetService<SistemaContable.Data.ApplicationDbContext>();
                
                var todasCuentas = await _context.CuentasContables.ToListAsync();
                Console.WriteLine($"Total de cuentas en BD: {todasCuentas.Count}");
                
                // Buscar cuentas que contienen el término de consulta manualmente
                var cuentasFiltradas = todasCuentas
                    .Where(c => c.Codigo.Contains(query, StringComparison.OrdinalIgnoreCase) || 
                               c.Nombre.Contains(query, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                
                Console.WriteLine($"Cuentas con '{query}': {cuentasFiltradas.Count}");
                
                // Construir la respuesta
                var result = new
                {
                    TotalCuentas = todasCuentas.Count,
                    CuentasEncontradas = cuentasFiltradas.Count,
                    Resultados = cuentasFiltradas.Select(c => new
                    {
                        ID = c.Id,
                        Codigo = c.Codigo,
                        Nombre = c.Nombre,
                        TipoCuenta = c.TipoCuenta,
                        UsoCuenta = c.UsoCuenta,
                        Activo = c.Activo
                    }).ToList()
                };
                
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        // Método privado para configurar el formato decimal según preferencias de la empresa
        private async Task ConfigurarFormatoDecimal()
        {
            try
            {
                // Obtener la empresa actual
                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                var empresa = await _dbContext.Empresas.FindAsync(empresaId);
                
                // Configuración de formato decimal
                string separadorDecimal = empresa?.SeparadorDecimal ?? ",";
                int precisionDecimal = empresa?.PrecisionDecimal ?? 2;
                
                ViewBag.SeparadorDecimal = separadorDecimal;
                ViewBag.PrecisionDecimal = precisionDecimal;
                
                Console.WriteLine($"Configuración de formato decimal: Separador={ViewBag.SeparadorDecimal}, Precisión={ViewBag.PrecisionDecimal}");
            }
            catch (Exception ex)
            {
                // En caso de error, usar configuración por defecto
                Console.WriteLine($"Error al obtener configuración decimal: {ex.Message}");
                ViewBag.SeparadorDecimal = ",";
                ViewBag.PrecisionDecimal = 2;
            }
        }

        // Acción para obtener una previsualización del código que se generará
        [HttpGet]
        public async Task<IActionResult> ObtenerPrevisualizacionCodigo(int numeracionId)
        {
            try
            {
                var numeracion = await _numeracionRepository.GetByIdAsync(numeracionId);
                if (numeracion == null)
                {
                    return NotFound(new { message = "Numeración no encontrada" });
                }
                
                // Generar la previsualización del código
                var codigo = $"{numeracion.Prefijo}-{numeracion.NumeroActual}";
                
                return Json(new { codigo = codigo });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error: {ex.Message}" });
            }
        }

        // Acción para ver los detalles de una entrada de diario
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var entradaDiario = await _entradaDiarioRepository.GetByIdWithDetailsAsync(id);
            if (entradaDiario == null)
            {
                return NotFound();
            }

            var viewModel = new EntradaDiarioViewModel
            {
                Id = entradaDiario.Id,
                Fecha = entradaDiario.Fecha,
                TipoEntradaId = entradaDiario.TipoEntradaId,
                NumeracionId = entradaDiario.NumeracionId,
                Codigo = entradaDiario.Codigo,
                Observaciones = entradaDiario.Observaciones,
                Estado = entradaDiario.Estado.ToString(),
                Movimientos = entradaDiario.Movimientos.Select(m => new MovimientoContableViewModel
                {
                    Id = m.Id,
                    CuentaContableId = m.CuentaContableId,
                    ContactoId = m.ContactoId,
                    TipoContacto = m.TipoContacto,
                    NumeroDocumento = m.NumeroDocumento,
                    Descripcion = m.Descripcion,
                    Debito = m.Debito,
                    Credito = m.Credito,
                    CuentaContableNombre = m.CuentaContable?.Nombre,
                    ContactoNombre = m.Contacto != null ? m.Contacto.NombreRazonSocial : null
                }).ToList()
            };
            
            // Cargar el tipo de entrada y numeración para mostrarlos
            var tipoEntrada = await _tipoEntradaDiarioRepository.GetByIdAsync(entradaDiario.TipoEntradaId);
            var numeracion = await _numeracionRepository.GetByIdAsync(entradaDiario.NumeracionId);
            
            ViewBag.TipoEntradaNombre = tipoEntrada?.Nombre;
            ViewBag.NumeracionNombre = numeracion?.Nombre;
            
            // Configurar formato decimal
            await ConfigurarFormatoDecimal();
            
            return View(viewModel);
        }

        // Acción para la edición de una entrada de diario (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var entradaDiario = await _entradaDiarioRepository.GetByIdWithDetailsAsync(id);
            if (entradaDiario == null)
            {
                return NotFound();
            }
            
            // Solo se pueden editar entradas en estado Abierto
            if (entradaDiario.Estado != EstadoEntradaDiario.Abierto)
            {
                TempData["Error"] = "Solo se pueden editar entradas en estado Abierto";
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new EntradaDiarioViewModel
            {
                Id = entradaDiario.Id,
                Fecha = entradaDiario.Fecha,
                TipoEntradaId = entradaDiario.TipoEntradaId,
                NumeracionId = entradaDiario.NumeracionId,
                Codigo = entradaDiario.Codigo,
                Observaciones = entradaDiario.Observaciones,
                Estado = entradaDiario.Estado.ToString(),
                TiposEntrada = new SelectList(await _tipoEntradaDiarioRepository.GetAllAsync(), "Id", "Nombre"),
                Movimientos = entradaDiario.Movimientos.Select(m => new MovimientoContableViewModel
                {
                    Id = m.Id,
                    EntradaDiarioId = m.EntradaDiarioId,
                    CuentaContableId = m.CuentaContableId,
                    ContactoId = m.ContactoId,
                    TipoContacto = m.TipoContacto,
                    NumeroDocumento = m.NumeroDocumento,
                    Descripcion = m.Descripcion,
                    Debito = m.Debito,
                    Credito = m.Credito
                }).ToList()
            };
            
            // Cargar datos para Select2
            CargarCuentasContables();
            CargarContactos();
            
            // Obtener la configuración de formato decimal de la empresa actual
            await ConfigurarFormatoDecimal();
            
            return View(viewModel);
        }

        // Acción para la edición de una entrada de diario (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EntradaDiarioViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }
            
            try
            {
                // Obtener la entrada original para verificar su estado
                var entradaOriginal = await _entradaDiarioRepository.GetByIdWithDetailsAsync(id);
                if (entradaOriginal == null)
                {
                    return NotFound();
                }
                
                // Solo se pueden editar entradas en estado Abierto
                if (entradaOriginal.Estado != EstadoEntradaDiario.Abierto)
                {
                    TempData["Error"] = "Solo se pueden editar entradas en estado Abierto";
                    return RedirectToAction(nameof(Index));
                }
                
                // Ignorar las validaciones del ModelState
                ModelState.Clear();
                
                // Procesar valores de DebitoStr y CreditoStr si están presentes
                if (viewModel.Movimientos != null)
                {
                    foreach (var movimiento in viewModel.Movimientos)
                    {
                        // Convertir string a decimal si hay valores en DebitoStr o CreditoStr
                        if (!string.IsNullOrWhiteSpace(movimiento.DebitoStr))
                        {
                            string valorLimpio = movimiento.DebitoStr.Replace(",", "");
                            if (decimal.TryParse(valorLimpio, out decimal valorDecimal))
                            {
                                movimiento.Debito = valorDecimal;
                            }
                        }
                        
                        if (!string.IsNullOrWhiteSpace(movimiento.CreditoStr))
                        {
                            string valorLimpio = movimiento.CreditoStr.Replace(",", "");
                            if (decimal.TryParse(valorLimpio, out decimal valorDecimal))
                            {
                                movimiento.Credito = valorDecimal;
                            }
                        }
                    }
                }
                
                // Verificar que débitos = créditos
                decimal totalDebito = viewModel.Movimientos?.Sum(m => m.Debito) ?? 0;
                decimal totalCredito = viewModel.Movimientos?.Sum(m => m.Credito) ?? 0;
                
                if (Math.Abs(totalDebito - totalCredito) > 0.001m)
                {
                    ModelState.AddModelError("", "El total de débitos debe ser igual al total de créditos");
                    viewModel.TiposEntrada = new SelectList(await _tipoEntradaDiarioRepository.GetAllAsync(), "Id", "Nombre");
                    await CargarDatosAuxiliares(viewModel);
                    return View(viewModel);
                }
                
                // Validar que cada movimiento tenga al menos un valor en débito o crédito (no ambos cero)
                Console.WriteLine("Validando movimientos...");
                bool hayMovimientosValidos = false;

                foreach (var movimiento in viewModel.Movimientos.Where(m => m.CuentaContableId > 0))
                {
                    Console.WriteLine($"Movimiento - Cuenta: {movimiento.CuentaContableId}, Débito: {movimiento.Debito}, Crédito: {movimiento.Credito}");
                    
                    // Si al menos uno de los dos es mayor que cero, es un movimiento válido
                    if (movimiento.Debito > 0 || movimiento.Credito > 0)
                    {
                        hayMovimientosValidos = true;
                    }
                    else
                    {
                        Console.WriteLine("Movimiento con débito y crédito en cero");
                    }
                }

                if (!hayMovimientosValidos)
                {
                    ModelState.AddModelError("", "Debe haber al menos un movimiento con valor en Débito o Crédito");
                    viewModel.TiposEntrada = new SelectList(await _tipoEntradaDiarioRepository.GetAllAsync(), "Id", "Nombre");
                    await CargarDatosAuxiliares(viewModel);
                    return View(viewModel);
                }
                
                // Actualizar la entrada de diario
                var entradaDiario = new EntradaDiario
                {
                    Id = viewModel.Id,
                    Fecha = viewModel.Fecha.ToUniversalTime(),
                    TipoEntradaId = entradaOriginal.TipoEntradaId, // No permitir cambiar tipo
                    NumeracionId = entradaOriginal.NumeracionId, // No permitir cambiar numeración
                    Codigo = entradaOriginal.Codigo, // Mantener código original
                    Observaciones = viewModel.Observaciones,
                    Estado = EstadoEntradaDiario.Abierto,
                    FechaCreacion = entradaOriginal.FechaCreacion,
                    FechaModificacion = DateTime.UtcNow
                };
                
                // Preparar los movimientos actualizados
                var movimientosActualizados = new List<MovimientoContable>();
                foreach (var movimiento in viewModel.Movimientos.Where(m => m.CuentaContableId > 0))
                {
                    movimientosActualizados.Add(new MovimientoContable
                    {
                        Id = movimiento.Id,
                        EntradaDiarioId = entradaDiario.Id,
                        CuentaContableId = movimiento.CuentaContableId,
                        ContactoId = movimiento.ContactoId,
                        TipoContacto = movimiento.TipoContacto,
                        NumeroDocumento = movimiento.NumeroDocumento,
                        Descripcion = movimiento.Descripcion,
                        Debito = movimiento.Debito,
                        Credito = movimiento.Credito
                    });
                }
                
                // Actualizar en la base de datos
                entradaDiario.Movimientos = movimientosActualizados;
                await _entradaDiarioRepository.UpdateAsync(entradaDiario);
                
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log de error
                Console.WriteLine($"Error al editar entrada de diario: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                
                ModelState.AddModelError("", $"Error al editar: {ex.Message}");
                viewModel.TiposEntrada = new SelectList(await _tipoEntradaDiarioRepository.GetAllAsync(), "Id", "Nombre");
                await CargarDatosAuxiliares(viewModel);
                return View(viewModel);
            }
        }

        // Acción para mostrar la pantalla de confirmación de eliminación
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var entradaDiario = await _entradaDiarioRepository.GetByIdWithDetailsAsync(id);
            if (entradaDiario == null)
            {
                return NotFound();
            }
            
            // Solo se pueden eliminar entradas en estado Abierto
            if (entradaDiario.Estado != EstadoEntradaDiario.Abierto)
            {
                TempData["Error"] = "Solo se pueden eliminar entradas en estado Abierto";
                return RedirectToAction(nameof(Index));
            }
            
            var viewModel = new EntradaDiarioViewModel
            {
                Id = entradaDiario.Id,
                Fecha = entradaDiario.Fecha,
                Codigo = entradaDiario.Codigo,
                Observaciones = entradaDiario.Observaciones,
                Estado = entradaDiario.Estado.ToString(),
                Movimientos = entradaDiario.Movimientos.Select(m => new MovimientoContableViewModel
                {
                    Id = m.Id,
                    CuentaContableId = m.CuentaContableId,
                    ContactoId = m.ContactoId,
                    Debito = m.Debito,
                    Credito = m.Credito,
                    CuentaContableNombre = m.CuentaContable?.Nombre,
                    ContactoNombre = m.Contacto != null ? m.Contacto.NombreRazonSocial : null
                }).ToList()
            };
            
            // Cargar el tipo de entrada y numeración para mostrarlos
            var tipoEntrada = await _tipoEntradaDiarioRepository.GetByIdAsync(entradaDiario.TipoEntradaId);
            ViewBag.TipoEntradaNombre = tipoEntrada?.Nombre;
            
            // Configurar formato decimal
            await ConfigurarFormatoDecimal();
            
            return View(viewModel);
        }

        // Acción para eliminar definitivamente la entrada
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var entradaDiario = await _entradaDiarioRepository.GetByIdAsync(id);
                if (entradaDiario == null)
                {
                    return NotFound();
                }
                
                // Solo se pueden eliminar entradas en estado Abierto
                if (entradaDiario.Estado != EstadoEntradaDiario.Abierto)
                {
                    TempData["Error"] = "Solo se pueden eliminar entradas en estado Abierto";
                    return RedirectToAction(nameof(Index));
                }
                
                // Eliminar la entrada
                await _entradaDiarioRepository.DeleteAsync(id);
                
                TempData["Success"] = "Entrada de diario eliminada correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al eliminar: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // Método para cerrar una entrada de diario (cambiar estado a Cerrado)
        [HttpPost]
        public async Task<IActionResult> Cerrar(int id)
        {
            try
            {
                var entradaDiario = await _entradaDiarioRepository.GetByIdAsync(id);
                if (entradaDiario == null)
                {
                    return NotFound();
                }
                
                // Solo se pueden cerrar entradas en estado Abierto
                if (entradaDiario.Estado != EstadoEntradaDiario.Abierto)
                {
                    return BadRequest("Solo se pueden cerrar entradas en estado Abierto");
                }
                
                // Cambiar estado a Cerrado
                entradaDiario.Estado = EstadoEntradaDiario.Cerrado;
                entradaDiario.FechaCierre = DateTime.UtcNow;
                
                // Actualizar en la base de datos
                await _entradaDiarioRepository.UpdateAsync(entradaDiario);
                
                return Ok(new { success = true, message = "Entrada cerrada correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        // Método para anular una entrada de diario (cambiar estado a Anulado)
        [HttpPost]
        public async Task<IActionResult> Anular(int id)
        {
            try
            {
                var entradaDiario = await _entradaDiarioRepository.GetByIdAsync(id);
                if (entradaDiario == null)
                {
                    return NotFound();
                }
                
                // Solo se pueden anular entradas en estado Abierto o Cerrado
                if (entradaDiario.Estado == EstadoEntradaDiario.Anulado)
                {
                    return BadRequest("La entrada ya está anulada");
                }
                
                // Cambiar estado a Anulado
                entradaDiario.Estado = EstadoEntradaDiario.Anulado;
                entradaDiario.FechaAnulacion = DateTime.UtcNow;
                
                // Actualizar en la base de datos
                await _entradaDiarioRepository.UpdateAsync(entradaDiario);
                
                return Ok(new { success = true, message = "Entrada anulada correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        // Acción para exportar las entradas a Excel
        [HttpGet]
        public async Task<IActionResult> Exportar()
        {
            try
            {
                var entradas = await _entradaDiarioRepository.GetAllWithDetailsAsync();
                
                // Crear un nuevo paquete Excel
                using (var package = new OfficeOpenXml.ExcelPackage())
                {
                    // Agregar una hoja de trabajo
                    var worksheet = package.Workbook.Worksheets.Add("Entradas de Diario");
                    
                    // Agregar encabezados
                    worksheet.Cells[1, 1].Value = "Código";
                    worksheet.Cells[1, 2].Value = "Fecha";
                    worksheet.Cells[1, 3].Value = "Tipo";
                    worksheet.Cells[1, 4].Value = "Observaciones";
                    worksheet.Cells[1, 5].Value = "Total";
                    worksheet.Cells[1, 6].Value = "Estado";
                    
                    // Estilo para los encabezados
                    using (var range = worksheet.Cells[1, 1, 1, 6])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                        range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }
                    
                    // Agregar datos
                    int row = 2;
                    foreach (var entrada in entradas)
                    {
                        var tipoEntrada = await _tipoEntradaDiarioRepository.GetByIdAsync(entrada.TipoEntradaId);
                        
                        worksheet.Cells[row, 1].Value = entrada.Codigo;
                        worksheet.Cells[row, 2].Value = entrada.Fecha.ToLocalTime();
                        worksheet.Cells[row, 3].Value = tipoEntrada?.Nombre;
                        worksheet.Cells[row, 4].Value = entrada.Observaciones;
                        
                        // Calcular total (suma de débitos o créditos, son iguales)
                        decimal total = entrada.Movimientos.Sum(m => m.Debito);
                        worksheet.Cells[row, 5].Value = total;
                        
                        worksheet.Cells[row, 6].Value = entrada.Estado.ToString();
                        
                        row++;
                    }
                    
                    // Dar formato a la columna de fechas
                    worksheet.Column(2).Style.Numberformat.Format = "dd/MM/yyyy";
                    
                    // Dar formato a la columna de montos
                    worksheet.Column(5).Style.Numberformat.Format = "$#,##0.00";
                    
                    // Ajustar el ancho de las columnas
                    worksheet.Cells.AutoFitColumns();
                    
                    // Devolver como archivo para descargar
                    var stream = new MemoryStream();
                    package.SaveAs(stream);
                    stream.Position = 0;
                    
                    string fileName = $"Entradas_Diario_{DateTime.Now:yyyyMMdd}.xlsx";
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al exportar: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
} 