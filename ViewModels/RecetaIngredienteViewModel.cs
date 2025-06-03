using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaContable.ViewModels
{
    public class RecetaIngredienteViewModel
    {
        public int Id { get; set; }
        public int ProductoCompuestoId { get; set; }

        [Required(ErrorMessage = "El item es requerido")]
        [Display(Name = "Item")]
        public int ItemId { get; set; }

        [Required(ErrorMessage = "El contenedor/unidad es requerido")]
        [Display(Name = "Contenedor/Unidad")]
        public int ItemContenedorId { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida")]
        [Display(Name = "Cantidad")]
        [Range(0.001, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor que 0")]
        public decimal Cantidad { get; set; }

        [Required(ErrorMessage = "El costo unitario es requerido")]
        [Display(Name = "Costo Unitario")]
        [Range(0, double.MaxValue, ErrorMessage = "El costo debe ser mayor o igual a 0")]
        public decimal CostoUnitario { get; set; }

        // Calculado
        public decimal CostoTotal => CostoUnitario * Cantidad;

        // Para mostrar información
        public string? NombreItem { get; set; }
        public string? MarcaNombre { get; set; }
        public string? UnidadMedidaNombre { get; set; }

        // Para el dropdown de items
        public SelectList? ItemsDisponibles { get; set; }
        public SelectList? ContenedoresDisponibles { get; set; }
    }
}