using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace SistemaContable.Controllers
{
    public class EmpresasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmpresasController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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

            // Pasar listas a la vista
            ViewBag.Countries = DataLists.LatinAmericanCountries;
            ViewBag.Currencies = DataLists.LatinAmericanCountries
                .GroupBy(c => c.Currency)
                .Select(g => g.First())
                .ToList();
            
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

            // Pasar listas a la vista en caso de error
            ViewBag.Countries = DataLists.LatinAmericanCountries;
            ViewBag.Currencies = DataLists.LatinAmericanCountries
                .GroupBy(c => c.Currency)
                .Select(g => g.First())
                .ToList();
            
            return View(empresa);
        }

        // POST: Empresas/SubirLogo
        [HttpPost]
        public async Task<IActionResult> SubirLogo(IFormFile archivo)
        {
            try
            {
                if (archivo == null || archivo.Length == 0)
                    return Json(new { success = false, message = "No se ha seleccionado ningún archivo" });

                if (!archivo.ContentType.StartsWith("image/"))
                    return Json(new { success = false, message = "El archivo debe ser una imagen" });

                // Obtener la empresa
                var empresa = await _context.Empresas.FirstOrDefaultAsync();
                if (empresa == null)
                    return Json(new { success = false, message = "No se encontró la empresa" });

                // Crear carpeta si no existe
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "images", "logos");
                if (!Directory.Exists(uploadDir))
                    Directory.CreateDirectory(uploadDir);

                // Generar nombre único
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(archivo.FileName);
                string filePath = Path.Combine(uploadDir, fileName);

                // Guardar archivo
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await archivo.CopyToAsync(stream);
                }

                // Actualizar URL en la base de datos
                string dbPath = "/images/logos/" + fileName;
                empresa.LogoUrl = dbPath;
                await _context.SaveChangesAsync();

                return Json(new { 
                    success = true, 
                    message = "Logo actualizado correctamente", 
                    logoUrl = dbPath 
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        // POST: Empresas/Cancelar
        [HttpPost]
        public IActionResult Cancelar()
        {
            return RedirectToAction("Index", "Home");
        }
    }
} 