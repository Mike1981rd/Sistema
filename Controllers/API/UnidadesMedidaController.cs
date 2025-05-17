using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Services;

namespace SistemaContable.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnidadesMedidaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;

        public UnidadesMedidaController(ApplicationDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpGet("Buscar")]
        public async Task<IActionResult> Buscar(string term = "")
        {
            var empresaId = _userService.GetEmpresaId();
            
            var query = _context.UnidadesMedida
                .Where(u => u.EmpresaId == empresaId && u.Estado);

            if (!string.IsNullOrEmpty(term))
            {
                query = query.Where(u => 
                    u.Nombre.Contains(term) || 
                    u.Abreviatura.Contains(term));
            }

            var unidades = await query
                .OrderBy(u => u.Nombre)
                .Select(u => new 
                {
                    id = u.Id,
                    text = u.Nombre + " (" + u.Abreviatura + ")"
                })
                .Take(20)
                .ToListAsync();

            return Ok(new { results = unidades });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var empresaId = _userService.GetEmpresaId();
            
            var unidad = await _context.UnidadesMedida
                .Where(u => u.Id == id && u.EmpresaId == empresaId)
                .Select(u => new 
                {
                    id = u.Id,
                    nombre = u.Nombre + " (" + u.Abreviatura + ")",
                    abreviatura = u.Abreviatura
                })
                .FirstOrDefaultAsync();

            if (unidad == null)
            {
                return NotFound();
            }

            return Ok(unidad);
        }
    }
}