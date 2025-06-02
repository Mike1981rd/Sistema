using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Models;
using SistemaContable.Services;

namespace SistemaContable.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;

        public ItemController(ApplicationDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpGet("{id}/Contenedores")]
        public async Task<IActionResult> GetContenedores(int id)
        {
            var contenedores = await _context.ItemContenedores
                .Where(ic => ic.ItemId == id)
                .Include(ic => ic.UnidadMedida)
                .OrderBy(ic => ic.Orden)
                .Select(ic => new 
                {
                    id = ic.Id,
                    unidadMedidaId = ic.UnidadMedidaId,
                    nombre = ic.Nombre,
                    unidadMedidaNombre = ic.UnidadMedida != null ? ic.UnidadMedida.Nombre : "",
                    factor = ic.Factor,
                    etiqueta = ic.Etiqueta,
                    costo = ic.Costo,
                    orden = ic.Orden,
                    esPrincipal = ic.EsPrincipal
                })
                .ToListAsync();

            return Ok(contenedores);
        }

        [HttpGet("{id}/Proveedores")]
        public async Task<IActionResult> GetProveedores(int id)
        {
            var proveedores = await _context.ItemProveedores
                .Where(ip => ip.ItemId == id)
                .Select(ip => new 
                {
                    id = ip.Id,
                    proveedorId = ip.ProveedorId,
                    nombreProveedor = ip.Proveedor != null ? ip.Proveedor.NombreRazonSocial : "",  // Using NombreRazonSocial
                    unidadMedidaCompraId = ip.UnidadMedidaCompraId,
                    esPrincipal = ip.EsPrincipal,
                    precioCompra = ip.PrecioCompra,
                    factorConversion = ip.FactorConversion,
                    ultimaActualizacion = ip.UltimaActualizacionPrecio
                })
                .OrderByDescending(ip => ip.esPrincipal)
                .ThenBy(ip => ip.nombreProveedor)
                .ToListAsync();

            return Ok(proveedores);
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> BuscarItems(bool activos = true)
        {
            try
            {
                var empresaId = _userService.GetEmpresaId();

                var query = _context.Items
                    .Include(i => i.Marca)
                    .Include(i => i.Categoria)
                    .Include(i => i.Contenedores)
                        .ThenInclude(ic => ic.UnidadMedida)
                    .Where(i => i.EmpresaId == empresaId);

                if (activos)
                {
                    query = query.Where(i => i.Activo);
                }

                var items = await query
                    .Select(i => new
                    {
                        id = i.Id,
                        codigo = i.Codigo,
                        nombre = i.Nombre,
                        marca = i.Marca != null ? new { id = i.Marca.Id, nombre = i.Marca.Nombre } : null,
                        categoria = i.Categoria != null ? new { id = i.Categoria.Id, nombre = i.Categoria.Nombre } : null,
                        contenedores = i.Contenedores.Select(ic => new
                        {
                            id = ic.Id,
                            nombre = ic.Nombre,
                            unidadMedida = ic.UnidadMedida != null ? new { id = ic.UnidadMedida.Id, nombre = ic.UnidadMedida.Nombre } : null,
                            costoUnitario = ic.Costo,
                            factor = ic.Factor,
                            esPrincipal = ic.EsPrincipal
                        }).OrderBy(c => c.esPrincipal ? 0 : 1).ThenBy(c => c.nombre).ToList()
                    })
                    .OrderBy(i => i.nombre)
                    .ToListAsync();

                return Ok(new { success = true, items });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = ex.Message });
            }
        }
    }
}