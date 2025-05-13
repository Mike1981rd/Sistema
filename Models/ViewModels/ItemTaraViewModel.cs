using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaContable.Models.ViewModels
{
    public class ItemTaraViewModel
    {
        public int Id { get; set; }

        public int ItemId { get; set; }

        [Required(ErrorMessage = "El contenedor es requerido")]
        [Display(Name = "Contenedor")]
        public int ItemContenedorId { get; set; }
        public string? ContenedorNombre { get; set; }

        [Required(ErrorMessage = "El valor de tara es requerido")]
        [Range(0, double.MaxValue, ErrorMessage = "El valor de tara debe ser mayor o igual a 0")]
        [Display(Name = "Tara")]
        public decimal ValorTara { get; set; }

        [StringLength(100, ErrorMessage = "La observaci�n no puede exceder los 100 caracteres")]
        [Display(Name = "Observaci�n")]
        public string? Observacion { get; set; }

        // Para el dropdown de selecci�n de contenedor
        public SelectList? ContenedoresDisponibles { get; set; }

        // Propiedad calculada para mostrar informaci�n adicional
        public string InfoContenedor => $"{ContenedorNombre}" + (ValorTara > 0 ? $" (Tara: {ValorTara})" : "");
    }
}