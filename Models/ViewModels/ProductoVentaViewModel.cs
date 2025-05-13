using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaContable.Models.ViewModels
{
    public class ProductoVentaViewModel
    {
        public int Id { get; set; }
        
        public int ItemId { get; set; }
        
        [Required(ErrorMessage = "El contenedor es requerido")]
        [Display(Name = "Contenedor")]
        public int ItemContenedorId { get; set; }
        public string? ContenedorNombre { get; set; }
        
        [Required(ErrorMessage = "El nombre del producto es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }
        
        [Required(ErrorMessage = "La cantidad es requerida")]
        [Range(0.001, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero")]
        [Display(Name = "Cantidad")]
        public decimal Cantidad { get; set; } = 1;
        
        [Required(ErrorMessage = "El precio de venta es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero")]
        [Display(Name = "Precio de Venta")]
        public decimal PrecioVenta { get; set; }
        
        [Display(Name = "Costo")]
        public decimal Costo { get; set; }
        
        [Display(Name = "Costo Total")]
        public decimal CostoTotal => Costo * Cantidad;
        
        [Display(Name = "Margen de Utilidad (%)")]
        public decimal MargenUtilidad => Costo > 0 ? ((PrecioVenta - Costo) / Costo) * 100 : 0;
        
        [Display(Name = "Impuesto")]
        public int? ImpuestoId { get; set; }
        public string? ImpuestoNombre { get; set; }
        
        [Display(Name = "Disponible para Venta")]
        public bool DisponibleParaVenta { get; set; } = true;
        
        [Display(Name = "Requiere Preparación")]
        public bool RequierePreparacion { get; set; }
        
        [Range(0, 1440, ErrorMessage = "El tiempo de preparación debe estar entre 0 y 1440 minutos")]
        [Display(Name = "Tiempo de Preparación (minutos)")]
        public int? TiempoPreparacion { get; set; }
        
        // Para el dropdown de selección de contenedor
        public SelectList? ContenedoresDisponibles { get; set; }
        
        // Para el dropdown de selección de impuesto
        public SelectList? ImpuestosDisponibles { get; set; }
    }
} 