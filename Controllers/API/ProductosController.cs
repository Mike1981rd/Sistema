using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Models;
using SistemaContable.Services;
using SistemaContable.ViewModels.Productos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaContable.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmpresaService _empresaService;
        private readonly IUserService _userService;

        public ProductosController(ApplicationDbContext context, IEmpresaService empresaService, IUserService userService)
        {
            _context = context;
            _empresaService = empresaService;
            _userService = userService;
        }

        /// <summary>
        /// Obtiene lista de productos con paginación y filtros
        /// </summary>
        /// <param name="filtro">Parámetros de filtrado y paginación</param>
        /// <returns>Lista paginada de productos</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoListDto>>> ObtenerProductos([FromQuery] ProductoFiltroDto filtro)
        {
            try
            {
                // Obtener empresa actual
                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                if (empresaId <= 0)
                {
                    return BadRequest(new { mensaje = "No se ha seleccionado una empresa válida" });
                }

                // Query base con includes necesarios, filtrando por empresa
                var query = _context.ProductosVenta
                    .Include(p => p.Categoria)
                    .Include(p => p.Variantes)
                    .Where(p => p.EmpresaId == empresaId)
                    .AsQueryable();

                // Aplicar filtros
                if (filtro.CategoriaId.HasValue)
                {
                    query = query.Where(p => p.CategoriaId == filtro.CategoriaId.Value);
                }

                if (filtro.ImpuestoId.HasValue)
                {
                    query = query.Where(p => p.ImpuestoId == filtro.ImpuestoId.Value);
                }

                if (filtro.EsActivo.HasValue)
                {
                    query = query.Where(p => p.EsActivo == filtro.EsActivo.Value);
                }

                if (!string.IsNullOrWhiteSpace(filtro.TextoBusqueda))
                {
                    var textoBusqueda = filtro.TextoBusqueda.ToLower();
                    query = query.Where(p => 
                        p.Nombre.ToLower().Contains(textoBusqueda) ||
                        (p.NombreCortoTPV != null && p.NombreCortoTPV.ToLower().Contains(textoBusqueda)) ||
                        (p.PLU != null && p.PLU.ToLower().Contains(textoBusqueda)) ||
                        (p.Descripcion != null && p.Descripcion.ToLower().Contains(textoBusqueda))
                    );
                }

                if (filtro.PrecioMinimo.HasValue)
                {
                    query = query.Where(p => p.PrecioVenta >= filtro.PrecioMinimo.Value);
                }

                if (filtro.PrecioMaximo.HasValue)
                {
                    query = query.Where(p => p.PrecioVenta <= filtro.PrecioMaximo.Value);
                }

                if (filtro.PermiteModificadores.HasValue)
                {
                    query = query.Where(p => p.PermiteModificadores == filtro.PermiteModificadores.Value);
                }

                if (filtro.DisponibleParaVenta.HasValue)
                {
                    query = query.Where(p => p.DisponibleParaVenta == filtro.DisponibleParaVenta.Value);
                }

                // Aplicar ordenamiento
                switch (filtro.OrdenarPor?.ToLower())
                {
                    case "nombre":
                        query = filtro.OrdenDescendente ? 
                            query.OrderByDescending(p => p.Nombre) : 
                            query.OrderBy(p => p.Nombre);
                        break;
                    case "precio":
                        query = filtro.OrdenDescendente ? 
                            query.OrderByDescending(p => p.PrecioVenta) : 
                            query.OrderBy(p => p.PrecioVenta);
                        break;
                    case "categoria":
                        query = filtro.OrdenDescendente ? 
                            query.OrderByDescending(p => p.Categoria.Nombre) : 
                            query.OrderBy(p => p.Categoria.Nombre);
                        break;
                    case "fecha":
                        query = filtro.OrdenDescendente ? 
                            query.OrderByDescending(p => p.FechaCreacion) : 
                            query.OrderBy(p => p.FechaCreacion);
                        break;
                    default:
                        query = query.OrderBy(p => p.Nombre);
                        break;
                }

                // Calcular total antes de paginar
                var totalItems = await query.CountAsync();

                // Aplicar paginación
                var skip = (filtro.Pagina - 1) * filtro.TamanoPagina;
                query = query.Skip(skip).Take(filtro.TamanoPagina);

                // Ejecutar query y mapear a DTO
                var productos = await query.Select(p => new ProductoListDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    NombreCortoTPV = p.NombreCortoTPV,
                    PrecioVenta = p.PrecioVenta,
                    CategoriaNombre = p.Categoria.Nombre,
                    CategoriaId = p.CategoriaId,
                    EsActivo = p.EsActivo,
                    ImagenUrl = p.ImagenUrl,
                    ColorBotonTPV = p.ColorBotonTPV,
                    PLU = p.PLU,
                    NumeroVariantes = p.Variantes.Count(),
                    PermiteModificadores = p.PermiteModificadores
                }).ToListAsync();

                // Agregar información de paginación en headers
                Response.Headers.Add("X-Total-Count", totalItems.ToString());
                Response.Headers.Add("X-Page-Count", Math.Ceiling((double)totalItems / filtro.TamanoPagina).ToString());

                return Ok(productos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al obtener productos", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un producto específico por ID
        /// </summary>
        /// <param name="id">ID del producto</param>
        /// <returns>Detalle completo del producto</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDetalleDto>> ObtenerProductoPorId(int id)
        {
            try
            {
                // Obtener empresa actual
                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                if (empresaId <= 0)
                {
                    return BadRequest(new { mensaje = "No se ha seleccionado una empresa válida" });
                }

                // Query con todas las relaciones necesarias, filtrando por empresa
                var producto = await _context.ProductosVenta
                    .Include(p => p.Categoria)
                    .Include(p => p.Impuesto)
                    .Include(p => p.ProductoVentaImpuestos)
                        .ThenInclude(pvi => pvi.Impuesto)
                    .Include(p => p.RutaImpresora)
                    .Include(p => p.Variantes)
                    .Include(p => p.ProductoModificadorGrupos)
                        .ThenInclude(pmg => pmg.GrupoModificadores)
                            .ThenInclude(g => g.Modificadores)
                    .FirstOrDefaultAsync(p => p.Id == id && p.EmpresaId == empresaId);

                if (producto == null)
                {
                    return NotFound(new { mensaje = "Producto no encontrado" });
                }

                // Mapear a DTO
                var productoDto = new ProductoDetalleDto
                {
                    Id = producto.Id,
                    Nombre = producto.Nombre,
                    NombreCortoTPV = producto.NombreCortoTPV,
                    Descripcion = producto.Descripcion,
                    PLU = producto.PLU,
                    PrecioVenta = producto.PrecioVenta,
                    Costo = producto.Costo,
                    ImagenUrl = producto.ImagenUrl,
                    ColorBotonTPV = producto.ColorBotonTPV,
                    OrdenClasificacion = producto.OrdenClasificacion,
                    EsActivo = producto.EsActivo,
                    PermiteModificadores = producto.PermiteModificadores,
                    RequierePuntoCoccion = producto.RequierePuntoCoccion,
                    Cantidad = producto.Cantidad,
                    CostoTotal = producto.CostoTotal,
                    DisponibleParaVenta = producto.DisponibleParaVenta,
                    RequierePreparacion = producto.RequierePreparacion,
                    TiempoPreparacion = producto.TiempoPreparacion,
                    ItemId = producto.ItemId,
                    ItemContenedorId = producto.ItemContenedorId,
                    CategoriaId = producto.CategoriaId,
                    ImpuestoId = producto.ImpuestoId,
                    RutaImpresoraId = producto.RutaImpresoraId,
                    FechaCreacion = producto.FechaCreacion,
                    FechaModificacion = producto.FechaModificacion,
                    // Cuentas contables
                    CuentaVentasId = producto.CuentaVentasId,
                    CuentaComprasInventariosId = producto.CuentaComprasInventariosId,
                    CuentaCostoVentasGastosId = producto.CuentaCostoVentasGastosId,
                    CuentaDescuentosId = producto.CuentaDescuentosId,
                    CuentaDevolucionesId = producto.CuentaDevolucionesId,
                    CuentaAjustesId = producto.CuentaAjustesId,
                    CuentaCostoMateriaPrimaId = producto.CuentaCostoMateriaPrimaId,
                    
                    // Mapear relaciones
                    Categoria = new CategoriaSimpleDto
                    {
                        Id = producto.Categoria.Id,
                        Nombre = producto.Categoria.Nombre
                    }
                };

                // Mantener compatibilidad con campo único de impuesto
                if (producto.Impuesto != null)
                {
                    productoDto.Impuesto = new ImpuestoSimpleDto
                    {
                        Id = producto.Impuesto.Id,
                        Nombre = producto.Impuesto.Nombre,
                        Porcentaje = producto.Impuesto.Porcentaje
                    };
                }
                
                // Mapear la nueva colección de impuestos
                if (producto.ProductoVentaImpuestos != null && producto.ProductoVentaImpuestos.Any())
                {
                    productoDto.Impuestos = producto.ProductoVentaImpuestos
                        .OrderBy(pvi => pvi.Orden)
                        .Select(pvi => new ImpuestoSimpleDto
                        {
                            Id = pvi.Impuesto.Id,
                            Nombre = pvi.Impuesto.Nombre,
                            Porcentaje = pvi.Impuesto.Porcentaje
                        })
                        .ToList();
                }

                if (producto.RutaImpresora != null)
                {
                    productoDto.RutaImpresora = new RutaImpresoraSimpleDto
                    {
                        Id = producto.RutaImpresora.Id,
                        Nombre = producto.RutaImpresora.Nombre
                    };
                }

                // Mapear variantes
                productoDto.Variantes = producto.Variantes?.Select(v => new VarianteProductoDto
                {
                    Id = v.Id,
                    Nombre = v.Nombre,
                    PLUVariante = v.PLUVariante,
                    PrecioAdicionalOAbsoluto = v.PrecioAdicionalOAbsoluto,
                    AjustePrecioTipo = v.AjustePrecioTipo.ToString(),
                    Stock = v.Stock,
                    OrdenClasificacion = v.OrdenClasificacion,
                    EsActivo = v.EsActivo
                }).ToList() ?? new List<VarianteProductoDto>();

                // Mapear grupos de modificadores
                productoDto.GruposModificadores = producto.ProductoModificadorGrupos?.Select(pmg => new GrupoModificadoresAsignadoDto
                {
                    GrupoModificadoresId = pmg.GrupoModificadoresId,
                    Nombre = pmg.GrupoModificadores.Nombre,
                    EsForzado = pmg.GrupoModificadores.EsForzado,
                    MinSeleccion = pmg.GrupoModificadores.MinSeleccion,
                    MaxSeleccion = pmg.GrupoModificadores.MaxSeleccion,
                    TipoVisualizacionTPV = pmg.GrupoModificadores.TipoVisualizacionTPV.ToString(),
                    OrdenEspecificoProducto = pmg.OrdenEspecificoProducto,
                    Modificadores = pmg.GrupoModificadores.Modificadores?.Select(m => new ModificadorSimpleDto
                    {
                        Id = m.Id,
                        Nombre = m.Nombre,
                        PrecioAdicional = m.PrecioAdicional,
                        OrdenClasificacion = m.OrdenClasificacion,
                        EsActivo = m.EsActivo
                    }).ToList() ?? new List<ModificadorSimpleDto>()
                }).ToList() ?? new List<GrupoModificadoresAsignadoDto>();

                // Devolver explícitamente Ok() para evitar el wrapper de ActionResult
                return Ok(productoDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al obtener el producto", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo producto
        /// </summary>
        /// <param name="productoDto">Datos del producto a crear</param>
        /// <returns>Producto creado</returns>
        [HttpPost]
        public async Task<ActionResult<ProductoDetalleDto>> CrearProducto([FromBody] ProductoCrearDto productoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Verificar que las entidades relacionadas existan
                var categoriaExiste = await _context.Categorias.AnyAsync(c => c.Id == productoDto.CategoriaId);
                if (!categoriaExiste)
                {
                    return BadRequest(new { mensaje = "La categoría especificada no existe" });
                }

                if (productoDto.ItemId.HasValue)
                {
                    var itemExiste = await _context.Items.AnyAsync(i => i.Id == productoDto.ItemId.Value);
                    if (!itemExiste)
                    {
                        return BadRequest(new { mensaje = "El item especificado no existe" });
                    }
                }

                if (productoDto.ImpuestoId.HasValue)
                {
                    var impuestoExiste = await _context.Impuestos.AnyAsync(i => i.Id == productoDto.ImpuestoId.Value);
                    if (!impuestoExiste)
                    {
                        return BadRequest(new { mensaje = "El impuesto especificado no existe" });
                    }
                }

                if (productoDto.RutaImpresoraId.HasValue)
                {
                    var rutaExiste = await _context.RutasImpresora.AnyAsync(r => r.Id == productoDto.RutaImpresoraId.Value);
                    if (!rutaExiste)
                    {
                        return BadRequest(new { mensaje = "La ruta de impresora especificada no existe" });
                    }
                }

                // Generar PLU automáticamente si no se proporciona
                if (string.IsNullOrWhiteSpace(productoDto.PLU))
                {
                    // Generar PLU único basado en timestamp y ID aleatorio
                    var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                    var random = new Random().Next(100, 999);
                    var pluCandidato = $"PLU{timestamp}{random}";
                    
                    // Verificar que no exista (muy improbable pero por seguridad)
                    while (await _context.ProductosVenta.AnyAsync(p => p.PLU == pluCandidato))
                    {
                        random = new Random().Next(100, 999);
                        pluCandidato = $"PLU{timestamp}{random}";
                    }
                    
                    productoDto.PLU = pluCandidato;
                }
                else
                {
                    // Si se proporciona PLU, verificar unicidad
                    var pluExiste = await _context.ProductosVenta.AnyAsync(p => p.PLU == productoDto.PLU);
                    if (pluExiste)
                    {
                        return BadRequest(new { mensaje = "Ya existe un producto con ese PLU" });
                    }
                }

                // Crear el nuevo producto
                var producto = new ProductoVenta
                {
                    Nombre = productoDto.Nombre,
                    NombreCortoTPV = productoDto.NombreCortoTPV,
                    Descripcion = productoDto.Descripcion,
                    PLU = productoDto.PLU,
                    PrecioVenta = productoDto.PrecioVenta,
                    Costo = productoDto.Costo,
                    ImagenUrl = productoDto.ImagenUrl,
                    ColorBotonTPV = productoDto.ColorBotonTPV,
                    OrdenClasificacion = productoDto.OrdenClasificacion,
                    EsActivo = productoDto.EsActivo,
                    PermiteModificadores = productoDto.PermiteModificadores,
                    RequierePuntoCoccion = productoDto.RequierePuntoCoccion,
                    ItemId = productoDto.ItemId,
                    ItemContenedorId = productoDto.ItemContenedorId,
                    CategoriaId = productoDto.CategoriaId,
                    ImpuestoId = productoDto.ImpuestoId,
                    RutaImpresoraId = productoDto.RutaImpresoraId,
                    Cantidad = productoDto.Cantidad,
                    CostoTotal = productoDto.Costo * productoDto.Cantidad,
                    DisponibleParaVenta = productoDto.DisponibleParaVenta,
                    RequierePreparacion = productoDto.RequierePreparacion,
                    TiempoPreparacion = productoDto.TiempoPreparacion,
                    EmpresaId = productoDto.EmpresaId,
                    // Cuentas contables
                    CuentaVentasId = productoDto.CuentaVentasId,
                    CuentaComprasInventariosId = productoDto.CuentaComprasInventariosId,
                    CuentaCostoVentasGastosId = productoDto.CuentaCostoVentasGastosId,
                    CuentaDescuentosId = productoDto.CuentaDescuentosId,
                    CuentaDevolucionesId = productoDto.CuentaDevolucionesId,
                    CuentaAjustesId = productoDto.CuentaAjustesId,
                    CuentaCostoMateriaPrimaId = productoDto.CuentaCostoMateriaPrimaId,
                    // Auditoría
                    UsuarioCreacionId = _userService.GetUserId()
                };

                _context.ProductosVenta.Add(producto);
                await _context.SaveChangesAsync();
                
                // Guardar impuestos múltiples
                if (productoDto.ImpuestoIds != null && productoDto.ImpuestoIds.Any())
                {
                    int orden = 0;
                    foreach (var impuestoId in productoDto.ImpuestoIds.Distinct())
                    {
                        // Verificar que el impuesto existe
                        var impuestoExiste = await _context.Impuestos.AnyAsync(i => i.Id == impuestoId);
                        if (!impuestoExiste)
                        {
                            return BadRequest(new { mensaje = $"El impuesto con ID {impuestoId} no existe" });
                        }
                        
                        var productoImpuesto = new ProductoVentaImpuesto
                        {
                            ProductoVentaId = producto.Id,
                            ImpuestoId = impuestoId,
                            Orden = orden++,
                            EmpresaId = productoDto.EmpresaId,
                            UsuarioCreacionId = _userService.GetUserId()
                        };
                        _context.ProductoVentaImpuestos.Add(productoImpuesto);
                    }
                    await _context.SaveChangesAsync();
                }
                // Si no se enviaron ImpuestoIds pero sí ImpuestoId (compatibilidad)
                else if (productoDto.ImpuestoId.HasValue)
                {
                    // Verificar que el impuesto existe
                    var impuestoExiste = await _context.Impuestos.AnyAsync(i => i.Id == productoDto.ImpuestoId.Value);
                    if (!impuestoExiste)
                    {
                        return BadRequest(new { mensaje = $"El impuesto con ID {productoDto.ImpuestoId.Value} no existe" });
                    }
                    
                    var productoImpuesto = new ProductoVentaImpuesto
                    {
                        ProductoVentaId = producto.Id,
                        ImpuestoId = productoDto.ImpuestoId.Value,
                        Orden = 0,
                        EmpresaId = productoDto.EmpresaId,
                        UsuarioCreacionId = 1 // TODO: Obtener del usuario actual
                    };
                    _context.ProductoVentaImpuestos.Add(productoImpuesto);
                    await _context.SaveChangesAsync();
                }

                // 3. Manejo de múltiples precios (nuevo)
                await ProcesarPreciosProducto(producto.Id, productoDto.Precios, productoDto.PrecioVenta, productoDto.EmpresaId);

                // Retornar el producto creado
                return CreatedAtAction(
                    nameof(ObtenerProductoPorId), 
                    new { id = producto.Id }, 
                    await ObtenerProductoPorId(producto.Id)
                );
            }
            catch (Exception ex)
            {
                // Log más detallado del error
                var innerException = ex.InnerException != null ? ex.InnerException.Message : "";
                var stackTrace = ex.StackTrace;
                
                return StatusCode(500, new { 
                    mensaje = "Error al crear el producto", 
                    detalle = ex.Message,
                    innerException = innerException,
                    stackTrace = stackTrace
                });
            }
        }

        /// <summary>
        /// Actualiza un producto existente
        /// </summary>
        /// <param name="id">ID del producto</param>
        /// <param name="productoDto">Datos actualizados del producto</param>
        /// <returns>NoContent si la actualización es exitosa</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarProducto(int id, [FromBody] ProductoActualizarDto productoDto)
        {
            if (id != productoDto.Id)
            {
                return BadRequest(new { mensaje = "El ID del producto no coincide" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // DEBUG: Log datos recibidos
            Console.WriteLine($"[DEBUG] ActualizarProducto - ID: {id}");
            Console.WriteLine($"[DEBUG] Nombre: {productoDto.Nombre}");
            Console.WriteLine($"[DEBUG] PrecioVenta: {productoDto.PrecioVenta}");
            Console.WriteLine($"[DEBUG] Precios recibidos: {productoDto.Precios?.Count ?? 0}");
            if (productoDto.Precios != null)
            {
                for (int i = 0; i < productoDto.Precios.Count; i++)
                {
                    var precio = productoDto.Precios[i];
                    Console.WriteLine($"[DEBUG] Precio[{i}]:");
                    Console.WriteLine($"  - Id: {precio.Id}");
                    Console.WriteLine($"  - NombreNivel: '{precio.NombreNivel}'");
                    Console.WriteLine($"  - PrecioBase: {precio.PrecioBase}");
                    Console.WriteLine($"  - PrecioTotal: {precio.PrecioTotal}");
                    Console.WriteLine($"  - ImpuestoIds: [{string.Join(",", precio.ImpuestoIds)}]");
                    Console.WriteLine($"  - Orden: {precio.Orden}");
                    Console.WriteLine($"  - EsPrincipal: {precio.EsPrincipal}");
                    Console.WriteLine($"  - Activo: {precio.Activo}");
                }
            }

            try
            {
                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                if (empresaId <= 0)
                {
                    return BadRequest(new { mensaje = "No se ha seleccionado una empresa válida" });
                }

                var producto = await _context.ProductosVenta.FindAsync(id);
                if (producto == null)
                {
                    return NotFound(new { mensaje = "Producto no encontrado" });
                }

                // Verificar entidades relacionadas
                var categoriaExiste = await _context.Categorias.AnyAsync(c => c.Id == productoDto.CategoriaId);
                if (!categoriaExiste)
                {
                    return BadRequest(new { mensaje = "La categoría especificada no existe" });
                }

                if (productoDto.ImpuestoId.HasValue)
                {
                    var impuestoExiste = await _context.Impuestos.AnyAsync(i => i.Id == productoDto.ImpuestoId.Value);
                    if (!impuestoExiste)
                    {
                        return BadRequest(new { mensaje = "El impuesto especificado no existe" });
                    }
                }

                // Manejar PLU en edición
                if (string.IsNullOrWhiteSpace(productoDto.PLU))
                {
                    // Si el PLU está vacío, generar uno nuevo automáticamente
                    var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                    var random = new Random().Next(100, 999);
                    var pluCandidato = $"PLU{timestamp}{random}";
                    
                    // Verificar que no exista
                    while (await _context.ProductosVenta.AnyAsync(p => p.PLU == pluCandidato && p.Id != id))
                    {
                        random = new Random().Next(100, 999);
                        pluCandidato = $"PLU{timestamp}{random}";
                    }
                    
                    productoDto.PLU = pluCandidato;
                }
                else if (productoDto.PLU != producto.PLU)
                {
                    // Si se cambió el PLU, verificar que no exista
                    var pluExiste = await _context.ProductosVenta.AnyAsync(p => p.PLU == productoDto.PLU && p.Id != id);
                    if (pluExiste)
                    {
                        return BadRequest(new { mensaje = $"Ya existe un producto con el PLU {productoDto.PLU}" });
                    }
                }

                // Actualizar propiedades del producto
                producto.Nombre = productoDto.Nombre;
                producto.NombreCortoTPV = productoDto.NombreCortoTPV;
                producto.Descripcion = productoDto.Descripcion;
                producto.PLU = productoDto.PLU;
                producto.PrecioVenta = productoDto.PrecioVenta;
                producto.Costo = productoDto.Costo;
                producto.ImagenUrl = productoDto.ImagenUrl;
                producto.ColorBotonTPV = productoDto.ColorBotonTPV;
                producto.OrdenClasificacion = productoDto.OrdenClasificacion;
                producto.EsActivo = productoDto.EsActivo;
                producto.PermiteModificadores = productoDto.PermiteModificadores;
                producto.RequierePuntoCoccion = productoDto.RequierePuntoCoccion;
                producto.ItemId = productoDto.ItemId;
                producto.ItemContenedorId = productoDto.ItemContenedorId;
                producto.CategoriaId = productoDto.CategoriaId;
                producto.ImpuestoId = productoDto.ImpuestoId;
                producto.RutaImpresoraId = productoDto.RutaImpresoraId;
                producto.Cantidad = productoDto.Cantidad;
                producto.DisponibleParaVenta = productoDto.DisponibleParaVenta;
                producto.RequierePreparacion = productoDto.RequierePreparacion;
                producto.TiempoPreparacion = productoDto.TiempoPreparacion;
                
                // Propiedades de Contabilidad
                producto.CuentaVentasId = productoDto.CuentaVentasId;
                producto.CuentaComprasInventariosId = productoDto.CuentaComprasInventariosId;
                producto.CuentaCostoVentasGastosId = productoDto.CuentaCostoVentasGastosId;
                producto.CuentaDescuentosId = productoDto.CuentaDescuentosId;
                producto.CuentaDevolucionesId = productoDto.CuentaDevolucionesId;
                producto.CuentaAjustesId = productoDto.CuentaAjustesId;
                producto.CuentaCostoMateriaPrimaId = productoDto.CuentaCostoMateriaPrimaId;

                // Actualizar producto en el contexto
                _context.Entry(producto).State = EntityState.Modified;

                // Limpiar impuestos del sistema viejo (se manejará por precios múltiples)
                var impuestosExistentes = await _context.ProductoVentaImpuestos
                    .Where(pi => pi.ProductoVentaId == producto.Id)
                    .ToListAsync();
                _context.ProductoVentaImpuestos.RemoveRange(impuestosExistentes);

                // 2. Manejo de múltiples precios (nuevo)
                Console.WriteLine($"[DEBUG] Llamando ProcesarPreciosProducto con empresaId: {empresaId}");
                Console.WriteLine($"[DEBUG] productoDto.Precios?.Count: {productoDto.Precios?.Count ?? 0}");
                
                try
                {
                    await ProcesarPreciosProducto(producto.Id, productoDto.Precios, productoDto.PrecioVenta, empresaId);
                    Console.WriteLine($"[DEBUG] ProcesarPreciosProducto completado");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Error en ProcesarPreciosProducto: {ex.Message}");
                    Console.WriteLine($"[ERROR] StackTrace: {ex.StackTrace}");
                    throw; // Re-lanzar para que se maneje en el catch principal
                }
                
                await _context.SaveChangesAsync();
                Console.WriteLine($"[DEBUG] SaveChangesAsync completado");

                return Ok(new { id = producto.Id, mensaje = "Producto actualizado correctamente" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProductoExiste(id))
                {
                    return NotFound(new { mensaje = "El producto fue eliminado mientras se actualizaba" });
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al actualizar el producto", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un producto
        /// </summary>
        /// <param name="id">ID del producto a eliminar</param>
        /// <returns>NoContent si la eliminación es exitosa</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            try
            {
                var producto = await _context.ProductosVenta.FindAsync(id);
                if (producto == null)
                {
                    return NotFound(new { mensaje = "Producto no encontrado" });
                }

                _context.ProductosVenta.Remove(producto);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                // Verificar si hay relaciones que impiden la eliminación
                if (ex.InnerException != null && ex.InnerException.Message.Contains("FK_"))
                {
                    return Conflict(new { mensaje = "No se puede eliminar el producto porque tiene registros relacionados" });
                }
                throw;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al eliminar el producto", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Verifica si un producto existe
        /// </summary>
        /// <param name="id">ID del producto</param>
        /// <returns>True si existe, false si no</returns>
        private async Task<bool> ProductoExiste(int id)
        {
            return await _context.ProductosVenta.AnyAsync(p => p.Id == id);
        }

        /// <summary>
        /// Obtiene la receta de un producto
        /// </summary>
        /// <param name="productoId">ID del producto</param>
        /// <returns>Receta del producto con ingredientes</returns>
        [HttpGet("{productoId}/receta")]
        public async Task<ActionResult<RecetaProductoDto>> ObtenerReceta(int productoId)
        {
            try
            {
                // Obtener empresa actual
                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                if (empresaId <= 0)
                {
                    return BadRequest(new { mensaje = "No se ha seleccionado una empresa válida" });
                }

                var producto = await _context.ProductosVenta
                    .Include(p => p.IngredientesDeEsteProducto)
                        .ThenInclude(ri => ri.Item)
                            .ThenInclude(i => i.Marca)
                    .Include(p => p.IngredientesDeEsteProducto)
                        .ThenInclude(ri => ri.ItemContenedor)
                            .ThenInclude(ic => ic.UnidadMedida)
                    .FirstOrDefaultAsync(p => p.Id == productoId && p.EmpresaId == empresaId);

                if (producto == null)
                {
                    return NotFound(new { mensaje = "Producto no encontrado" });
                }

                var recetaDto = new RecetaProductoDto
                {
                    ProductoId = productoId,
                    NotasReceta = producto.NotasReceta,
                    MargenGanancia = producto.MargenGananciaReceta,
                    Ingredientes = producto.IngredientesDeEsteProducto?.Select(ri => new RecetaIngredienteDto
                    {
                        Id = ri.Id,
                        ItemId = ri.ItemId,
                        ItemContenedorId = ri.ItemContenedorId,
                        Cantidad = ri.Cantidad,
                        CostoUnitario = ri.CostoUnitario,
                        NombreItem = ri.Item?.Nombre,
                        MarcaNombre = ri.Item?.Marca?.Nombre,
                        UnidadMedidaNombre = ri.ItemContenedor?.UnidadMedida?.Nombre
                    }).ToList() ?? new List<RecetaIngredienteDto>()
                };

                return Ok(recetaDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al obtener la receta", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Guarda o actualiza la receta de un producto
        /// </summary>
        /// <param name="productoId">ID del producto</param>
        /// <param name="recetaDto">Datos de la receta</param>
        /// <returns>Receta guardada</returns>
        [HttpPost("{productoId}/receta")]
        public async Task<ActionResult<RecetaProductoDto>> GuardarReceta(int productoId, [FromBody] RecetaProductoDto recetaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Obtener empresa actual
                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                if (empresaId <= 0)
                {
                    return BadRequest(new { mensaje = "No se ha seleccionado una empresa válida" });
                }

                var producto = await _context.ProductosVenta
                    .Include(p => p.IngredientesDeEsteProducto)
                    .FirstOrDefaultAsync(p => p.Id == productoId && p.EmpresaId == empresaId);

                if (producto == null)
                {
                    return NotFound(new { mensaje = "Producto no encontrado" });
                }

                // Actualizar información general de la receta
                producto.NotasReceta = recetaDto.NotasReceta;
                producto.MargenGananciaReceta = recetaDto.MargenGanancia;

                // Eliminar ingredientes existentes
                if (producto.IngredientesDeEsteProducto != null && producto.IngredientesDeEsteProducto.Any())
                {
                    _context.RecetasIngredientes.RemoveRange(producto.IngredientesDeEsteProducto);
                }

                // Agregar nuevos ingredientes
                var nuevosIngredientes = new List<RecetaIngrediente>();
                decimal costoTotalReceta = 0;

                // DEBUG: Verificar que tenemos el productoId correcto
                if (productoId <= 0)
                {
                    return BadRequest(new { mensaje = $"ProductoId inválido: {productoId}" });
                }

                foreach (var ingredienteDto in recetaDto.Ingredientes)
                {
                    var nuevoIngrediente = new RecetaIngrediente
                    {
                        ProductoCompuestoId = productoId,
                        ItemId = ingredienteDto.ItemId,
                        ItemContenedorId = ingredienteDto.ItemContenedorId,
                        Cantidad = ingredienteDto.Cantidad,
                        CostoUnitario = ingredienteDto.CostoUnitario,
                        CostoTotal = ingredienteDto.Cantidad * ingredienteDto.CostoUnitario,
                        EmpresaId = empresaId,
                        FechaCreacion = DateTime.UtcNow,
                        UsuarioCreacionId = _userService.GetUserId()
                    };

                    costoTotalReceta += nuevoIngrediente.CostoTotal;
                    nuevosIngredientes.Add(nuevoIngrediente);
                }

                // Actualizar costo total de la receta
                producto.CostoTotalReceta = costoTotalReceta;

                // Agregar nuevos ingredientes
                await _context.RecetasIngredientes.AddRangeAsync(nuevosIngredientes);

                // Guardar cambios
                await _context.SaveChangesAsync();

                // Recargar y devolver la receta actualizada
                return await ObtenerReceta(productoId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al guardar la receta", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Elimina todos los ingredientes de la receta de un producto
        /// </summary>
        /// <param name="productoId">ID del producto</param>
        /// <returns>Resultado de la operación</returns>
        [HttpDelete("{productoId}/receta")]
        public async Task<IActionResult> EliminarReceta(int productoId)
        {
            try
            {
                // Obtener empresa actual
                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                if (empresaId <= 0)
                {
                    return BadRequest(new { mensaje = "No se ha seleccionado una empresa válida" });
                }

                var producto = await _context.ProductosVenta
                    .Include(p => p.IngredientesDeEsteProducto)
                    .FirstOrDefaultAsync(p => p.Id == productoId && p.EmpresaId == empresaId);

                if (producto == null)
                {
                    return NotFound(new { mensaje = "Producto no encontrado" });
                }

                // Eliminar ingredientes
                if (producto.IngredientesDeEsteProducto != null && producto.IngredientesDeEsteProducto.Any())
                {
                    _context.RecetasIngredientes.RemoveRange(producto.IngredientesDeEsteProducto);
                }

                // Limpiar campos de receta
                producto.NotasReceta = null;
                producto.MargenGananciaReceta = null;
                producto.CostoTotalReceta = null;

                await _context.SaveChangesAsync();

                return Ok(new { mensaje = "Receta eliminada exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al eliminar la receta", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene los precios de un producto específico
        /// </summary>
        /// <param name="productoId">ID del producto</param>
        /// <returns>Lista de precios del producto</returns>
        [HttpGet("{productoId}/precios")]
        public async Task<ActionResult<List<ProductoPrecioDetalleDto>>> ObtenerPreciosProducto(int productoId)
        {
            try
            {
                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                if (empresaId <= 0)
                {
                    return BadRequest(new { mensaje = "No se ha seleccionado una empresa válida" });
                }

                var precios = await _context.ProductoVentaPrecios
                    .Include(p => p.ImpuestosEspecificos)
                        .ThenInclude(pi => pi.Impuesto)
                    .Include(p => p.ListaPrecio)
                    .Where(p => p.ProductoVentaId == productoId && p.Activo)
                    .OrderBy(p => p.Orden)
                    .ToListAsync();

                var preciosDto = precios.Select(p => new ProductoPrecioDetalleDto
                {
                    Id = p.Id,
                    NombreNivel = p.NombreNivel,
                    PrecioBase = p.PrecioBase,
                    PrecioTotal = p.PrecioTotal,
                    Orden = p.Orden,
                    EsPrincipal = p.EsPrincipal,
                    ListaPrecioId = p.ListaPrecioId,
                    Descripcion = p.Descripcion,
                    Activo = p.Activo,
                    ImpuestoIds = p.ImpuestosEspecificos?.OrderBy(pi => pi.Orden).Select(pi => pi.ImpuestoId).ToList() ?? new List<int>(),
                    ImpuestosAplicados = p.ImpuestosEspecificos?.OrderBy(pi => pi.Orden).Select(pi => new ImpuestoAplicadoDto
                    {
                        Id = pi.ImpuestoId,
                        Nombre = pi.Impuesto?.Nombre ?? "",
                        Porcentaje = pi.Impuesto?.Porcentaje ?? 0,
                        PorcentajeOverride = pi.PorcentajeOverride,
                        PorcentajeEfectivo = pi.ObtenerPorcentajeEfectivo(),
                        Orden = pi.Orden
                    }).ToList() ?? new List<ImpuestoAplicadoDto>(),
                    PorcentajeTotalImpuestos = p.ImpuestosEspecificos?.Sum(pi => pi.ObtenerPorcentajeEfectivo()) ?? 0
                }).ToList();

                return Ok(preciosDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al obtener precios del producto", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Procesa los precios de un producto (crear/actualizar/eliminar)
        /// </summary>
        /// <param name="productoId">ID del producto</param>
        /// <param name="preciosDto">Lista de precios del DTO</param>
        /// <param name="precioVentaFallback">Precio de venta para compatibilidad</param>
        /// <param name="empresaId">ID de la empresa</param>
        private async Task ProcesarPreciosProducto(int productoId, List<ProductoPrecioDto> preciosDto, decimal precioVentaFallback, int empresaId)
        {
            try
            {
                // DEBUG: Log de los precios recibidos
                if (preciosDto != null && preciosDto.Any())
                {
                    Console.WriteLine($"[DEBUG] ProcesarPreciosProducto - ProductoId: {productoId}, Precios recibidos: {preciosDto.Count}");
                    foreach (var precio in preciosDto)
                    {
                        Console.WriteLine($"[DEBUG] Precio: Id={precio.Id}, Nombre={precio.NombreNivel}, Base={precio.PrecioBase}, Total={precio.PrecioTotal}, Impuestos={string.Join(",", precio.ImpuestoIds)}");
                    }
                }
                else
                {
                    Console.WriteLine($"[DEBUG] ProcesarPreciosProducto - ProductoId: {productoId}, No se recibieron precios o lista vacía");
                }

                // 1. Si no se enviaron precios, crear uno por defecto usando PrecioVenta
                if (preciosDto == null || !preciosDto.Any())
                {
                    // Solo crear precio por defecto si no existe ninguno
                    var tienePrecios = await _context.ProductoVentaPrecios
                        .AnyAsync(p => p.ProductoVentaId == productoId);
                        
                    if (!tienePrecios && precioVentaFallback > 0)
                    {
                        Console.WriteLine($"[DEBUG] Creando precio por defecto: {precioVentaFallback}");
                        var precioDefault = new ProductoVentaPrecio
                        {
                            ProductoVentaId = productoId,
                            NombreNivel = "Precio Base",
                            PrecioBase = precioVentaFallback,
                            PrecioTotal = precioVentaFallback,
                            Orden = 0,
                            EsPrincipal = true,
                            Activo = true,
                            EmpresaId = empresaId,
                            FechaCreacion = DateTime.UtcNow,
                            UsuarioCreacionId = _userService.GetUserId()
                        };
                        _context.ProductoVentaPrecios.Add(precioDefault);
                        await _context.SaveChangesAsync();
                    }
                    return;
                }

                // 2. Obtener precios existentes
                var preciosExistentes = await _context.ProductoVentaPrecios
                    .Include(p => p.ImpuestosEspecificos)
                    .Where(p => p.ProductoVentaId == productoId)
                    .ToListAsync();

                // 3. Procesar cada precio del DTO
                foreach (var precioDto in preciosDto)
                {
                    Console.WriteLine($"[DEBUG] Procesando precio: {precioDto.NombreNivel}, Base: {precioDto.PrecioBase}, Total: {precioDto.PrecioTotal}");
                    Console.WriteLine($"[DEBUG] Impuestos recibidos para {precioDto.NombreNivel}: [{string.Join(",", precioDto.ImpuestoIds ?? new List<int>())}] (Total: {precioDto.ImpuestoIds?.Count ?? 0})");
                    
                    if (precioDto.Id.HasValue && precioDto.Id.Value > 0)
                    {
                        // Actualizar precio existente
                        var precioExistente = preciosExistentes.FirstOrDefault(p => p.Id == precioDto.Id.Value);
                        if (precioExistente != null)
                        {
                            Console.WriteLine($"[DEBUG] Actualizando precio existente ID: {precioExistente.Id}");
                            Console.WriteLine($"[DEBUG] Valores antes: Base={precioExistente.PrecioBase}, Total={precioExistente.PrecioTotal}");
                            
                            precioExistente.NombreNivel = precioDto.NombreNivel;
                            precioExistente.PrecioBase = precioDto.PrecioBase;
                            precioExistente.PrecioTotal = precioDto.PrecioTotal;
                            precioExistente.Orden = precioDto.Orden;
                            precioExistente.EsPrincipal = precioDto.EsPrincipal;
                            precioExistente.ListaPrecioId = precioDto.ListaPrecioId;
                            precioExistente.Descripcion = precioDto.Descripcion;
                            precioExistente.Activo = precioDto.Activo;
                            precioExistente.FechaModificacion = DateTime.UtcNow;
                            precioExistente.UsuarioModificacionId = _userService.GetUserId();

                            Console.WriteLine($"[DEBUG] Valores después: Base={precioExistente.PrecioBase}, Total={precioExistente.PrecioTotal}");

                            // Actualizar impuestos específicos de este precio
                            await ActualizarImpuestosPrecio(precioExistente.Id, precioDto.ImpuestoIds, empresaId);
                        }
                        else
                        {
                            Console.WriteLine($"[DEBUG] No se encontró precio existente con ID: {precioDto.Id.Value}");
                        }
                    }
                    else
                    {
                        // Crear nuevo precio
                        Console.WriteLine($"[DEBUG] Creando nuevo precio: {precioDto.NombreNivel}");
                        var nuevoPrecio = new ProductoVentaPrecio
                        {
                            ProductoVentaId = productoId,
                            NombreNivel = precioDto.NombreNivel,
                            PrecioBase = precioDto.PrecioBase,
                            PrecioTotal = precioDto.PrecioTotal,
                            Orden = precioDto.Orden,
                            EsPrincipal = precioDto.EsPrincipal,
                            ListaPrecioId = precioDto.ListaPrecioId,
                            Descripcion = precioDto.Descripcion,
                            Activo = precioDto.Activo,
                            EmpresaId = empresaId,
                            FechaCreacion = DateTime.UtcNow,
                            UsuarioCreacionId = _userService.GetUserId()
                        };
                        
                        Console.WriteLine($"[DEBUG] Nuevo precio creado con Base={nuevoPrecio.PrecioBase}, Total={nuevoPrecio.PrecioTotal}");
                        _context.ProductoVentaPrecios.Add(nuevoPrecio);
                        await _context.SaveChangesAsync(); // Necesario para obtener el ID
                        
                        Console.WriteLine($"[DEBUG] Nuevo precio guardado con ID: {nuevoPrecio.Id}");
                        
                        // Agregar impuestos específicos del nuevo precio
                        await ActualizarImpuestosPrecio(nuevoPrecio.Id, precioDto.ImpuestoIds, empresaId);
                    }
                }

                // 4. Eliminar precios que no están en el DTO (soft delete)
                var idsEnDto = preciosDto.Where(p => p.Id.HasValue).Select(p => p.Id.Value).ToList();
                var preciosAEliminar = preciosExistentes.Where(p => !idsEnDto.Contains(p.Id)).ToList();
                
                foreach (var precioAEliminar in preciosAEliminar)
                {
                    precioAEliminar.Activo = false;
                    precioAEliminar.FechaModificacion = DateTime.UtcNow;
                    precioAEliminar.UsuarioModificacionId = _userService.GetUserId();
                }

                // 5. Asegurar que hay al menos un precio principal activo
                await AsegurarPrecioPrincipal(productoId);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al procesar precios del producto: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Actualiza los impuestos específicos de un precio
        /// </summary>
        /// <param name="precioId">ID del precio</param>
        /// <param name="impuestoIds">Lista de IDs de impuestos</param>
        /// <param name="empresaId">ID de la empresa</param>
        private async Task ActualizarImpuestosPrecio(int precioId, List<int> impuestoIds, int empresaId)
        {
            Console.WriteLine($"[DEBUG] ActualizarImpuestosPrecio - precioId: {precioId}, impuestoIds: [{string.Join(",", impuestoIds ?? new List<int>())}]");
            
            // Eliminar impuestos existentes
            var impuestosExistentes = await _context.ProductoVentaPrecioImpuestos
                .Where(pi => pi.ProductoVentaPrecioId == precioId)
                .ToListAsync();
            Console.WriteLine($"[DEBUG] Eliminando {impuestosExistentes.Count} impuestos existentes para precio {precioId}");
            _context.ProductoVentaPrecioImpuestos.RemoveRange(impuestosExistentes);

            // Agregar nuevos impuestos si se proporcionaron
            if (impuestoIds?.Any() == true)
            {
                Console.WriteLine($"[DEBUG] Agregando {impuestoIds.Count} impuestos nuevos para precio {precioId}");
                int orden = 0;
                foreach (var impuestoId in impuestoIds)
                {
                    var impuestoPrecio = new ProductoVentaPrecioImpuesto
                    {
                        ProductoVentaPrecioId = precioId,
                        ImpuestoId = impuestoId,
                        Orden = orden++,
                        Activo = true,
                        EmpresaId = empresaId,
                        FechaCreacion = DateTime.UtcNow,
                        UsuarioCreacionId = _userService.GetUserId()
                    };
                    Console.WriteLine($"[DEBUG] Agregando impuesto {impuestoId} con orden {orden - 1} para precio {precioId}");
                    _context.ProductoVentaPrecioImpuestos.Add(impuestoPrecio);
                }
            }
            else
            {
                Console.WriteLine($"[DEBUG] No hay impuestos para agregar al precio {precioId}");
            }
            
            // Guardar cambios de impuestos inmediatamente
            await _context.SaveChangesAsync();
            Console.WriteLine($"[DEBUG] Cambios de impuestos guardados para precio {precioId}");
        }

        /// <summary>
        /// Asegura que el producto tenga al menos un precio principal activo
        /// </summary>
        /// <param name="productoId">ID del producto</param>
        private async Task AsegurarPrecioPrincipal(int productoId)
        {
            var preciosActivos = await _context.ProductoVentaPrecios
                .Where(p => p.ProductoVentaId == productoId && p.Activo)
                .OrderBy(p => p.Orden)
                .ToListAsync();

            if (preciosActivos.Any())
            {
                var tienePrincipal = preciosActivos.Any(p => p.EsPrincipal);
                
                if (!tienePrincipal)
                {
                    // Marcar el primer precio como principal
                    preciosActivos.First().EsPrincipal = true;
                }
            }
        }
    }
}