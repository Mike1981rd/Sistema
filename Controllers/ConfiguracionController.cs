using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Models;

namespace SistemaContable.Controllers
{
    public class ConfiguracionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConfiguracionController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Obtener o crear la empresa por defecto
            var empresa = await _context.Empresas.FirstOrDefaultAsync();
            
            if (empresa == null)
            {
                empresa = new Empresa
                {
                    Nombre = "Aurora Contabilidad",
                    NumeroIdentificacion = "",
                    TipoIdentificacion = "RNC",
                    MonedaPrincipal = "DOP",
                    PrecisionDecimal = 2,
                    SeparadorDecimal = ",",
                    Activo = true
                };
                
                _context.Empresas.Add(empresa);
                await _context.SaveChangesAsync();
            }
            
            return View(empresa);
        }
        
        [HttpPost]
        public async Task<IActionResult> GuardarEmpresa(Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                empresa.FechaActualizacion = DateTime.Now;
                
                _context.Update(empresa);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }
            
            return View("Index", empresa);
        }
    }
} 