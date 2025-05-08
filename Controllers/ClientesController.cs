using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Models;
using SistemaContable.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.Text.Json;

namespace SistemaContable.Controllers
{
    /// <summary>
    /// Controlador para la gestión de clientes y proveedores en el sistema.
    /// Permite realizar operaciones CRUD y gestionar archivos asociados a los clientes.
    /// </summary>
    public class ClientesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ClientesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            var clientes = await _context.Clientes
                .Include(c => c.TipoIdentificacion)
                .Include(c => c.Municipio)
                .Include(c => c.Pais)
                .Include(c => c.PlazoPago)
                .Include(c => c.TipoNcf)
                .Include(c => c.ListaPrecio)
                .Include(c => c.Vendedor)
                .Where(c => c.EsCliente)
                .ToListAsync();

            return View(clientes);
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.TipoIdentificacion)
                .Include(c => c.Municipio)
                .Include(c => c.Pais)
                .Include(c => c.PlazoPago)
                .Include(c => c.TipoNcf)
                .Include(c => c.ListaPrecio)
                .Include(c => c.Vendedor)
                .Include(c => c.CuentaPorCobrar)
                .Include(c => c.CuentaPorPagar)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (cliente == null)
            {
                return NotFound();
            }

            var viewModel = new ClienteDetalleViewModel
            {
                Cliente = cliente,
                CuentasPorCobrar = 0,
                AnticiposRecibidos = 0,
                AnticiposEntregados = 0,
                PorPagar = 0,
                NotasCredito = 0,
                NotasDebito = 0
            };

            // Aquí se podrían cargar los datos financieros reales del cliente
            // Esto se implementará en el futuro cuando tengamos la lógica de facturas y pagos

            return View(viewModel);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            CargarViewBags();
            return View();
        }

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente, Microsoft.AspNetCore.Http.IFormFile imagen)
        {
            if (ModelState.IsValid)
            {
                // Procesar la imagen si se proporcionó una
                if (imagen != null && imagen.Length > 0)
                {
                    cliente.ImagenUrl = await GuardarImagen(imagen);
                }

                // Asegurar que al menos uno de los dos se seleccione
                if (!cliente.EsCliente && !cliente.EsProveedor)
                {
                    cliente.EsCliente = true; // Por defecto es cliente
                }

                // Establecer la fecha de creación
                cliente.FechaCreacion = DateTime.UtcNow;

                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            CargarViewBags();
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            CargarViewBags();

            // Cargar los textos para las cuentas contables existentes
            if (cliente.CuentaPorCobrarId.HasValue)
            {
                var cuentaPorCobrar = await _context.CuentasContables.FindAsync(cliente.CuentaPorCobrarId);
                if (cuentaPorCobrar != null)
                {
                    ViewData["CuentaPorCobrarTexto"] = $"{cuentaPorCobrar.Codigo} - {cuentaPorCobrar.Nombre}";
                }
            }

            if (cliente.CuentaPorPagarId.HasValue)
            {
                var cuentaPorPagar = await _context.CuentasContables.FindAsync(cliente.CuentaPorPagarId);
                if (cuentaPorPagar != null)
                {
                    ViewData["CuentaPorPagarTexto"] = $"{cuentaPorPagar.Codigo} - {cuentaPorPagar.Nombre}";
                }
            }

            // Cargar datos financieros para la visualización en el formulario
            ViewBag.CuentasPorCobrar = 0; // Se implementará en el futuro
            ViewBag.AnticiposRecibidos = 0;
            ViewBag.AnticiposEntregados = 0;
            ViewBag.PorPagar = 0;
            ViewBag.NotasCredito = 0;
            ViewBag.NotasDebito = 0;

            return View(cliente);
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cliente cliente, Microsoft.AspNetCore.Http.IFormFile imagen)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Obtener el cliente actual de la base de datos
                    var clienteActual = await _context.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
                    if (clienteActual == null)
                    {
                        return NotFound();
                    }

                    // Procesar la imagen si se proporcionó una nueva
                    if (imagen != null && imagen.Length > 0)
                    {
                        // Eliminar la imagen anterior si existe
                        if (!string.IsNullOrEmpty(clienteActual.ImagenUrl))
                        {
                            EliminarImagen(clienteActual.ImagenUrl);
                        }

                        cliente.ImagenUrl = await GuardarImagen(imagen);
                    }
                    else
                    {
                        // Mantener la imagen actual
                        cliente.ImagenUrl = clienteActual.ImagenUrl;
                    }

                    // Asegurar que al menos uno de los dos se seleccione
                    if (!cliente.EsCliente && !cliente.EsProveedor)
                    {
                        cliente.EsCliente = true; // Por defecto es cliente
                    }

                    // Mantener la fecha de creación original
                    cliente.FechaCreacion = clienteActual.FechaCreacion;
                    cliente.FechaModificacion = DateTime.UtcNow;

                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Id))
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

            CargarViewBags();
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.TipoIdentificacion)
                .Include(c => c.Municipio)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            
            if (cliente != null)
            {
                // Eliminar la imagen si existe
                if (!string.IsNullOrEmpty(cliente.ImagenUrl))
                {
                    EliminarImagen(cliente.ImagenUrl);
                }
                
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }
        
        // GET: Clientes/BuscarCuentasContables
        [HttpGet]
        public async Task<IActionResult> BuscarCuentasContables(string term)
        {
            if (string.IsNullOrEmpty(term))
            {
                return Json(new { results = new List<object>() });
            }

            var cuentas = await _context.CuentasContables
                .Where(c => c.Nombre.Contains(term, StringComparison.OrdinalIgnoreCase) || 
                           c.Codigo.Contains(term, StringComparison.OrdinalIgnoreCase))
                .Take(10)
                .Select(c => new 
                {
                    id = c.Id,
                    text = $"{c.Codigo} - {c.Nombre}"
                })
                .ToListAsync();

            return Json(new { results = cuentas });
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }

        private void CargarViewBags()
        {
            ViewBag.TipoIdentificacionId = new SelectList(_context.TiposIdentificacion, "Id", "Nombre");
            ViewBag.MunicipioId = new SelectList(_context.Municipios, "Id", "Nombre");
            ViewBag.PaisId = new SelectList(_context.Paises, "Id", "Nombre");
            ViewBag.PlazoPagoId = new SelectList(_context.PlazosPago, "Id", "Nombre");
            ViewBag.TipoNcfId = new SelectList(_context.ComprobantesFiscales, "Id", "Nombre");
            ViewBag.ListaPrecioId = new SelectList(_context.ListasPrecios, "Id", "Nombre");
            ViewBag.VendedorId = new SelectList(_context.Vendedores, "Id", "Nombre");
            
            // Obtener cuentas contables para cargar en Select2
            var cuentasContables = _context.CuentasContables
                .Where(c => c.Activo)
                .OrderBy(c => c.Codigo)
                .ToList();
            
            // Serializar para su uso en los controles Select2
            ViewBag.CuentasContablesJson = JsonSerializer.Serialize(
                cuentasContables.Select(c => new { 
                    id = c.Id, 
                    text = $"{c.Codigo} - {c.Nombre}",
                    codigo = c.Codigo,
                    nombre = c.Nombre
                }));
        }

        private async Task<string?> GuardarImagen(Microsoft.AspNetCore.Http.IFormFile imagen)
        {
            if (imagen == null)
            {
                return null;
            }

            // Validar que sea una imagen
            var extensionesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(imagen.FileName).ToLower();
            if (!extensionesPermitidas.Contains(extension))
            {
                throw new InvalidOperationException("El archivo debe ser una imagen válida.");
            }

            // Validar tamaño máximo (2MB)
            var tamañoMaximo = 2 * 1024 * 1024; // 2MB
            if (imagen.Length > tamañoMaximo)
            {
                throw new InvalidOperationException("El tamaño máximo permitido es 2MB.");
            }

            // Crear directorio para imágenes de clientes si no existe
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "clientes");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Generar nombre único para la imagen
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Guardar la imagen
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imagen.CopyToAsync(fileStream);
            }

            // Devolver la ruta relativa para guardar en la base de datos
            return $"/uploads/clientes/{uniqueFileName}";
        }

        private void EliminarImagen(string imagenUrl)
        {
            if (string.IsNullOrEmpty(imagenUrl))
            {
                return;
            }

            try
            {
                // Obtener la ruta completa del archivo
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, imagenUrl.TrimStart('/'));
                
                // Verificar si el archivo existe y eliminarlo
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            catch (Exception)
            {
                // Manejar cualquier error que ocurra al eliminar el archivo
                // Se podría registrar en un log
            }
        }
    }
} 