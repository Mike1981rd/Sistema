using System.ComponentModel.DataAnnotations;
using SistemaContable.Models.Enums;

namespace SistemaContable.ViewModels
{
    public class GrupoModificadoresViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(50)]
        [Display(Name = "Nombre del Grupo")]
        public string Nombre { get; set; }

        [Display(Name = "Es Obligatorio")]
        public bool EsForzado { get; set; } = false;

        [Display(Name = "Mínimo de Selecciones")]
        [Range(0, int.MaxValue, ErrorMessage = "El valor debe ser 0 o mayor")]
        public int MinSeleccion { get; set; } = 0;

        [Display(Name = "Máximo de Selecciones")]
        [Range(0, int.MaxValue, ErrorMessage = "El valor debe ser 0 o mayor")]
        public int MaxSeleccion { get; set; } = 1;

        [Display(Name = "Tipo de Visualización")]
        public TipoVisualizacionTPV TipoVisualizacionTPV { get; set; } = TipoVisualizacionTPV.Lista;

        public List<ModificadorViewModel> Modificadores { get; set; } = new();
    }
}