using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
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
        public async Task<IActionResult> Buscar(string q)
        {
            if (string.IsNullOrEmpty(q))
            {
                return Ok(new object[0]);
            }

            var cuentas = await _context.CuentasContables
                .Where(c => c.Nombre.Contains(q) || c.Codigo.Contains(q))
                .Select(c => new { id = c.Id, codigo = c.Codigo, nombre = c.Nombre })
                .Take(10)
                .ToListAsync();

            return Ok(cuentas);
        }
    }
} 