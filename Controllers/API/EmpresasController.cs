using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using System.Threading.Tasks;
using System.Linq;

namespace SistemaContable.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmpresasController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("primera")]
        public async Task<IActionResult> GetPrimeraEmpresa()
        {
            var empresa = await _context.Empresas
                .OrderBy(e => e.Id)
                .Select(e => new { e.Id, e.Nombre })
                .FirstOrDefaultAsync();

            if (empresa == null)
                return NotFound(new { message = "No hay empresas en el sistema" });

            return Ok(empresa);
        }

        [HttpPost("set-session/{id}")]
        public IActionResult SetEmpresaSession(int id)
        {
            HttpContext.Session.SetInt32("EmpresaId", id);
            return Ok(new { message = $"EmpresaId {id} establecido en sesión" });
        }

        [HttpGet("current")]
        public IActionResult GetCurrentEmpresa()
        {
            var empresaId = HttpContext.Session.GetInt32("EmpresaId") ?? 0;
            return Ok(new { empresaId });
        }
    }
}