using Microsoft.AspNetCore.Mvc;
using SistemaContable.Models;
using SistemaContable.Repositories.Interfaces;
using SistemaContable.ViewModels;
using System.Threading.Tasks;

namespace SistemaContable.Controllers
{
    public class NumeracionEntradaDiarioController : Controller
    {
        private readonly INumeracionEntradaDiarioRepository _repository;
        private readonly ITipoEntradaDiarioRepository _tipoRepository;

        public NumeracionEntradaDiarioController(
            INumeracionEntradaDiarioRepository repository,
            ITipoEntradaDiarioRepository tipoRepository)
        {
            _repository = repository;
            _tipoRepository = tipoRepository;
        }

        // Acción para crear una nueva numeración (modal)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NumeracionEntradaDiarioViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var numeracion = new NumeracionEntradaDiario
                {
                    TipoEntradaDiarioId = viewModel.TipoEntradaDiarioId,
                    Nombre = viewModel.Nombre,
                    Prefijo = viewModel.Prefijo,
                    NumeroActual = viewModel.NumeroActual,
                    EsPreferida = viewModel.EsPreferida
                };

                // Si es preferida, actualizar las demás para que no lo sean
                if (viewModel.EsPreferida)
                {
                    await _repository.DesmarcarPreferidas(viewModel.TipoEntradaDiarioId);
                }

                await _repository.CreateAsync(numeracion);
                return Json(new { success = true, id = numeracion.Id, nombre = numeracion.Nombre });
            }

            return Json(new { success = false, message = "Datos inválidos" });
        }
    }
} 