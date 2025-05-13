using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Models;
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

        // GET: api/cuentas-contables/buscar?q=search
        [HttpGet("buscar")]
        public async Task<IActionResult> Buscar(string q, bool exactId = false)
        {
            if (string.IsNullOrEmpty(q))
            {
                return Ok(new object[0]);
            }

            IQueryable<CuentaContable> query = _context.CuentasContables;

            // Si exactId es true, buscar por ID exacto
            if (exactId && int.TryParse(q, out int cuentaId))
            {
                query = query.Where(c => c.Id == cuentaId);
            }
            else
            {
                // Búsqueda normal por nombre o código
                query = query.Where(c => c.Nombre.Contains(q) || c.Codigo.Contains(q));
            }

            var cuentas = await query
                .Select(c => new { id = c.Id, codigo = c.Codigo, nombre = c.Nombre })
                .Take(10)
                .ToListAsync();

            return Ok(cuentas);
        }
    }
} 