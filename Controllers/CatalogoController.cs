using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Models;
using SistemaContable.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaContable.Controllers
{
    [Route("contabilidad/[controller]")]
    public class CatalogoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmpresaService _empresaService;

        public CatalogoController(ApplicationDbContext context, IEmpresaService empresaService)
        {
            _context = context;
            _empresaService = empresaService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var empresaId = _empresaService.ObtenerEmpresaActualId();
            
            // Obtener cuentas de la empresa actual incluyendo las subcuentas
            var cuentas = await _context.CuentasContables
                .Where(c => c.EmpresaId == empresaId)
                .Include(c => c.SubCuentas)
                .OrderBy(c => c.Codigo)
                .ToListAsync();

            return View("~/Views/CatalogoCuentas/Index.cshtml", cuentas);
        }

        // Estos métodos redirigen a los correspondientes en CatalogoCuentasController
        [HttpGet("create")]
        public IActionResult Create()
        {
            // Redirigir a la acción Create del CatalogoCuentasController
            return RedirectToAction("Create", "CatalogoCuentas");
        }

        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            // Redirigir a la acción Edit del CatalogoCuentasController
            return RedirectToAction("Edit", "CatalogoCuentas", new { id });
        }

        [HttpGet("delete/{id}")]
        public IActionResult Delete(int id)
        {
            // Redirigir a la acción Delete del CatalogoCuentasController
            return RedirectToAction("Delete", "CatalogoCuentas", new { id });
        }

        [HttpGet("movimientos/{id}")]
        public IActionResult Movimientos(int id)
        {
            // Redirigir a la acción Movimientos del CatalogoCuentasController
            return RedirectToAction("Movimientos", "CatalogoCuentas", new { id });
        }

        [HttpGet("saldos-iniciales")]
        public IActionResult SaldosIniciales()
        {
            // Redirigir a la acción SaldosIniciales del CatalogoCuentasController
            return RedirectToAction("SaldosIniciales", "CatalogoCuentas");
        }

        [HttpGet("importar")]
        public IActionResult ImportarExcel()
        {
            // Redirigir a la acción ImportarExcel del CatalogoCuentasController
            return RedirectToAction("ImportarExcel", "CatalogoCuentas");
        }
    }
} 