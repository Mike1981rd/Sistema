using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaContable.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuentasContablesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CuentasContablesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CuentasContables/Buscar
        [HttpGet("Buscar")]
        public async Task<IActionResult> Buscar(string term = "", string tipo = "")
        {
            try
            {
                // Use empresa 4 as temporary default
                int empresaId = 4;
                
                // Try to get from session
                var empresaIdClaim = User.FindFirst("EmpresaId");
                if (empresaIdClaim != null && int.TryParse(empresaIdClaim.Value, out int claimEmpresaId))
                {
                    empresaId = claimEmpresaId;
                }
                
                Console.WriteLine($"[CuentasContables.Buscar] term: '{term}', tipo: '{tipo}', empresaId: {empresaId}");

                var query = _context.CuentasContables
                    .Where(c => c.Activo && c.EmpresaId == empresaId);

                // If term is a number, search by ID first
                if (int.TryParse(term, out int cuentaId))
                {
                    Console.WriteLine($"[CuentasContables.Buscar] Searching by ID: {cuentaId}");
                    var byId = await query.Where(c => c.Id == cuentaId)
                        .Select(c => new {
                            id = c.Id,
                            text = $"{c.Codigo} - {c.Nombre}",
                            codigo = c.Codigo,
                            nombre = c.Nombre
                        })
                        .FirstOrDefaultAsync();
                    
                    if (byId != null)
                    {
                        Console.WriteLine($"[CuentasContables.Buscar] Found by ID: {byId.text}");
                        return Ok(new { results = new[] { byId } });
                    }
                }

                // Otherwise search by term in name or code
                if (!string.IsNullOrEmpty(term))
                {
                    query = query.Where(c => 
                        c.Nombre.Contains(term) || 
                        c.Codigo.Contains(term));
                }

                // Filter by type if specified
                if (!string.IsNullOrEmpty(tipo))
                {
                    switch (tipo)
                    {
                        case "Ventas":
                            query = query.Where(c => c.Categoria == "Ingreso");
                            break;
                        case "ComprasInventarios":
                            query = query.Where(c => c.Categoria == "Activo" && 
                                (c.UsoCuenta == "Inventario" || c.Nombre.Contains("Inventario")));
                            break;
                        case "CostoVentasGastos":
                            query = query.Where(c => c.Categoria == "Costo" || c.Categoria == "Gasto");
                            break;
                        case "Descuentos":
                        case "Devoluciones":
                        case "Ajustes":
                            query = query.Where(c => c.Categoria == "Ingreso" || c.Categoria == "Gasto");
                            break;
                    }
                }

                var cuentas = await query
                    .Select(c => new {
                        id = c.Id,
                        text = $"{c.Codigo} - {c.Nombre}",
                        codigo = c.Codigo,
                        nombre = c.Nombre
                    })
                    .Take(20)
                    .ToListAsync();

                Console.WriteLine($"[CuentasContables.Buscar] Found {cuentas.Count} accounts");
                return Ok(new { results = cuentas });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CuentasContables.Buscar] Error: {ex.Message}");
                Console.WriteLine($"[CuentasContables.Buscar] Stack: {ex.StackTrace}");
                return StatusCode(500, new { error = ex.Message, details = ex.StackTrace });
            }
        }

        // GET: api/CuentasContables/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var cuenta = await _context.CuentasContables
                    .Where(c => c.Id == id)
                    .Select(c => new {
                        id = c.Id,
                        text = $"{c.Codigo} - {c.Nombre}",
                        codigo = c.Codigo,
                        nombre = c.Nombre
                    })
                    .FirstOrDefaultAsync();

                if (cuenta == null)
                {
                    return NotFound();
                }

                return Ok(cuenta);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CuentasContables.GetById] Error: {ex.Message}");
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}