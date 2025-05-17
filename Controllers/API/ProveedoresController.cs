using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Services;

namespace SistemaContable.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;

        public ProveedoresController(ApplicationDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpGet("Buscar")]
        public async Task<IActionResult> Buscar(string term = "")
        {
            var empresaId = _userService.GetEmpresaId();
            // Log para depuración
            System.Console.WriteLine($"[ProveedoresAPI/Buscar] EmpresaId: {empresaId}, Term: '{term}'");
            
            var query = _context.Clientes
                .Where(c => c.EmpresaId == empresaId && c.EsProveedor);

            if (!string.IsNullOrEmpty(term))
            {
                string lowerTerm = term.ToLower();
                query = query.Where(c => 
                    (c.NombreRazonSocial != null && c.NombreRazonSocial.ToLower().Contains(lowerTerm)) || 
                    (c.NumeroIdentificacion != null && c.NumeroIdentificacion.ToLower().Contains(lowerTerm)));
            }

            var proveedores = await query
                .OrderBy(c => c.NombreRazonSocial)
                .Select(c => new 
                {
                    id = c.Id,
                    text = c.NombreRazonSocial,
                    ruc = c.NumeroIdentificacion
                })
                .Take(20)
                .ToListAsync();

            return Ok(new { results = proveedores });
        }
    }
}