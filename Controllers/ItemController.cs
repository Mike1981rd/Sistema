using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Models;
using SistemaContable.Models.ViewModels;
using SistemaContable.Services;
using SistemaContable.Helpers; // A�adido para PaginatedList
using Microsoft.AspNetCore.Hosting;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Npgsql.Internal;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using SistemaContable.ViewModels;

namespace SistemaContable.Controllers
{
    public class ItemController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IUserService _userService;

        public ItemController(
            ApplicationDbContext context,
            IWebHostEnvironment hostEnvironment,
            IUserService userService)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _userService = userService;
        }

        // GET: Item
        [HttpGet]
        [Route("/inventario/servicios")]
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? categoriaId, int? proveedorId, int? almacenId, int? marcaId, int? pageNumber)
        {
            var empresaId = _userService.GetEmpresaId();

            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["CodeSortParm"] = sortOrder == "Code" ? "code_desc" : "Code";
            ViewData["CategorySortParm"] = sortOrder == "Category" ? "category_desc" : "Category";
            ViewData["StockSortParm"] = sortOrder == "Stock" ? "stock_desc" : "Stock";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentCategoriaId"] = categoriaId;
            ViewData["CurrentProveedorId"] = proveedorId;
            ViewData["CurrentAlmacenId"] = almacenId;
            ViewData["CurrentMarcaId"] = marcaId;

            // Cargar listas para filtros
            ViewData["Categorias"] = new SelectList(_context.Categorias
                .Where(c => c.EmpresaId == empresaId && c.Estado)
                .OrderBy(c => c.Nombre), "Id", "Nombre");

            // Usar Cliente como Proveedor - filtrando por EsProveedor
            ViewData["Proveedores"] = new SelectList(_context.Clientes
                .Where(p => p.EmpresaId == empresaId && p.EsProveedor)
                .OrderBy(p => p.NombreRazonSocial), "Id", "NombreRazonSocial");

            ViewData["Almacenes"] = new SelectList(_context.Almacenes
                .Where(a => a.EmpresaId == empresaId && a.Estado)
                .OrderBy(a => a.Nombre), "Id", "Nombre");

            ViewData["Marcas"] = new SelectList(_context.Marcas
                .Where(m => m.EmpresaId == empresaId && m.Estado)
                .OrderBy(m => m.Nombre), "Id", "Nombre");

            // Consulta base con includes
            var query = _context.Items
                .Where(i => i.EmpresaId == empresaId)
                .Include(i => i.Categoria)
                .Include(i => i.Marca)
                .Include(i => i.UnidadMedidaInventario)
                .Include(i => i.Impuesto)
                .AsQueryable();

            // Aplicar filtros
            if (!String.IsNullOrEmpty(searchString))
            {
                query = query.Where(i =>
                    i.Nombre.Contains(searchString) ||
                    i.Codigo.Contains(searchString) ||
                    i.CodigoBarras.Contains(searchString) ||
                    i.Descripcion.Contains(searchString));
            }

            if (categoriaId.HasValue)
            {
                query = query.Where(i => i.CategoriaId == categoriaId.Value);
            }

            if (proveedorId.HasValue)
            {
                query = query.Where(i => i.Proveedores.Any(p => p.ProveedorId == proveedorId.Value));
            }

            if (almacenId.HasValue)
            {
                query = query.Where(i => i.Almacenes.Any(a => a.AlmacenId == almacenId.Value));
            }

            if (marcaId.HasValue)
            {
                query = query.Where(i => i.MarcaId == marcaId.Value);
            }

            // Aplicar ordenamiento
            switch (sortOrder)
            {
                case "name_desc":
                    query = query.OrderByDescending(i => i.Nombre);
                    break;
                case "Code":
                    query = query.OrderBy(i => i.Codigo);
                    break;
                case "code_desc":
                    query = query.OrderByDescending(i => i.Codigo);
                    break;
                case "Category":
                    query = query.OrderBy(i => i.Categoria.Nombre);
                    break;
                case "category_desc":
                    query = query.OrderByDescending(i => i.Categoria.Nombre);
                    break;
                case "Stock":
                    query = query.OrderBy(i => i.StockActual);
                    break;
                case "stock_desc":
                    query = query.OrderByDescending(i => i.StockActual);
                    break;
                default:
                    query = query.OrderBy(i => i.Nombre);
                    break;
            }

            // Obtener configuraci�n de empresa para separador decimal
            var empresa = await _context.Empresas.FindAsync(empresaId);
            ViewBag.SeparadorDecimal = empresa?.SeparadorDecimal ?? ",";
            ViewBag.PrecisionDecimal = empresa?.PrecisionDecimal ?? 2;

            int pageSize = 10;
            return View(await PaginatedList<Item>.CreateAsync(query.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Item/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresaId = _userService.GetEmpresaId();

            var item = await _context.Items
                .Include(i => i.Categoria)
                .Include(i => i.Marca)
                .Include(i => i.UnidadMedidaInventario)
                .Include(i => i.Impuesto)
                .Include(i => i.CuentaVentas)
                .Include(i => i.CuentaComprasInventarios)
                .Include(i => i.CuentaCostoVentasGastos)
                .Include(i => i.CuentaDescuentos)
                .Include(i => i.CuentaDevoluciones)
                .Include(i => i.CuentaAjustes)
                .Include(i => i.Proveedores)
                    .ThenInclude(p => p.Proveedor)
                .Include(i => i.Proveedores)
                    .ThenInclude(p => p.UnidadMedidaCompra)
                .Include(i => i.Almacenes)
                    .ThenInclude(a => a.Almacen)
                .Include(i => i.Contenedores)
                    .ThenInclude(c => c.UnidadMedida)
                .Include(i => i.Taras)
                    .ThenInclude(t => t.ItemContenedor)
                .Include(i => i.ProductosVenta)
                    .ThenInclude(p => p.ItemContenedor)
                .FirstOrDefaultAsync(i => i.Id == id && i.EmpresaId == empresaId);

            if (item == null)
            {
                return NotFound();
            }

            // Obtener configuraci�n de empresa para separador decimal
            var empresa = await _context.Empresas.FindAsync(empresaId);
            ViewBag.SeparadorDecimal = empresa?.SeparadorDecimal ?? ",";
            ViewBag.PrecisionDecimal = empresa?.PrecisionDecimal ?? 2;

            return View(item);
        }

        // GET: Item/Create
        public IActionResult Create()
        {
            // Ensure empresa is set in session
            HttpContext.Session.SetInt32("EmpresaActualId", 4);
            var viewModel = PrepararViewModel(null);
            return View(viewModel);
        }

        // POST: Item/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ItemViewModel viewModel)
        {
            var empresaId = _userService.GetEmpresaId();

            // Temporalmente, ignorar completamente ProductoVenta
            viewModel.ProductoVenta = null;
            
            // Remover validación de ProductoVenta y proveedores problemáticos
            var keysToRemove = ModelState.Keys
                .Where(k => k.StartsWith("ProductoVenta") || 
                           k.Contains("PrecioCompra") ||
                           k.Contains("PrecioVenta"))
                .ToList();
                
            // Si hay proveedores vacíos (sin ProveedorId), remover sus validaciones
            if (viewModel.Proveedores != null)
            {
                for (int i = 0; i < viewModel.Proveedores.Count; i++)
                {
                    if (viewModel.Proveedores[i].ProveedorId <= 0)
                    {
                        keysToRemove.AddRange(ModelState.Keys.Where(k => k.StartsWith($"Proveedores[{i}]")).ToList());
                    }
                }
            }
                
            foreach (var key in keysToRemove)
            {
                ModelState.Remove(key);
            }
            
            // Remover errores de campos vacíos
            var emptyFieldErrors = ModelState.Where(x => x.Value.Errors.Any(e => e.ErrorMessage == "The value '' is invalid.")).ToList();
            foreach (var error in emptyFieldErrors)
            {
                ModelState.Remove(error.Key);
            }

            // Si el modelo no es v�lido, preparar de nuevo y devolver la vista
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Por favor verifique los campos requeridos.";
                viewModel = PrepararViewModel(viewModel);
                return View(viewModel);
            }

            // Crear nuevo item con los datos b�sicos
            var item = new Item
            {
                EmpresaId = empresaId,
                Codigo = viewModel.Codigo,
                CodigoBarras = viewModel.CodigoBarras,
                Nombre = viewModel.Nombre,
                Descripcion = viewModel.Descripcion,
                Estado = viewModel.Estado,
                CategoriaId = viewModel.CategoriaId,
                MarcaId = viewModel.MarcaId,
                UnidadMedidaInventarioId = viewModel.UnidadMedidaInventarioId,
                NivelMinimo = viewModel.NivelMinimo,
                StockActual = viewModel.StockActual,
                Rendimiento = viewModel.Rendimiento,
                ImpuestoId = viewModel.ImpuestoId,
                CuentaVentasId = viewModel.CuentaVentasId,
                CuentaComprasInventariosId = viewModel.CuentaComprasInventariosId,
                CuentaCostoVentasGastosId = viewModel.CuentaCostoVentasGastosId,
                CuentaDescuentosId = viewModel.CuentaDescuentosId,
                CuentaDevolucionesId = viewModel.CuentaDevolucionesId,
                CuentaAjustesId = viewModel.CuentaAjustesId,
                CuentaCostoMateriaPrimaId = viewModel.CuentaCostoMateriaPrimaId,
                FechaCreacion = DateTime.Now,
                UsuarioCreacionId = _userService.GetUserId()
            };

            // Procesar imagen si se ha subido
            if (viewModel.ItemImage != null)
            {
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images", "items");

                // Crear directorio si no existe
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Generar nombre de archivo �nico basado en GUID
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + viewModel.ItemImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Guardar el archivo
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await viewModel.ItemImage.CopyToAsync(fileStream);
                }

                // Guardar la ruta relativa en la entidad
                item.ImagenUrl = "/images/items/" + uniqueFileName;
            }

            // A�adir item a contexto
            try
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al guardar el item: {ex.Message}";
                viewModel = PrepararViewModel(viewModel);
                return View(viewModel);
            }

            // Procesar colecci�n de proveedores
            if (viewModel.Proveedores != null && viewModel.Proveedores.Any())
            {
                foreach (var proveedorVM in viewModel.Proveedores)
                {
                    if (proveedorVM.ProveedorId > 0)
                    {
                        var itemProveedor = new ItemProveedor
                        {
                            EmpresaId = empresaId,
                            ItemId = item.Id,
                            ProveedorId = proveedorVM.ProveedorId,
                            NombreCompra = proveedorVM.NombreCompra,
                            CodigoProveedor = proveedorVM.CodigoProveedor,
                            PrecioCompra = proveedorVM.PrecioCompra,
                            UnidadMedidaCompraId = proveedorVM.UnidadMedidaCompraId,
                            FactorConversion = proveedorVM.FactorConversion,
                            EsPrincipal = proveedorVM.EsPrincipal,
                            UltimaActualizacionPrecio = DateTime.Now
                        };

                        _context.ItemProveedores.Add(itemProveedor);
                    }
                }
            }

            // Procesar colecci�n de contenedores
            if (viewModel.Contenedores != null && viewModel.Contenedores.Any())
            {
                int orden = 1;
                foreach (var contenedorVM in viewModel.Contenedores)
                {
                    var itemContenedor = new ItemContenedor
                    {
                        EmpresaId = empresaId,
                        ItemId = item.Id,
                        UnidadMedidaId = contenedorVM.UnidadMedidaId,
                        Nombre = contenedorVM.Nombre,
                        Etiqueta = contenedorVM.Etiqueta,
                        ContenedorSuperiorId = contenedorVM.ContenedorSuperiorId,
                        Factor = contenedorVM.Factor,
                        Costo = contenedorVM.Costo,
                        EsPrincipal = contenedorVM.EsPrincipal,
                        EsContenedorCompra = contenedorVM.EsContenedorCompra,
                        Orden = orden++
                    };

                    _context.ItemContenedores.Add(itemContenedor);
                }

                await _context.SaveChangesAsync();

                // Procesar taras (despu�s de guardar contenedores)
                if (viewModel.Taras != null && viewModel.Taras.Any())
                {
                    foreach (var taraVM in viewModel.Taras)
                    {
                        if (taraVM.ItemContenedorId > 0 && taraVM.ValorTara > 0)
                        {
                            var itemTara = new ItemTara
                            {
                                EmpresaId = empresaId,
                                ItemId = item.Id,
                                ItemContenedorId = taraVM.ItemContenedorId,
                                ValorTara = taraVM.ValorTara,
                                Observacion = taraVM.Observacion
                            };

                            _context.ItemTaras.Add(itemTara);
                        }
                    }
                }
            }

            // Procesar colecci�n de almacenes
            if (viewModel.Almacenes != null && viewModel.Almacenes.Any())
            {
                foreach (var almacenVM in viewModel.Almacenes)
                {
                    if (almacenVM.AlmacenId > 0)
                    {
                        var itemAlmacen = new ItemAlmacen
                        {
                            EmpresaId = empresaId,
                            ItemId = item.Id,
                            AlmacenId = almacenVM.AlmacenId,
                            Stock = almacenVM.Stock,
                            NivelMinimo = almacenVM.NivelMinimo,
                            Ubicacion = almacenVM.Ubicacion
                        };

                        _context.ItemAlmacenes.Add(itemAlmacen);
                    }
                }
            }

            // Procesar producto de venta - SOLO si tiene datos válidos
            // Por ahora, esta funcionalidad está pendiente de implementación
            // No procesamos ProductoVenta hasta que se implemente la funcionalidad completa
            /*
            if (viewModel.ProductoVenta != null && viewModel.ProductoVenta.ItemContenedorId > 0 && viewModel.ProductoVenta.PrecioVenta > 0)
            {
                var productoVenta = new ProductoVenta
                {
                    EmpresaId = empresaId,
                    ItemId = item.Id,
                    ItemContenedorId = viewModel.ProductoVenta.ItemContenedorId,
                    Nombre = viewModel.ProductoVenta.Nombre,
                    Cantidad = viewModel.ProductoVenta.Cantidad,
                    PrecioVenta = viewModel.ProductoVenta.PrecioVenta,
                    Costo = viewModel.ProductoVenta.Costo,
                    CostoTotal = viewModel.ProductoVenta.Costo * viewModel.ProductoVenta.Cantidad,
                    ImpuestoId = viewModel.ProductoVenta.ImpuestoId,
                    DisponibleParaVenta = viewModel.ProductoVenta.DisponibleParaVenta,
                    RequierePreparacion = viewModel.ProductoVenta.RequierePreparacion,
                    TiempoPreparacion = viewModel.ProductoVenta.TiempoPreparacion
                };

                _context.ProductosVenta.Add(productoVenta);
            }
            */

            try
            {
                await _context.SaveChangesAsync();
                TempData["Success"] = "Item creado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al guardar los datos adicionales: {ex.Message}";
                viewModel = PrepararViewModel(viewModel);
                return View(viewModel);
            }
        }


        // GET: Item/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var empresaId = _userService.GetEmpresaId();

            var item = await _context.Items
                .Include(i => i.Categoria)
                .Include(i => i.Marca)
                .Include(i => i.Proveedores)
                    .ThenInclude(p => p.Proveedor)
                .Include(i => i.Proveedores)
                    .ThenInclude(p => p.UnidadMedidaCompra)
                .Include(i => i.Almacenes)
                    .ThenInclude(a => a.Almacen)
                .Include(i => i.Contenedores)
                    .ThenInclude(c => c.UnidadMedida)
                .Include(i => i.Taras)
                    .ThenInclude(t => t.ItemContenedor)
                .Include(i => i.ProductosVenta)
                    .ThenInclude(p => p.ItemContenedor)
                .FirstOrDefaultAsync(i => i.Id == id && i.EmpresaId == empresaId);

            if (item == null)
            {
                return NotFound();
            }

            // Crear el viewModel inicial con las propiedades básicas
            var viewModel = new ItemViewModel
            {
                Id = item.Id,
                Codigo = item.Codigo,
                CodigoBarras = item.CodigoBarras,
                Nombre = item.Nombre,
                Descripcion = item.Descripcion,
                Estado = item.Estado,
                CategoriaId = item.CategoriaId,
                MarcaId = item.MarcaId,
                UnidadMedidaInventarioId = item.UnidadMedidaInventarioId,
                NivelMinimo = item.NivelMinimo,
                StockActual = item.StockActual,
                Rendimiento = item.Rendimiento,
                ImpuestoId = item.ImpuestoId,
                CuentaVentasId = item.CuentaVentasId,
                CuentaComprasInventariosId = item.CuentaComprasInventariosId,
                CuentaCostoVentasGastosId = item.CuentaCostoVentasGastosId,
                CuentaDescuentosId = item.CuentaDescuentosId,
                CuentaDevolucionesId = item.CuentaDevolucionesId,
                CuentaAjustesId = item.CuentaAjustesId,
                CuentaCostoMateriaPrimaId = item.CuentaCostoMateriaPrimaId,
                ImagenUrl = item.ImagenUrl
            };
            
            // Ahora preparar el viewModel con todos los SelectLists manteniendo los valores seleccionados
            viewModel = PrepararViewModel(viewModel);

            // Mapear proveedores
            viewModel.Proveedores = item.Proveedores.Select(p => new ItemProveedorViewModel
            {
                Id = p.Id,
                ItemId = p.ItemId,
                ProveedorId = p.ProveedorId,
                ProveedorNombre = p.Proveedor?.NombreRazonSocial,
                NombreCompra = p.NombreCompra,
                CodigoProveedor = p.CodigoProveedor,
                PrecioCompra = p.PrecioCompra,
                UnidadMedidaCompraId = p.UnidadMedidaCompraId,
                UnidadMedidaNombre = p.UnidadMedidaCompra?.Nombre,
                FactorConversion = p.FactorConversion,
                EsPrincipal = p.EsPrincipal,
                UltimaActualizacionPrecio = p.UltimaActualizacionPrecio
            }).ToList();

            // Mapear contenedores
            viewModel.Contenedores = item.Contenedores
                .OrderBy(c => c.Orden)
                .Select(c => new ItemContenedorViewModel
                {
                    Id = c.Id,
                    ItemId = c.ItemId,
                    Nombre = c.Nombre,
                    Etiqueta = c.Etiqueta,
                    ContenedorSuperiorId = c.ContenedorSuperiorId,
                    ContenedorSuperiorNombre = c.ContenedorSuperior?.Nombre,
                    Factor = c.Factor,
                    Costo = c.Costo,
                    EsPrincipal = c.EsPrincipal,
                    EsContenedorCompra = c.EsContenedorCompra,
                    Orden = c.Orden,
                    UnidadMedidaId = c.UnidadMedidaId,
                    UnidadMedidaNombre = c.UnidadMedida?.Nombre
                }).ToList();

            // Mapear taras
            viewModel.Taras = item.Taras.Select(t => new ItemTaraViewModel
            {
                Id = t.Id,
                ItemId = t.ItemId,
                ItemContenedorId = t.ItemContenedorId,
                ContenedorNombre = t.ItemContenedor?.Nombre,
                ValorTara = t.ValorTara,
                Observacion = t.Observacion
            }).ToList();

            // Mapear almacenes
            viewModel.Almacenes = item.Almacenes.Select(a => new ItemAlmacenViewModel
            {
                Id = a.Id,
                ItemId = a.ItemId,
                AlmacenId = a.AlmacenId,
                AlmacenNombre = a.Almacen?.Nombre,
                Stock = a.Stock,
                NivelMinimo = a.NivelMinimo,
                Ubicacion = a.Ubicacion
            }).ToList();

            // Mapear producto de venta
            var productoVenta = item.ProductosVenta.FirstOrDefault();
            if (productoVenta != null)
            {
                viewModel.ProductoVenta = new ProductoVentaViewModel
                {
                    Id = productoVenta.Id,
                    ItemId = productoVenta.ItemId,
                    ItemContenedorId = productoVenta.ItemContenedorId,
                    ContenedorNombre = productoVenta.ItemContenedor?.Nombre,
                    Nombre = productoVenta.Nombre,
                    Cantidad = productoVenta.Cantidad,
                    PrecioVenta = productoVenta.PrecioVenta,
                    Costo = productoVenta.Costo,
                    ImpuestoId = productoVenta.ImpuestoId,
                    DisponibleParaVenta = productoVenta.DisponibleParaVenta,
                    RequierePreparacion = productoVenta.RequierePreparacion,
                    TiempoPreparacion = productoVenta.TiempoPreparacion
                };
            }
            else
            {
                viewModel.ProductoVenta = new ProductoVentaViewModel
                {
                    ItemId = item.Id,
                    Nombre = item.Nombre,
                    Cantidad = 1,
                    DisponibleParaVenta = true
                };
            }

            // Obtener configuraci�n de empresa para separador decimal
            var empresa = await _context.Empresas.FindAsync(empresaId);
            viewModel.SeparadorDecimal = empresa?.SeparadorDecimal ?? ",";

            return View(viewModel);
        }

        // POST: Item/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ItemViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            var empresaId = _userService.GetEmpresaId();

            if (!ModelState.IsValid)
            {
                viewModel = PrepararViewModel(viewModel);
                return View(viewModel);
            }

            // Buscar item existente
            var item = await _context.Items
                .Include(i => i.Proveedores)
                .Include(i => i.Almacenes)
                .Include(i => i.Contenedores)
                .Include(i => i.Taras)
                .Include(i => i.ProductosVenta)
                .FirstOrDefaultAsync(i => i.Id == id && i.EmpresaId == empresaId);

            if (item == null)
            {
                return NotFound();
            }

            // Actualizar propiedades b�sicas
            item.Codigo = viewModel.Codigo;
            item.CodigoBarras = viewModel.CodigoBarras;
            item.Nombre = viewModel.Nombre;
            item.Descripcion = viewModel.Descripcion;
            item.Estado = viewModel.Estado;
            item.CategoriaId = viewModel.CategoriaId;
            item.MarcaId = viewModel.MarcaId;
            item.UnidadMedidaInventarioId = viewModel.UnidadMedidaInventarioId;
            item.NivelMinimo = viewModel.NivelMinimo;
            item.StockActual = viewModel.StockActual;
            item.Rendimiento = viewModel.Rendimiento;
            item.ImpuestoId = viewModel.ImpuestoId;
            item.CuentaVentasId = viewModel.CuentaVentasId;
            item.CuentaComprasInventariosId = viewModel.CuentaComprasInventariosId;
            item.CuentaCostoVentasGastosId = viewModel.CuentaCostoVentasGastosId;
            item.CuentaDescuentosId = viewModel.CuentaDescuentosId;
            item.CuentaDevolucionesId = viewModel.CuentaDevolucionesId;
            item.CuentaAjustesId = viewModel.CuentaAjustesId;
            item.CuentaCostoMateriaPrimaId = viewModel.CuentaCostoMateriaPrimaId;
            item.FechaModificacion = DateTime.Now;
            item.UsuarioModificacionId = _userService.GetUserId();

            // Procesar imagen si se ha subido
            if (viewModel.ItemImage != null)
            {
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images", "items");

                // Crear directorio si no existe
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Borrar imagen anterior si existe
                if (!string.IsNullOrEmpty(item.ImagenUrl))
                {
                    string oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, item.ImagenUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Generar nombre de archivo �nico basado en GUID
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + viewModel.ItemImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Guardar el archivo
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await viewModel.ItemImage.CopyToAsync(fileStream);
                }

                // Guardar la ruta relativa en la entidad
                item.ImagenUrl = "/images/items/" + uniqueFileName;
            }

            // Actualizar proveedores: eliminar los existentes y a�adir los nuevos
            _context.ItemProveedores.RemoveRange(item.Proveedores);

            if (viewModel.Proveedores != null && viewModel.Proveedores.Any())
            {
                foreach (var proveedorVM in viewModel.Proveedores)
                {
                    if (proveedorVM.ProveedorId > 0)
                    {
                        var itemProveedor = new ItemProveedor
                        {
                            EmpresaId = empresaId,
                            ItemId = item.Id,
                            ProveedorId = proveedorVM.ProveedorId,
                            NombreCompra = proveedorVM.NombreCompra,
                            CodigoProveedor = proveedorVM.CodigoProveedor,
                            PrecioCompra = proveedorVM.PrecioCompra,
                            UnidadMedidaCompraId = proveedorVM.UnidadMedidaCompraId,
                            FactorConversion = proveedorVM.FactorConversion,
                            EsPrincipal = proveedorVM.EsPrincipal,
                            UltimaActualizacionPrecio = DateTime.Now
                        };

                        _context.ItemProveedores.Add(itemProveedor);
                    }
                }
            }

            // Actualizar contenedores
            _context.ItemContenedores.RemoveRange(item.Contenedores);

            if (viewModel.Contenedores != null && viewModel.Contenedores.Any())
            {
                int orden = 1;
                foreach (var contenedorVM in viewModel.Contenedores)
                {
                    var itemContenedor = new ItemContenedor
                    {
                        EmpresaId = empresaId,
                        ItemId = item.Id,
                        UnidadMedidaId = contenedorVM.UnidadMedidaId,
                        Nombre = contenedorVM.Nombre,
                        Etiqueta = contenedorVM.Etiqueta,
                        ContenedorSuperiorId = contenedorVM.ContenedorSuperiorId,
                        Factor = contenedorVM.Factor,
                        Costo = contenedorVM.Costo,
                        EsPrincipal = contenedorVM.EsPrincipal,
                        EsContenedorCompra = contenedorVM.EsContenedorCompra,
                        Orden = orden++
                    };

                    _context.ItemContenedores.Add(itemContenedor);
                }

                await _context.SaveChangesAsync();
            }

            // Actualizar taras
            _context.ItemTaras.RemoveRange(item.Taras);

            if (viewModel.Taras != null && viewModel.Taras.Any())
            {
                foreach (var taraVM in viewModel.Taras)
                {
                    if (taraVM.ItemContenedorId > 0 && taraVM.ValorTara > 0)
                    {
                        var itemTara = new ItemTara
                        {
                            EmpresaId = empresaId,
                            ItemId = item.Id,
                            ItemContenedorId = taraVM.ItemContenedorId,
                            ValorTara = taraVM.ValorTara,
                            Observacion = taraVM.Observacion
                        };

                        _context.ItemTaras.Add(itemTara);
                    }
                }
            }

            // Actualizar almacenes
            _context.ItemAlmacenes.RemoveRange(item.Almacenes);

            if (viewModel.Almacenes != null && viewModel.Almacenes.Any())
            {
                foreach (var almacenVM in viewModel.Almacenes)
                {
                    if (almacenVM.AlmacenId > 0)
                    {
                        var itemAlmacen = new ItemAlmacen
                        {
                            EmpresaId = empresaId,
                            ItemId = item.Id,
                            AlmacenId = almacenVM.AlmacenId,
                            Stock = almacenVM.Stock,
                            NivelMinimo = almacenVM.NivelMinimo,
                            Ubicacion = almacenVM.Ubicacion
                        };

                        _context.ItemAlmacenes.Add(itemAlmacen);
                    }
                }
            }

            // Actualizar producto de venta
            _context.ProductosVenta.RemoveRange(item.ProductosVenta);

            if (viewModel.ProductoVenta != null && viewModel.ProductoVenta.ItemContenedorId > 0)
            {
                var productoVenta = new ProductoVenta
                {
                    EmpresaId = empresaId,
                    ItemId = item.Id,
                    ItemContenedorId = viewModel.ProductoVenta.ItemContenedorId,
                    Nombre = viewModel.ProductoVenta.Nombre,
                    Cantidad = viewModel.ProductoVenta.Cantidad,
                    PrecioVenta = viewModel.ProductoVenta.PrecioVenta,
                    Costo = viewModel.ProductoVenta.Costo,
                    CostoTotal = viewModel.ProductoVenta.Costo * viewModel.ProductoVenta.Cantidad,
                    ImpuestoId = viewModel.ProductoVenta.ImpuestoId,
                    DisponibleParaVenta = viewModel.ProductoVenta.DisponibleParaVenta,
                    RequierePreparacion = viewModel.ProductoVenta.RequierePreparacion,
                    TiempoPreparacion = viewModel.ProductoVenta.TiempoPreparacion
                };

                _context.ProductosVenta.Add(productoVenta);
            }

            // Guardar todos los cambios
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(viewModel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            TempData["SuccessMessage"] = "Item actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Item/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empresaId = _userService.GetEmpresaId();
            var item = await _context.Items
                .FirstOrDefaultAsync(i => i.Id == id && i.EmpresaId == empresaId);

            if (item == null)
            {
                return NotFound();
            }

            // Borrar imagen si existe
            if (!string.IsNullOrEmpty(item.ImagenUrl))
            {
                string imagePath = Path.Combine(_hostEnvironment.WebRootPath, item.ImagenUrl.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Item eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Item/ToggleEstado/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleEstado(int id)
        {
            var empresaId = _userService.GetEmpresaId();
            var item = await _context.Items
                .FirstOrDefaultAsync(i => i.Id == id && i.EmpresaId == empresaId);

            if (item == null)
            {
                return NotFound();
            }

            // Cambiar estado
            item.Estado = !item.Estado;
            item.FechaModificacion = DateTime.Now;
            item.UsuarioModificacionId = _userService.GetUserId();

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Item {(item.Estado ? "activado" : "desactivado")} correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Item/GenerarCodigoBarras
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GenerarCodigoBarras()
        {
            try
            {
                // Generar un código de barras único basado en timestamp + número aleatorio
                string timestamp = DateTime.Now.ToString("yyMMddHHmmss");
                Random random = new Random();
                string randomDigits = random.Next(100, 999).ToString();
                
                string codigoBarras = $"{timestamp}{randomDigits}";
                
                return Json(new { success = true, codigoBarras = codigoBarras });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: Item/GenerarCodigoBarrasGet        [HttpGet]        [ActionName("GenerarCodigoBarrasGet")]        public IActionResult GenerarCodigoBarrasGet()        {            try            {                // Generar un código de barras único basado en timestamp + número aleatorio                string timestamp = DateTime.Now.ToString("yyMMddHHmmss");                Random random = new Random();                string randomDigits = random.Next(100, 999).ToString();                                string codigoBarras = $"{timestamp}{randomDigits}";                                return Json(new { success = true, codigo = codigoBarras, codigoBarras = codigoBarras });            }            catch (Exception ex)            {                return Json(new { success = false, message = ex.Message });            }        }        // POST: Item/ExportarCodigoBarras
        [HttpPost]
        public IActionResult ExportarCodigoBarras(string codigo, string nombre)
        {
            if (string.IsNullOrEmpty(codigo))
            {
                return Json(new { success = false, message = "El código de barras es requerido." });
            }

            // Generar PDF con código de barras
            string fileName = $"CodigoBarras_{nombre}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            string filePath = Path.Combine(_hostEnvironment.WebRootPath, "temp", fileName);

            // Crear directorio si no existe
            string directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Crear documento PDF
            using (var document = new Document(PageSize.A4, 10, 10, 10, 10))
            {
                var writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                document.Open();

                // Añadir título
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16, BaseColor.BLACK);
                var title = new Paragraph($"Código de Barras: {nombre}", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);
                document.Add(Chunk.NEWLINE);

                // Crear código de barras
                var barcode = new iTextSharp.text.pdf.Barcode128();
                barcode.Code = codigo;
                barcode.CodeType = iTextSharp.text.pdf.Barcode128.CODE128;
                barcode.TextAlignment = Element.ALIGN_CENTER;
                barcode.Size = 20;
                barcode.Font = null;

                // Añadir el código de barras al documento
                var image = barcode.CreateImageWithBarcode(writer.DirectContent, BaseColor.BLACK, BaseColor.WHITE);
                image.Alignment = Element.ALIGN_CENTER;
                image.ScalePercent(150);
                document.Add(image);

                // Añadir el código como texto
                var codeText = new Paragraph(codigo);
                codeText.Alignment = Element.ALIGN_CENTER;
                document.Add(codeText);

                document.Close();
            }

            // Devolver URL del archivo
            string fileUrl = $"/temp/{fileName}";
            return Json(new { success = true, fileUrl });
        }

        // GET: Item/ObtenerDatosCategoria
        [HttpGet]
        public async Task<IActionResult> ObtenerDatosCategoria(int id)
        {
            var empresaId = _userService.GetEmpresaId();
            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(c => c.Id == id && c.EmpresaId == empresaId);

            if (categoria == null)
            {
                return NotFound();
            }

            return Json(new
            {
                impuestoId = categoria.ImpuestoId,
                cuentaVentasId = categoria.CuentaVentasId,
                cuentaComprasInventariosId = categoria.CuentaComprasInventariosId,
                cuentaCostoVentasGastosId = categoria.CuentaCostoVentasGastosId,
                cuentaDescuentosId = categoria.CuentaDescuentosId,
                cuentaDevolucionesId = categoria.CuentaDevolucionesId,
                cuentaAjustesId = categoria.CuentaAjustesId,
                cuentaCostoMateriaPrimaId = categoria.CuentaCostoMateriaPrimaId
            });
        }

        // GET: Item/CrearCategoria
        [HttpPost]
        public async Task<IActionResult> CrearCategoria([FromBody] Categoria categoria)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Datos inválidos." });
            }

            var empresaId = _userService.GetEmpresaId();
            categoria.EmpresaId = empresaId;
            categoria.Estado = true;
            categoria.FechaCreacion = DateTime.Now;
            categoria.UsuarioCreacionId = _userService.GetUserId();

            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true,
                id = categoria.Id,
                nombre = categoria.Nombre,
                impuestoId = categoria.ImpuestoId,
                cuentaVentasId = categoria.CuentaVentasId,
                cuentaComprasInventariosId = categoria.CuentaComprasInventariosId,
                cuentaCostoVentasGastosId = categoria.CuentaCostoVentasGastosId,
                cuentaDescuentosId = categoria.CuentaDescuentosId,
                cuentaDevolucionesId = categoria.CuentaDevolucionesId,
                cuentaAjustesId = categoria.CuentaAjustesId,
                cuentaCostoMateriaPrimaId = categoria.CuentaCostoMateriaPrimaId
            });
        }

        // GET: Item/CrearMarca
        [HttpPost]
        public async Task<IActionResult> CrearMarca([FromBody] Marca marca)
        {
            if (string.IsNullOrEmpty(marca.Nombre))
            {
                return Json(new { success = false, message = "El nombre es requerido." });
            }

            var empresaId = _userService.GetEmpresaId();
            marca.EmpresaId = empresaId;
            marca.Estado = true;
            marca.FechaCreacion = DateTime.Now;
            marca.UsuarioCreacionId = _userService.GetUserId();

            _context.Marcas.Add(marca);
            await _context.SaveChangesAsync();

            return Json(new { success = true, id = marca.Id, nombre = marca.Nombre });
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
                
                var empresaId = _userService.GetEmpresaId();
                if (empresaId <= 0)
                {
                    return Json(new { results = new List<object>() });
                }
                
                // Buscar cuentas contables que coincidan con el término (case-insensitive)
                var cuentas = await _context.CuentasContables
                    .Where(c => c.EmpresaId == empresaId && c.Activo &&
                           (EF.Functions.ILike(c.Codigo, $"%{term}%") || 
                            EF.Functions.ILike(c.Nombre, $"%{term}%") || 
                           (c.Descripcion != null && EF.Functions.ILike(c.Descripcion, $"%{term}%"))))
                    .OrderBy(c => c.Codigo)
                    .Take(20) // Limitar resultados
                    .ToListAsync();
                
                Console.WriteLine($"Encontradas {cuentas.Count()} cuentas contables para devolver al cliente");
                
                // Imprimir los primeros resultados para depuración
                foreach (var cuenta in cuentas.Take(5))
                {
                    Console.WriteLine($"- Cuenta encontrada: id={cuenta.Id}, código='{cuenta.Codigo}', nombre='{cuenta.Nombre}'");
                }
                
                // Asegurarnos de que los datos estén en el formato esperado por Select2
                var results = cuentas.Select(c => new
                {
                    id = c.Id,
                    text = $"{c.Codigo} - {c.Nombre}", // Este es el texto que se muestra en el select
                    codigo = c.Codigo,
                    nombre = c.Nombre
                }).ToList();
                
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

        // GET: Item/GenerarCodigoAutomatico
        [HttpGet]
        public async Task<IActionResult> GenerarCodigoAutomatico()
        {
            var empresaId = _userService.GetEmpresaId();
            
            // Obtener último código y generar siguiente
            var ultimoCodigo = await _context.Items
                .Where(i => i.EmpresaId == empresaId)
                .OrderByDescending(i => i.Id)
                .Select(i => i.Codigo)
                .FirstOrDefaultAsync();
            
            string nuevoCodigo;
            
            if (string.IsNullOrEmpty(ultimoCodigo))
                nuevoCodigo = "IT-00001";
            else
            {
                int numero = int.Parse(ultimoCodigo.Substring(3)) + 1;
                nuevoCodigo = $"IT-{numero:D5}";
            }
            
            return Json(new { success = true, codigo = nuevoCodigo });
        }

        // POST: Item/ImprimirCodigoBarras
        [HttpPost]
        public IActionResult ImprimirCodigoBarras(string codigoBarras, string nombre, string formato, int cantidad)
        {
            if (string.IsNullOrEmpty(codigoBarras))
            {
                return BadRequest("El código de barras es requerido");
            }
            
            // Crear el modelo para la vista de impresión
            var model = new ImpresionCodigoBarrasViewModel
            {
                CodigoBarras = codigoBarras,
                Nombre = nombre,
                Formato = formato,
                Cantidad = cantidad
            };
            
            // Devolver la vista que generará el PDF
            return View("ImprimirCodigoBarras", model);
        }

        // Obtener conversiones de unidades para un item
        [HttpGet]
        public async Task<IActionResult> ObtenerConversiones(int id)
        {
            try
            {
                // Verificar que el item existe
                var itemExiste = await _context.Items.AnyAsync(i => i.Id == id);
                if (!itemExiste)
                    return NotFound(new { message = "Item no encontrado" });
                
                // Consultar los ItemContenedores relacionados con este item
                var conversiones = await _context.ItemContenedores
                    .Where(ic => ic.ItemId == id)
                    .Select(ic => new {
                        id = ic.Id,
                        contenedorId = ic.Id,
                        contenedorNombre = ic.Nombre,
                        etiqueta = ic.Etiqueta ?? ic.Nombre, // Usar nombre si la etiqueta es nula
                        cantidad = ic.Factor,
                        costo = ic.Costo
                    })
                    .ToListAsync();
                
                return Json(conversiones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> BuscarContenedores(string term = "")
        {
            var empresaId = _userService.GetEmpresaId();
            var query = _context.ItemContenedores.Where(c => c.EmpresaId == empresaId);
            
            if (!string.IsNullOrEmpty(term))
            {
                term = term.ToLower();
                query = query.Where(c => c.Nombre.ToLower().Contains(term));
            }
            
            var contenedores = await query.Take(10).Select(c => new {
                id = c.Id,
                text = c.Nombre
            }).ToListAsync();
            
            return Json(new { results = contenedores });
        }


        // Mtodos privados
        private ItemViewModel PrepararViewModel(ItemViewModel viewModel = null)
        {
            var empresaId = _userService.GetEmpresaId();

            if (viewModel == null)
            {
                viewModel = new ItemViewModel
                {
                    Estado = true,
                    Rendimiento = 100,
                    Proveedores = new List<ItemProveedorViewModel>(),
                    Contenedores = new List<ItemContenedorViewModel>(),
                    Taras = new List<ItemTaraViewModel>(),
                    Almacenes = new List<ItemAlmacenViewModel>(),
                    ProductoVenta = null
                };
            }

            // Cargar listas de selección con valores seleccionados si existen
            viewModel.CategoriasDisponibles = new SelectList(
                _context.Categorias
                    .Where(c => c.EmpresaId == empresaId && c.Estado)
                    .OrderBy(c => c.Nombre),
                "Id", "Nombre", viewModel.CategoriaId
            );

            viewModel.MarcasDisponibles = new SelectList(
                _context.Marcas
                    .Where(m => m.EmpresaId == empresaId && m.Estado)
                    .OrderBy(m => m.Nombre),
                "Id", "Nombre", viewModel.MarcaId
            );

            viewModel.UnidadesMedidaDisponibles = new SelectList(
                _context.UnidadesMedida
                    .Where(u => u.EmpresaId == empresaId && u.Estado)
                    .OrderBy(u => u.Nombre),
                "Id", "Nombre", viewModel.UnidadMedidaInventarioId
            );

            viewModel.ImpuestosDisponibles = new SelectList(
                _context.Impuestos
                    .Where(i => i.EmpresaId == empresaId && i.Estado)
                    .OrderBy(i => i.Nombre),
                "Id", "Nombre", viewModel.ImpuestoId
            );

            // Listas de cuentas contables
            var cuentasContables = _context.CuentasContables
                .Where(c => c.EmpresaId == empresaId && c.Estado)
                .OrderBy(c => c.Codigo)
                .Select(c => new { Id = c.Id, Nombre = $"{c.Codigo} - {c.Nombre}" })
                .ToList();

            viewModel.CuentasVentasDisponibles = new SelectList(cuentasContables, "Id", "Nombre", viewModel.CuentaVentasId);
            viewModel.CuentasComprasInventariosDisponibles = new SelectList(cuentasContables, "Id", "Nombre", viewModel.CuentaComprasInventariosId);
            viewModel.CuentasCostoVentasGastosDisponibles = new SelectList(cuentasContables, "Id", "Nombre", viewModel.CuentaCostoVentasGastosId);
            viewModel.CuentasDescuentosDisponibles = new SelectList(cuentasContables, "Id", "Nombre", viewModel.CuentaDescuentosId);
            viewModel.CuentasDevolucionesDisponibles = new SelectList(cuentasContables, "Id", "Nombre", viewModel.CuentaDevolucionesId);
            viewModel.CuentasAjustesDisponibles = new SelectList(cuentasContables, "Id", "Nombre", viewModel.CuentaAjustesId);
            viewModel.CuentasCostoMateriaPrimaDisponibles = new SelectList(cuentasContables, "Id", "Nombre", viewModel.CuentaCostoMateriaPrimaId);

            // Lista de proveedores (usando Cliente con EsProveedor=true)
            // En el caso de proveedores, se seleccionan individualmente por cada ItemProveedor
            viewModel.ProveedoresDisponibles = new SelectList(
                _context.Clientes
                    .Where(p => p.EmpresaId == empresaId && p.EsProveedor)
                    .OrderBy(p => p.NombreRazonSocial),
                "Id", "NombreRazonSocial"
            );

            // Obtener configuración de empresa para separador decimal
            var empresa = _context.Empresas.Find(empresaId);
            viewModel.SeparadorDecimal = empresa?.SeparadorDecimal ?? ",";

            return viewModel;
        }

        private string GenerarCodigoUnico()
        {
            // Generar código EAN-13
            Random random = new Random();
            string codigo = "";

            // Generar los primeros 12 dígitos
            for (int i = 0; i < 12; i++)
            {
                codigo += random.Next(0, 10);
            }

            // Calcular el dígito de verificación
            int suma = 0;
            for (int i = 0; i < 12; i++)
            {
                int digito = int.Parse(codigo[i].ToString());
                suma += (i % 2 == 0) ? digito : digito * 3;
            }

            int digitoVerificacion = (10 - (suma % 10)) % 10;
            codigo += digitoVerificacion;

            return codigo;
        }

        // GET: Item/DebugEmpresa
        public IActionResult DebugEmpresa()
        {
            return View();
        }
        
        // GET: Item/CheckDatabase
        public IActionResult CheckDatabase()
        {
            return View();
        }
        
        // GET: Item/CheckImpuestos
        public IActionResult CheckImpuestos()
        {
            return View();
        }
        
        // GET: Item/TestSelect2
        public IActionResult TestSelect2()
        {
            // Set empresa 4 in session for testing
            HttpContext.Session.SetInt32("EmpresaActualId", 4);
            return View();
        }
        
        // GET: Item/TestCategoryInheritance
        public IActionResult TestCategoryInheritance()
        {
            // Set empresa 4 in session for testing
            HttpContext.Session.SetInt32("EmpresaActualId", 4);
            return View();
        }
        
        
        // GET: Item/SimpleDiagnostic
        public IActionResult SimpleDiagnostic()
        {
            return View();
        }
        
        // GET: Item/SimpleDebug/{id}
        public async Task<IActionResult> SimpleDebug(int id)
        {
            var empresaId = _userService.GetEmpresaId();
            
            var item = await _context.Items
                .Include(i => i.Categoria)
                .Include(i => i.Marca)
                .Include(i => i.Impuesto)
                .FirstOrDefaultAsync(i => i.Id == id && i.EmpresaId == empresaId);
                
            if (item == null)
            {
                return NotFound($"Item {id} no encontrado para empresa {empresaId}");
            }
            
            var viewModel = new ItemViewModel
            {
                Id = item.Id,
                Codigo = item.Codigo,
                Nombre = item.Nombre,
                CategoriaId = item.CategoriaId,
                MarcaId = item.MarcaId,
                ImpuestoId = item.ImpuestoId,
                Estado = item.Estado
            };
            
            viewModel = PrepararViewModel(viewModel);
            
            return View(viewModel);
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.Id == id);
        }

        // Diagnostic action to check empresa and data
        public async Task<IActionResult> DiagnosticData()
        {
            var empresaId = _userService.GetEmpresaId();
            var empresa = _context.Empresas.Find(empresaId);
            
            var model = new
            {
                EmpresaId = empresaId,
                Empresa = empresa,
                CategoriasCount = await _context.Categorias.Where(c => c.EmpresaId == empresaId).CountAsync(),
                MarcasCount = await _context.Marcas.Where(m => m.EmpresaId == empresaId).CountAsync(),
                ImpuestosCount = await _context.Impuestos.Where(i => i.EmpresaId == empresaId && i.Activo).CountAsync(),
                ProductosCount = await _context.ProductosVenta.CountAsync()
            };

            if (empresa != null)
            {
                ViewBag.Categorias = await _context.Categorias
                    .Where(c => c.EmpresaId == empresaId)
                    .OrderBy(c => c.Nombre)
                    .Select(c => new { c.Id, c.Nombre })
                    .ToListAsync();

                ViewBag.Marcas = await _context.Marcas
                    .Where(m => m.EmpresaId == empresaId)
                    .OrderBy(m => m.Nombre)
                    .Select(m => new { m.Id, m.Nombre })
                    .ToListAsync();

                ViewBag.Impuestos = await _context.Impuestos
                    .Where(i => i.EmpresaId == empresaId && i.Activo)
                    .OrderBy(i => i.Nombre)
                    .Select(i => new { i.Id, i.Nombre, i.Porcentaje })
                    .ToListAsync();

                ViewBag.ProductosImpuestos = await _context.ProductosVenta
                    .Where(p => p.EmpresaId == empresaId)
                    .Include(p => p.Impuesto)
                    .OrderBy(p => p.Nombre)
                    .Take(10)
                    .Select(p => new { 
                        p.Id, 
                        p.Nombre, 
                        PorcentajeImpuesto = p.Impuesto != null ? p.Impuesto.Porcentaje : 0,
                        ImpuestoId = p.ImpuestoId,
                        ImpuestoNombre = p.Impuesto.Nombre
                    })
                    .ToListAsync();

                ViewBag.AllEmpresas = await _context.Empresas
                    .Select(e => new { e.Id, e.Nombre })
                    .ToListAsync();
            }

            return View(model);
        }
        
        // GET: Item/ObtenerProveedores/5
        [HttpGet]
        public async Task<IActionResult> ObtenerProveedores(int id)
        {
            var empresaId = _userService.GetEmpresaId();
            
            var proveedores = await _context.ItemProveedores
                .Where(p => p.ItemId == id && p.EmpresaId == empresaId)
                .Include(p => p.Proveedor)
                .Include(p => p.UnidadMedidaCompra)
                .Select(p => new {
                    proveedorId = p.ProveedorId,
                    proveedorNombre = p.Proveedor.NombreRazonSocial,
                    esPrincipal = p.EsPrincipal,
                    unidadMedidaCompraId = p.UnidadMedidaCompraId,
                    unidadMedidaNombre = p.UnidadMedidaCompra != null ? p.UnidadMedidaCompra.Nombre : "",
                    precioCompra = p.PrecioCompra,
                    factorConversion = p.FactorConversion,
                    codigoProveedor = p.CodigoProveedor,
                    nombreCompra = p.NombreCompra
                })
                .ToListAsync();
            
            return Json(proveedores);
        }
        
        // GET: Item/ObtenerContenedores/5
        [HttpGet]
        public async Task<IActionResult> ObtenerContenedores(int id)
        {
            var empresaId = _userService.GetEmpresaId();
            
            var contenedores = await _context.ItemContenedores
                .Where(c => c.ItemId == id && c.EmpresaId == empresaId)
                .Include(c => c.UnidadMedida)
                .OrderBy(c => c.Orden)
                .Select(c => new {
                    id = c.Id,
                    itemId = c.ItemId,
                    unidadMedidaId = c.UnidadMedidaId,
                    unidadMedidaNombre = c.UnidadMedida != null ? $"{c.UnidadMedida.Nombre} ({c.UnidadMedida.Abreviatura})" : "",
                    cantidad = c.Factor,
                    etiqueta = c.Etiqueta,
                    costo = c.Costo
                })
                .ToListAsync();
            
            return Json(contenedores);
        }
    }
}