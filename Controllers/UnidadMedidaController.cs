using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Models;
using SistemaContable.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaContable.Controllers
{
    public class UnidadMedidaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmpresaService _empresaService;

        public UnidadMedidaController(
            ApplicationDbContext context,
            IEmpresaService empresaService)
        {
            _context = context;
            _empresaService = empresaService;
        }

        // GET: UnidadMedida/ObtenerTodas
        [HttpGet]
        public async Task<JsonResult> ObtenerTodas()
        {
            try
            {
                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                if (empresaId <= 0)
                {
                    return Json(new { error = "Empresa no seleccionada" });
                }

                var unidades = await _context.UnidadesMedida
                    .Where(u => u.Estado)
                    .OrderBy(u => u.Nombre)
                    .Select(u => new { 
                        id = u.Id, 
                        nombre = u.Nombre,
                        abreviatura = u.Abreviatura
                    })
                    .ToListAsync();

                return Json(unidades);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ObtenerTodas: {ex.Message}");
                return Json(new { error = ex.Message });
            }
        }
    }
} 