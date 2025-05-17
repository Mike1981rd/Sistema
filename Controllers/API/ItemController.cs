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
                .Select(ic => new 
                {
                    id = ic.Id,
                    unidadMedidaId = ic.UnidadMedidaId,
                    unidadMedidaNombre = ic.UnidadMedida != null ? ic.UnidadMedida.Nombre : "",
                    etiqueta = ic.Etiqueta,
                    cantidad = ic.Factor,  // Using Factor instead of Cantidad
                    costo = ic.Costo,
                    orden = ic.Orden
                })
                .OrderBy(ic => ic.orden)
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
    }
}