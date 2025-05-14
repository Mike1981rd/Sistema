using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using SistemaContable.Data;
using SistemaContable.Models;
using Microsoft.EntityFrameworkCore;

namespace SistemaContable.Controllers
{
    public class ContenedorController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ContenedorController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Buscar(string term = "")
        {
            var query = _context.Contenedores.AsQueryable();
            if (!string.IsNullOrEmpty(term))
            {
                term = term.ToLower();
                query = query.Where(c => c.Nombre.ToLower().Contains(term));
            }
            var contenedores = await query
                .Select(c => new { id = c.Id, text = c.Nombre })
                .Take(10)
                .ToListAsync();

            return Json(new { results = contenedores });
        }

        [HttpPost]
        public async Task<IActionResult> Create(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return Json(new { success = false, message = "El nombre es requerido" });

            var contenedor = new Contenedor { Nombre = nombre };
            _context.Contenedores.Add(contenedor);
            await _context.SaveChangesAsync();

            return Json(new { success = true, id = contenedor.Id, nombre = contenedor.Nombre });
        }

        [HttpPost]
        [Route("Contenedor/Edit/{id}")]
        public async Task<IActionResult> Edit(int id, string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return Json(new { success = false, message = "El nombre es requerido" });

            var contenedor = await _context.Contenedores.FindAsync(id);
            if (contenedor == null)
                return Json(new { success = false, message = "Contenedor no encontrado" });

            contenedor.Nombre = nombre;
            await _context.SaveChangesAsync();

            return Json(new { success = true, id = contenedor.Id, nombre = contenedor.Nombre });
        }
    }
} 