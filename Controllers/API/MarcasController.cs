using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Services;

namespace SistemaContable.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarcasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmpresaService _empresaService;

        public MarcasController(ApplicationDbContext context, IEmpresaService empresaService)
        {
            _context = context;
            _empresaService = empresaService;
        }

        [HttpGet("Buscar")]
        public async Task<IActionResult> Buscar(string term = "")
        {
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            
            var query = _context.Marcas
                .Where(m => m.EmpresaId == empresaId && m.Estado);

            if (!string.IsNullOrEmpty(term))
            {
                query = query.Where(m => m.Nombre.Contains(term));
            }

            var marcas = await query
                .OrderBy(m => m.Nombre)
                .Select(m => new 
                {
                    id = m.Id,
                    text = m.Nombre
                })
                .Take(20)
                .ToListAsync();

            return Ok(new { results = marcas });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            
            var marca = await _context.Marcas
                .Where(m => m.Id == id && m.EmpresaId == empresaId)
                .Select(m => new 
                {
                    id = m.Id,
                    nombre = m.Nombre
                })
                .FirstOrDefaultAsync();

            if (marca == null)
            {
                return NotFound();
            }

            return Ok(marca);
        }
    }
}