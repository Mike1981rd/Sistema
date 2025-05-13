using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaContable.Models.ViewModels
{
    public class ItemAlmacenViewModel
    {
        public int Id { get; set; }

        public int ItemId { get; set; }

        [Required(ErrorMessage = "El almacén es requerido")]
        [Display(Name = "Almacén")]
        public int AlmacenId { get; set; }
        public string? AlmacenNombre { get; set; }

        [Required(ErrorMessage = "El stock es requerido")]
        [Range(0, double.MaxValue, ErrorMessage = "El stock debe ser mayor o igual a 0")]
        [Display(Name = "Stock")]
        public decimal Stock { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El nivel mínimo debe ser mayor o igual a 0")]
        [Display(Name = "Nivel Mínimo")]
        public decimal NivelMinimo { get; set; }

        [StringLength(100, ErrorMessage = "La ubicación no puede exceder los 100 caracteres")]
        [Display(Name = "Ubicación")]
        public string? Ubicacion { get; set; }

        // Para el dropdown de selección de almacén
        public SelectList? AlmacenesDisponibles { get; set; }

        // Propiedades calculadas para UI
        [Display(Name = "Estado Stock")]
        public string EstadoStock => Stock <= NivelMinimo ? "Bajo" : "Normal";

        public string ColorEstadoStock => Stock <= NivelMinimo ? "text-danger" : "text-success";
    }
}