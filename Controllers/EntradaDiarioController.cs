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
            await CargarCuentasContables();
            await CargarContactos();
            
            return View(viewModel);
        }

        // Acción Create POST para guardar la nueva entrada de diario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EntradaDiarioViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Verificar que débitos = créditos
                if (viewModel.TotalDebito != viewModel.TotalCredito)
                {
                    ModelState.AddModelError("", "El total de débitos debe ser igual al total de créditos");
                    viewModel.TiposEntrada = new SelectList(await _tipoEntradaDiarioRepository.GetAllAsync(), "Id", "Nombre");
                    await CargarCuentasContables();
                    await CargarContactos();
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
            await CargarCuentasContables();
            await CargarContactos();
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
            var cuentas = await _cuentaContableRepository.BuscarPorNombreOCodigoAsync(term);
            return Json(cuentas.Select(c => new
            {
                id = c.Id,
                text = $"{c.Codigo} - {c.Nombre}",
                codigo = c.Codigo,
                nombre = c.Nombre
            }));
        }

        // Acción para buscar contactos (para Select2)
        [HttpGet]
        public async Task<IActionResult> BuscarContactos(string term)
        {
            var contactos = await _contactoRepository.BuscarPorNombreAsync(term);
            return Json(contactos.Select(c => new
            {
                id = c.Id,
                text = c.Nombre,
                tipo = c.EsCliente ? "C" : "P", // C para clientes, P para proveedores
                identificacion = c.Identificacion
            }));
        }
        
        // Método privado para cargar cuentas contables para Select2
        private async Task CargarCuentasContables()
        {
            var cuentas = await _cuentaContableRepository.GetCuentasMovimientoAsync(1); // Empresa ID 1 por defecto
            
            ViewBag.CuentasContablesJson = JsonSerializer.Serialize(cuentas.Select(c => new
            {
                id = c.Id,
                text = $"{c.Codigo} - {c.Nombre}",
                codigo = c.Codigo,
                nombre = c.Nombre
            }));
        }
        
        // Método privado para cargar contactos para Select2
        private async Task CargarContactos()
        {
            var contactos = await _contactoRepository.GetAllAsync();
            
            ViewBag.ContactosJson = JsonSerializer.Serialize(contactos.Where(c => c.Activo).Select(c => new
            {
                id = c.Id,
                text = c.Nombre,
                tipo = c.EsCliente ? "C" : "P", // C para clientes, P para proveedores
                identificacion = c.Identificacion
            }));
        }

        // Implementa las acciones Edit, Details, Delete
        // [...]
    }
} 