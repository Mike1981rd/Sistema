using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Models;
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

        public ProductosController(ApplicationDbContext context)
        {
            _context = context;
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
                // Query base con includes necesarios
                var query = _context.ProductosVenta
                    .Include(p => p.Categoria)
                    .Include(p => p.Variantes)
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
                // Query con todas las relaciones necesarias
                var producto = await _context.ProductosVenta
                    .Include(p => p.Categoria)
                    .Include(p => p.Impuesto)
                    .Include(p => p.RutaImpresora)
                    .Include(p => p.Variantes)
                    .Include(p => p.ProductoModificadorGrupos)
                        .ThenInclude(pmg => pmg.GrupoModificadores)
                            .ThenInclude(g => g.Modificadores)
                    .FirstOrDefaultAsync(p => p.Id == id);

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
                    
                    // Mapear relaciones
                    Categoria = new CategoriaSimpleDto
                    {
                        Id = producto.Categoria.Id,
                        Nombre = producto.Categoria.Nombre
                    }
                };

                if (producto.Impuesto != null)
                {
                    productoDto.Impuesto = new ImpuestoSimpleDto
                    {
                        Id = producto.Impuesto.Id,
                        Nombre = producto.Impuesto.Nombre,
                        Porcentaje = producto.Impuesto.Porcentaje
                    };
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

                var itemExiste = await _context.Items.AnyAsync(i => i.Id == productoDto.ItemId);
                if (!itemExiste)
                {
                    return BadRequest(new { mensaje = "El item especificado no existe" });
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

                // Verificar unicidad del PLU si se proporciona
                if (!string.IsNullOrWhiteSpace(productoDto.PLU))
                {
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
                    EmpresaId = productoDto.EmpresaId
                };

                _context.ProductosVenta.Add(producto);
                await _context.SaveChangesAsync();

                // Retornar el producto creado
                return CreatedAtAction(
                    nameof(ObtenerProductoPorId), 
                    new { id = producto.Id }, 
                    await ObtenerProductoPorId(producto.Id)
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al crear el producto", detalle = ex.Message });
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

            try
            {
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

                // Verificar unicidad del PLU si cambió
                if (!string.IsNullOrWhiteSpace(productoDto.PLU) && productoDto.PLU != producto.PLU)
                {
                    var pluExiste = await _context.ProductosVenta.AnyAsync(p => p.PLU == productoDto.PLU && p.Id != id);
                    if (pluExiste)
                    {
                        return BadRequest(new { mensaje = "Ya existe otro producto con ese PLU" });
                    }
                }

                // Actualizar campos
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
                producto.CostoTotal = productoDto.Costo * productoDto.Cantidad;
                producto.DisponibleParaVenta = productoDto.DisponibleParaVenta;
                producto.RequierePreparacion = productoDto.RequierePreparacion;
                producto.TiempoPreparacion = productoDto.TiempoPreparacion;

                try
                {
                    await _context.SaveChangesAsync();
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

                return NoContent();
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
    }
}