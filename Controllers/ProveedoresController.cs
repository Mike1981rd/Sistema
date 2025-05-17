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
using SistemaContable.Services;

namespace SistemaContable.Controllers
{
    /// <summary>
    /// Controlador para la gestión de proveedores en el sistema.
    /// Permite realizar operaciones CRUD y gestionar archivos asociados a los proveedores.
    /// </summary>
    public class ProveedoresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmpresaService _empresaService;

        public ProveedoresController(
            ApplicationDbContext context, 
            IWebHostEnvironment webHostEnvironment,
            IEmpresaService empresaService)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _empresaService = empresaService;
        }

        // GET: Proveedores
        public async Task<IActionResult> Index()
        {
            var proveedores = await _context.Clientes
                .Include(c => c.TipoIdentificacion)
                .Include(c => c.Municipio)
                .Include(c => c.Pais)
                .Include(c => c.PlazoPago)
                .Include(c => c.TipoNcf)
                .Include(c => c.ListaPrecio)
                .Include(c => c.Vendedor)
                .Where(c => c.EsProveedor)
                .ToListAsync();

            return View(proveedores);
        }

        // GET: Proveedores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Clientes
                .Include(c => c.TipoIdentificacion)
                .Include(c => c.Municipio)
                .Include(c => c.Pais)
                .Include(c => c.PlazoPago)
                .Include(c => c.TipoNcf)
                .Include(c => c.ListaPrecio)
                .Include(c => c.Vendedor)
                .Include(c => c.CuentaPorCobrar)
                .Include(c => c.CuentaPorPagar)
                .FirstOrDefaultAsync(m => m.Id == id && m.EsProveedor);

            if (proveedor == null)
            {
                return NotFound();
            }

            var viewModel = new ClienteDetalleViewModel
            {
                Cliente = proveedor,
                CuentasPorCobrar = 0,
                AnticiposRecibidos = 0,
                AnticiposEntregados = 0,
                PorPagar = 0,
                NotasCredito = 0,
                NotasDebito = 0
            };

            // Aquí se podrían cargar los datos financieros reales del proveedor
            // Esto se implementará en el futuro cuando tengamos la lógica de compras y pagos

            return View(viewModel);
        }

        // GET: Proveedores/Create
        public async Task<IActionResult> Create()
        {
            CargarViewBags();
            
            // Obtener la empresa actual para sus configuraciones
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            var empresa = await _context.Empresas.FindAsync(empresaId);
            
            // Configuración de formato decimal
            ViewBag.SeparadorDecimal = empresa?.SeparadorDecimal ?? ",";
            ViewBag.PrecisionDecimal = empresa?.PrecisionDecimal ?? 2;
            
            return View();
        }

        // POST: Proveedores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente proveedor, Microsoft.AspNetCore.Http.IFormFile imagen, string Pais)
        {
            if (ModelState.IsValid)
            {
                // Obtener el EmpresaId actual
                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                if (empresaId == 0) // O alguna otra validación de que se obtuvo un ID de empresa válido
                {
                    ModelState.AddModelError("", "No se pudo determinar la empresa actual. No se puede crear el proveedor.");
                    CargarViewBags();
                    return View(proveedor);
                }
                proveedor.EmpresaId = empresaId; // Asignar EmpresaId

                // Procesar la imagen si se proporcionó una
                if (imagen != null && imagen.Length > 0)
                {
                    proveedor.ImagenUrl = await GuardarImagen(imagen);
                }

                // Procesar el país seleccionado
                if (!string.IsNullOrEmpty(Pais))
                {
                    // Buscar el ID del país en la base de datos o crearlo si no existe
                    var paisEntity = await _context.Paises.FirstOrDefaultAsync(p => p.Nombre == Pais);
                    if (paisEntity == null)
                    {
                        // Obtener el código del país desde DataLists
                        var paisData = DataLists.LatinAmericanCountries.FirstOrDefault(c => c.Name == Pais);
                        if (paisData != null)
                        {
                            paisEntity = new Pais
                            {
                                Nombre = Pais,
                                Codigo = paisData.Code
                            };
                            _context.Paises.Add(paisEntity);
                            await _context.SaveChangesAsync();
                        }
                    }
                    
                    // Asignar el ID del país al proveedor
                    if (paisEntity != null)
                    {
                        proveedor.PaisId = paisEntity.Id;
                    }
                }

                // Establecer como proveedor
                proveedor.EsProveedor = true;

                // Establecer la fecha de creación
                proveedor.FechaCreacion = DateTime.UtcNow;

                _context.Add(proveedor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            CargarViewBags();
            return View(proveedor);
        }

        // GET: Proveedores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Clientes.FindAsync(id);
            if (proveedor == null || !proveedor.EsProveedor)
            {
                return NotFound();
            }

            CargarViewBags();

            // Cargar los textos para las cuentas contables existentes
            if (proveedor.CuentaPorCobrarId.HasValue)
            {
                var cuentaPorCobrar = await _context.CuentasContables.FindAsync(proveedor.CuentaPorCobrarId);
                if (cuentaPorCobrar != null)
                {
                    ViewData["CuentaPorCobrarTexto"] = $"{cuentaPorCobrar.Codigo} - {cuentaPorCobrar.Nombre}";
                }
            }

            if (proveedor.CuentaPorPagarId.HasValue)
            {
                var cuentaPorPagar = await _context.CuentasContables.FindAsync(proveedor.CuentaPorPagarId);
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
            
            // Obtener la empresa actual para sus configuraciones
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            var empresa = await _context.Empresas.FindAsync(empresaId);
            
            // Configuración de formato decimal
            ViewBag.SeparadorDecimal = empresa?.SeparadorDecimal ?? ",";
            ViewBag.PrecisionDecimal = empresa?.PrecisionDecimal ?? 2;

            return View(proveedor);
        }

        // POST: Proveedores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cliente proveedor, Microsoft.AspNetCore.Http.IFormFile imagen, string Pais)
        {
            if (id != proveedor.Id)
            {
                return NotFound();
            }

            // Validar EmpresaId ANTES de cargar proveedorActual para asegurar el contexto correcto.
            var empresaIdActualServicio = await _empresaService.ObtenerEmpresaActualId();
            if (empresaIdActualServicio == 0)
            {
                ModelState.AddModelError("", "No se pudo determinar la empresa actual. No se puede editar el proveedor.");
                CargarViewBags(); // Asegúrate de que CargarViewBags esté disponible y funcione aquí
                return View(proveedor);
            }

            try
            {
                // Obtener el proveedor actual de la base de datos
                var proveedorActual = await _context.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id && c.EsProveedor && c.EmpresaId == empresaIdActualServicio);
                if (proveedorActual == null)
                {
                    // Podría ser que el proveedor no exista, o no pertenezca a la empresa actual.
                    return NotFound("Proveedor no encontrado o no pertenece a la empresa actual.");
                }

                // Asignar el EmpresaId de forma segura. 
                // El EmpresaId del proveedor no debería cambiar en una edición normal a través de este flujo.
                // Usamos el ID de la empresa del servicio para asegurar que el usuario solo edite proveedores de su empresa.
                proveedor.EmpresaId = empresaIdActualServicio;

                // Procesar la imagen si se proporcionó una nueva
                if (imagen != null && imagen.Length > 0)
                {
                    // Eliminar la imagen anterior si existe
                    if (!string.IsNullOrEmpty(proveedorActual.ImagenUrl))
                    {
                        EliminarImagen(proveedorActual.ImagenUrl);
                    }

                    proveedor.ImagenUrl = await GuardarImagen(imagen);
                }
                else
                {
                    // Mantener la imagen actual
                    proveedor.ImagenUrl = proveedorActual.ImagenUrl;
                }
                
                // Procesar el país seleccionado
                if (!string.IsNullOrEmpty(Pais))
                {
                    // Buscar el ID del país en la base de datos o crearlo si no existe
                    var paisEntity = await _context.Paises.FirstOrDefaultAsync(p => p.Nombre == Pais);
                    if (paisEntity == null)
                    {
                        // Obtener el código del país desde DataLists
                        var paisData = DataLists.LatinAmericanCountries.FirstOrDefault(c => c.Name == Pais);
                        if (paisData != null)
                        {
                            paisEntity = new Pais
                            {
                                Nombre = Pais,
                                Codigo = paisData.Code
                            };
                            _context.Paises.Add(paisEntity);
                            await _context.SaveChangesAsync();
                        }
                    }
                    
                    // Asignar el ID del país al proveedor
                    if (paisEntity != null)
                    {
                        proveedor.PaisId = paisEntity.Id;
                    }
                }
                else if (proveedorActual.PaisId.HasValue)
                {
                    // Mantener el país anterior si no se selecciona uno nuevo
                    proveedor.PaisId = proveedorActual.PaisId;
                }

                // Asegurar que se mantenga como proveedor
                proveedor.EsProveedor = true;
                
                // Mantener si era cliente también
                proveedor.EsCliente = proveedorActual.EsCliente;

                // Mantener la fecha de creación original y otras propiedades importantes
                proveedor.FechaCreacion = proveedorActual.FechaCreacion;
                proveedor.FechaModificacion = DateTime.UtcNow;

                // Actualizar el proveedor en la base de datos
                _context.Update(proveedor);
                await _context.SaveChangesAsync();
                
                // Redirigir a la lista de proveedores después de guardar
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log de errores para diagnóstico
                Console.WriteLine($"Error al actualizar proveedor: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Ha ocurrido un error al guardar los cambios. Por favor, inténtelo de nuevo.");
                
                // Cargar los datos necesarios para volver a mostrar el formulario
                CargarViewBags();
                
                // Obtener la empresa actual para sus configuraciones
                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                var empresa = await _context.Empresas.FindAsync(empresaId);
                
                // Configuración de formato decimal
                ViewBag.SeparadorDecimal = empresa?.SeparadorDecimal ?? ",";
                ViewBag.PrecisionDecimal = empresa?.PrecisionDecimal ?? 2;
                
                return View(proveedor);
            }
        }

        // GET: Proveedores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Clientes
                .Include(c => c.TipoIdentificacion)
                .Include(c => c.Municipio)
                .FirstOrDefaultAsync(m => m.Id == id && m.EsProveedor);

            if (proveedor == null)
            {
                return NotFound();
            }

            return View(proveedor);
        }

        // POST: Proveedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proveedor = await _context.Clientes.FindAsync(id);
            
            if (proveedor != null && proveedor.EsProveedor)
            {
                // Si es ambos, solo quitar la marca de proveedor
                if (proveedor.EsCliente)
                {
                    proveedor.EsProveedor = false;
                    _context.Update(proveedor);
                }
                else
                {
                    // Eliminar la imagen si existe
                    if (!string.IsNullOrEmpty(proveedor.ImagenUrl))
                    {
                        EliminarImagen(proveedor.ImagenUrl);
                    }
                    
                    _context.Clientes.Remove(proveedor);
                }
                
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }
        
        // GET: Proveedores/BuscarCuentasContables
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

        // GET: Proveedores/BuscarVendedores
        [HttpGet]
        public async Task<IActionResult> BuscarVendedores(string term)
        {
            try
            {
                Console.WriteLine($"BuscarVendedores llamado con término: '{term}'");
                
                if (string.IsNullOrEmpty(term))
                {
                    var todosVendedores = await _context.Vendedores
                        .Where(v => v.Activo)
                        .OrderBy(v => v.Nombre)
                        .Take(20)
                        .Select(v => new 
                        {
                            id = v.Id,
                            text = v.Nombre
                        })
                        .ToListAsync();
                    
                    Console.WriteLine($"Devolviendo {todosVendedores.Count} vendedores (sin término)");
                    return Json(new { results = todosVendedores });
                }

                var vendedores = await _context.Vendedores
                    .Where(v => v.Activo && v.Nombre.Contains(term, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(v => v.Nombre)
                    .Take(10)
                    .Select(v => new 
                    {
                        id = v.Id,
                        text = v.Nombre
                    })
                    .ToListAsync();

                Console.WriteLine($"Devolviendo {vendedores.Count} vendedores para el término '{term}'");
                return Json(new { results = vendedores });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en BuscarVendedores: {ex.Message}");
                // Incluso en caso de error, devolvemos un objeto válido pero vacío
                // para que el cliente pueda seguir funcionando
                return Json(new { results = new List<object>(), error = ex.Message });
            }
        }

        // POST: Proveedores/CrearVendedor
        [HttpPost]
        public async Task<IActionResult> CrearVendedor([FromBody] Dictionary<string, string> datos)
        {
            try
            {
                Console.WriteLine("CrearVendedor llamado con datos: " + 
                    (datos != null ? string.Join(", ", datos.Select(x => $"{x.Key}={x.Value}")) : "null"));
                
                if (datos == null || !datos.ContainsKey("nombre") || string.IsNullOrWhiteSpace(datos["nombre"]))
                {
                    Console.WriteLine("Error: Nombre faltante o vacío");
                    return BadRequest(new { success = false, message = "El nombre es obligatorio" });
                }

                var nombreVendedor = datos["nombre"].Trim();
                Console.WriteLine($"Nombre de vendedor a crear: '{nombreVendedor}'");

                // Verificar si ya existe un vendedor con ese nombre
                var vendedorExistente = await _context.Vendedores
                    .Where(v => v.Nombre.ToLower() == nombreVendedor.ToLower())
                    .FirstOrDefaultAsync();

                if (vendedorExistente != null)
                {
                    if (vendedorExistente.Activo)
                    {
                        // Si existe y está activo, retornar ese vendedor
                        Console.WriteLine($"Vendedor existente y activo encontrado, ID: {vendedorExistente.Id}");
                        return Json(new { 
                            success = true, 
                            message = "El vendedor ya existe", 
                            vendedor = new { 
                                id = vendedorExistente.Id, 
                                text = vendedorExistente.Nombre 
                            } 
                        });
                    }
                    else
                    {
                        // Si existe pero está inactivo, reactivarlo
                        Console.WriteLine($"Reactivando vendedor existente, ID: {vendedorExistente.Id}");
                        vendedorExistente.Activo = true;
                        _context.Update(vendedorExistente);
                        await _context.SaveChangesAsync();
                        
                        return Json(new { 
                            success = true, 
                            message = "Vendedor reactivado", 
                            vendedor = new { 
                                id = vendedorExistente.Id, 
                                text = vendedorExistente.Nombre 
                            } 
                        });
                    }
                }

                // Crear nuevo vendedor
                var nuevoVendedor = new Vendedor
                {
                    Nombre = nombreVendedor,
                    Activo = true,
                    FechaCreacion = DateTime.UtcNow,
                    PorcentajeComision = 0 // Valor por defecto, se puede editar después
                };

                _context.Vendedores.Add(nuevoVendedor);
                await _context.SaveChangesAsync();

                Console.WriteLine($"Nuevo vendedor creado con ID: {nuevoVendedor.Id}");
                return Json(new { 
                    success = true, 
                    message = "Vendedor creado exitosamente", 
                    vendedor = new { 
                        id = nuevoVendedor.Id, 
                        text = nuevoVendedor.Nombre 
                    } 
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en CrearVendedor: {ex.Message}");
                return StatusCode(500, new { success = false, message = $"Error interno: {ex.Message}" });
            }
        }

        // PUT o POST: Proveedores/EditarVendedor
        [HttpPut]
        [HttpPost]
        public async Task<IActionResult> EditarVendedor([FromBody] Dictionary<string, string> datos)
        {
            if (datos == null || !datos.ContainsKey("id") || !datos.ContainsKey("nombre") || 
                string.IsNullOrWhiteSpace(datos["id"]) || string.IsNullOrWhiteSpace(datos["nombre"]))
            {
                return BadRequest(new { success = false, message = "ID y nombre son obligatorios" });
            }

            if (!int.TryParse(datos["id"], out int vendedorId))
            {
                return BadRequest(new { success = false, message = "ID inválido" });
            }

            var nombreVendedor = datos["nombre"].Trim();

            // Verificar si ya existe un vendedor con ese nombre (diferente del que estamos editando)
            var existeNombre = await _context.Vendedores
                .Where(v => v.Nombre.ToLower() == nombreVendedor.ToLower() && v.Id != vendedorId)
                .AnyAsync();

            if (existeNombre)
            {
                return BadRequest(new { success = false, message = "Ya existe un vendedor con ese nombre" });
            }

            // Buscar el vendedor a editar
            var vendedor = await _context.Vendedores.FindAsync(vendedorId);
            if (vendedor == null)
            {
                return NotFound(new { success = false, message = "Vendedor no encontrado" });
            }

            // Actualizar el nombre
            vendedor.Nombre = nombreVendedor;
            _context.Update(vendedor);
            await _context.SaveChangesAsync();

            return Json(new { 
                success = true, 
                message = "Vendedor actualizado exitosamente", 
                vendedor = new { 
                    id = vendedor.Id, 
                    text = vendedor.Nombre 
                } 
            });
        }

        // DELETE o POST: Proveedores/EliminarVendedor
        [HttpDelete]
        [HttpPost]
        public async Task<IActionResult> EliminarVendedor([FromBody] Dictionary<string, string> datos)
        {
            if (datos == null || !datos.ContainsKey("id") || string.IsNullOrWhiteSpace(datos["id"]))
            {
                return BadRequest(new { success = false, message = "ID es obligatorio" });
            }

            if (!int.TryParse(datos["id"], out int vendedorId))
            {
                return BadRequest(new { success = false, message = "ID inválido" });
            }

            // Buscar el vendedor a eliminar
            var vendedor = await _context.Vendedores.FindAsync(vendedorId);
            if (vendedor == null)
            {
                return NotFound(new { success = false, message = "Vendedor no encontrado" });
            }

            // Verificar si el vendedor está siendo usado en clientes o proveedores
            var estaEnUso = await _context.Clientes.AnyAsync(c => c.VendedorId == vendedorId);

            if (estaEnUso)
            {
                // Si está en uso, solo marcarlo como inactivo
                vendedor.Activo = false;
                _context.Update(vendedor);
            }
            else
            {
                // Si no está en uso, eliminar físicamente
                _context.Vendedores.Remove(vendedor);
            }

            await _context.SaveChangesAsync();

            return Json(new { 
                success = true, 
                message = estaEnUso ? "Vendedor desactivado" : "Vendedor eliminado" 
            });
        }

        private bool ProveedorExists(int id)
        {
            return _context.Clientes.Any(e => e.Id == id && e.EsProveedor);
        }

        private void CargarViewBags()
        {
            ViewBag.TipoIdentificacionId = new SelectList(_context.TiposIdentificacion, "Id", "Nombre");
            ViewBag.MunicipioId = new SelectList(_context.Municipios, "Id", "Nombre");
            
            // Usar el mismo enfoque que el módulo de empresas (DataLists.LatinAmericanCountries)
            ViewBag.Countries = DataLists.LatinAmericanCountries;
            
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

            // Crear directorio para imágenes de proveedores si no existe
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "proveedores");
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
            return $"/uploads/proveedores/{uniqueFileName}";
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