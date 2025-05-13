using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaContable.Models.ViewModels
{
    public class ItemProveedorViewModel
    {
        public int Id { get; set; }

        public int ItemId { get; set; }

        [Required(ErrorMessage = "El proveedor es requerido")]
        [Display(Name = "Proveedor")]
        public int ProveedorId { get; set; }
        public string? ProveedorNombre { get; set; }

        [StringLength(100, ErrorMessage = "El nombre de compra no puede exceder los 100 caracteres")]
        [Display(Name = "Nombre de Compra")]
        public string? NombreCompra { get; set; }

        [StringLength(50, ErrorMessage = "El código de proveedor no puede exceder los 50 caracteres")]
        [Display(Name = "Código Proveedor")]
        public string? CodigoProveedor { get; set; }

        [Required(ErrorMessage = "El precio de compra es requerido")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero")]
        [Display(Name = "Precio de Compra")]
        public decimal PrecioCompra { get; set; }

        [Required(ErrorMessage = "La unidad de medida es requerida")]
        [Display(Name = "Unidad de Compra")]
        public int UnidadMedidaCompraId { get; set; }
        public string? UnidadMedidaNombre { get; set; }

        [Required(ErrorMessage = "El factor de conversión es requerido")]
        [Range(0.000001, double.MaxValue, ErrorMessage = "El factor debe ser mayor que cero")]
        [Display(Name = "Factor de Conversión")]
        public decimal FactorConversion { get; set; } = 1;

        [Display(Name = "Principal")]
        public bool EsPrincipal { get; set; }

        [Display(Name = "Última Actualización")]
        public DateTime? UltimaActualizacionPrecio { get; set; }

        // Propiedad calculada para mostrar el precio unitario (no se guarda en la BD)
        [Display(Name = "Precio por Unidad")]
        public decimal PrecioUnitario => PrecioCompra / (FactorConversion > 0 ? FactorConversion : 1);

        // Para el select2 del proveedor
        public SelectList? ProveedoresDisponibles { get; set; }

        // Para el select2 de unidad de medida
        public SelectList? UnidadesMedidaDisponibles { get; set; }
    }
}