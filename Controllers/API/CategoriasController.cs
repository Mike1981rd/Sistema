using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Services;

namespace SistemaContable.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmpresaService _empresaService;

        public CategoriasController(ApplicationDbContext context, IEmpresaService empresaService)
        {
            _context = context;
            _empresaService = empresaService;
        }

        [HttpGet("Buscar")]
        public async Task<IActionResult> Buscar(string term = "")
        {
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            
            var query = _context.Categorias
                .Where(c => c.EmpresaId == empresaId && c.Estado);

            if (!string.IsNullOrEmpty(term))
            {
                query = query.Where(c => c.Nombre.Contains(term));
            }

            var categorias = await query
                .OrderBy(c => c.Nombre)
                .Select(c => new 
                {
                    id = c.Id,
                    text = c.Nombre
                })
                .Take(20)
                .ToListAsync();

            return Ok(new { results = categorias });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            
            var categoria = await _context.Categorias
                .Where(c => c.Id == id && c.EmpresaId == empresaId)
                .Select(c => new 
                {
                    id = c.Id,
                    nombre = c.Nombre
                })
                .FirstOrDefaultAsync();

            if (categoria == null)
            {
                return NotFound();
            }

            return Ok(categoria);
        }
    }
}