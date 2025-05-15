using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaContable.ViewModels
{
    public class RecetaIngredienteViewModel
    {
        public int Id { get; set; }
        public int ProductoCompuestoId { get; set; }

        [Required(ErrorMessage = "El ingrediente es requerido")]
        [Display(Name = "Ingrediente")]
        public int IngredienteProductoId { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida")]
        [Display(Name = "Cantidad")]
        [Range(0.001, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor que 0")]
        public decimal Cantidad { get; set; }

        [StringLength(20)]
        [Display(Name = "Unidad de Medida")]
        public string? UnidadMedida { get; set; }

        // Para mostrar información
        public string? NombreIngrediente { get; set; }
        public decimal? CostoIngrediente { get; set; }
        public decimal CostoTotal => (CostoIngrediente ?? 0) * Cantidad;

        // Para el dropdown de productos
        public SelectList? ProductosDisponibles { get; set; }
    }
}