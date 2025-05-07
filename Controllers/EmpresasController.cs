using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Models;
using System;
using System.Threading.Tasks;

namespace SistemaContable.Controllers
{
    public class EmpresasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmpresasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Empresas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Empresas.ToListAsync());
        }

        // GET: Empresas/Configurar
        public async Task<IActionResult> Configurar()
        {
            // Obtener la primera empresa o crear una nueva si no existe
            var empresa = await _context.Empresas.FirstOrDefaultAsync();
            
            if (empresa == null)
            {
                empresa = new Empresa
                {
                    Nombre = "Mi Empresa",
                    NumeroIdentificacion = "000000000",
                    TipoIdentificacion = "RNC",
                    Direccion = "Dirección por defecto",
                    Ciudad = "Ciudad por defecto",
                    Provincia = "Provincia por defecto",
                    Pais = "República Dominicana",
                    MonedaPrincipal = "DOP",
                    PrecisionDecimal = 2,
                    SeparadorDecimal = ".",
                    ResponsabilidadTributaria = "Régimen General",
                    Activo = true,
                    FechaCreacion = DateTime.UtcNow
                };
                
                _context.Add(empresa);
                await _context.SaveChangesAsync();
            }
            
            return View(empresa);
        }

        // POST: Empresas/Configurar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Configurar(Empresa empresa)
        {
            // Verificar campos obligatorios
            if (string.IsNullOrEmpty(empresa.Nombre))
                empresa.Nombre = "Empresa por defecto";
                
            if (string.IsNullOrEmpty(empresa.NumeroIdentificacion))
                empresa.NumeroIdentificacion = "000000000";
                
            if (string.IsNullOrEmpty(empresa.TipoIdentificacion))
                empresa.TipoIdentificacion = "RNC";
                
            if (string.IsNullOrEmpty(empresa.Direccion))
                empresa.Direccion = "Dirección por defecto";
                
            if (string.IsNullOrEmpty(empresa.Ciudad))
                empresa.Ciudad = "Ciudad por defecto";
                
            if (string.IsNullOrEmpty(empresa.Provincia))
                empresa.Provincia = "Provincia por defecto";
                
            if (string.IsNullOrEmpty(empresa.Pais))
                empresa.Pais = "República Dominicana";
                
            if (string.IsNullOrEmpty(empresa.MonedaPrincipal))
                empresa.MonedaPrincipal = "DOP";
                
            if (string.IsNullOrEmpty(empresa.SeparadorDecimal))
                empresa.SeparadorDecimal = ".";

            if (ModelState.IsValid)
            {
                empresa.FechaActualizacion = DateTime.UtcNow;
                
                try
                {
                    _context.Update(empresa);
                    await _context.SaveChangesAsync();
                    
                    TempData["SuccessMessage"] = "Datos de la empresa guardados correctamente.";
                    return RedirectToAction(nameof(Configurar));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Error al guardar: " + ex.InnerException?.Message ?? ex.Message);
                }
            }
            
            return View(empresa);
        }

        // POST: Empresas/Cancelar
        [HttpPost]
        public IActionResult Cancelar()
        {
            return RedirectToAction("Index", "Home");
        }
    }
} 