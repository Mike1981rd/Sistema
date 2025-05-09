using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaContable.Models;
using SistemaContable.Repositories.Interfaces;
using SistemaContable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace SistemaContable.Controllers
{
    public class EntradaDiarioController : Controller
    {
        private readonly IEntradaDiarioRepository _entradaDiarioRepository;
        private readonly ITipoEntradaDiarioRepository _tipoEntradaDiarioRepository;
        private readonly INumeracionEntradaDiarioRepository _numeracionRepository;
        private readonly ICuentaContableRepository _cuentaContableRepository;
        private readonly IContactoRepository _contactoRepository;

        public EntradaDiarioController(
            IEntradaDiarioRepository entradaDiarioRepository,
            ITipoEntradaDiarioRepository tipoEntradaDiarioRepository,
            INumeracionEntradaDiarioRepository numeracionRepository,
            ICuentaContableRepository cuentaContableRepository,
            IContactoRepository contactoRepository)
        {
            _entradaDiarioRepository = entradaDiarioRepository;
            _tipoEntradaDiarioRepository = tipoEntradaDiarioRepository;
            _numeracionRepository = numeracionRepository;
            _cuentaContableRepository = cuentaContableRepository;
            _contactoRepository = contactoRepository;
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
            
            return View(viewModel);
        }

        // Acción Create POST para guardar la nueva entrada de diario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EntradaDiarioViewModel viewModel)
        {
            // Normalizar los valores de débito y crédito antes de la validación
            if (viewModel.Movimientos != null)
            {
                foreach (var movimiento in viewModel.Movimientos)
                {
                    // Si hay string representación, convertirla a decimal
                    if (!string.IsNullOrEmpty(movimiento.DebitoStr))
                    {
                        // Convertir de formato "1.234,56" a formato numérico reconocible por el modelo
                        string valorLimpio = movimiento.DebitoStr.Replace(".", "").Replace(",", ".");
                        if (decimal.TryParse(valorLimpio, out decimal valorDecimal))
                        {
                            movimiento.Debito = valorDecimal;
                        }
                    }
                    
                    if (!string.IsNullOrEmpty(movimiento.CreditoStr))
                    {
                        // Convertir de formato "1.234,56" a formato numérico reconocible por el modelo
                        string valorLimpio = movimiento.CreditoStr.Replace(".", "").Replace(",", ".");
                        if (decimal.TryParse(valorLimpio, out decimal valorDecimal))
                        {
                            movimiento.Credito = valorDecimal;
                        }
                    }
                }
            }
            
            if (ModelState.IsValid)
            {
                // Verificar que débitos = créditos
                decimal totalDebito = viewModel.Movimientos?.Sum(m => m.Debito) ?? 0;
                decimal totalCredito = viewModel.Movimientos?.Sum(m => m.Credito) ?? 0;
                
                if (Math.Abs(totalDebito - totalCredito) > 0.001m)
                {
                    ModelState.AddModelError("", "El total de débitos debe ser igual al total de créditos");
                    viewModel.TiposEntrada = new SelectList(await _tipoEntradaDiarioRepository.GetAllAsync(), "Id", "Nombre");
                    CargarCuentasContables();
                    CargarContactos();
                    return View(viewModel);
                }

                // Obtener la numeración
                var numeracion = await _numeracionRepository.GetByIdAsync(viewModel.NumeracionId);
                
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

            // Si llegamos aquí, algo falló
            viewModel.TiposEntrada = new SelectList(await _tipoEntradaDiarioRepository.GetAllAsync(), "Id", "Nombre");
            CargarCuentasContables();
            CargarContactos();
            return View(viewModel);
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

        // Acción para buscar contactos (para Select2)
        [HttpGet]
        public async Task<IActionResult> BuscarContactos(string term)
        {
            try
            {
                // Asegurarse de que term no sea null
                term = term ?? string.Empty;
                
                Console.WriteLine($"Buscando contactos (clientes/proveedores) con término: '{term}'");
                
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

        // Implementa las acciones Edit, Details, Delete
        // [...]
    }
} 