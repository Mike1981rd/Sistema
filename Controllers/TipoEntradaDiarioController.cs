using Microsoft.AspNetCore.Mvc;
using SistemaContable.Models;
using SistemaContable.Repositories.Interfaces;
using SistemaContable.ViewModels;
using System.Threading.Tasks;

namespace SistemaContable.Controllers
{
    public class TipoEntradaDiarioController : Controller
    {
        private readonly ITipoEntradaDiarioRepository _repository;

        public TipoEntradaDiarioController(ITipoEntradaDiarioRepository repository)
        {
            _repository = repository;
        }

        // Acción para crear un nuevo tipo (modal)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TipoEntradaDiarioViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Verificar si el código ya existe
                if (await _repository.ExisteCodigoAsync(viewModel.Codigo))
                {
                    return Json(new { success = false, message = "El código ya existe" });
                }

                var tipoEntrada = new TipoEntradaDiario
                {
                    Codigo = viewModel.Codigo,
                    Nombre = viewModel.Nombre
                };

                await _repository.CreateAsync(tipoEntrada);
                return Json(new { success = true, id = tipoEntrada.Id, nombre = tipoEntrada.Nombre });
            }

            return Json(new { success = false, message = "Datos inválidos" });
        }
    }
} 