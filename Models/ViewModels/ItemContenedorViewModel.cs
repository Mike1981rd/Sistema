using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaContable.Models.ViewModels
{
    public class ItemContenedorViewModel
    {
        public int Id { get; set; }

        public int ItemId { get; set; }

        [Required(ErrorMessage = "El nombre del contenedor es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        [Display(Name = "Contenedor")]
        public string Nombre { get; set; }

        [Display(Name = "Etiqueta")]
        public string Etiqueta { get; set; }

        [Display(Name = "Contenedor Superior")]
        public int? ContenedorSuperiorId { get; set; }
        public string? ContenedorSuperiorNombre { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida")]
        [Range(0.000001, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero")]
        [Display(Name = "Cantidad")]
        public decimal Factor { get; set; } = 1;

        [Display(Name = "Costo")]
        public decimal Costo { get; set; }

        [Display(Name = "Es Principal")]
        public bool EsPrincipal { get; set; }

        [Display(Name = "Usar para Compras")]
        public bool EsContenedorCompra { get; set; }

        [Display(Name = "Orden")]
        public int Orden { get; set; }

        [Required(ErrorMessage = "La unidad de medida es requerida")]
        [Display(Name = "Unidad de Medida")]
        public int UnidadMedidaId { get; set; }
        public string? UnidadMedidaNombre { get; set; }

        // Para el dropdown de selección de unidad
        public SelectList? UnidadesMedidaDisponibles { get; set; }

        // Para el dropdown de selección de contenedor superior
        public SelectList? ContenedoresSuperiorDisponibles { get; set; }

        // Propiedad calculada para mostrar información adicional
        public string EtiquetaCompleta => $"{Nombre}" + (ContenedorSuperiorNombre != null ? $" / {ContenedorSuperiorNombre}" : "");
    }
}