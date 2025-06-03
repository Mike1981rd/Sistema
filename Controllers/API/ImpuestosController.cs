using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaContable.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImpuestosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmpresaService _empresaService;

        public ImpuestosController(ApplicationDbContext context, IEmpresaService empresaService)
        {
            _context = context;
            _empresaService = empresaService;
        }

        [HttpGet("Buscar")]
        public async Task<IActionResult> Buscar(string term = "")
        {
            try
            {
                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                
                if (empresaId <= 0)
                {
                    return Ok(new { results = new List<object>() });
                }
                
                var query = _context.Impuestos
                    .Where(i => i.EmpresaId == empresaId && i.Estado);

                if (!string.IsNullOrWhiteSpace(term))
                {
                    // Si el término es un número, buscar por ID exacto
                    if (int.TryParse(term, out int impuestoId))
                    {
                        query = query.Where(i => i.Id == impuestoId);
                    }
                    else
                    {
                        // Si no es un número, buscar por nombre
                        query = query.Where(i => i.Nombre.Contains(term));
                    }
                }

                var impuestos = await query
                    .OrderBy(i => i.Nombre)
                    .Select(i => new
                    {
                        id = i.Id,
                        text = i.Nombre,
                        porcentaje = i.Porcentaje ?? 0
                    })
                    .Take(20)
                    .ToListAsync();

                return Ok(new { results = impuestos });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en API Impuestos/Buscar: {ex.Message}");
                return Ok(new { results = new List<object>() });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetImpuestos(string search = null, int page = 1)
        {
            try
            {
                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                
                if (empresaId <= 0)
                {
                    return Ok(new List<object>());
                }
                
                var query = _context.Impuestos
                    .Where(i => i.EmpresaId == empresaId);

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(i => i.Nombre.Contains(search));
                }

                var impuestos = await query
                    .OrderBy(i => i.Nombre)
                    .Select(i => new
                    {
                        id = i.Id,
                        nombre = i.Nombre,
                        porcentaje = i.Porcentaje ?? 0
                    })
                    .ToListAsync();

                return Ok(impuestos);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en API Impuestos/GetImpuestos: {ex.Message}");
                return Ok(new List<object>());
            }
        }
    }
}